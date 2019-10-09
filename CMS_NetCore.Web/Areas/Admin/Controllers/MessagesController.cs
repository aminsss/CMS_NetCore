using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MessagesController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        // GET: Admin/Messages
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetMessages(int page = 1, int pageSize = 5, string searchString = "")
        {
            var list = await _messageService.GetBySearch(page, pageSize, searchString, User.Identity.Name);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               moblie = obj.UsersFrom.moblie,
                               Subject = obj.Subject,
                               AddedDate = obj.AddedDate,
                               Email = obj.Email,
                               ISRead = obj.ISRead,
                               MessageId = obj.MessageId,
                           });

            return Json(new { getList, totalCount, numPages });
        }

        // GET: Admin/Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _messageService.GetById(id);
            if (message == null)
            {
                return NotFound();
            }
            else if (message.ISRead == false)
            {
               await _messageService.Edit(message);
            }

            return View(message);
        }

        // GET: Admin/Messages/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id != null)
            {
                Message message = await _messageService.GetById(id);
                if (message.FromUser != null)
                    ViewBag.Email = message.UsersFrom.Email;
                else if (message.Email != null)
                    ViewBag.Email = message.Email;
            }
            return View();
        }

        // POST: Admin/Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Message message, string MessageTo)
        {
            if (MessageTo != null)
            {
                int msgID;
                int.TryParse(MessageTo, out msgID);
                Message M =await _messageService.GetById(msgID);
                message.ToUser = M.FromUser;
                message.FromUser = M.ToUser;
                message.SenderName = M.UsersTo.Name;
            }
            else
            {
                var from =await _userService.GetUserByIdentity(User.Identity.Name);
                message.FromUser = from.UserId;
                message.SenderName = from.Name;
            }

            await _messageService.Add(message);
            try
            {
                //SendEmailSMS.SendEmail("amin.savari91@gmail.com", message.Email, message.Subject, message.ContentMessage);
            }
            catch
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _messageService.GetById(id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Admin/Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _messageService.GetById(id);
            await _messageService.Remove(message);
            return RedirectToAction(nameof(Index));
        }

    }
}
