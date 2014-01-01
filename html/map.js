var baseCanvas, baseCtx, selCanvas, selCtx;
var provinces, factions, abandoned, map, regions = [];
var totalImages, loadedImages;

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
	).done(finishedLoadingJson);
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

function finishedLoadingJson(provincesResult, factionsResult, mapResults)
{
	provinces = provincesResult[0].provinces;
	factions = factionsResult[0].factions;
	abandoned = factionsResult[0].abandoned;
	map = mapResults[0].map;
	preprocessProvinceData();
	preprocessFactionData();
	loadFactionImages();
}

function loadedImage()
{
	loadedImages++;
	if (loadedImages > 0 && totalImages == loadedImages) {
		finishedLoadingImages();
	}
}

function loadFactionImages()
{
	totalImages = loadedImages = 0;
	var count = 0;
	for (var f = 0; f < factions.length; f++) {		
		if (typeof factions[f].image !== "undefined") {
			factions[f].imageobject = new Image;
			$(factions[f].imageobject).load(loadedImage);
			factions[f].imageobject.src = factions[f].image;
			count++;
		}
	}
	totalImages = count;
}

function finishedLoadingImages()
{
	drawMap();
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
		var province = provinces[provinceId];
		$("#infobox").css({left: screenx + 5, top: screeny + 5 });
		$("#infoId").html(provinceId);
		$("#infoArea").html(areaToString(province.area));
		$("#infoHeartland").css({display: province.heartland ? "block" : "none"});

		$("#infoIfProvince,#infoIfPrevious,#infoIfContested,#infoIfFaction,#infoIfRegion,#infoIfVassal,#infoImage").css(
			{display: "none"}
		);

		if (province.faction != -1) {
			$("#infoIfProvince").css({display: "block" });
			$("#infoProvince").html(province.name);
		}

		if (province.previousfactions.length > 0) {
			var list = province.previousfactions.map(function(x) { return factions[x].name; });
			$("#infoPrevious").html(list.join(', '));
			$("#infoIfPrevious").css({display: "block" });
		}
		
		
		if (province.contestedby >= 0) {
			$("#infoContested").html(factions[province.contestedby].name);
			$("#infoIfContested").css({display: "block" });
		}

		if (province.faction >= 0) {
			$("#infoFaction").html(province.factionname);
			$("#infoIfFaction").css({display: "block" });
		}
		
		if (province.region >= 0) {
			$("#infoRegion").html(province.regionname);
			$("#infoIfRegion").css({display: "block" });
		}
		
		if (province.faction >= 0) {
			var faction = factions[province.faction];

			if (faction.vassalof >= 0) {
				$("#infoVassal").html(factions[faction.vassalof].name);
				$("#infoIfVassal").css({display: "block" });
			}

			if (typeof faction.image !== "undefined") {
				$("#infoImage").css({"background-image": "url(" + faction.image + ")"});
				$("#infoImage").css({display: "block" });
			}
		}
	}
	$("#infobox").css({display: visible ? "block" : "none"});
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

