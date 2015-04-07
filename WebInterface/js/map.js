

var posX = 0;
var posY = 0;

var canvas = document.getElementById("map");

var bw = canvas.width;
var bh = canvas.height;
var p = 0;


var context = canvas.getContext("2d");
function drawBoard(){
for (var x = 0; x <= bw; x += 10) {
    context.moveTo(0.5 + x + p, p);
    context.lineTo(0.5 + x + p, bh + p);
}


for (var x = 0; x <= bh; x += 10) {
    context.moveTo(p, 0.5 + x + p);
    context.lineTo(bw + p, 0.5 + x + p);
}

context.strokeStyle = "black";
context.stroke();
}

drawBoard();
$("#pos").html("X : " + posX + " Y : " + posY);
setInterval(function() {


$.get( "http://"+ip+"/?GetPositionX=true&robot=true", function( data ) {
		posX = data;
	});

$.get( "http://"+ip+"/?GetPositionY=true&robot=true", function( data ) {
		posY = data;
	});

	context.fillStyle = "#FF0000";

	context.clearRect ( 0 , 0 , canvas.width, canvas.height );

	context.fillRect(posX,posY,10,10);
	drawBoard();

	$("#pos").html("X : " + posX + " Y : " + posY);

}, 2500);