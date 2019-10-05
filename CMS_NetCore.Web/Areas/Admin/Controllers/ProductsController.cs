using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS_NetCore.DomainClasses;
using CMS_NetCore.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CMS_NetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductGroupService _productGroupService;
        private readonly IAttributeGrpService _attributeGrpService;
        private readonly IDetailItemService _detailItemService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductDetailService _productDetailService;
        private readonly IDetailGroupService _detailGroupService;
        private readonly IHostingEnvironment _env;

        public ProductsController(IProductService productService, IProductGroupService productGroupService,
            IAttributeGrpService attributeGrpService, IDetailItemService detailItemService, IProductAttributeService productAttributeService
            , IProductDetailService productDetailService, IDetailGroupService detailGroupService, IHostingEnvironment env)
        {
            _productService = productService;
            _productGroupService = productGroupService;
            _attributeGrpService = attributeGrpService;
            _detailItemService = detailItemService;
            _productAttributeService = productAttributeService;
            _productDetailService = productDetailService;
            _detailGroupService = detailGroupService;
            _env = env;
        }

        // GET: Admin/Products
        public  IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProducts(string searchString ,int page = 1, int pageSize = 5)
        {
            searchString = searchString ?? string.Empty;

            var list = await _productService.GetBySearch(page, pageSize, searchString);
            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);

            var getList = (from obj in list.Records
                           select new
                           {
                               productimage = obj.ProductImage,
                               producttitle = obj.ProductTitle,
                               addeddate = obj.AddedDate,
                               grouptitle = obj.ProductGroup.GroupTitle,
                               productid = obj.ProductId
                           });

            return Json(new { getList, totalCount, numPages });
        }

       
        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, string galleyFiles, string tags)
        {
            if (ModelState.IsValid)
            {
                //-----------------Attribute----------------------
                //for selecting attributeGroups for a product
                foreach (var atrgrp in await _attributeGrpService.GetByProductGroupId(product.ProductGroupId))
                {
                    if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()].Any())
                    {
                        product.Product_Attribut.Add(new Product_Attribut()
                        {
                            AttributItemId = Convert.ToInt32(Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()]),
                            ProductId = product.ProductId,
                        });
                    }
                }

                //-----------------Details----------------------
                //getting all detail related to a group of product to fill out all details of this product
                foreach (var detItm in await _detailItemService.GetByProductGroupId(product))
                {
                    product.ProductDetail.Add(new ProductDetail()
                    {
                        DetailItemId = detItm.DetailItemId,
                        ProductId = product.ProductId,
                        Value = Request.Form["detItem_" + detItm.DetailItemId.ToString()]
                    });
                }

                await _productService.Add(product, galleyFiles, tags);
                return RedirectToAction(nameof(Edit), product.ProductId);
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, string galleyFiles, string tags)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //get request.form of ProductAttribute and edit
                    await AddAttributechanged(product);
                    //get request.form of ProductDetail and edit 
                    await AddDetailChanged(product);
                    //Update the Product entity and SaveAsync All Entities
                    await _productService.Edit(product, galleyFiles, tags);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _productService.ProductExsitence(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit),product.ProductId);
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetById(id);
            
            await _productService.Remove(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string fileName = "no-photo.png";
                var uploads = Path.Combine(_env.WebRootPath, "Upload\\ProductImages");

                if (file != null)
                {
                        fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        //---------------------resize Images ----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(128);
                        img.Resize(Path.Combine(uploads, fileName), Path.Combine(uploads, "thumbnail", fileName));
                }

                //saving new store and the images
                return Json(new { status = "Done", src = Path.Combine(uploads, fileName), ImageName = fileName });
            }
            return Json(new { status = "Error" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadGallery(List<IFormFile> files)
        {
            string names = "";
            var uploads = Path.Combine(_env.WebRootPath, "Upload\\ProductImages");

            foreach (var file in files)
            {
                string galleryname = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, galleryname), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                names += galleryname + ",";

                //---------------------resize Images ----------------------
                InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(350);
                img.Resize(Path.Combine(uploads, galleryname), Path.Combine(uploads, "thumbnail", galleryname));
            }
            //saving new store and the images
            return Json(new { status = "Done", ImagesName = names });
        }

        private async Task AddAttributechanged(Product product)
        {
            List<Product_Attribut> productAttribut = new List<Product_Attribut>();

            foreach (var atrgrp in await _attributeGrpService.GetByProductGroupId(product.ProductGroupId))
            {
                {
                    //finding product attribute if that is inserted before or not
                    var find = await _productAttributeService.GetProductAttribute(product.ProductId, atrgrp.AttributGrpId);

                    if (find != null)
                    {
                        if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()] == "none")
                        {
                            _productAttributeService.Remove(find);
                        }
                        //if user clicked one of attribute of this grp 
                        else if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()].Any())
                        {
                            find.AttributItemId = Convert.ToInt32(Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()]);
                            _productAttributeService.Edit(find);
                        }
                    }
                    else
                    {
                        if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()].Any() && Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()] != "none")
                        {
                            productAttribut.Add(new Product_Attribut()
                            {
                                AttributItemId = Convert.ToInt32(Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()]),
                                ProductId = product.ProductId,
                            });
                        }
                    }
                }
            }
            if (productAttribut != null)
                _productAttributeService.Add(productAttribut);
        }

        private async Task AddDetailChanged(Product product)
        {
            List<ProductDetail> productDetail = new List<ProductDetail>();
            foreach (var detItm in await _detailItemService.GetByProductGroupId(product))
            {
                var find = await _productDetailService.GetProductDetail(product.ProductId, detItm.DetailItemId);
                if (find != null)
                {
                    find.Value = Request.Form["detItem_" + detItm.DetailItemId.ToString()];
                    _productDetailService.Edit(find);
                }
                else
                {
                    productDetail.Add(new ProductDetail
                    {
                        DetailItemId = detItm.DetailItemId,
                        ProductId = product.ProductId,
                        Value = Request.Form["detItem_" + detItm.DetailItemId.ToString()]
                    });
                }
            }
            if (productDetail != null)
                _productDetailService.Add(productDetail);
        }
    }
}
