
var ip = "10.8.110.204";


$.get( "http://"+ip+"/", function( data ) {
	if(data)
	{
		$("#status").html('<span style="Color:green">Offline</span>');
	}
});

$("#start_motors").click(function()
{
	$.get( "http://"+ip+"/?Start=true&robot=true", function( data ) {
	});
});

$("#stop_motors").click(function()
{
	$.get( "http://"+ip+"/?Stop=true&robot=true", function( data ) {
	});
});

$("#turn_left").click(function()
{
	$.get( "http://"+ip+"?Left=true&robot=true", function( data ) {
	});
});

$("#turn_right").click(function()
{
	$.get( "http://"+ip+"/?Right=true&robot=true", function( data ) {
	});
});

$("#go_forward").click(function()
{
	$.get( "http://"+ip+"/?Forward=true&robot=true", function( data ) {
	});
});

$("#go_backward").click(function()
{
	$.get( "http://"+ip+"/?Backward=true&robot=true", function( data ) {
	});
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
			$("#status").html('<span style="Color:green">Offline</span>');
		}
	});

}, 10000);


$( "body" ).keypress(function( event ) {
	var key = event.keyCode;
	console.log(key);
	switch(key)
	{
		case 122:
		$.get( "http://"+ip+"/?Forward=true&robot=true", function( data ) {
		});
		break;
		case 115:
		$.get( "http://"+ip+"/?Backward=true&robot=true", function( data ) {
		});
		break;
		case 113:
		$.get( "http://"+ip+"/?Left=true&robot=true", function( data ) {
		});
		break;
		case 100:
		$.get( "http://"+ip+"/?Right=true&robot=true", function( data ) {
		});
		break;
	}

});