
namespace CMS_NetCore.DataLayer
{
    using CMS_NetCore.DomainClasses;
    using CMS_NetCore.DataLayer.EntityConfigurations;
    using Microsoft.EntityFrameworkCore;


    public class AppDbContext : DbContext
    {
        // Your context has been configured to use a 'ApplicationDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Koshop.DataLayer.ApplicationDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ApplicationDbContext' 
        // connection string in the application configuration file.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public virtual DbSet<Address_User> Address_Users { get; set; }
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
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsGallery> NewsGalleries { get; set; }
        public virtual DbSet<NewsGroup> NewsGroups { get; set; }
        public virtual DbSet<NewsTag> NewsTags { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<ProductGallery> ProductGalleries { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<ProductTag> ProductTages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserStore> UserStores { get; set; }
        public virtual DbSet<AttributGrp> AttributGrps { get; set; }
        public virtual DbSet<AttributItem> AttributItems { get; set; }
        public virtual DbSet<chartPost> chartPosts { get; set; }
        public virtual DbSet<Product_Attribut> Product_Attributs { get; set; }
        public virtual DbSet<ProductRequest> ProductRequests { get; set; }
        public virtual DbSet<tbl_Help> tbl_Helps { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketGroup> TicketGroups { get; set; }
        public virtual DbSet<TicketMsg> TicketMsgs { get; set; }
        public virtual DbSet<StoreTime> StoreTimes { get; set; }
        public virtual DbSet<DetailItem> DetailItems { get; set; }
        public virtual DbSet<DetailGroup> DetailGroups { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<StoreInfo> StoreInfo { get; set; }
        public virtual DbSet<StoreFollower> StoreFollowers { get; set; }
        public virtual DbSet<StoresProduct> StoresProducts { get; set; }
        public virtual DbSet<RequestDetail> RequestDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new Address_UserConfig());
            builder.ApplyConfiguration(new AttributGrpConfig());
            builder.ApplyConfiguration(new chartPostConfig());
            builder.ApplyConfiguration(new CityConfig());
            builder.ApplyConfiguration(new ComponentConfig());
            builder.ApplyConfiguration(new ContactModuleConfig());
            builder.ApplyConfiguration(new DetailItemConfig());
            builder.ApplyConfiguration(new StoreInfoConfig());
            builder.ApplyConfiguration(new ModuleConfig());
            builder.ApplyConfiguration(new UserConfig());

            base.OnModelCreating(builder);

        }
    }
}

