using Hymson.MES.SignalRHub.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Hymson.MES.SignalRHub.Hubs
{
    [Authorize]
    public class NotificationHub : Hub<IChatClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnDisconnectedAsync(ex);
        }

        #region 给web界面发送信息
        /// <summary>
        /// 向指定群组发送信息
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="message">信息内容</param>  
        /// <returns></returns>
        public async Task SendMessageToGroupAsync(string groupName, ClientMessageDto message)
        {
            await Clients.Group(groupName).ReceiveMessage(message);
        }

        /// <summary>
        /// 向指定成员发送信息
        /// </summary>
        /// <param name="user">成员名</param>
        /// <param name="message">信息内容</param>
        /// <returns></returns>
        public async Task SendPrivateMessage(string user, ClientMessageDto message)
        {
            await Clients.User(user).ReceiveMessage(message);
        }

        public async Task SendMessage(ClientMessageDto message)
        {
            await Clients.All.ReceiveMessage(message);
        }
        #endregion
    }
}
