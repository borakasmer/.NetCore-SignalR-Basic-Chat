using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using signalRCore.Models;

namespace signalRCore.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chat")
                .WithConsoleLogger()
                .Build();
                 connection.On<string>("Send2", data =>
                {
                    Console.WriteLine($"Received: {data}");
                }); 

                await connection.StartAsync();

                await connection.InvokeAsync("Send2", "Hello From Index");

                return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class Chat : Hub
    {
         public override Task OnConnectedAsync(){
            return Clients.Client(Context.ConnectionId).InvokeAsync("SetConnectionId",Context.ConnectionId);
        } 
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }  

        public Task Send2(string message)
        {
            return Clients.All.InvokeAsync("Send2", message);
        }  
    }
}
