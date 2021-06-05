using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TypeMeWeb.Models;

namespace TypeMeWeb.Hubs
{
    public class ChatHub : Hub
    {
        public async Task AgregarAGrupo(string nombreDeGrupo)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, nombreDeGrupo);
        }

        public async Task EnviarMensaje(MensajeDominio mensaje)
        {
            await Clients.All.SendAsync("RecibirMensaje", mensaje);
        }
    }
}