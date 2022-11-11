using System;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Int32;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class MessagesController : Controller
{
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;

    public MessagesController(
        IMessageService messageService,
        IUserService userService
    )
    {
        _messageService = messageService;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult> GetMessages(
        int page = 1,
        int pageSize = 5,
        string searchString = ""
    )
    {
        var messages = await _messageService.GetBySearch(
            page,
            pageSize,
            searchString,
            User.Identity!.Name
        );

        var totalCount = messages.TotalCount;
        var numPages = (int)Math.Ceiling((float)totalCount / pageSize);

        var getList = from message in messages.Records
            select new
            {
                moblie = message.UserFrom.Mobile,
                message.Subject,
                AddedDate = message.CreatedDate,
                message.Email,
                ISRead = message.IsRead,
                MessageId = message.Id,
            };

        return Json(new { getList, totalCount, numPages });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var message = await _messageService.GetById(id);
        if (message == null)
            return NotFound();

        if (message.IsRead == false)
            await _messageService.Edit(message);

        return View(message);
    }

    public async Task<IActionResult> Create(int? id)
    {
        if (id == null)
            return View();

        var message = await _messageService.GetById(id);
        if (message.FromUser != null)
            ViewBag.Email = message.UserFrom.Email;
        else if (message.Email != null)
            ViewBag.Email = message.Email;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Message message,
        string messageTo
    )
    {
        if (messageTo != null)
        {
            TryParse(
                messageTo,
                out var messageId
            );

            var newMessage = await _messageService.GetById(messageId);
            message.ToUser = newMessage.FromUser;
            message.FromUser = newMessage.ToUser;
            message.SenderName = newMessage.UserTo.Name;
        }
        else
        {
            var fromUser = await _userService.GetUserByIdentity(User.Identity!.Name);
            message.FromUser = fromUser.Id;
            message.SenderName = fromUser.Name;
        }

        await _messageService.Add(message);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var message = await _messageService.GetById(id);
        if (message == null)
            return NotFound();

        return View(message);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var message = await _messageService.GetById(id);
        await _messageService.Remove(message);
        return RedirectToAction(nameof(Index));
    }
}