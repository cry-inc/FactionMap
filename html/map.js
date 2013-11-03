var baseCanvas, baseCtx, selCanvas, selCtx;
var provinces, factions, map, regions = [];

$(document).ready(startup);

function startup()
{
	setupCanvas();
	$("#maplink").click(showMap);
	$("#pointslink").click(showPoints);
	$.when(
		$.getJSON("provinces.json"),
		$.getJSON("factions.json"),
		$.getJSON("map.json")
	).done(finishedLoading);
}

function setupCanvas()
{
	baseCanvas = document.getElementById("baselayer");
	baseCtx = baseCanvas.getContext("2d");
	selCanvas = document.getElementById("selectionlayer");
	selCtx = selCanvas.getContext("2d");
	baseCanvas.width = selCanvas.width = $("#drawstack").width();
	baseCanvas.height = selCanvas.height = $("#drawstack").height();
}

function finishedLoading(provincesResult, factionsResult, mapResults)
{
	provinces = provincesResult[0].provinces;
	factions = factionsResult[0].factions;
	map = mapResults[0].map;
	preprocessProvinces();
	preprocessFactions();
	drawBaseMap();
	createPointsTable();
	hookEvents();
}

function hookEvents()
{
	$("#drawstack").mousemove(onMouseOverMap);
	$("#sortname").click(function(){createPointsTable("name")});
	$("#sortpoints").click(function(){createPointsTable("-points")});
	$("#sortprovinces").click(function(){createPointsTable("-provinces")});
	$("#sortregions").click(function(){createPointsTable("-regions")});
	$("#sortvassals").click(function(){createPointsTable("-vassals")});
	$("#sortarea").click(function(){createPointsTable("-area")});
}

function showInfoBox(visible, screenx, screeny, provinceId)
{
	if (visible) {
		$("#infobox").css({display: "block", left: screenx + 5, top: screeny + 5 });
		var province = provinces[provinceId];
		var msg = "Id: " + provinceId + ", Area: " + areaToString(province.area) + "<br />";
		if (province.faction != -1) {
			var faction = factions[province.faction];
			msg += "Name: " + province.name + "<br />";
			msg += "Faction: " + faction.name + "<br />";
			if (province.heartland)
				msg += "Heartland!<br />";
			if (province.region != -1)
				msg += "Region: " + province.regionname + "<br />";
			if (faction.vassalof != -1)
				msg += "Vassals of: " + factions[faction.vassalof].name + "<br />";
			if (typeof faction.image !== "undefined")
				msg += "<img class=\"fimginfo\" src=\"" + faction.image + "\" /><br />";
		}
		$("#infobox").html(msg);
	} else
		$("#infobox").css({display: "none"});
}

function highlightProvince(provinceId)
{
	selCtx.clearRect(0, 0, selCanvas.width, selCanvas.height);
	if (provinceId != -1) {
		var province = provinces[provinceId];
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
	for (var i = 0; i < provinces.length; i++) {
		if (pointInProvince(point, provinces[i])) {
			found = provinces[i].id;
			break;
		}
	}
	showInfoBox((found != -1), event.pageX, event.pageY, found);
	highlightProvince(found);
	showCoords(point);
}

function showCoords(point)
{
	var xd = map.bottomrightx - map.topleftx;
	var yd = map.toplefty - map.bottomrighty;
	var x = map.topleftx + point.x * xd;
	var y = map.toplefty - point.y * yd;
	$("#coords").html(Math.round(x) + "|" + Math.round(y));
}

function drawBaseMap()
{
	drawProvinces();
	drawRegionBorders();
	drawFactionBorders();
}

function drawProvinces()
{
	baseCtx.lineWidth = 0.5;
	baseCtx.strokeStyle = "black";
	$.each(provinces, function(k, province) {
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
			var opacity = "0.4";
			if (province.heartland) opacity = "0.5";
			baseCtx.fillStyle = "rgba(" + factions[province.faction].color + ", " + opacity + ")";
			baseCtx.fill();
			/*
			var vassalof = factions[province.faction].vassalof;
			if (vassalof != -1) {
				var img = document.createElement('canvas');
				img.width = 10; img.height = 10;
				var imgctx = img.getContext("2d");
				imgctx.fillStyle = "rgba(" + factions[vassalof].color + ", 0.2)";
				imgctx.fillRect(0, 0, 5, 2);
				var pattern = baseCtx.createPattern(img, "repeat");
				baseCtx.fillStyle = pattern;
				baseCtx.fill();
			}
			*/
		}
		baseCtx.stroke();
	});
}

