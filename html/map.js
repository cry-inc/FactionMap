$(document).ready( function(){ loadMapData(); } )

var canvas;
var ctx;
var mapData;
var factionData;

function loadMapData()
{
	canvas = document.getElementById("map");
	ctx = canvas.getContext("2d");
	canvas.width = $(canvas).width();
	canvas.height = $(canvas).height();
	$.getJSON("map.json", function(data) {
		mapData = data;
		preprocessMapData();
		loadFactionData();
	});
	
}

function loadFactionData()
{
	$.getJSON("factions.json", function(data) {
		factionData = data;
		preprocessFactionData();
		drawBorders();
		$("#map").mousemove(onMouseOverMap);
	});
}

function showInfoBox(visible, x, y, provinceId)
{
	if (visible) {
		$("#infobox").css({display: "block", left: x + 5, top: y + 5 });
		var faction = "-";
		if (mapData.polygons[provinceId].faction != -1)
			faction = factionData.factions[mapData.polygons[provinceId].faction].name;
		var msg = "Province number: " + provinceId + "<br />";
		msg += "Faction: " + faction + "<br />";
		msg += "Name: -<br />";
		msg += "Region: -<br />";
		$("#infobox").html(msg);
	} else
		$("#infobox").css({display: "none"});
}

function onMouseOverMap(event)
{
	var ofs = $(canvas).offset();
	var rx = event.pageX - ofs.left;
	var ry = event.pageY - ofs.top;
	rx /= canvas.width;
	ry /= canvas.height;
	var point = {x: rx, y: ry};
	var found = -1;
	for (var i = 0; i < mapData.polygons.length; i++) {
		if (pointInPolygon(point, mapData.polygons[i])) {
			found = mapData.polygons[i].id;
			break;
		}
	}
	showInfoBox((found != -1), event.pageX, event.pageY, found);
}

function drawBorders()
{
	ctx.lineWidth = 0.5;
	ctx.strokeStyle = "black";
	$.each(mapData.polygons, function(k, poly) {
		var cx = poly.points[0].x * canvas.width;
		var cy = poly.points[0].y * canvas.height;
		ctx.beginPath();
		ctx.moveTo(cx, cy);
		for (var i = 1; i < poly.points.length; i++) {
			var x = poly.points[i].x * canvas.width;
			var y = poly.points[i].y * canvas.height;
			cx += x;
			cy += y;
			ctx.lineTo(x, y);
		}
		ctx.closePath();
		if (poly.faction != -1) {
			ctx.fillStyle = "rgba(" + factionData.factions[poly.faction].color + ", 0.5)";
			ctx.fill();
		}
		ctx.stroke();
		
		cx /= poly.points.length;
		cy /= poly.points.length;
		ctx.fillStyle = "black";
		ctx.fillText(poly.id, cx, cy);
	});
}

function pointInPolygon(pt, polygon)
{
	var c = false;
	if (pt.x >= polygon.xmin && pt.x <= polygon.xmax && pt.y >= polygon.ymin && pt.y <= polygon.ymax) {
		var poly = polygon.points;
		for (var i = -1, l = poly.length, j = l - 1; ++i < l; j = i)
			((poly[i].y <= pt.y && pt.y < poly[j].y) || (poly[j].y <= pt.y && pt.y < poly[i].y))
				&& (pt.x < (poly[j].x - poly[i].x) * (pt.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)
				&& (c = !c);
	}
	return c;
}

function preprocessMapData()
{
	for (var i = 0; i < mapData.polygons.length; i++) {
		mapData.polygons[i].faction = -1;
		findBoundingBox(mapData.polygons[i]);
	}
}

function preprocessFactionData()
{
	for (var i = 0; i < factionData.factions.length; i++) {
		for (var j = 0; j < factionData.factions[i].provinces.length; j++) {
			var pid = factionData.factions[i].provinces[j];
			mapData.polygons[pid].faction = i;
		}
	}
}

function findBoundingBox(p)
{
	p.xmin = p.points[0].x;
	p.ymin = p.points[0].y;
	p.xmax = p.points[0].x;
	p.ymax = p.points[0].y;
	for (var i = 0; i < p.points.length; i++) {
		if (p.points[i].x < p.xmin) p.xmin = p.points[i].x;
		if (p.points[i].y < p.ymin) p.ymin = p.points[i].y;
		if (p.points[i].x > p.xmax) p.xmax = p.points[i].x;
		if (p.points[i].y > p.ymax) p.ymax = p.points[i].y;
	}
}
