import * as THREE from "../node_modules/three/build/three.module.js"

window.handleRenderingInit = function (jsonArray) {
	const polyhedron = JSON.parse(jsonArray);
	console.log("parsed array")
	console.log(polyhedron);
	renderingInit(polyhedron)
}

function renderingInit(polyhedron) {
	const width = window.innerWidth, height = window.innerHeight;

	// init
	const camera = new THREE.PerspectiveCamera(70, width / height, 0.01, 10);
	camera.position.z = 1;
	
	const scene = new THREE.Scene();

	const ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
	scene.add(ambientLight);
	const directionalLight = new THREE.DirectionalLight(0xffffff, 1.5);
	directionalLight.position.set(1, 1, 1);
	scene.add(directionalLight);

	drawPolyhedron(polyhedron, scene)
	const edges = getUniqueEdges(polyhedron)
	drawEdges(edges, scene)

	const renderer = new THREE.WebGLRenderer({ antialias: true });
	renderer.setSize(width, height);
	renderer.setAnimationLoop(animate);
	document.body.appendChild(renderer.domElement);

	function animate(time) {
		scene.children.forEach(mesh => {
			mesh.rotation.x = time / 2000;
			mesh.rotation.y = time / 1000;
		});

		renderer.render(scene, camera);

	}
}

function drawPolyhedron(polyhedron, scene) {
	polyhedron.Faces.forEach(face => {
		const vertices = face.Vertices.map(v => new THREE.Vector3(v.X, v.Y, v.Z));
		const geometry = new THREE.BufferGeometry().setFromPoints(vertices);
		const material = new THREE.MeshPhongMaterial({ color: 0xff0000, side: THREE.DoubleSide });
		const mesh = new THREE.Mesh(geometry, material);

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