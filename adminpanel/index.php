<?php
	
	$updatePassword = 'password';
	$factionFile = '../factions.json';
	$message = '&nbsp';
	$factionData = '';

	if (isset($_POST['password']) && isset($_POST['data'])) {
		$factionData = strip_tags($_POST['data']);
		if ($_POST['password'] === $updatePassword) {
			$result = file_put_contents($factionFile, $factionData);
			if (!$result) {
				$message = '<span style="color:red">Could not save file ' . $factionFile . '!</span>';
			} else {
				$message = '<span style="color:green">Saved to file ' . $factionFile . '!</span>';
			}
		} else {
			$message = '<span style="color:red">Invalid password. Changes are NOT saved!</span>';
		}
	}
	
	if ($factionData === '') {
		$factionData = file_get_contents($factionFile);
		if (!$factionData) {
			$message = '<span style="color:red">Could not load file ' . $factionFile . '!</span>';
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
		</script>
	</head>
	<body>
		<div>
			<?php echo $message; ?>
		</div>
		<form method="POST">
			Password: <input type="text" name="password" /><br />
			<textarea id="ta" autocorrect="off" autocapitalize="off" spellcheck="false"
				style="width:90%;height:90%" name="data"><?php echo $factionData; ?></textarea><br />
			<input type="submit" value="Save" />
		</form>
	</body>
</html>