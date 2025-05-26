
function connectToSignalR() {
	console.log("Connecting to SignalR...");
	window.notificationDivs = new Array();
	var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
	conn.on("DisplayNotification", displayNotification);
	conn.start().then(function () {
		console.log("SignalR has started.");
	}).catch(function (err) {
		console.log(err);
	});
}

function displayNotification(user, message) {
	console.log(message);
	var data = JSON.parse(message);
	var $target = $("#signalr-notifications");
	const $div = $(`
		<div>${data.Manufacturer} ${data.ModelName} (${data.Year}, ${data.Color})<br />
		PRICE: ${data.Price} ${data.Currency}<br />
		<a href="/vehicles/details/${data.Registration}">click for more!</a>
		</div>`);
	$div.css("background-color", data.Color);
	$target.prepend($div);
	window.setTimeout(function () {
		$div.fadeOut(2000, function () {
			$div.remove();
		});
	}, 5000);
}

$(document).ready(connectToSignalR);