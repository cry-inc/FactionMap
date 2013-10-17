$(document).ready( function(){ loadMapData(); } )

var baseCanvas;
var baseCtx;
var selCanvas;
var selCtx;
var mapData;
var factionData;

function loadMapData()
{
	baseCanvas = document.getElementById("baselayer");
	baseCtx = baseCanvas.getContext("2d");
	selCanvas = document.getElementById("selectionlayer");
	selCtx = selCanvas.getContext("2d");
	baseCanvas.width = selCanvas.width = $("#drawstack").width();
	baseCanvas.height = selCanvas.height = $("#drawstack").height();
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
		drawBaseMap();
		$("#drawstack").mousemove(onMouseOverMap);
	});
}

function showInfoBox(visible, x, y, provinceId)
{
	if (visible) {
		$("#infobox").css({display: "block", left: x + 5, top: y + 5 });
		var faction = "-";
		if (mapData.provinces[provinceId].faction != -1)
			faction = factionData.factions[mapData.provinces[provinceId].faction].name;
		var msg = "Province Id: " + provinceId + "<br />";
		msg += "Faction: " + faction + "<br />";
		msg += "Name: -<br />";
		msg += "Region: -<br />";
		$("#infobox").html(msg);
	} else
		$("#infobox").css({display: "none"});
}

function highlightProvince(provinceId)
{
	selCtx.clearRect(0, 0, selCanvas.width, selCanvas.height);
	if (provinceId != -1) {
		var province = mapData.provinces[provinceId];
		selCtx.strokeStyle = "orange";
		selCtx.lineWidth = 3;
		var x = province.points[0].x * selCanvas.width;
		var y = province.points[0].y * selCanvas.height;
		selCtx.beginPath();
		selCtx.moveTo(x, y);
		for (var i = 1; i < province.points.length; i++) {
			x = province.points[i].x * selCanvas.width;
			y = province.points[i].y * selCanvas.height;
			selCtx.lineTo(x, y);
		}
		selCtx.closePath();
		selCtx.stroke();
	}
}

function onMouseOverMap(event)
{
	var ofs = $("#drawstack").offset();
	var rx = event.pageX - ofs.left;
	var ry = event.pageY - ofs.top;
	rx /= baseCanvas.width;
	ry /= baseCanvas.height;
	var point = {x: rx, y: ry};
	var found = -1;
	for (var i = 0; i < mapData.provinces.length; i++) {
		if (pointInProvince(point, mapData.provinces[i])) {
			found = mapData.provinces[i].id;
			break;
		}
	}
	showInfoBox((found != -1), event.pageX, event.pageY, found);
	highlightProvince(found);
}

function drawBaseMap()
{
	baseCtx.lineWidth = 0.5;
	baseCtx.strokeStyle = "black";
	$.each(mapData.provinces, function(k, province) {
		var x = province.points[0].x * baseCanvas.width;
		var y = province.points[0].y * baseCanvas.height;
		baseCtx.beginPath();
		baseCtx.moveTo(x, y);
		for (var i = 1; i < province.points.length; i++) {
			x = province.points[i].x * baseCanvas.width;
			y = province.points[i].y * baseCanvas.height;
			baseCtx.lineTo(x, y);
		}
		baseCtx.closePath();
		if (province.faction != -1) {
			baseCtx.fillStyle = "rgba(" + factionData.factions[province.faction].color + ", 0.5)";
			baseCtx.fill();
		}
		baseCtx.stroke();
	});
}

function pointInProvince(pt, province)
{
	var c = false;
	if (pt.x >= province.xmin && pt.x <= province.xmax && pt.y >= province.ymin && pt.y <= province.ymax) {
		var poly = province.points;
		for (var i = -1, l = poly.length, j = l - 1; ++i < l; j = i)
			((poly[i].y <= pt.y && pt.y < poly[j].y) || (poly[j].y <= pt.y && pt.y < poly[i].y))
				&& (pt.x < (poly[j].x - poly[i].x) * (pt.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)
				&& (c = !c);
	}
	return c;
}

function preprocessMapData()
{
	for (var i = 0; i < mapData.provinces.length; i++) {
		mapData.provinces[i].faction = -1;
		buildPoints(mapData.provinces[i]);
		findBoundingBox(mapData.provinces[i]);
	}
}

function preprocessFactionData()
{
	for (var i = 0; i < factionData.factions.length; i++) {
		for (var j = 0; j < factionData.factions[i].provinces.length; j++) {
			var pid = factionData.factions[i].provinces[j];
			mapData.provinces[pid].faction = i;
		}
	}
}

function buildPoints(p)
{
	var points = [];
	for (var i=0; i<p.edges.length; i++) {
		for (var j=0; j<p.edges[i].xpoints.length && j<p.edges[i].ypoints.length; j++) {
			points.push({x: p.edges[i].xpoints[j], y: p.edges[i].ypoints[j]});
		}
	}
	p.points = points;
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

function findCenter(p)
{
	p.center = {x: 0, y: 0};
}
