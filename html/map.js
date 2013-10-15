$(document).ready( function(){ loadMapData(); } )

var mapData;
var factionData;

function loadMapData()
{
	
	$.getJSON("map.json", function(data) {
		mapData = data;
		loadFactionData();
	});
	
}

function loadFactionData()
{
	$.getJSON("factions.json", function(data) {
		factionData = data;
		drawBorders();
	});
}

function drawBorders()
{
	var canvas = document.getElementById("map");
	var ctx = canvas.getContext("2d");
	
	ctx.lineWidth = 0.5;
	ctx.strokeStyle = "rgb(0, 0, 0)";
	$.each(mapData.polygons, function(k, poly) {
		ctx.beginPath();
		ctx.moveTo(
			poly.points[0].x * canvas.width,
			poly.points[0].y * canvas.height
		);
		for (var i = 1; i < poly.points.length; i++) {
			ctx.lineTo(
				poly.points[i].x * canvas.width,
				poly.points[i].y * canvas.height
			);
		}
		ctx.closePath();
		ctx.stroke();
	});
}