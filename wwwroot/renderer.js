import * as THREE from 'three';
import { Line2 } from 'three/addons/lines/Line2.js';
import { LineMaterial } from 'three/addons/lines/LineMaterial.js';
import { LineGeometry } from 'three/addons/lines/LineGeometry.js';
import { Earcut } from 'three/extras/Earcut.js';

// ---------------------------
// Globals and initialization
// ---------------------------
let scene, renderer, width, height;
let doAnimation = false;
function renderingInit() {
	width = window.innerWidth
	height = window.innerHeight;
	
	scene = new THREE.Scene();

	renderer = new THREE.WebGLRenderer({ antialias: true });
	renderer.setSize(width, height);
	document.body.appendChild(renderer.domElement);	

	return { scene, renderer }
}

renderingInit(); 

// ---------------------------
// Blazor input/event handling
// ---------------------------
window.handleRendering2D = function (jsonArray) {
	const shapes = JSON.parse(jsonArray);
	console.log("Parsed 2D shapes")
	console.log(shapes);
	drawScene2D(scene, renderer, shapes);
}

window.handleRendering3D = function (jsonArray) {
	const polyhedron = JSON.parse(jsonArray);
	console.log("Parsed polyhedra:")
	console.log(polyhedron);
	drawScene3D(scene, renderer, polyhedron);
}


// --------------------------
// 2D Rendering
// --------------------------
function getBounds2D(shapes) {
	let minX = Infinity, minY = Infinity, maxX = -Infinity, maxY = -Infinity;
	shapes.forEach(shape => {
		shape.Vertices.forEach(v => {
			minX = Math.min(minX, v.X);
			maxX = Math.max(maxX, v.X);
			minY = Math.min(minY, v.Y);
			maxY = Math.max(maxY, v.Y);
		});
	});
	minX -= 1;
	minY -= 1;
	maxX += 1;
	maxY += 1;
	const centerX = (minX + maxX) / 2;
	const centerY = (minY + maxY) / 2;
	const width = maxX - minX;
	const height = maxY - minY;
	return { centerX, centerY, width, height };
}

function getColor(status) {
	switch (status) {
		case 0:
			return { color: new THREE.Color(1, 0, 0), opacity: 0.2 };
		case 1:
			return { color: new THREE.Color(0, 0, 1), opacity: 1 };
		case 2:
			return { color: new THREE.Color(1, 0, 0), opacity: 1 };
		case 3:
			return { color: new THREE.Color(0, 1, 0), opacity: 1 };
		default:
			return { color: new THREE.Color(1, 1, 1), opacity: 1 };
	}
}

function sortVertices2D(vertices2D) {
	const centerX = vertices2D.reduce((sum, v) => sum + v[0], 0) / vertices2D.length;
	const centerY = vertices2D.reduce((sum, v) => sum + v[1], 0) / vertices2D.length;

	const sortedIndices = vertices2D.map((v, i) => i).sort((a, b) => {
		const angleA = Math.atan2(vertices2D[a][1] - centerY, vertices2D[a][0] - centerX);
		const angleB = Math.atan2(vertices2D[b][1] - centerY, vertices2D[b][0] - centerX);
		return angleA - angleB;
	});

	return sortedIndices;
}

function triangulate2D(vertices2D) {
	const sortedIndices = sortVertices2D(vertices2D);
	const sortedVertices2D = sortedIndices.map(i => vertices2D[i])
	const indices = Earcut.triangulate(sortedVertices2D.flat(), [], 2);
    return indices.map(i => sortedIndices[i]); 
}

function drawShape(shape, scene) {
	const { color, opacity } = getColor(shape.Status);

	const shapeMaterial = new THREE.MeshBasicMaterial({
		color: color,
		side: THREE.DoubleSide,
		transparent: opacity < 1,
		opacity: opacity
	});

	const vertices = shape.Vertices.map(v => new THREE.Vector3(v.X, v.Y, 0))
	const indices = triangulate2D(vertices.map(v => [v.x, v.y]));
	const shapeGeometry = new THREE.BufferGeometry();
	shapeGeometry.setFromPoints(vertices);
	shapeGeometry.setIndex(indices);

	const shapeMesh = new THREE.Mesh(shapeGeometry, shapeMaterial);
	scene.add(shapeMesh);
}

function drawShapeEdge(edge, scene) {
	const edgeColor = edge.Connector ? 0x000000 : 0xffffff;
	const edgeMaterial = new LineMaterial({
		color: edgeColor,
		linewidth: 3
	});

	const edgePoints = [
		new THREE.Vector3(edge.Start.X, edge.Start.Y, 0),
		new THREE.Vector3(edge.End.X, edge.End.Y, 0)
	];
	const edgeGeometry = new LineGeometry();
	edgeGeometry.setPositions(edgePoints.map(v => v.toArray()).flat());

	const line = new Line2(edgeGeometry, edgeMaterial);
	scene.add(line);
}

