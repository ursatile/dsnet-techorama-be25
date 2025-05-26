using System.Threading.Tasks.Sources;
using Microsoft.AspNetCore.SignalR;

namespace Autobarn.Website.Hubs;

public class AutobarnHub : Hub {
	public async Task NotifyWebsiteUsers(string user, string message) {
		await Clients.All.SendAsync("DisplayNotification", user, message);
	}
}