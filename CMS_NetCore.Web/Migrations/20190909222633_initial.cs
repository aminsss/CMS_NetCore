using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS_NetCore.Web.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chartPosts",
                columns: table => new
                {
                    chartPostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Postduty = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chartPosts", x => x.chartPostId);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ComponentName = table.Column<string>(maxLength: 30, nullable: true),
                    ActionName = table.Column<string>(maxLength: 30, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 30, nullable: true),
                    Descroption = table.Column<string>(maxLength: 500, nullable: true),
                    AdminAction = table.Column<string>(maxLength: 30, nullable: true),
                    AdminController = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "MenuGroups",
                columns: table => new
                {
                    MenuGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuTitile = table.Column<string>(maxLength: 50, nullable: false),
                    MenuType = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuGroups", x => x.MenuGroupId);
                });

            migrationBuilder.CreateTable(
                name: "NewsGroups",
                columns: table => new
                {
                    NewsGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    GroupTitle = table.Column<string>(maxLength: 50, nullable: false),
                    Depth = table.Column<int>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    AliasName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsGroups", x => x.NewsGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PositionTitle = table.Column<string>(maxLength: 50, nullable: true),
                    PositionName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                });

            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    ProductGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    GroupTitle = table.Column<string>(nullable: false),
                    Depth = table.Column<int>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    AliasName = table.Column<string>(nullable: false),
                    type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.ProductGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleTitle = table.Column<string>(maxLength: 50, nullable: false),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateName = table.Column<string>(maxLength: 50, nullable: true),
                    StateIcon = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "TicketGroups",
                columns: table => new
                {
                    TicketGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(maxLength: 50, nullable: true),
                    chartPostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketGroups", x => x.TicketGroupId);
                    table.ForeignKey(
                        name: "FK_TicketGroups_chartPosts_chartPostId",
                        column: x => x.chartPostId,
                        principalTable: "chartPosts",
                        principalColumn: "chartPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    MenuId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuTitle = table.Column<string>(maxLength: 50, nullable: false),
                    PageName = table.Column<string>(maxLength: 50, nullable: false),
                    Depth = table.Column<int>(nullable: true),
                    Path = table.Column<string>(maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PageType = table.Column<string>(maxLength: 100, nullable: true),
                    PageContetnt = table.Column<string>(maxLength: 100, nullable: true),
                    MenuGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_Menus_MenuGroups_MenuGroupId",
                        column: x => x.MenuGroupId,
                        principalTable: "MenuGroups",
                        principalColumn: "MenuGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    ModuleTitle = table.Column<string>(maxLength: 50, nullable: true),
                    PositionId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Accisibility = table.Column<string>(nullable: true),
                    ComponentId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                    table.ForeignKey(
                        name: "FK_Modules_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modules_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttributGrps",
                columns: table => new
                {
                    AttributGrpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Attr_type = table.Column<string>(maxLength: 100, nullable: true),
                    ProductGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributGrps", x => x.AttributGrpId);
                    table.ForeignKey(
                        name: "FK_AttributGrps_ProductGroups_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroups",
                        principalColumn: "ProductGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailGroups",
                columns: table => new
                {
                    DetailGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ProductGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailGroups", x => x.DetailGroupId);
                    table.ForeignKey(
                        name: "FK_DetailGroups_ProductGroups_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroups",
                        principalColumn: "ProductGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: false),
                    ProductGroupId = table.Column<int>(nullable: false),
                    ProductTitle = table.Column<string>(maxLength: 100, nullable: false),
                    ProductImage = table.Column<string>(maxLength: 100, nullable: true),
                    AliasName = table.Column<string>(nullable: false),
                    GroupModel = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductGroups_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroups",
                        principalColumn: "ProductGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: true),
                    ActiveCode = table.Column<string>(maxLength: 100, nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    ISActive = table.Column<bool>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Profile = table.Column<string>(maxLength: 50, nullable: true),
                    MeliID = table.Column<string>(maxLength: 50, nullable: true),
                    BirthDate = table.Column<string>(maxLength: 50, nullable: true),
                    moblie = table.Column<string>(maxLength: 50, nullable: true),
                    phoneNo = table.Column<string>(maxLength: 50, nullable: true),
                    State = table.Column<int>(nullable: true),
                    City = table.Column<int>(nullable: true),
                    Adress = table.Column<string>(maxLength: 50, nullable: true),
                    chartPostId = table.Column<int>(nullable: true),
                    activecodeDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_chartPosts_chartPostId",
                        column: x => x.chartPostId,
                        principalTable: "chartPosts",
                        principalColumn: "chartPostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityName = table.Column<string>(maxLength: 30, nullable: true),
                    CityIcon = table.Column<byte[]>(nullable: true),
                    StateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactModules",
                columns: table => new
                {
                    ContactModuleId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PhoneNum = table.Column<string>(nullable: true),
                    MobileNum = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactModules", x => x.ContactModuleId);
                    table.ForeignKey(
                        name: "FK_ContactModules_Modules_ContactModuleId",
                        column: x => x.ContactModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HtmlModules",
                columns: table => new
                {
                    HtmlModuleId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    HtmlText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlModules", x => x.HtmlModuleId);
                    table.ForeignKey(
                        name: "FK_HtmlModules_Modules_HtmlModuleId",
                        column: x => x.HtmlModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuModules",
                columns: table => new
                {
                    MenuModuleId = table.Column<int>(nullable: false),
                    MenuGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuModules", x => x.MenuModuleId);
                    table.ForeignKey(
                        name: "FK_MenuModules_MenuGroups_MenuGroupId",
                        column: x => x.MenuGroupId,
                        principalTable: "MenuGroups",
                        principalColumn: "MenuGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuModules_Modules_MenuModuleId",
                        column: x => x.MenuModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModulePages",
                columns: table => new
                {
                    ModulePageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePages", x => x.ModulePageId);
                    table.ForeignKey(
                        name: "FK_ModulePages_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModulePages_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttributItems",
                columns: table => new
                {
                    AttributItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    idfilter = table.Column<string>(nullable: true),
                    AttributGrpId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributItems", x => x.AttributItemId);
                    table.ForeignKey(
                        name: "FK_AttributItems_AttributGrps_AttributGrpId",
                        column: x => x.AttributGrpId,
                        principalTable: "AttributGrps",
                        principalColumn: "AttributGrpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailItems",
                columns: table => new
                {
                    DetailItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DetailTitle = table.Column<string>(maxLength: 100, nullable: true),
                    DetailType = table.Column<string>(maxLength: 100, nullable: true),
                    DetailGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailItems", x => x.DetailItemId);
                    table.ForeignKey(
                        name: "FK_DetailItems_DetailGroups_DetailGroupId",
                        column: x => x.DetailGroupId,
                        principalTable: "DetailGroups",
                        principalColumn: "DetailGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductGalleries",
                columns: table => new
                {
                    ProductGalleryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGalleries", x => x.ProductGalleryId);
                    table.ForeignKey(
                        name: "FK_ProductGalleries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTages",
                columns: table => new
                {
                    ProductTagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    TagTitle = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTages", x => x.ProductTagId);
                    table.ForeignKey(
                        name: "FK_ProductTages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address_Users",
                columns: table => new
                {
                    Address_UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameFamily = table.Column<string>(maxLength: 100, nullable: true),
                    MobileNo = table.Column<string>(maxLength: 100, nullable: true),
                    HomeNo = table.Column<string>(maxLength: 100, nullable: true),
                    State = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    PostAddress = table.Column<string>(maxLength: 100, nullable: true),
                    postalCode = table.Column<string>(maxLength: 100, nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address_Users", x => x.Address_UserId);
                    table.ForeignKey(
                        name: "FK_Address_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    FromUser = table.Column<int>(nullable: true),
                    ToUser = table.Column<int>(nullable: true),
                    Subject = table.Column<string>(maxLength: 50, nullable: true),
                    ContentMessage = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    ISRead = table.Column<bool>(nullable: true),
                    SenderName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Users_FromUser",
                        column: x => x.FromUser,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ToUser",
                        column: x => x.ToUser,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    NewsGroupId = table.Column<int>(nullable: false),
                    NewsTitle = table.Column<string>(maxLength: 50, nullable: false),
                    NewsDescription = table.Column<string>(nullable: true),
                    NewsImage = table.Column<string>(maxLength: 150, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    AliasName = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_News_NewsGroups_NewsGroupId",
                        column: x => x.NewsGroupId,
                        principalTable: "NewsGroups",
                        principalColumn: "NewsGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_News_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    IsFinally = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRequests",
                columns: table => new
                {
                    ProductRequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NAme = table.Column<string>(nullable: true),
                    ProductGroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    status = table.Column<string>(maxLength: 150, nullable: true),
                    Descript = table.Column<string>(nullable: true),
                    Response = table.Column<string>(maxLength: 150, nullable: true),
                    details = table.Column<string>(maxLength: 150, nullable: true),
                    productID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequests", x => x.ProductRequestId);
                    table.ForeignKey(
                        name: "FK_ProductRequests_ProductGroups_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroups",
                        principalColumn: "ProductGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Helps",
                columns: table => new
                {
                    tbl_HelpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    kihasti = table.Column<bool>(nullable: true),
                    kojai = table.Column<bool>(nullable: true),
                    chidari = table.Column<bool>(nullable: true),
                    kala = table.Column<bool>(nullable: true),
                    pishkhan = table.Column<bool>(nullable: true),
                    firsttime = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Helps", x => x.tbl_HelpId);
                    table.ForeignKey(
                        name: "FK_tbl_Helps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    TicketGroupId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketGroups_TicketGroupId",
                        column: x => x.TicketGroupId,
                        principalTable: "TicketGroups",
                        principalColumn: "TicketGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<string>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    StoreName = table.Column<string>(maxLength: 50, nullable: false),
                    StoreAddress = table.Column<string>(maxLength: 300, nullable: false),
                    Descriptions = table.Column<string>(nullable: true),
                    StoreIcon = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    SiteName = table.Column<string>(maxLength: 30, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 25, nullable: false),
                    SiteAddress = table.Column<string>(maxLength: 250, nullable: true),
                    Favorite = table.Column<int>(nullable: true),
                    SeeStore = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Stores_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactPersons",
                columns: table => new
                {
                    ContactPersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactModuleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPersons", x => x.ContactPersonId);
                    table.ForeignKey(
                        name: "FK_ContactPersons_ContactModules_ContactModuleId",
                        column: x => x.ContactModuleId,
                        principalTable: "ContactModules",
                        principalColumn: "ContactModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactPersons_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Attributs",
                columns: table => new
                {
                    Product_AttributId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    isChecked = table.Column<bool>(nullable: true),
                    Value = table.Column<string>(maxLength: 100, nullable: true),
                    AttributItemId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Attributs", x => x.Product_AttributId);
                    table.ForeignKey(
                        name: "FK_Product_Attributs_AttributItems_AttributItemId",
                        column: x => x.AttributItemId,
                        principalTable: "AttributItems",
                        principalColumn: "AttributItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Attributs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    ProductDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(maxLength: 100, nullable: true),
                    DetailItemId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.ProductDetailId);
                    table.ForeignKey(
                        name: "FK_ProductDetails_DetailItems_DetailItemId",
                        column: x => x.DetailItemId,
                        principalTable: "DetailItems",
                        principalColumn: "DetailItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsGalleries",
                columns: table => new
                {
                    NewsGalleryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NewsId = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsGalleries", x => x.NewsGalleryId);
                    table.ForeignKey(
                        name: "FK_NewsGalleries_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsTags",
                columns: table => new
                {
                    NewsTagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NewsId = table.Column<int>(nullable: false),
                    TagsTitle = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTags", x => x.NewsTagId);
                    table.ForeignKey(
                        name: "FK_NewsTags_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCount = table.Column<int>(nullable: false),
                    ProductPrice = table.Column<int>(nullable: false),
                    Sum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestDetails",
                columns: table => new
                {
                    RequestDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    value = table.Column<string>(maxLength: 50, nullable: true),
                    DetailItemId = table.Column<int>(nullable: false),
                    ProductRequestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDetails", x => x.RequestDetailId);
                    table.ForeignKey(
                        name: "FK_RequestDetails_DetailItems_DetailItemId",
                        column: x => x.DetailItemId,
                        principalTable: "DetailItems",
                        principalColumn: "DetailItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestDetails_ProductRequests_ProductRequestId",
                        column: x => x.ProductRequestId,
                        principalTable: "ProductRequests",
                        principalColumn: "ProductRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketMsgs",
                columns: table => new
                {
                    TicketMsgId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    TicketId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMsgs", x => x.TicketMsgId);
                    table.ForeignKey(
                        name: "FK_TicketMsgs_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketMsgs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreFollowers",
                columns: table => new
                {
                    StoreFollowerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    StoreId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreFollowers", x => x.StoreFollowerId);
                    table.ForeignKey(
                        name: "FK_StoreFollowers_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreFollowers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreInfo",
                columns: table => new
                {
                    StoreInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>( nullable: true),
                    banner = table.Column<string>(maxLength: 50, nullable: true),
                    ZindexMap = table.Column<string>(maxLength: 50, nullable: true),
                    latitute = table.Column<double>(nullable: true),
                    lngitute = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreInfo", x => x.StoreInfoId);
                    table.ForeignKey(
                        name: "FK_StoreInfo_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoresProducts",
                columns: table => new
                {
                    StoresProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    StoreId = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: true),
                    OffPrice = table.Column<string>(maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Color = table.Column<string>(maxLength: 100, nullable: true),
                    linkSale = table.Column<string>(maxLength: 100, nullable: true),
                    detail = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoresProducts", x => x.StoresProductId);
                    table.ForeignKey(
                        name: "FK_StoresProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoresProducts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreTimes",
                columns: table => new
                {
                    StoreTimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<string>(nullable: true),
                    Days = table.Column<string>(maxLength: 50, nullable: true),
                    fromTime = table.Column<string>(maxLength: 50, nullable: true),
                    toTime = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTimes", x => x.StoreTimeId);
                    table.ForeignKey(
                        name: "FK_StoreTimes_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserStores",
                columns: table => new
                {
                    UserStoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    StoreId = table.Column<string>( nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStores", x => x.UserStoreId);
                    table.ForeignKey(
                        name: "FK_UserStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserStores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_Users_UserId",
                table: "Address_Users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributGrps_ProductGroupId",
                table: "AttributGrps",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributItems_AttributGrpId",
                table: "AttributItems",
                column: "AttributGrpId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_ContactModuleId",
                table: "ContactPersons",
                column: "ContactModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPersons_UserId",
                table: "ContactPersons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailGroups_ProductGroupId",
                table: "DetailGroups",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailItems_DetailGroupId",
                table: "DetailItems",
                column: "DetailGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuModules_MenuGroupId",
                table: "MenuModules",
                column: "MenuGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenuGroupId",
                table: "Menus",
                column: "MenuGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUser",
                table: "Messages",
                column: "FromUser");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToUser",
                table: "Messages",
                column: "ToUser");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePages_MenuId",
                table: "ModulePages",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePages_ModuleId",
                table: "ModulePages",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ComponentId",
                table: "Modules",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_PositionId",
                table: "Modules",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_News_NewsGroupId",
                table: "News",
                column: "NewsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_News_UserId",
                table: "News",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsGalleries_NewsId",
                table: "NewsGalleries",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTags_NewsId",
                table: "NewsTags",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Attributs_AttributItemId",
                table: "Product_Attributs",
                column: "AttributItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Attributs_ProductId",
                table: "Product_Attributs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_DetailItemId",
                table: "ProductDetails",
                column: "DetailItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequests_ProductGroupId",
                table: "ProductRequests",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequests_UserId",
                table: "ProductRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGroupId",
                table: "Products",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTages_ProductId",
                table: "ProductTages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetails_DetailItemId",
                table: "RequestDetails",
                column: "DetailItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetails_ProductRequestId",
                table: "RequestDetails",
                column: "ProductRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreFollowers_StoreId",
                table: "StoreFollowers",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreFollowers_UserId",
                table: "StoreFollowers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreInfo_StoreId",
                table: "StoreInfo",
                column: "StoreId",
                unique: true,
                filter: "[StoreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CityId",
                table: "Stores",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_StoresProducts_ProductId",
                table: "StoresProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoresProducts_StoreId",
                table: "StoresProducts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTimes_StoreId",
                table: "StoreTimes",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Helps_UserId",
                table: "tbl_Helps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketGroups_chartPostId",
                table: "TicketGroups",
                column: "chartPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMsgs_TicketId",
                table: "TicketMsgs",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMsgs_UserId",
                table: "TicketMsgs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketGroupId",
                table: "Tickets",
                column: "TicketGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_chartPostId",
                table: "Users",
                column: "chartPostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStores_StoreId",
                table: "UserStores",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStores_UserId",
                table: "UserStores",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address_Users");

            migrationBuilder.DropTable(
                name: "ContactPersons");

            migrationBuilder.DropTable(
                name: "HtmlModules");

            migrationBuilder.DropTable(
                name: "MenuModules");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ModulePages");

            migrationBuilder.DropTable(
                name: "NewsGalleries");

            migrationBuilder.DropTable(
                name: "NewsTags");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Product_Attributs");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "ProductGalleries");

            migrationBuilder.DropTable(
                name: "ProductTages");

            migrationBuilder.DropTable(
                name: "RequestDetails");

            migrationBuilder.DropTable(
                name: "StoreFollowers");

            migrationBuilder.DropTable(
                name: "StoreInfo");

            migrationBuilder.DropTable(
                name: "StoresProducts");

            migrationBuilder.DropTable(
                name: "StoreTimes");

            migrationBuilder.DropTable(
                name: "tbl_Helps");

            migrationBuilder.DropTable(
                name: "TicketMsgs");

            migrationBuilder.DropTable(
                name: "UserStores");

            migrationBuilder.DropTable(
                name: "ContactModules");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AttributItems");

            migrationBuilder.DropTable(
                name: "DetailItems");

            migrationBuilder.DropTable(
                name: "ProductRequests");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "MenuGroups");

            migrationBuilder.DropTable(
                name: "NewsGroups");

            migrationBuilder.DropTable(
                name: "AttributGrps");

            migrationBuilder.DropTable(
                name: "DetailGroups");

            migrationBuilder.DropTable(
                name: "TicketGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "chartPosts");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
