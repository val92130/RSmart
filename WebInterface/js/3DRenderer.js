
var container, scene, camera, renderer, controls, stats;
var keyboard = new THREEx.KeyboardState();
var clock = new THREE.Clock();
// custom global variables
var cube;
var ip = "10.8.110.204";
var robot;
var robotX = 0;
var robotY = 0;
var robotLight;
var skyBox;
var spotlight;
var skyBoxBack;

init();
animate();

// FUNCTIONS 		
function init() 
{

	// SCENE
	scene = new THREE.Scene();
	//scene.fog = new THREE.FogExp2( 0x9999ff, 0.00020 );
	// CAMERA
	var SCREEN_WIDTH = window.innerWidth, SCREEN_HEIGHT = window.innerHeight;
	var VIEW_ANGLE = 65, ASPECT = SCREEN_WIDTH / SCREEN_HEIGHT, NEAR = 0.1, FAR = 40000;
	camera = new THREE.PerspectiveCamera( VIEW_ANGLE, ASPECT, NEAR, FAR);
	scene.add(camera);
	camera.position.set(0,150,400);
	camera.lookAt(scene.position);	

	// RENDERER
	if ( Detector.webgl )
		renderer = new THREE.WebGLRenderer( {antialias:true} );
	else
		renderer = new THREE.CanvasRenderer(); 
	renderer.setSize(SCREEN_WIDTH, SCREEN_HEIGHT);
	renderer.shadowMapEnabled = true;

	container = document.getElementById( 'ThreeJS' );
	container.appendChild( renderer.domElement );

	// EVENTS
	THREEx.WindowResize(renderer, camera);
	THREEx.FullScreen.bindKey({ charCode : 'm'.charCodeAt(0) });

	// CONTROLS
	controls = new THREE.OrbitControls( camera, renderer.domElement );

	// LIGHT


// spotlight #1 -- yellow, dark shadow
spotlight = new THREE.SpotLight(0xffff00);
spotlight.position.set(-60,150,-100);
spotlight.shadowDarkness = 0.95;
spotlight.intensity = 2;
	// must enable shadow casting ability for the light
	spotlight.castShadow = true;
	scene.add(spotlight);

	// spotlight #2 -- red, light shadow
	robotLight = new THREE.SpotLight(0xff0000);
	robotLight.position.set(60,150,-60);
	scene.add(robotLight);
	robotLight.shadowDarkness = 0.70;
	robotLight.intensity = 5;
	robotLight.castShadow = true;
	
	// FLOOR

	var size = 500, step = 10;

	var geometry = new THREE.Geometry();

	for ( var i = - size; i <= size; i += step ) {

		geometry.vertices.push( new THREE.Vector3( - size, 0, i ) );
		geometry.vertices.push( new THREE.Vector3(   size, 0, i ) );

		geometry.vertices.push( new THREE.Vector3( i, 0, - size ) );
		geometry.vertices.push( new THREE.Vector3( i, 0,   size ) );

	}


	
	var material = new THREE.LineBasicMaterial( { color: "rgb(38,194,129)", transparent: true, opacity: 0.4 } );

	var line = new THREE.Line( geometry, material, THREE.LinePieces );
	scene.add( line );

	var floorTexture = new THREE.ImageUtils.loadTexture( floortile );
	floorTexture.wrapS = floorTexture.wrapT = THREE.RepeatWrapping; 
	floorTexture.repeat.set( size/10, size/10 );
	var floorMaterial = new THREE.MeshLambertMaterial( { map: floorTexture, side: THREE.DoubleSide } );
	var floorGeometry = new THREE.PlaneGeometry(size*2, size*2, 10, 10);
	var floor = new THREE.Mesh(floorGeometry, floorMaterial);
	floor.position.y = -0.5;
	floor.rotation.x = Math.PI / 2;
	floor.receiveShadow = true;
	scene.add(floor);
	
	////////////
	// CUSTOM //
	////////////
	
	// axes
	var axes = new THREE.AxisHelper(100);
	scene.add( axes );
	
	// skybox creation 
	var skyGeometry = new THREE.SphereGeometry( 8000, 60, 40 );	

	var materialArray = [];
	for (var i = 0; i < 6; i++)
		materialArray.push( new THREE.MeshBasicMaterial({
			map: THREE.ImageUtils.loadTexture( stars ),
			side: THREE.BackSide
		}));

	var skyMaterial = new THREE.MeshFaceMaterial( materialArray );
	skyBox = new THREE.Mesh( skyGeometry, skyMaterial );

	var materialArrayBack = [];
	for (var i = 0; i < 6; i++)
		materialArrayBack.push( new THREE.MeshBasicMaterial({
			map: THREE.ImageUtils.loadTexture( universe ),
			side: THREE.FrontSide
		}));

	var skyMaterialBack = new THREE.MeshFaceMaterial( materialArrayBack );
	skyBoxBack = new THREE.Mesh( skyGeometry, skyMaterialBack );


	scene.add( skyBox );
	scene.add( skyBoxBack );

	// ROBOT INITIALISATION

	var geometry = new THREE.BoxGeometry( 2, 2, 2 );
	var material = new THREE.MeshLambertMaterial( { color: 0xffffff, shading: THREE.FlatShading, overdraw: 0.5 } );

	robot = new THREE.Mesh( geometry, material );

	robot.scale.y = 5;
	robot.scale.x = 5;
	robot.scale.z = 5;

	robot.position.x = robotX;
	robot.position.y = 5;
	robot.position.z = robotY;

	robot.castShadow = true;



	scene.add( robot );
	
}

function animate() 
{
	requestAnimationFrame( animate );
	render();		
	update();
}

function update()
{
	if ( keyboard.pressed("z") ) 
	{ 
		// do something
	}

	robot.position.x = robotX;
	robot.position.z = robotY;
	controls.update();
	skyBox.rotation.y += 0.0002;
	skyBoxBack.rotation.y += 0.0004;

}

function render() 
{
	renderer.render( scene, camera );
}

setInterval(function() {

	$.get( "http://"+ip+"/?GetPositionX=true&robot=true", function( data ) {
		robotX = data;
	});

	$.get( "http://"+ip+"/?GetPositionY=true&robot=true", function( data ) {
		robotY = data;
	});

	$.getJSON( "http://"+ip+"/?GetObstacles=true&robot=true", function( data ) {
		for(var i = 0; i < data.length; i++)
		{
			var geometry = new THREE.BoxGeometry( 2, 2, 2 );
			var material = new THREE.MeshLambertMaterial( { color: 0xffffff, shading: THREE.FlatShading, overdraw: 0.5 } );

			var box = new THREE.Mesh( geometry, material );

			box.scale.y = 1;
			box.scale.x = 3;

			box.position.x = data[i].X;
			box.position.y = 1;
			box.position.z = data[i].Y;;

			scene.add( box );
		}
	});
}, 2500);
