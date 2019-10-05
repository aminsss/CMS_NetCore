using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DataLayer;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly INewsGroupService _newsGroupService;
        private readonly INewsTagService _newsTagService;
        private readonly INewsGalleryService _newsGalleryService;
        private readonly IUserService _userService;
        private readonly IHostingEnvironment _env;

        public NewsController(INewsService newsService, INewsGroupService newsGroupService, IUserService userService
            ,INewsTagService newsTagService,INewsGalleryService newsGalleryService , IHostingEnvironment env)
        {
            _newsGroupService = newsGroupService;
            _newsService = newsService;
            _newsTagService = newsTagService;
            _newsGalleryService = newsGalleryService;
            _userService = userService;
            _env = env;
        }

        // GET: Admin/News
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> GetNews(int page = 1, int pageSize = 5, string searchString = "")
        {
            searchString = searchString ?? string.Empty;

            var list = await _newsService.GetBySearch(page, pageSize, searchString);
            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);

            var getList = from obj in list.Records
                          select new
                          {
                              newsimage = obj.NewsImage,
                              newstitle = obj.NewsTitle,
                              addeddate = obj.AddedDate,
                              grouptitle = obj.NewsGroup.GroupTitle,
                              newsid = obj.NewsId
                          };

            return Json(new { getList, totalCount, numPages });
        }

        // GET: Admin/News/Create
        public  IActionResult Create()
        {
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news, IFormFile newsimage,List<IFormFile> newsgallery, string tags)
        {
            if (ModelState.IsValid)
            {
                news.UserId = 1010;
                //news.UserId = (await _userService.GetUserByIdentity(User.Identity.Name)).UserId;
                var fileName = "no-photo.jpg";
                if (ModelState.IsValid)
                {
                    //--------------Create News Images ------------------
                    if (newsimage != null)
                    {
                        var uploads = Path.Combine(_env.WebRootPath, "Upload\\NewsImages");
                        fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(newsimage.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await newsimage.CopyToAsync(fileStream);
                        }
                        //---------------------resize Images ----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(128);
                        img.Resize(Path.Combine(uploads, fileName), Path.Combine(uploads, "thumbnail", fileName));
                    }
                    news.NewsImage = fileName;

                    //------------Create Gallery Product --------------
                    foreach (var file in newsgallery)
                    {
                        var uploads = Path.Combine(_env.WebRootPath, "Upload\\NewsImages");
                        string galleryname = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, galleryname), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        news.NewsGallery.Add(new NewsGallery()
                        {
                            NewsId = news.NewsId,
                            ImageName = galleryname
                        });
                        //---------------------resize Images ----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(350);
                        img.Resize(Path.Combine(uploads, galleryname), Path.Combine(uploads, "thumbnail", galleryname));
                    }
                    //-------------------Tags---------------------
                    if (!string.IsNullOrEmpty(tags))
                    {
                        string[] tag = tags.Split('-');
                        foreach (string t in tag)
                        {
                            news.NewsTag.Add(new NewsTag()
                            {
                                NewsId = news.NewsId,
                                TagsTitle = t.Trim()
                            });
                        }
                    }

                    await _newsService.Add(news);
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["NewsGroupId"] = new SelectList(await _newsGroupService.GetAll(), "NewsGroupId", "AliasName", news.NewsGroupId);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _newsService.GetById(id);
            if (news == null)
            {
                return NotFound();
            }
            string tags = "";
            foreach (var t in news.NewsTag)
            {
                tags += t.TagsTitle + "-";
            }
            if (tags.EndsWith("-"))
            {
                tags = tags.Substring(0, tags.Length - 1);
                ViewBag.tag = tags;
            }
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news, IFormFile newsimage, List<IFormFile> newsgallery, string tags)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var uploads = Path.Combine(_env.WebRootPath, "Upload\\NewsImages");
                    news.UserId = 1010;
                    //news.UserId = (await _userService.GetUserByIdentity(User.Identity.Name)).UserId;
                    //----------------------Edit News Image -----------------------
                    if (newsimage != null)
                    {
                        if (news.NewsImage != "no-photo.jpg")
                        {
                            if (System.IO.File.Exists(Path.Combine(uploads, news.NewsImage)))
                                System.IO.File.Delete(Path.Combine(uploads, news.NewsImage));
                            if (System.IO.File.Exists(Path.Combine(uploads, "thumbnail", news.NewsImage)))
                                System.IO.File.Delete(Path.Combine(uploads, "thumbnail", news.NewsImage));
                        }
                        news.NewsImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(newsimage.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, news.NewsImage), FileMode.Create))
                        {
                            await newsimage.CopyToAsync(fileStream);
                        }
                        //---------------------resize Images ----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(128);
                        img.Resize(Path.Combine(uploads, news.NewsImage), Path.Combine(uploads, "thumbnail", news.NewsImage));
                    }

                    //------------Edit Gallery Product --------------
                    List<NewsGallery> newsGalleries = new List<NewsGallery>();

                    foreach (var file in newsgallery)
                    {
                        string galleryname = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, galleryname), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        newsGalleries.Add(new NewsGallery()
                        {
                            NewsId = news.NewsId,
                            ImageName = galleryname
                        });
                        //---------------------resize Images ----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(350);
                        img.Resize(Path.Combine(uploads, galleryname), Path.Combine(uploads, "thumbnail", galleryname));
                    }
                    await _newsGalleryService.Add(newsGalleries);

                    //------------Edit TAgs ------------------
                    await _newsTagService.RemoveByNewsId(news.NewsId);
                    if (!string.IsNullOrEmpty(tags))
                    {
                        List<NewsTag> newsTags = new List<NewsTag>();
                        foreach (string tag in tags.Split('-'))
                        {
                            newsTags.Add(new NewsTag()
                            {
                                NewsId = news.NewsId,
                                TagsTitle = tag.Trim()
                            });
                        }
                        await _newsTagService.Add(newsTags);
                    }

                    await _newsService.Edit(news);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _newsService.NewsExistence(news.NewsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: Admin/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _newsService.GetById(id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var news = await _newsService.GetById(id);
            await _newsService.Remove(news);
            return Json(true);
        }
    }
}
