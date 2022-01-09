using Chat.Data;
using Chat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContext;

        public ChatHub(AppDbContext appDbContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContextAccessor;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendPrivateMessage(string recieverId, string message)
        {
            IdentityUser loggedInUser = await _userManager.GetUserAsync(_httpContext.HttpContext.User);
            await Clients.User(recieverId).SendAsync("ReceiveMessage", loggedInUser.UserName, message);

            Message newMessage = new Message()
            {
                Text = message,
                RecieverId = recieverId,
                SenderId = loggedInUser.Id,
                CreatedDate = DateTime.Now
            };

            _appDbContext.Messages.Add(newMessage);
            _appDbContext.SaveChanges();
        }

        public async Task Typing(string recieverId)
        {
            await Clients.User(recieverId).SendAsync("ShowTyping");
        }
        public async Task HideTyping(string recieverId)
        {
            await Clients.User(recieverId).SendAsync("HideTyping");
        }
       
    }
}
