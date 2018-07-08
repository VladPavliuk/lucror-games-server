using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LucrorGames
{
    public class GameSessionHub : Hub
    {
        public async Task CloseSession(SessionData sessionData) => await Clients.Group(sessionData.Room).SendAsync("CloseSession", sessionData);
        public async Task JoinRoom(string roomName) => await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public class SessionData
    {
        public string Room { get; set; }
        public decimal Score { get; set; }
    }
}