function drawMap()
{
	drawProvinces();
	drawRegionBorders();
	drawVassalBorders();
	drawPointOfInterests();
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
		if (province.faction >= 0) {
			var opacity = "0.4";
			if (province.heartland) opacity = "0.7";
			baseCtx.fillStyle = "rgba(" + factions[province.faction].color + ", " + opacity + ")";
			baseCtx.fill();
			if (province.contestedby != -1) {
				var img = document.createElement('canvas');
				img.width = 5; img.height = 10;
				var imgctx = img.getContext("2d");
				imgctx.fillStyle = "rgba(" + factions[province.contestedby].color + ", " + opacity + ")";
				imgctx.fillRect(0, 0, 3, 10);
				var pattern = baseCtx.createPattern(img, "repeat");
				baseCtx.fillStyle = pattern;
				baseCtx.fill();
			}
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

function drawVassalBorders()
{
	// buffers for offscreen drawing
	var borderCanvas = document.createElement("canvas");
	var provinceCanvas = document.createElement("canvas");
    borderCanvas.width = provinceCanvas.width = baseCanvas.width;
    borderCanvas.height = provinceCanvas.height = baseCanvas.height;
    var borderCtx = borderCanvas.getContext("2d");
	var provinceCtx = provinceCanvas.getContext("2d");

	borderCtx.lineWidth = 10;
	borderCtx.lineJoin = "round";
	borderCtx.miterLimit = borderCtx.lineWidth;
	
	for (var f = 0; f < factions.length; f++) {
		var faction = factions[f];
		var factionId = f;
		if (faction.vassalof != -1) {
			factionId = faction.vassalof
			var color = factions[factionId].color;

			// draw all faction provinces as filled polygons
			provinceCtx.clearRect(0, 0, provinceCanvas.width, provinceCanvas.height);
			provinceCtx.fillStyle = "white";
			for (var p = 0; p < faction.provinces.length; p++) {
				var province = provinces[faction.provinces[p].id];
				var x = province.points[0].x * provinceCanvas.width;
				var y = province.points[0].y * provinceCanvas.height;
				provinceCtx.beginPath();
				provinceCtx.moveTo(x, y);
				for (var i = 1; i < province.points.length; i++) {
					x = province.points[i].x * provinceCanvas.width;
					y = province.points[i].y * provinceCanvas.height;
					provinceCtx.lineTo(x, y);
				}
				provinceCtx.closePath();
				provinceCtx.fill();
			}

			// draw faction borders as thick line
			borderCtx.strokeStyle = "rgba(" + color + ", 0.5)";
			borderCtx.clearRect(0, 0, borderCanvas.width, borderCanvas.height);
			for (var e = 0; e < faction.edges.length; e++) {
				var edge = faction.edges[e];
				var x = edge.points[0].x * borderCanvas.width;
				var y = edge.points[0].y * borderCanvas.height;
				borderCtx.beginPath();
				borderCtx.moveTo(x, y);
				for (var p=1; p<edge.points.length; p++) {
					x = edge.points[p].x * borderCanvas.width;
					y = edge.points[p].y * borderCanvas.height;
					borderCtx.lineTo(x, y);
				}
				borderCtx.closePath();
				borderCtx.stroke();
			}

			// copy only the part of the faction borders inside of the provinces
			// to the final image (using a composition of the two offscreen buffers)
			borderCtx.globalCompositeOperation = 'destination-in';
			borderCtx.drawImage(provinceCanvas, 0, 0);
			baseCtx.drawImage(borderCanvas, 0, 0);
		}
	}
}

function drawPointOfInterests()
{
	// Mark the capital provinces of all factions
	for (var f = 0; f < factions.length; f++) {
		if (factions[f].capital >= 0) {
			var province = provinces[factions[f].capital];
			var x = province.centroid.x * baseCanvas.width;
			var y = province.centroid.y * baseCanvas.height;
			if (typeof factions[f].imageobject !== "undefined") {
				baseCtx.drawImage(factions[f].imageobject, x - 24, y - 24, 48, 48);
			} else {
				baseCtx.fillStyle = 'black';
				fillStar(baseCtx, x, y, 10, 5, 0.4)
			}
		}
	}
}

function fillStar(ctx, x, y, radius, points, insetFactor)
{
    ctx.save();
    ctx.beginPath();
    ctx.translate(x, y);
    ctx.moveTo(0, 0 - radius);
    for (var i = 0; i < points; i++) {
        ctx.rotate(Math.PI / points);
        ctx.lineTo(0, 0 - (radius * insetFactor));
        ctx.rotate(Math.PI / points);
        ctx.lineTo(0, 0 - radius);
    }
    ctx.fill();
    ctx.restore();
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

function calculateArea(province)
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

function calculateCentroid(province)
{
	var xs = 0, ys = 0;
	for (var p=0; p<province.points.length-1; p++) {
		xs +=	(province.points[p].x + province.points[p+1].x) *
				(province.points[p].x * province.points[p+1].y - province.points[p+1].x * province.points[p].y);
		ys +=	(province.points[p].y + province.points[p+1].y) *
				(province.points[p].x * province.points[p+1].y - province.points[p+1].x * province.points[p].y);
	}
	province.centroid = {x: xs / (6 * province.area), y: ys / (6 * province.area)};
}

function preprocessProvinceData()
{
	for (var p = 0; p < provinces.length; p++) {
		
		// Set faction and regions of provinces to neutral
		provinces[p].faction = -1;
		provinces[p].factionname = "";
		provinces[p].name = "";
		provinces[p].region = -1;
		provinces[p].regionname = "";
		provinces[p].contestedby = -1;
		provinces[p].previousfactions = [];
		provinces[p].heartland = false;
		provinces[p].capital = false;
		
		// Merge edges to one large polygon
		buildPoints(provinces[p]);
		
		// Find bounding box of provinces
		findBoundingBox(provinces[p]);
		
		// Calculate area of province
		calculateArea(provinces[p]);
		
		// Calculate centroid of province
		calculateCentroid(provinces[p]);
	}
}

function preprocessFactionData()
{
	// Mark abandoned provinces with name and previous factions
	for (var a = 0; a < abandoned.length; a++) {
		var pid = abandoned[a].id;
		provinces[pid].faction = -2;
		provinces[pid].name = abandoned[a].name;
		if (typeof abandoned[a].previousfactions !== "undefined") {
			provinces[pid].previousfactions =
				factionIdsToNumbers(abandoned[a].previousfactions);
		}
	}

	// Mark provinces with name + faction info and extract regions
	for (var f = 0; f < factions.length; f++) {
		factions[f].vassalof = -1;
		factions[f].edges = [];
		for (var p = 0; p < factions[f].provinces.length; p++) {
			var pid = factions[f].provinces[p].id;
			provinces[pid].name = factions[f].provinces[p].name;
			provinces[pid].faction = f;
			provinces[pid].factionname = factions[f].name;
			if (typeof factions[f].provinces[p].contestedby !== "undefined") {
				var shortName = factions[f].provinces[p].contestedby;
				provinces[pid].contestedby = findFactionId(shortName);
			}
			if (typeof factions[f].provinces[p].previousfactions !== "undefined") {
				provinces[pid].previousfactions =
					factionIdsToNumbers(factions[f].provinces[p].previousfactions);
			}
		}
		if (typeof factions[f].capital !== "undefined") {
			provinces[factions[f].capital].capital = true;
		} else {
			factions[f].capital = -1;
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
	
	// Replace vassal id with correct faction number and set faction.vassalof
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
		if (provinces[i].faction >= 0) {
			provinces[i].heartland = true;
			for (var j = 0; j < provinces[i].edges.length; j++) {
				var neighborId = provinces[i].edges[j].neighbor;
				if (neighborId < 0 || provinces[neighborId].faction < 0 ||
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
				if (!regions[r].provinces.contains(edges[e].neighbor)) {
					regions[r].edges.push(edges[e]);
				}
			}
		}
	}
	
	// Find borders of all factions for drawing
	for (var p = 0; p < provinces.length; p++) {
		var province = provinces[p];
		if (province.faction >= 0) {
			var faction = province.faction;
			for (var e = 0; e < province.edges.length; e++) {
				var edge = province.edges[e];
				edge.province = p;
				// Important: create a copy of the edge object
				if (edge.neighbor < 0) {	
					factions[faction].edges.push(jQuery.extend({}, edge));
				} else {
					var nf = provinces[edge.neighbor].faction;
					if (nf != faction) {
						factions[faction].edges.push(jQuery.extend({}, edge));
					}
				}
			}
		}
	}
	
	// Connect faction border to polygon(s)
	for (var f = 0; f < factions.length; f++) {
		var connectCount;
		do { connectCount = connectEdges(factions[f].edges); }
		while (connectCount > 0);
	}
	
	// Calculate points and areas of factions
	for (var f = 0; f < factions.length; f++) {
		calculatePointsAndArea(f);
	}
}

function factionIdsToNumbers(factionIds)
{
	var factionNumbers = [];
	for (var f=0; f<factionIds.length; f++) {
		factionNumbers.push(findFactionId(factionIds[f]));
	}
	return factionNumbers;
}

function connectEdges(edges)
{
	for (var e1 = 0; e1 < edges.length; e1++) {
		for (var e2 = 0; e2 < edges.length; e2++) {
			if (e1 != e2 && isConnected(edges[e1].points[edges[e1].points.length - 1], edges[e2].points[0])) {
				edges[e1].points = edges[e1].points.concat(edges[e2].points);
				edges.remove(e2);
				return 1;
			}
		}
	}
	return 0;
}

function isConnected(p1, p2)
{
	var xd = p1.x - p2.x;
	var yd = p1.y - p2.y;
	var dist = Math.sqrt(xd * xd + yd * yd);
	return dist < 0.001;
}

function buildPoints(province)
{
	province.points = [];
	for (var i=0; i<province.edges.length; i++) {
		var edge = province.edges[i];
		edge.points = [];
		for (var j=0; j<edge.xpoints.length && j<edge.ypoints.length; j++) {
			var point = {x: edge.xpoints[j], y: edge.ypoints[j]};
			province.points.push(point);
			edge.points.push(point);
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
		image: faction.image,
		color: faction.color
	};
}

function generateHtmlRow(fo)
{
	var image = "&nbsp;";
	if (typeof fo.image !== "undefined") {
		image = "<img class=\"fimgtable\" src=\"" + fo.image + "\" />";
	}

	var html = "<tr>";
	html += "<td class=\"imgcell\" style=\"background-color:rgb(" + fo.color + ")\">" + image + "</td>";
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
	$("#coords").css({display: "block"});
	$("#points").css({display: "none"});
}

function showPoints()
{
	$("#drawstack").css({display: "none"});
	$("#coords").css({display: "none"});
	$("#infobox").css({display: "none"});
	$("#points").css({display: "block"});
}

Array.prototype.remove = function(from, to) {
	var rest = this.slice((to || from) + 1 || this.length);
	this.length = from < 0 ? this.length + from : from;
	return this.push.apply(this, rest);
};

Array.prototype.contains = function(item) {
	for (var i = 0; i < this.length; i++)
		if (this[i] == item) return true;
	return false;
};
