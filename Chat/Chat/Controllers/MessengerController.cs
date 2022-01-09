using Chat.Data;
using Chat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Controllers
{
    public class MessengerController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MessengerController(AppDbContext appDbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            string callerId = _userManager.GetUserId(User);
            return View(_appDbContext.CustomUsers.Where(cu => cu.Id != callerId).ToList());
        }
        public IActionResult Chat(string recieverId)
        {
            string senderId = _userManager.GetUserId(User);
            VmMessage model = new VmMessage();
            model.User = _appDbContext.CustomUsers.Find(recieverId);
            model.Messages = _appDbContext.Messages
                                                .Where(m => (m.SenderId == senderId && m.RecieverId == recieverId) 
                                                              || (m.SenderId == recieverId && m.RecieverId == senderId))
                                                .ToList();
            model.SenderId = senderId;

            return View(model);
        }
    }
}
