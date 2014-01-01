This interactive HTML5 faction map is intened for PVP Minecraft servers. It was originally developed for http://minecraftcenter.net/.

This repository contains the HTML, CSS and JavaScript code for the map (see folder "html"). It does also include a tool written in C#, used for the vectorization of bitmaps with hand drawn maps (see folder "vectorization"). This tool is only necessary for the inital setup of a map. A simple web interface for editing the map content is also included. The web interface is written in PHP and is located in the folder "adminpanel".

Setting up a new map
====================

1. Upload everything from the "html" folder to your webspace.
2. Create a background image using Dynmap or something similar. Upload it as map.png.
3. Put the ingame corner coordinates of the background image into the file map.json.
4. Load the background image into GIMP or Photoshop, create a new layer and draw the borders of your provinces. Use a black pen with hard borders (no alpha transparency!). Pay attention to the details and avoid gaps in the borders. Islands (provinces inside another provinces) are not supported!
5. Export the borders without the background as binary (black & white) image. Borders should be black, the background white.
6. Use a thinning tool to make all borders 1px thick. I used ImageJ for this task (http://rsb.info.nih.gov/ij/index.html).
7. Compile the vectorization tool (with Visual Studio Express for C#) and use it to create the provinces.json file. Use "Load Binary Image" to load the thinned image and press "Start". The tool is finished when it reports "Extracted polygons in xxxxx ms". Use the "Export JSON" button to save the results as provinces.json. Be aware that this tool is not well tested and may crash or fail without a proper error message. It also may take a few minutes to finish.
8. OPTIONAL: Upload the PHP admin panel and set the password and paths. Make sure your PHP is allowed to read and write the factions.json file!
9. Edit the faction data inside the factions.json file by hand or using the PHP admin panel.

Updating the map
================

The faction.json file is written in JSON, which is very similar to XML. JSON is not a programming language. This means you don't have to be a programmer to update the map! There are strict rules for valid entries in the faction file.

Basic structure: Each faction has its own block in the "factions" section, containing all provinces, regions, etc. Each faction block is separated by a comma. No comma after the last block. Here is an annotated example for a faction block:

	{
		"id": "travellers", // Mandatory, must be unique and should contain no upper case, special chars or spaces!
		"image": "traveller.png", // Optional, the relative or absolute path to a small faction logo as PNG file with alpha channel. Data URLs are also possible! (see http://dataurl.net/ for details)
		"name": "Travellers of the Windrose", // Mandatory, contains the full name of the faction
		"color": "70,140,220", // Mandatory, contains the RGB encoded faction color (use Paint or GIMP for this).
		"capital": 2, // Optional, contains the Id of the capital province.
		"provinces": [ // Mandatory, contains blocks with the claimed provinces, separated by comma.
			{"id": 2, "name": "Windfall Keys"}, // Each block contains the province Id and a name.
			{"id": 1, "name": "Upper Antides", "previousfactions": ["somefactionid1", "somefactionid2"]}, // Provinces can have a optional list with previous owners
			{"id": 16, "name": "Lower Antides", "contestedby": "somefactionid"} // Provinces can be contested by another faction
		],
		"vassals": [ "factionid1", "factionid2" ], // Optional, contains a comma separated list of vassals. Contains the Ids (and not names!) of other factions.
		"regions": [ { "name": "Deep Blue", "provinces": [2, 1, 16] } ] // Optional, contains a comma separated list of region blocks. Each region block has a name and a list with the relevant province Ids.
	}
	
There is another section called "abandoned". This sectionis contains a list of abandoned/deserted provinces. The province blocks are just like the ones for active sections. The "name" and "id" fields are mandatory, the "previousfactions" field is optional.

Remember: All entries and blocks are separated by commas! Do not omit the list brackets! They are always required, even when the list does only contain one item. Another common mistake is to put a comma before closing brackets of blocks and lists.

Other stuff
===========

There is currently no way to transfer existing faction data to a new, edited or enlarged map. This due to the changed province ids. Different input images will lead to different province Ids, even for unchanged provinces.