function drawRegionBorders()
{
	baseCtx.lineWidth = 2;
	baseCtx.strokeStyle = "black";
	for (var r = 0; r < regions.length; r++) {
		var region = regions[r];
		for (var e = 0; e < region.edges.length; e++) {
			var edge = region.edges[e];
			var x = edge.points[0].x * baseCanvas.width;
			var y = edge.points[0].y * baseCanvas.height;
			baseCtx.beginPath();
			baseCtx.moveTo(x, y);
			for (var p=1; p<edge.points.length; p++) {
				x = edge.points[p].x * baseCanvas.width;
				y = edge.points[p].y * baseCanvas.height;
				baseCtx.lineTo(x, y);
			}
			baseCtx.stroke();
		}
	}
}

function drawFactionBorders()
{
	baseCtx.lineWidth = 3;
	for (var f = 0; f < factions.length; f++) {
		var faction = factions[f];
		var factionId = f;
		if (faction.vassalof != -1) {
			factionId = faction.vassalof
			var color = factions[factionId].color;
			baseCtx.strokeStyle = "rgba(" + color + ", 0.5)";
			for (var e = 0; e < faction.edges.length; e++) {
				var edge = faction.edges[e];
				var x = edge.points[0].x * baseCanvas.width;
				var y = edge.points[0].y * baseCanvas.height;
				baseCtx.beginPath();
				baseCtx.moveTo(x, y);
				for (var p=1; p<edge.points.length; p++) {
					x = edge.points[p].x * baseCanvas.width;
					y = edge.points[p].y * baseCanvas.height;
					baseCtx.lineTo(x, y);
				}
				baseCtx.stroke();
			}
		}
	}
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

function provinceArea(province)
{
	var a1 = 0, a2 = 0;
	for (var p=0; p<province.points.length-1; p++) {
		a1 += province.points[p].x * province.points[p+1].y;
		a2 += province.points[p].y * province.points[p+1].x;
	}
	a1 += province.points[province.points.length-1].x * province.points[0].y;
	a2 += province.points[province.points.length-1].y * province.points[0].x;
	province.area = (a1 - a2) / 2;
}

function preprocessProvinces()
{
	for (var i = 0; i < provinces.length; i++) {
		
		// Set faction and regions of provinces to neutral
		provinces[i].faction = -1;
		provinces[i].region = -1;
		provinces[i].regionname = "";
		
		// Merge edges to one large polygon
		buildPoints(provinces[i]);
		
		// Find bounding box of provinces
		findBoundingBox(provinces[i]);
		
		// Calculate area of province
		provinceArea(provinces[i]);
	}
}

function preprocessFactions()
{
	// Mark provinces with name + faction info and extract regions
	for (var f = 0; f < factions.length; f++) {
		factions[f].vassalof = -1;
		factions[f].edges = [];
		for (var p = 0; p < factions[f].provinces.length; p++) {
			var pid = factions[f].provinces[p].id;
			provinces[pid].name = factions[f].provinces[p].name;
			provinces[pid].faction = f;
		}
		if (typeof factions[f].regions === "undefined") {
			factions[f].regions = [];
		}
		for (var r = 0; r < factions[f].regions.length; r++) {
			factions[f].regions[r].faction = f;
			regions.push(factions[f].regions[r]);
			for (var p = 0; p < factions[f].regions[r].provinces.length; p++) {
				var pid = factions[f].regions[r].provinces[p];
				provinces[pid].region = regions.length - 1;
				provinces[pid].regionname = factions[f].regions[r].name;
			}
		}
	}
	
	// Replace vassal id with correct number and set faction.vassalof
	for (var f = 0; f < factions.length; f++) {
		if (typeof factions[f].vassals === "undefined") {
			factions[f].vassals = [];
		}
		for (var v = 0; v < factions[f].vassals.length; v++) {
			var vshort = factions[f].vassals[v];
			var vid = findFactionId(vshort);
			factions[f].vassals[v] = vid;
			factions[vid].vassalof = f;
		}
	}
	
	// Check provinces for heartland
	for (var i = 0; i < provinces.length; i++) {
		provinces[i].heartland = false;
		if (provinces[i].faction != -1) {
			provinces[i].heartland = true;
			for (var j = 0; j < provinces[i].edges.length; j++) {
				var neighborId = provinces[i].edges[j].neighbor;
				if (provinces[neighborId].faction == -1 ||
					provinces[neighborId].faction != provinces[i].faction) {
					provinces[i].heartland = false;
					break;
				}
			}
		}
	}
	
	// Find borders of all regions for drawing
	for (var r = 0; r < regions.length; r++) {
		regions[r].edges = [];
		for (var p = 0; p < regions[r].provinces.length; p++) {
			var pid = regions[r].provinces[p];
			var edges = provinces[pid].edges;
			for (var e = 0; e < edges.length; e++) {
				if (!isInList(edges[e].neighbor, regions[r].provinces)) {
					regions[r].edges.push(edges[e]);
				}
			}
		}
	}
	
	// Find borders of all factions for drawing
	for (var p = 0; p < provinces.length; p++) {
		var province = provinces[p];
		if (province.faction != -1) {
			var faction = province.faction;
			for (var e = 0; e < province.edges.length; e++) {
				var edge = province.edges[e];
				if (edge.neighbor == -1) {
					factions[faction].edges.push(edge);
				} else {
					var nf = provinces[edge.neighbor].faction;
					if (nf != faction) {
						factions[faction].edges.push(edge);
					}
				}
			}
		}
	}
	
	// Calculate points and areas of factions
	for (var f = 0; f < factions.length; f++) {
		calculatePointsAndArea(f);
	}
}

function isInList(item, list)
{
	for (var i=0; i<list.length; i++) {
		if (item == list[i])
			return true;
	}
	return false;
}

function buildPoints(p)
{
	p.points = [];
	for (var i=0; i<p.edges.length; i++) {
		p.edges[i].points = [];
		for (var j=0; j<p.edges[i].xpoints.length && j<p.edges[i].ypoints.length; j++) {
			var point = {x: p.edges[i].xpoints[j], y: p.edges[i].ypoints[j]};
			p.points.push(point);
			p.edges[i].points.push(point);
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

function findFactionId(shortname)
{
	for (var f = 0; f < factions.length; f++) {
		if (shortname == factions[f].id) {
			return f;
		}
	}
	return -1;
}

function calculatePointsAndArea(factionid)
{
	var faction = factions[factionid];
	faction.area = 0;
	faction.points = 0;
	faction.points += faction.provinces.length * 10;
	faction.points += faction.regions.length * 50;
	for (var p=0; p<faction.provinces.length; p++) {
		var pid = faction.provinces[p].id;
		faction.area += provinces[pid].area;
		if (provinces[pid].heartland)
			faction.points += 5;
	}
	for (var v=0; v<faction.vassals.length; v++) {
		var vid = faction.vassals[v];
		faction.points += 5 * factions[vid].provinces.length;
	}
}
function areaToString(area)
{
	return (Math.round(area * 10000) / 100) + "%";
}

function createTableObject(factionid)
{
	var faction = factions[factionid];
	return {
		name: faction.name,
		points: faction.points,
		provinces: faction.provinces.length,
		regions: faction.regions.length,
		vassals: faction.vassals.length,
		area: faction.area,
		image: faction.image
	};
}

function generateHtmlRow(fo)
{
	var image = "&nbsp;";
	if (typeof fo.image !== "undefined") {
		image = "<img class=\"fimgtable\" src=\"" + fo.image + "\" />";
	}

	var html = "<tr>";
	html += "<td>" + image + "</td>";
	html += "<td>" + fo.name + "</td>";
	html += "<td>" + fo.points + "</td>";
	html += "<td>" + fo.provinces + "</td>";
	html += "<td>" + fo.regions + "</td>";
	html += "<td>" + fo.vassals + "</td>";
	html += "<td>" + areaToString(fo.area) + "</td>";
	return html + "</tr>";
}

function createPointsTable(orderCriteria)
{
	if (typeof orderCriteria === "undefined") {
		orderCriteria = "-points";
	}
	
	var factionObjects = [];
	for (var f=0; f<factions.length; f++) {
		var factionObject = createTableObject(f);
		factionObjects.push(factionObject);
	}
	
	factionObjects.sort(dynamicSort(orderCriteria));
	
	var html = "";
	for (var f=0; f<factionObjects.length; f++)
		html += generateHtmlRow(factionObjects[f]);
	$("#tablebody").html(html);
}

function dynamicSort(property)
{
    var sortOrder = 1;
    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }
    return function (a, b) {
        var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
        return result * sortOrder;
    }
}

function showMap()
{
	$("#drawstack").css({display: "block"});
	$("#points").css({display: "none"});
}

function showPoints()
{
	$("#drawstack").css({display: "none"});
	$("#points").css({display: "block"});
}