function drawScene2D(scene, renderer, shapes) {
	scene.clear();
	doAnimation = false;

	let { centerX, centerY, width, height } = getBounds2D(shapes)
	const aspectRatio = width / height;
	const windowAspectRatio = window.innerWidth / window.innerHeight;
	const adjustedWidth = aspectRatio > windowAspectRatio ? width : height * windowAspectRatio;
	const adjustedHeight = aspectRatio > windowAspectRatio ? width / windowAspectRatio : height;

	let camera = new THREE.OrthographicCamera(
		centerX - adjustedWidth / 2, centerX + adjustedWidth / 2,   // left, right
		centerY + adjustedHeight / 2, centerY - adjustedHeight / 2, // top, bottom
		-1, 1 // near, far
	);

	shapes.forEach(shape => {
		drawShape(shape, scene)

		shape.Edges.forEach(edge => {
			drawShapeEdge(edge, scene)
		})
	});

	renderer.render(scene, camera);
}


// ---------------------------
// 3D Rendering
// ---------------------------
function setLighting() {
	const ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
	const directionalLight = new THREE.DirectionalLight(0xffffff, 1.5);
	directionalLight.position.set(1, 1, 1);
	scene.add(ambientLight);
	scene.add(directionalLight);
}


function drawScene3D(scene, renderer, polyhedron) {
	scene.clear();  
	setLighting()

	let camera = new THREE.PerspectiveCamera(70, width / height, 0.01, 10);
	camera.position.z = 2.5;

	drawPolyhedron(polyhedron, scene)
	const edges = getUniqueEdges(polyhedron)
	drawEdges(edges, scene)

	function animate(time) {
		if (!doAnimation) {
			return;
		}

		scene.children.forEach(mesh => {
			mesh.rotation.x = time / 2000;
			mesh.rotation.y = time / 1000;
		});

		renderer.render(scene, camera);
	}

	renderer.setAnimationLoop(animate);
	doAnimation = true;
}

function rotateTo2D(vertices, normal) {
	const quaternion = new THREE.Quaternion().setFromUnitVectors(normal, new THREE.Vector3(0, 1, 0));
	const rotationMatrix = new THREE.Matrix4().makeRotationFromQuaternion(quaternion);
	const rotatedVertices = vertices.map(v => v.clone().applyMatrix4(rotationMatrix));
	const vertices2D = rotatedVertices.map(v => [v.x, v.z]);
	return vertices2D;
}

function drawPolyhedron(polyhedron, scene) {
	var i = 0;
	polyhedron.Faces.forEach(face => {
		const vertices = face.Vertices.map(v => new THREE.Vector3(v.X, v.Y, v.Z));
		const material = new THREE.MeshPhongMaterial({ color: 0xff0000, side: THREE.DoubleSide });
		const geometry = new THREE.BufferGeometry().setFromPoints(vertices);

		// TODO probably a more elegant way to do this when merging the convex hull faces
		// look for alternate solutions
		const normal = new THREE.Vector3(face.Normal[0], face.Normal[1], face.Normal[2]);
		const vertices2D = rotateTo2D(vertices, normal);
		const indices = triangulate2D(vertices2D); 
		geometry.setIndex(indices);

		const mesh = new THREE.Mesh(geometry, material);
		mesh.geometry.computeVertexNormals(); // TODO might be faster to just use existing normals
		scene.add(mesh);
		i++;
	});
}

function getUniqueEdges(polyhedron) {
	const edges = [];

	polyhedron.Faces.forEach(face => {
		const unsortedVertices = face.Vertices.map(v => new THREE.Vector3(v.X, v.Y, v.Z));
		const normal = new THREE.Vector3(face.Normal[0], face.Normal[1], face.Normal[2]);
		const vertices2D = rotateTo2D(unsortedVertices, normal);
		const sortedIndices = sortVertices2D(vertices2D); 
		const vertices = sortedIndices.map(i => unsortedVertices[i]);

		for (let i = 0; i < vertices.length; i++) {
			const v1 = vertices[i];
			const v2 = vertices[(i + 1) % vertices.length];
			edges.push({ v1, v2 });
		}
	});

	const uniqueEdges = [];
	const edgeMap = new Map();

	edges.forEach(edge => {
		const key1 = `${edge.v1.x},${edge.v1.y},${edge.v1.z}-${edge.v2.x},${edge.v2.y},${edge.v2.z}`;
		const key2 = `${edge.v2.x},${edge.v2.y},${edge.v2.z}-${edge.v1.x},${edge.v1.y},${edge.v1.z}`;
		if (!edgeMap.has(key1) && !edgeMap.has(key2)) {
			uniqueEdges.push(edge);
			edgeMap.set(key1, true);
		}
	});

	return uniqueEdges;
}

function drawEdges(edges, scene) {
	edges.forEach(edge => {
		const path = new THREE.LineCurve3(edge.v1, edge.v2);
		const edgeGeometry = new THREE.TubeGeometry(path, 128, 0.002, 8, false);
		const edgeMaterial = new THREE.MeshBasicMaterial({ color: 0x000000 });
		const edgeMesh = new THREE.Mesh(edgeGeometry, edgeMaterial);
		scene.add(edgeMesh);
	});
}