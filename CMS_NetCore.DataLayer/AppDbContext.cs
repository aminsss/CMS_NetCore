using CMS_NetCore.DataLayer.EntityConfigurations;
using CMS_NetCore.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.DataLayer;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Component> Components { get; set; }
    public virtual DbSet<ContactModule> ContactModules { get; set; }
    public virtual DbSet<ContactPerson> ContactPersons { get; set; }
    public virtual DbSet<HtmlModule> HtmlModules { get; set; }
    public virtual DbSet<Menu> Menus { get; set; }
    public virtual DbSet<MenuGroup> MenuGroups { get; set; }
    public virtual DbSet<MenuModule> MenuModules { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<ModulePage> ModulePages { get; set; }
    public virtual DbSet<Module> Modules { get; set; }
    public virtual DbSet<MultiPictureModule> MultiPictureModules { get; set; }
    public virtual DbSet<MultiPictureItem> MultiPictureItems { get; set; }
    public virtual DbSet<News> News { get; set; }
    public virtual DbSet<NewsGallery> NewsGalleries { get; set; }
    public virtual DbSet<NewsGroup> NewsGroups { get; set; }
    public virtual DbSet<NewsTag> NewsTags { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Position> Positions { get; set; }
    public virtual DbSet<ProductGallery> ProductGalleries { get; set; }
    public virtual DbSet<ProductGroup> ProductGroups { get; set; }
    public virtual DbSet<ProductTag> ProductTags { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<Store> Stores { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserStore> UserStores { get; set; }
    public virtual DbSet<AttributeGroup> AttributeGroups { get; set; }
    public virtual DbSet<AttributeItem> AttributeItems { get; set; }
    public virtual DbSet<ChartPost> ChartPosts { get; set; }
    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }
    public virtual DbSet<ProductRequest> ProductRequests { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<TicketGroup> TicketGroups { get; set; }
    public virtual DbSet<TicketMessage> TicketMessages { get; set; }
    public virtual DbSet<StoreTime> StoreTimes { get; set; }
    public virtual DbSet<DetailItem> DetailItems { get; set; }
    public virtual DbSet<DetailGroup> DetailGroups { get; set; }
    public virtual DbSet<ProductDetail> ProductDetails { get; set; }
    public virtual DbSet<StoreInfo> StoreInfo { get; set; }
    public virtual DbSet<StoreFollower> StoreFollowers { get; set; }
    public virtual DbSet<StoreProduct> StoresProducts { get; set; }
    public virtual DbSet<RequestDetail> RequestDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserAddressConfig());
        builder.ApplyConfiguration(new AttributeGroupConfig());
        builder.ApplyConfiguration(new ChartPostConfig());
        builder.ApplyConfiguration(new CityConfig());
        builder.ApplyConfiguration(new ComponentConfig());
        builder.ApplyConfiguration(new ContactModuleConfig());
        builder.ApplyConfiguration(new DetailItemConfig());
        builder.ApplyConfiguration(new StoreInfoConfig());
        builder.ApplyConfiguration(new ModuleConfig());
        builder.ApplyConfiguration(new MultiPictureModuleConfig());
        builder.ApplyConfiguration(new UserConfig());

        base.OnModelCreating(builder);
    }
}