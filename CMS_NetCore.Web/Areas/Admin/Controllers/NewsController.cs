using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ServiceLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class NewsController : Controller
{
    private readonly INewsService _newsService;
    private readonly INewsGroupService _newsGroupService;
    private readonly INewsTagService _newsTagService;
    private readonly INewsGalleryService _newsGalleryService;
    private readonly IWebHostEnvironment _env;

    public NewsController(
        INewsService newsService,
        INewsGroupService newsGroupService,
        INewsTagService newsTagService,
        INewsGalleryService newsGalleryService,
        IWebHostEnvironment env
    )
    {
        _newsGroupService = newsGroupService;
        _newsService = newsService;
        _newsTagService = newsTagService;
        _newsGalleryService = newsGalleryService;
        _env = env;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult> GetNews(
        int page = 1,
        int pageSize = 5,
        string searchString = ""
    )
    {
        searchString ??= string.Empty;

        var list = await _newsService.GetBySearch(
            page,
            pageSize,
            searchString
        );

        var totalCount = list.TotalCount;
        var numPages = (int)Math.Ceiling((float)totalCount / pageSize);

        var getList = from news in list.Records
            select new
            {
                newsimage = news.NewsImage,
                newstitle = news.NewsTitle,
                addeddate = news.CreatedDate,
                grouptitle = news.NewsGroup.GroupTitle,
                newsid = news.Id
            };

        return Json(new { getList, totalCount, numPages });
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        News news,
        IFormFile newsImage,
        List<IFormFile> newsGallery,
        string tags
    )
    {
        if (ModelState.IsValid)
        {
            news.UserId = 1010;
            var fileName = "no-photo.jpg";
            if (ModelState.IsValid)
            {
                if (newsImage != null)
                {
                    var uploads = Path.Combine(
                        _env.WebRootPath,
                        "Upload\\NewsImages"
                    );
                    fileName = Guid.NewGuid().ToString().Replace(
                        "-",
                        ""
                    ) + Path.GetExtension(newsImage.FileName);
                    await using (var fileStream = new FileStream(
                                     Path.Combine(
                                         uploads,
                                         fileName
                                     ), FileMode.Create
                                 ))
                    {
                        await newsImage.CopyToAsync(fileStream);
                    }

                    var img = new ImageResizer(128);
                    img.Resize(
                        Path.Combine(
                            uploads,
                            fileName
                        ), Path.Combine(
                            uploads,
                            "thumbnail",
                            fileName
                        )
                    );
                }

                news.NewsImage = fileName;

                foreach (var file in newsGallery)
                {
                    var uploads = Path.Combine(
                        _env.WebRootPath,
                        "Upload\\NewsImages"
                    );
                    var galleryName = Guid.NewGuid().ToString().Replace(
                        "-",
                        ""
                    ) + Path.GetExtension(file.FileName);
                    await using (var fileStream = new FileStream(
                                     Path.Combine(
                                         uploads,
                                         galleryName
                                     ), FileMode.Create
                                 ))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    news.NewsGalleries.Add(
                        new NewsGallery()
                        {
                            NewsId = news.Id,
                            ImageName = galleryName
                        }
                    );

                    var img = new ImageResizer(350);

                    img.Resize(
                        Path.Combine(
                            uploads,
                            galleryName
                        ), Path.Combine(
                            uploads,
                            "thumbnail",
                            galleryName
                        )
                    );
                }

                if (!string.IsNullOrEmpty(tags))
                {
                    foreach (var tag in tags.Split('-'))
                    {
                        news.NewsTags.Add(
                            new NewsTag()
                            {
                                NewsId = news.Id,
                                TagsTitle = tag.Trim()
                            }
                        );
                    }
                }

                await _newsService.Add(news);
                return RedirectToAction(nameof(Index));
            }
        }

        ViewData["NewsGroupId"] = new SelectList(
            await _newsGroupService.GetAll(),
            "Id",
            "AliasName",
            news.NewsGroupId
        );

        return View(news);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var news = await _newsService.GetById(id);
        if (news == null)
            return NotFound();

        var tags = news.NewsTags.Aggregate(
            "",
            (
                current,
                newsTag
            ) => current + newsTag.TagsTitle + "-"
        );

        if (!tags.EndsWith("-"))
            return View(news);

        tags = tags[..^1];
        ViewBag.tag = tags;

        return View(news);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        News news,
        IFormFile newsImage,
        List<IFormFile> newsGallery,
        string tags
    )
    {
        if (id != news.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(news);

        try
        {
            var uploads = Path.Combine(
                _env.WebRootPath,
                "Upload\\NewsImages"
            );

            if (newsImage != null)
            {
                if (news.NewsImage != "no-photo.jpg")
                {
                    if (System.IO.File.Exists(
                            Path.Combine(
                                uploads,
                                news.NewsImage
                            )
                        ))
                        System.IO.File.Delete(
                            Path.Combine(
                                uploads,
                                news.NewsImage
                            )
                        );
                    if (System.IO.File.Exists(
                            Path.Combine(
                                uploads,
                                "thumbnail",
                                news.NewsImage
                            )
                        ))
                        System.IO.File.Delete(
                            Path.Combine(
                                uploads,
                                "thumbnail",
                                news.NewsImage
                            )
                        );
                }

                news.NewsImage = Guid.NewGuid().ToString().Replace(
                    "-",
                    ""
                ) + Path.GetExtension(newsImage.FileName);
                await using (var fileStream = new FileStream(
                                 Path.Combine(
                                     uploads,
                                     news.NewsImage
                                 ), FileMode.Create
                             ))
                {
                    await newsImage.CopyToAsync(fileStream);
                }

                var img = new ImageResizer(128);

                img.Resize(
                    Path.Combine(
                        uploads,
                        news.NewsImage
                    ), Path.Combine(
                        uploads,
                        "thumbnail",
                        news.NewsImage
                    )
                );
            }

            var newsGalleries = new List<NewsGallery>();

            foreach (var file in newsGallery)
            {
                var galleryName = Guid.NewGuid().ToString().Replace(
                    "-",
                    ""
                ) + Path.GetExtension(file.FileName);
                await using (var fileStream = new FileStream(
                                 Path.Combine(
                                     uploads,
                                     galleryName
                                 ), FileMode.Create
                             ))
                {
                    await file.CopyToAsync(fileStream);
                }

                newsGalleries.Add(
                    new NewsGallery()
                    {
                        NewsId = news.Id,
                        ImageName = galleryName
                    }
                );

                var img = new ImageResizer(350);

                img.Resize(
                    Path.Combine(
                        uploads,
                        galleryName
                    ), Path.Combine(
                        uploads,
                        "thumbnail",
                        galleryName
                    )
                );
            }

            await _newsGalleryService.Add(newsGalleries);

            await _newsTagService.RemoveByNewsId(news.Id);
            if (!string.IsNullOrEmpty(tags))
            {
                var newsTags = tags.Split('-')
                    .Select(
                        tag => new NewsTag()
                        {
                            NewsId = news.Id,
                            TagsTitle = tag.Trim()
                        }
                    )
                    .ToList();

                await _newsTagService.Add(newsTags);
            }

            await _newsService.Edit(news);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _newsService.IsExist(news.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var news = await _newsService.GetById(id);
        if (news == null)
            return NotFound();

        return View(news);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> DeleteConfirmed(int id)
    {
        var news = await _newsService.GetById(id);
        await _newsService.Remove(news);
        return Json(true);
    }
}