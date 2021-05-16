using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TypeMeWeb.Hubs
{
    public class ChatHub : Hub
    {
        public async Task EnviarMensaje(string user, string mensaje)
        {
            await Clients.All.SendAsync("RecibirMensaje", user, mensaje);
        }
    }
}