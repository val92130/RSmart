

$("#start_motors").click(function()
{
	$.get( "http://10.8.107.217:6268/?Start=true&robot=true", function( data ) {
	});
});

$("#stop_motors").click(function()
{
	$.get( "http://10.8.107.217:6268/?Stop=true&robot=true", function( data ) {
	});
});

$("#turn_left").click(function()
{
	$.get( "http://10.8.107.217:6268/?Left=true&robot=true", function( data ) {
	});
});

$("#turn_right").click(function()
{
	$.get( "http://10.8.107.217:6268/?Right=true&robot=true", function( data ) {
	});
});

$("#go_forward").click(function()
{
	$.get( "http://10.8.107.217:6268/?Forward=true&robot=true", function( data ) {
	});
});

$("#go_backward").click(function()
{
	$.get( "http://10.8.107.217:6268/?Backward=true&robot=true", function( data ) {
	});
});

$("#send_speed").click(function()
{
	var value = parseInt($("#speed_input").val(), 10);
	$.get( "http://10.8.107.217:6268/?Speed="+value+"&robot=true", function( data ) {
	});

});

setInterval(function() {
	$("#img").attr("src", "https://www.tradeit.fr/Webcam/image_upload/img.jpg?"+new Date().getTime());
	$('#img').html('<img src="https://www.tradeit.fr/Webcam/image_upload/img.jpg" id="img" class="img_centered" width=500 height=500></img>');
}, 500);


$( "body" ).keypress(function( event ) {
	var key = event.keyCode;
	console.log(key);
	switch(key)
	{
		case 122:
			$.get( "http://10.8.107.217:6268/?Forward=true&robot=true", function( data ) {
	});
			break;
		case 115:
			$.get( "http://10.8.107.217:6268/?Backward=true&robot=true", function( data ) {
	});
			break;
		case 113:
			$.get( "http://10.8.107.217:6268/?Left=true&robot=true", function( data ) {
	});
			break;
		case 100:
			$.get( "http://10.8.107.217:6268/?Right=true&robot=true", function( data ) {
	});
			break;
	}

});