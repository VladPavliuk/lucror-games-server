using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LucrorGames
{
    public class GameSessionHub : Hub
    {
        public async Task OpenSession(SessionData sessionData)
        {
            await Clients.All.SendAsync("CloseSession", sessionData);
        }
    }

    public class SessionData
    {
        public string Room { get; set; }
        public decimal Score { get; set; }
    }
}
