using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using CMS_NetCore.ServiceLayer;

namespace CMS_NetCore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IAttributeGroupService _attributeGroupService;
    private readonly IDetailItemService _detailItemService;
    private readonly IProductAttributeService _productAttributeService;
    private readonly IProductDetailService _productDetailService;
    private readonly IDetailGroupService _detailGroupService;
    private readonly IWebHostEnvironment _env;

    public ProductController(
        IProductService productService,
        IWebHostEnvironment env,
        IAttributeGroupService attributeGroupService,
        IDetailItemService detailItemService,
        IProductAttributeService productAttributeService
        , IProductDetailService productDetailService,
        IDetailGroupService detailGroupService
    )
    {
        _productService = productService;
        _attributeGroupService = attributeGroupService;
        _detailItemService = detailItemService;
        _productAttributeService = productAttributeService;
        _productDetailService = productDetailService;
        _detailGroupService = detailGroupService;
        _env = env;
    }

    // GET: Admin/Products
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<JsonResult> GetProducts(
        string searchString,
        int page = 1,
        int pageSize = 5
    )
    {
        searchString ??= string.Empty;

        var products = await _productService.GetBySearch(
            page,
            pageSize,
            searchString
        );

        var totalCount = products.TotalCount;
        var numPages = (int)Math.Ceiling((float)totalCount / pageSize);

        var list = from obj in products.Records
            select new
            {
                productImage = obj.Image,
                productTitle = obj.Title,
                Createddate = obj.CreatedDate,
                groupTitle = obj.ProductGroup.GroupTitle,
                productId = obj.Id
            };

        return Json(new { getList = list, totalCount, numPages });
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Product product,
        string galleryFiles,
        string tags
    )
    {
        if (!ModelState.IsValid)
            return View(product);

        foreach (var attributeGroup in await _attributeGroupService.GetByProductGroupId(product.ProductGroupId))
        {
            if (Request.Form["Grp_" + attributeGroup.Id.ToString()].Any())
            {
                product.ProductAttributes.Add(
                    new ProductAttribute
                    {
                        AttributeItemId = Convert.ToInt32(Request.Form["Grp_" + attributeGroup.Id]),
                        ProductId = product.Id,
                    }
                );
            }
        }

        foreach (var detailItem in await _detailItemService.GetByProductGroupId(product))
        {
            product.ProductDetails.Add(
                new ProductDetail
                {
                    DetailItemId = detailItem.Id,
                    ProductId = product.Id,
                    Value = Request.Form["detItem_" + detailItem.Id.ToString()]
                }
            );
        }

        await _productService.Add(
            product,
            galleryFiles,
            tags
        );

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _productService.GetIncludeById(id);
        if (product == null)
            return NotFound();

        var tags = product.ProductTags.Aggregate(
            string.Empty,
            (
                current,
                productTag
            ) => current + (productTag.TagTitle + "-")
        );

        ViewBag.tag = tags.EndsWith("-")
            ? tags[..^1]
            : tags;

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Product product,
        string galleryFiles,
        string tags
    )
    {
        if (id != product.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(product);

        try
        {
            await AddAttributeChanged(product);
            await AddDetailChanged(product);
            await _productService.Edit(
                product,
                galleryFiles,
                tags
            );
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _productService.IsExist(product.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(
            nameof(Edit),
            product.Id
        );
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _productService.GetById(id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productService.GetIncludeById(id);

        await _productService.Remove(
            product,
            _env.WebRootPath
        );

        return RedirectToAction(nameof(Index));
    }

    public async Task<JsonResult> DeleteGallery(int id)
    {
        await _productService.DeleteImage(
            id,
            _env.WebRootPath
        );

        return Json(true);
    }

    public IActionResult ShowNewGallery(string allPictures)
    {
        ViewBag.allpics = allPictures;
        return PartialView();
    }

    public async Task<IActionResult> GetAttributes(
        int? id,
        int? productGroupId
    )
    {
        if (id != null)
            ViewBag.ProductId = id;

        return PartialView(await _attributeGroupService.GetByProductGroupId(productGroupId));
    }

    public async Task<IActionResult> GetDetails(
        int? id,
        int? productGroupId
    )
    {
        if (id != null)
            ViewBag.ProductId = id;

        return PartialView(await _detailGroupService.GetByProductGroupId(productGroupId));
    }

    public async Task<JsonResult> UniqueAlias(
        string aliasName,
        int? productId
    )
    {
        return Json(
            !await _productService.UniqueAlias(
                aliasName,
                productId
            )
        );
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null)
            return Json(new { status = "Error" });

        var uploads = Path.Combine(
            _env.WebRootPath,
            "Upload\\ProductImages"
        );

        var fileName = Guid.NewGuid().ToString().Replace(
            "-",
            ""
        ) + Path.GetExtension(file.FileName);
        await using (var fileStream = new FileStream(
                         Path.Combine(
                             uploads,
                             fileName
                         ), FileMode.Create
                     ))
        {
            await file.CopyToAsync(fileStream);
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

        return Json(
            new
            {
                status = "Done", src = Path.Combine(
                    "\\Upload\\ProductImages",
                    fileName
                ),
                ImageName = fileName
            }
        );
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadGallery(List<IFormFile> files)
    {
        var names = "";
        var uploads = Path.Combine(
            _env.WebRootPath,
            "Upload\\ProductImages"
        );

        foreach (var file in files)
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

            names += galleryName + ",";

            //---------------------resize Images ----------------------
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

        return Json(new { status = "Done", imagesName = names });
    }

    private async Task AddAttributeChanged(Product product)
    {
        var productAttributes = new List<ProductAttribute>();

        foreach (var attributeGroup in await _attributeGroupService.GetByProductGroupId(product.ProductGroupId))
        {
            {
                var productAttribute = await _productAttributeService.GetProductAttribute(
                    product.Id,
                    attributeGroup.Id
                );

                if (productAttribute != null)
                {
                    if (Request.Form["Grp_" + attributeGroup.Id] == "none")
                        _productAttributeService.Remove(productAttribute);
                    else if (Request.Form["Grp_" + attributeGroup.Id].Any())
                    {
                        productAttribute.AttributeItemId =
                            Convert.ToInt32(Request.Form["Grp_" + attributeGroup.Id]);
                        _productAttributeService.Edit(productAttribute);
                    }
                }
                else
                {
                    if (Request.Form["Grp_" + attributeGroup.Id].Any() &&
                        Request.Form["Grp_" + attributeGroup.Id] != "none")
                    {
                        productAttributes.Add(
                            new ProductAttribute()
                            {
                                AttributeItemId = Convert.ToInt32(Request.Form["Grp_" + attributeGroup.Id]),
                                ProductId = product.Id,
                            }
                        );
                    }
                }
            }
        }

        _productAttributeService.Add(productAttributes);
    }

    private async Task AddDetailChanged(Product product)
    {
        var productDetail = new List<ProductDetail>();
        foreach (var detItm in await _detailItemService.GetByProductGroupId(product))
        {
            var detail = await _productDetailService.GetProductDetail(
                product.Id,
                detItm.Id
            );
            if (detail != null)
            {
                detail.Value = Request.Form["detItem_" + detItm.Id];
                _productDetailService.Edit(detail);
            }
            else
            {
                productDetail.Add(
                    new ProductDetail
                    {
                        DetailItemId = detItm.Id,
                        ProductId = product.Id,
                        Value = Request.Form["detItem_" + detItm.Id]
                    }
                );
            }
        }

        _productDetailService.Add(productDetail);
    }
}