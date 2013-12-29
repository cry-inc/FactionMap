<?php
	
	// start config variables
	$updatePassword = 'password';
	$factionFile = '../factions.json';
	$mapUrl = '../';
	// end config variables
	
	$message = '';
	$factionData = FALSE;
	$lastedit = filemtime($factionFile);

	if (isset($_POST['password']) && isset($_POST['data'])) {
		$factionData = strip_tags($_POST['data']);
		if ($_POST['password'] === $updatePassword) {
			if (isset($_POST['filetime']) && is_numeric($_POST['filetime']) && $_POST['filetime'] < $lastedit) {
				$message = '<span style="color:red">The file was modified since you opened it. To overwrite it, enter the password and save.</span>';
			} else {
				$result = file_put_contents($factionFile, $factionData);
				if (!$result) {
					$message = '<span style="color:red">Could not save file ' . $factionFile . '!</span>';
				} else {
					$message = '<span style="color:green">Saved to file ' . $factionFile . '!</span>';
				}
			}
		} else {
			$message = '<span style="color:red">Invalid password. Changes are NOT saved!</span>';
		}
	}
	
	if ($factionData === FALSE) {
		$factionData = file_get_contents($factionFile);
		if ($factionData === FALSE) {
			$message = '<span style="color:red">Could not load file ' . $factionFile . '!</span>';
			$factionData = '';
		}
	}

?>
<html>
	<head>
		<title>Faction Map Admin Panel</title>
		<script src="http://code.jquery.com/jquery-1.10.2.min.js"></script>
		<script type="text/javascript">
			$(document).delegate('#ta', 'keydown', function(e) {
				var keyCode = e.keyCode || e.which;
				if (keyCode == 9) {
					e.preventDefault();
					var start = $(this).get(0).selectionStart;
					var end = $(this).get(0).selectionEnd;
					// set textarea value to: text before caret + tab + text after caret
					$(this).val($(this).val().substring(0, start) + "\t" + $(this).val().substring(end));
					// put caret at right position again
					$(this).get(0).selectionStart = $(this).get(0).selectionEnd = start + 1;
				}
			});
			
			function checkAndSubmit() {
				try {
					JSON.parse(document.editform.data.value);
					return true;
				} catch (e) {
					alert("There seems to be a JSON syntax error! The data will not be saved.");
					console.log(e);
					return false;
				}
			}
		</script>
	</head>
	<body>
		<div>
			<?php echo $message; ?>
		</div>
		<form name="editform" method="POST" onsubmit="return checkAndSubmit()">
			Password: <input type="text" name="password" />
			<a target="_blank" href="<?php echo $mapUrl; ?>">Link to the Faction Map</a> <br />
			<textarea id="ta" autocorrect="off" autocapitalize="off" spellcheck="false"
				style="width:90%;height:90%" name="data"><?php echo $factionData; ?></textarea><br />
			<input type="hidden" name="filetime" value="<?php echo $lastEdit; ?>" />
			<input type="submit" value="Save" />
		</form>
	</body>
</html>