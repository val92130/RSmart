
var ip = "10.8.110.204";

$("#camera").hide();

$("#ip").html(ip);
$("#canvas_map").hide();


$("#btn_camera").click(function()
{
	if($("#camera").is(":visible"))
	{
		$("#camera").fadeOut("slow");
	}else
	{
		$("#camera").fadeIn("slow");
	}
});


$("#btn_map2d").click(function()
{
	if($("#canvas_map").is(":visible"))
	{
		$("#canvas_map").fadeOut("slow");
	}else
	{
		$("#canvas_map").fadeIn("slow");
	}
});

// check if robot is online
$.get( "http://"+ip+"/", function( data ) {
	if(data)
	{
		$("#status").html('<span style="Color:green">Online</span>');
	}
});

function StartMotors()
{
	$.get( "http://"+ip+"/?Start=true&robot=true", function( data ) {
	});
}

function StopMotors()
{
	$.get( "http://"+ip+"/?Stop=true&robot=true", function( data ) {
	});
}

function TurnLeft()
{
	$.get( "http://"+ip+"?Left=true&robot=true", function( data ) {
	});
}
function TurnRight()
{
	$.get( "http://"+ip+"/?Right=true&robot=true", function( data ) {
	});
}

function GoForward()
{
	$.get( "http://"+ip+"/?Forward=true&robot=true", function( data ) {
	});
}

function GoBackward()
{
	$.get( "http://"+ip+"/?Backward=true&robot=true", function( data ) {
	});
}

function GetDirection()
{
	$.get( "http://"+ip+"/?GetDirection=true&robot=true", function( data ) {
		$("#direction").html('<span style="Color:blue">'+data+'</span>');
	});
}

function GetMotorLeftStatus()
{
	$.get( "http://"+ip+"/?GetMotorStatusLeft=true&robot=true", function( data ) {
		$("#motorLeftStatus").html('<span style="Color:blue">'+data+'</span>');
		return data;
	});
}

function GetMotorRightStatus()
{
	$.get( "http://"+ip+"/?GetMotorStatusRight=true&robot=true", function( data ) {
		$("#motorRightStatus").html('<span style="Color:blue">'+data+'</span>');
		return data;
	});
}

$("#start_motors").click(function()
{
	StartMotors();
});


$("#stop_motors").click(function()
{
	StopMotors();
});

$("#turn_left").click(function()
{
	TurnLeft();
});

$("#turn_right").click(function()
{
	TurnRight();
});

$("#go_forward").click(function()
{
	GoForward();
});

$("#go_backward").click(function()
{
	GoBackward();
});

$("#send_speed").click(function()
{
	var value = parseInt($("#speed_input").val(), 10);
	$.get( "http://"+ip+"/?Speed="+value+"&robot=true", function( data ) {
	});

});

setInterval(function() {
	$("#img").attr("src", "https://www.tradeit.fr/Webcam/image_upload/img.jpg?"+new Date().getTime());
	$('#img').html('<img src="https://www.tradeit.fr/Webcam/image_upload/img.jpg" id="img" class="img_centered" width=500 height=500></img>');
	
}, 500);

setInterval(function() {
	$.get( "http://"+ip+"/", function( data ) {
		if(data)
		{
			$("#status").html('<span style="Color:green">Online</span>');
		}
	});

	GetDirection();
	GetMotorRightStatus();
	GetMotorLeftStatus()
}, 15000);


$( document ).keypress(function( event ) {
	var key = event.keyCode;
	console.log(key);
	switch(key)
	{
		case 122:
		GoForward();
		break;
		case 115:
		GoBackward();
		break;
		case 113:
		TurnLeft();
		break;
		case 100:
		TurnRight();
		break;
	}

});