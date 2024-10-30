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

	polyhedron.Faces.forEach(face => {
		const vertices = face.Vertices.map(v => new THREE.Vector3(v.X, v.Y, v.Z));
		const geometry = new THREE.BufferGeometry().setFromPoints(vertices);
		const material = new THREE.MeshBasicMaterial({ color: 0xff0000, side: THREE.DoubleSide });
		const mesh = new THREE.Mesh(geometry, material);
		scene.add(mesh);
	});

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