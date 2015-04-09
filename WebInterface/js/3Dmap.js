
var container;
var camera, scene, renderer;

var robotX = 0;
var robotY = 0;

var robot;



init();
animate();

function init() {

  container = document.createElement( 'div' );
  document.body.appendChild( container );

  var info = document.createElement( 'div' );
  info.style.position = 'absolute';
  info.style.top = '10px';
  info.style.width = '500px';
  info.style.textAlign = 'center';
  container.appendChild( info );

  camera = new THREE.PerspectiveCamera( 45, window.innerWidth / window.innerHeight, 1, 1000 );
  camera.position.x = 200;
  camera.position.y = 100;
  camera.position.z = 200;

  scene = new THREE.Scene();

        // Grid

        var size = 500, step = 10;

        var geometry = new THREE.Geometry();

        for ( var i = - size; i <= size; i += step ) {

          geometry.vertices.push( new THREE.Vector3( - size, 0, i ) );
          geometry.vertices.push( new THREE.Vector3(   size, 0, i ) );

          geometry.vertices.push( new THREE.Vector3( i, 0, - size ) );
          geometry.vertices.push( new THREE.Vector3( i, 0,   size ) );

        }

        var material = new THREE.LineBasicMaterial( { color: 0x000000, opacity: 0.2 } );

        var line = new THREE.Line( geometry, material, THREE.LinePieces );
        scene.add( line );

        // Cubes

        var geometry = new THREE.BoxGeometry( 2, 2, 2 );
        var material = new THREE.MeshLambertMaterial( { color: 0xffffff, shading: THREE.FlatShading, overdraw: 0.5 } );

        robot = new THREE.Mesh( geometry, material );

        robot.scale.y = 1;

        robot.position.x = robotX;
        robot.position.y = robotY;
        robot.position.z = 1;

        scene.add( robot );

        // Lights

        var ambientLight = new THREE.AmbientLight( Math.random() * 0x10 );
        scene.add( ambientLight );

        var directionalLight = new THREE.DirectionalLight( Math.random() * 0xffffff );
        directionalLight.position.x = Math.random() - 0.5;
        directionalLight.position.y = Math.random() - 0.5;
        directionalLight.position.z = Math.random() - 0.5;
        directionalLight.position.normalize();
        scene.add( directionalLight );

        var directionalLight = new THREE.DirectionalLight( Math.random() * 0xffffff );
        directionalLight.position.x = Math.random() - 0.5;
        directionalLight.position.y = Math.random() - 0.5;
        directionalLight.position.z = Math.random() - 0.5;
        directionalLight.position.normalize();
        scene.add( directionalLight );

        renderer = new THREE.CanvasRenderer();
        renderer.setClearColor( 0xf0f0f0 );
        renderer.setPixelRatio( window.devicePixelRatio );
        renderer.setSize( window.innerWidth, window.innerHeight );
        container.appendChild( renderer.domElement );

        window.addEventListener( 'resize', onWindowResize, false );

      }

      function onWindowResize() {

        camera.left = window.innerWidth / - 2;
        camera.right = window.innerWidth / 2;
        camera.top = window.innerHeight / 2;
        camera.bottom = window.innerHeight / - 2;

        camera.updateProjectionMatrix();

        renderer.setSize( window.innerWidth, window.innerHeight );
      }

      function animate() {

        requestAnimationFrame( animate );

        render();

      }

      function render() {

        var timer = Date.now() * 0.0001;

        camera.position.x = Math.cos( timer ) * 200;
        //camera.position.z = robot.position.z;
        camera.position.y = 200;
        camera.lookAt( scene.position );

        robot.position.x = robotX;
        robot.position.z = robotY;

        renderer.render( scene, camera );

      }

      setInterval(function() {

        $.get( "http://"+ip+"/?GetPositionX=true&robot=true", function( data ) {
          robotX = data;
        });

        $.get( "http://"+ip+"/?GetPositionY=true&robot=true", function( data ) {
          robotY = data;
        });

      }, 2500);