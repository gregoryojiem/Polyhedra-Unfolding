import * as THREE from "../node_modules/three/build/three.module.js"

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
		// add the shape to the scene 
		const vertices = shape.Vertices.map(v => new THREE.Vector3(v.X, v.Y, 0)); 

		const shapeGeometry = new THREE.BufferGeometry();
		shapeGeometry.setFromPoints(vertices);
		const shapeMaterial = new THREE.MeshBasicMaterial({
			color: new THREE.Color(shape.Color[0], shape.Color[1], shape.Color[2]),
			side: THREE.DoubleSide,
			transparent: shape.Color[3] < 1,
			opacity: shape.Color[3]
		});
		const shapeMesh = new THREE.Mesh(shapeGeometry, shapeMaterial);

		if (vertices.length > 3) {
			const indices = [];
			for (let i = 1; i < vertices.length - 1; i++) {
				indices.push(0, i, i + 1);
			}
			shapeGeometry.setIndex(indices);
		}

		scene.add(shapeMesh);

		// add a shape outline to the scene
		const outlineMaterial = new THREE.LineBasicMaterial({ color: 0xffffff });
		const outlinePoints = vertices.slice();
		outlinePoints.push(outlinePoints[0]);
		const outlineGeometry = new THREE.BufferGeometry().setFromPoints(outlinePoints);
		const outline = new THREE.Line(outlineGeometry, outlineMaterial);
		scene.add(outline);
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
	camera.position.z = 2;

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

function drawPolyhedron(polyhedron, scene) {
	polyhedron.Faces.forEach(face => {
		const vertices = face.Vertices.map(v => new THREE.Vector3(v.X, v.Y, v.Z));
		const geometry = new THREE.BufferGeometry().setFromPoints(vertices);
		const material = new THREE.MeshPhongMaterial({ color: 0xff0000, side: THREE.DoubleSide });
		const mesh = new THREE.Mesh(geometry, material);

		const indices = [];
		for (let i = 1; i < vertices.length - 1; i++) {
			indices.push(0, i, i + 1);
		}
		geometry.setIndex(indices);

		if (face.Normal) {
			mesh.geometry.computeVertexNormals();
		}

		scene.add(mesh);
	});
}

function getUniqueEdges(polyhedron) {
	const edges = [];

	polyhedron.Faces.forEach(face => {
		const vertices = face.Vertices.map(v => new THREE.Vector3(v.X, v.Y, v.Z));

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