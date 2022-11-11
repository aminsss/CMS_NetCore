using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using CMS_NetCore.ServiceLayer;

namespace CMS_NetCore.Web.Configs.Extentions
{
    public static class DIConfigExtention
    {
        public static IServiceCollection AddOurDIConfiguration(this IServiceCollection services)
        {

            services.AddScoped<IUserService, EfUserService>();
            services.AddScoped<IRoleService, EfRoleService>();
            services.AddScoped<IChartPost, EfChartPostService>();
            services.AddScoped<IProductGroupService, EfProductGroupService>();
            services.AddScoped<IAttributeGroupService, EfAttributeGroupService>();
            services.AddScoped<IAttributeItemService, EfAttributeItemService>();
            services.AddScoped<IMenuGroupService, EfMenuGroupService>();
            services.AddScoped<IMenuService, EfMenuService>();
            services.AddScoped<INewsGroupService, EfNewsGroupService>();
            services.AddScoped<INewsService, EfNewsService>();
            services.AddScoped<INewsTagService, EfNewsTagService>();
            services.AddScoped<INewsGalleryService, EfNewsGalleryService>();
            services.AddScoped<IDetailGroupService, EfDetailGroupService>();
            services.AddScoped<IDetailItemService, EfDetailItemService>();
            services.AddScoped<IProductService, EfProductService>();
            services.AddScoped<IProductTagService, EfProductTagService>();
            services.AddScoped<IProductGalleryService, EfProductGalleryService>();
            services.AddScoped<IProductDetailService, EfProductDetailService>();
            services.AddScoped<IProductAttributeService, EfProductAttributeService>();
            services.AddScoped<IOrderService, EfOrderService>();
            services.AddScoped<IOrderDetailService, EfOrderDetailService>();
            services.AddScoped<IMessageService, EfMessageService>();
            services.AddScoped<IModuleService, EfModuleService>();
            services.AddScoped<IPositionService, EfPositionService>();
            services.AddScoped<IComponentService, EfComponentService>();
            services.AddScoped<IModulePageService, EfModulePageService>();
            services.AddScoped<IHtmlModuleService, EfHtmlModuleService>();
            services.AddScoped<IMenuModuleService, EfMenuModuleService>();
            services.AddScoped<IContactModuleService, EfContactModuleService>();
            services.AddScoped<IContactPersonService, EfContactPersonService>();
            services.AddScoped<IMultiPictureModuleService, EfMultiPictureModuleService>();
            services.AddScoped<IMultiPictureItemService, EfMultiPictureItemService>();
            return services;
        }
    }
}
