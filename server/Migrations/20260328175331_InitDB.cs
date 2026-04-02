using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ColorHex = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardColumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    BoardId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardColumns_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ColumnId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    AssignedUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tasks_BoardColumns_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "BoardColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskTag",
                columns: table => new
                {
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskTag", x => new { x.TagsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_ProjectTaskTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTaskTag_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "role-admin-id", null, "Admin", "ADMIN" },
                    { "role-user-id", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-admin-1", 0, "e8fa7cc5-0701-4c22-9d55-b23f0333459a", "working@keter.ro", false, null, false, null, "WORKING@KETER.RO", "WORKING_RACCOON", "AQAAAAIAAYagAAAAEFpnQVi/Qveb/rBOG29Ak+iopKy6PtqJulgQe5MF/gmJrWYdjH5WuvFlUzBJUn1yWw==", null, false, "/../avatars/WorkingRaccoon.png", "93328776-7284-4cf0-b6e4-81df7add9606", false, "working_raccoon" },
                    { "user-member-1", 0, "74f9f2d1-c2ef-4781-bd71-40044854418c", "money@keter.ro", false, null, false, null, "MONEY@KETER.RO", "MONEY_RACCOON", "AQAAAAIAAYagAAAAELu9mrdPne73eamn31VPMoyIlX3sQvKDUW8gJWg31mVqHDydWRsaUJWD9oq0lcN3qA==", null, false, "/../avatars/CardRaccoon.png", "057d7c40-8839-4a76-b01f-70aed706e197", false, "money_raccoon" },
                    { "user-member-2", 0, "fc0a66e1-705e-4f00-94b5-ae772706caad", "cool@keter.ro", false, null, false, null, "COOL@KETER.RO", "COOL_RACCOON", "AQAAAAIAAYagAAAAEJ/vDdlno5YFmO4GQsUkFSCVlm4SKvG8HXB/7niElywkuqjFrI5M+WfaDZvpPOryTQ==", null, false, "/../avatars/coolRaccoon.png", "ca6b7980-4207-462c-9640-f401b7d40f52", false, "cool_raccoon" },
                    { "user-member-3", 0, "da692d93-83f5-4b21-8971-f98761176a35", "bath@keter.ro", false, null, false, null, "BATH@KETER.RO", "BATH_RACCOON", "AQAAAAIAAYagAAAAEPOnKazJ+kDZ7yzDRudiJEMVpSDhdf8frWGinggcPjLqReAIGvDc1JmSvm49EzUZDQ==", null, false, "/../avatars/BathRaccoon.png", "d2fab3f3-b36c-4b3d-852b-8885d938ebb3", false, "bath_raccoon" },
                    { "user-member-4", 0, "3c30b329-a2ea-4993-b033-1ea2800c0c54", "banana@keter.ro", false, null, false, null, "BANANA@KETER.RO", "BANANA_RACCOON", "AQAAAAIAAYagAAAAEN+b4fywr7HW1F0qBwgheaEbwY0OBWanlAPGoP6pyxNFMTfzuVVq1G7vKnG7B3UIMg==", null, false, "/../avatars/BananaRaccoon.png", "3dac9d85-5dfa-43c5-9136-01503a24421b", false, "banana_raccoon" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "role-admin-id", "user-admin-1" },
                    { "role-user-id", "user-member-1" },
                    { "role-user-id", "user-member-2" },
                    { "role-user-id", "user-member-3" },
                    { "role-user-id", "user-member-4" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "OwnerId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 3, 28, 17, 53, 30, 910, DateTimeKind.Utc).AddTicks(2430), "Proiect pentru facultate", "Platforma Keter", "user-admin-1" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), new DateTime(2026, 3, 28, 17, 53, 30, 910, DateTimeKind.Utc).AddTicks(2430), "E-commerce cu React și C#", "Magazin Online", "user-admin-1" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), new DateTime(2026, 3, 28, 17, 53, 30, 910, DateTimeKind.Utc).AddTicks(2440), "Aplicație de fitness", "Aplicație Mobilă", "user-admin-1" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), new DateTime(2026, 3, 28, 17, 53, 30, 910, DateTimeKind.Utc).AddTicks(2440), "ERP pentru firmă locală", "Sistem Gestiune", "user-admin-1" }
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name", "ProjectId" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222221"), "Backend C#", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Frontend React", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222223"), "Dezvoltare Magazin", new Guid("11111111-1111-1111-1111-111111111112") },
                    { new Guid("22222222-2222-2222-2222-222222222224"), "Marketing", new Guid("11111111-1111-1111-1111-111111111112") },
                    { new Guid("22222222-2222-2222-2222-222222222225"), "iOS App", new Guid("11111111-1111-1111-1111-111111111113") },
                    { new Guid("22222222-2222-2222-2222-222222222226"), "Android App", new Guid("11111111-1111-1111-1111-111111111113") },
                    { new Guid("22222222-2222-2222-2222-222222222227"), "Implementare ERP", new Guid("11111111-1111-1111-1111-111111111114") }
                });

            migrationBuilder.InsertData(
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "user-member-1" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), "user-member-2" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), "user-member-3" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), "user-member-4" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "user-member-1" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "user-member-3" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "user-member-2" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "user-member-4" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "user-member-1" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ColorHex", "Name", "ProjectId" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555501"), "#E27893", "Important", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-5555-5555-5555-555555555502"), "#734FCF", "Programming", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-5555-5555-5555-555555555503"), "#EDCF8E", "UI/UX", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-5555-5555-5555-555555555504"), "#C28CAE", "Social Media", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-5555-5555-5555-555555555505"), "#D9D9D9", "Optional", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-5555-5555-5555-555555555506"), "#E27893", "Important", new Guid("11111111-1111-1111-1111-111111111112") },
                    { new Guid("55555555-5555-5555-5555-555555555507"), "#734FCF", "Programming", new Guid("11111111-1111-1111-1111-111111111112") },
                    { new Guid("55555555-5555-5555-5555-555555555508"), "#EDCF8E", "UI/UX", new Guid("11111111-1111-1111-1111-111111111112") },
                    { new Guid("55555555-5555-5555-5555-555555555509"), "#C28CAE", "Social Media", new Guid("11111111-1111-1111-1111-111111111112") },
                    { new Guid("55555555-5555-5555-5555-555555555510"), "#D9D9D9", "Optional", new Guid("11111111-1111-1111-1111-111111111112") },
                    { new Guid("55555555-5555-5555-5555-555555555511"), "#E27893", "Important", new Guid("11111111-1111-1111-1111-111111111113") },
                    { new Guid("55555555-5555-5555-5555-555555555512"), "#734FCF", "Programming", new Guid("11111111-1111-1111-1111-111111111113") },
                    { new Guid("55555555-5555-5555-5555-555555555513"), "#EDCF8E", "UI/UX", new Guid("11111111-1111-1111-1111-111111111113") },
                    { new Guid("55555555-5555-5555-5555-555555555514"), "#C28CAE", "Social Media", new Guid("11111111-1111-1111-1111-111111111113") },
                    { new Guid("55555555-5555-5555-5555-555555555515"), "#D9D9D9", "Optional", new Guid("11111111-1111-1111-1111-111111111113") },
                    { new Guid("55555555-5555-5555-5555-555555555516"), "#E27893", "Important", new Guid("11111111-1111-1111-1111-111111111114") },
                    { new Guid("55555555-5555-5555-5555-555555555517"), "#734FCF", "Programming", new Guid("11111111-1111-1111-1111-111111111114") },
                    { new Guid("55555555-5555-5555-5555-555555555518"), "#EDCF8E", "UI/UX", new Guid("11111111-1111-1111-1111-111111111114") },
                    { new Guid("55555555-5555-5555-5555-555555555519"), "#C28CAE", "Social Media", new Guid("11111111-1111-1111-1111-111111111114") },
                    { new Guid("55555555-5555-5555-5555-555555555520"), "#D9D9D9", "Optional", new Guid("11111111-1111-1111-1111-111111111114") }
                });

            migrationBuilder.InsertData(
                table: "BoardColumns",
                columns: new[] { "Id", "BoardId", "Order", "Title" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444411"), new Guid("22222222-2222-2222-2222-222222222221"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444412"), new Guid("22222222-2222-2222-2222-222222222221"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444413"), new Guid("22222222-2222-2222-2222-222222222221"), 2, "Done" },
                    { new Guid("44444444-4444-4444-4444-444444444414"), new Guid("22222222-2222-2222-2222-222222222222"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444415"), new Guid("22222222-2222-2222-2222-222222222222"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444416"), new Guid("22222222-2222-2222-2222-222222222222"), 2, "Done" },
                    { new Guid("44444444-4444-4444-4444-444444444417"), new Guid("22222222-2222-2222-2222-222222222223"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444418"), new Guid("22222222-2222-2222-2222-222222222223"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444419"), new Guid("22222222-2222-2222-2222-222222222223"), 2, "Done" },
                    { new Guid("44444444-4444-4444-4444-444444444420"), new Guid("22222222-2222-2222-2222-222222222224"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444421"), new Guid("22222222-2222-2222-2222-222222222224"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444422"), new Guid("22222222-2222-2222-2222-222222222224"), 2, "Done" },
                    { new Guid("44444444-4444-4444-4444-444444444423"), new Guid("22222222-2222-2222-2222-222222222225"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444424"), new Guid("22222222-2222-2222-2222-222222222225"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444425"), new Guid("22222222-2222-2222-2222-222222222225"), 2, "Done" },
                    { new Guid("44444444-4444-4444-4444-444444444426"), new Guid("22222222-2222-2222-2222-222222222226"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444427"), new Guid("22222222-2222-2222-2222-222222222226"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444428"), new Guid("22222222-2222-2222-2222-222222222226"), 2, "Done" },
                    { new Guid("44444444-4444-4444-4444-444444444429"), new Guid("22222222-2222-2222-2222-222222222227"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444430"), new Guid("22222222-2222-2222-2222-222222222227"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444431"), new Guid("22222222-2222-2222-2222-222222222227"), 2, "Done" }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssignedUserId", "ColumnId", "Deadline", "Description", "Order", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333301"), "user-member-1", new Guid("44444444-4444-4444-4444-444444444411"), null, null, 0, "To Do", "Implementare JWT Token" },
                    { new Guid("33333333-3333-3333-3333-333333333302"), "user-member-2", new Guid("44444444-4444-4444-4444-444444444411"), null, null, 1, "To Do", "Creare endpoint Login" },
                    { new Guid("33333333-3333-3333-3333-333333333303"), "user-admin-1", new Guid("44444444-4444-4444-4444-444444444411"), null, null, 2, "To Do", "Securizare rute API" },
                    { new Guid("33333333-3333-3333-3333-333333333304"), "user-member-3", new Guid("44444444-4444-4444-4444-444444444412"), null, null, 0, "In Progress", "Creare DbContext" },
                    { new Guid("33333333-3333-3333-3333-333333333305"), "user-member-4", new Guid("44444444-4444-4444-4444-444444444412"), null, null, 1, "In Progress", "Migrari baza de date" },
                    { new Guid("33333333-3333-3333-3333-333333333306"), "user-member-1", new Guid("44444444-4444-4444-4444-444444444413"), null, null, 0, "Done", "Configurare Docker" },
                    { new Guid("33333333-3333-3333-3333-333333333307"), "user-member-3", new Guid("44444444-4444-4444-4444-444444444414"), null, null, 0, "To Do", "Design UI in SCSS" },
                    { new Guid("33333333-3333-3333-3333-333333333308"), "user-member-4", new Guid("44444444-4444-4444-4444-444444444414"), null, null, 1, "To Do", "Integrare Axios" },
                    { new Guid("33333333-3333-3333-3333-333333333309"), "user-admin-1", new Guid("44444444-4444-4444-4444-444444444415"), null, null, 0, "In Progress", "Componenta BoardCard" },
                    { new Guid("33333333-3333-3333-3333-333333333310"), "user-member-1", new Guid("44444444-4444-4444-4444-444444444415"), null, null, 1, "In Progress", "Meniu de navigare" },
                    { new Guid("33333333-3333-3333-3333-333333333311"), "user-member-2", new Guid("44444444-4444-4444-4444-444444444415"), null, null, 2, "In Progress", "Grafic statistic (CSS Grid)" },
                    { new Guid("33333333-3333-3333-3333-333333333312"), "user-member-3", new Guid("44444444-4444-4444-4444-444444444416"), null, null, 0, "Done", "Setup initial Vite" },
                    { new Guid("33333333-3333-3333-3333-333333333313"), "user-member-1", new Guid("44444444-4444-4444-4444-444444444417"), null, null, 0, "To Do", "Integrare Stripe pentru plati" },
                    { new Guid("33333333-3333-3333-3333-333333333314"), "user-member-3", new Guid("44444444-4444-4444-4444-444444444419"), null, null, 0, "Done", "Design Pagina de Produs" },
                    { new Guid("33333333-3333-3333-3333-333333333315"), "user-member-4", new Guid("44444444-4444-4444-4444-444444444420"), null, null, 0, "To Do", "Setup Campanie Facebook Ads" },
                    { new Guid("33333333-3333-3333-3333-333333333316"), "user-member-2", new Guid("44444444-4444-4444-4444-444444444421"), null, null, 0, "In Progress", "Design bannere promovare" },
                    { new Guid("33333333-3333-3333-3333-333333333317"), "user-member-3", new Guid("44444444-4444-4444-4444-444444444423"), null, null, 0, "To Do", "Integrare Apple Login" },
                    { new Guid("33333333-3333-3333-3333-333333333318"), "user-admin-1", new Guid("44444444-4444-4444-4444-444444444425"), null, null, 0, "Done", "Setup initial proiect Swift" },
                    { new Guid("33333333-3333-3333-3333-333333333319"), "user-member-1", new Guid("44444444-4444-4444-4444-444444444427"), null, null, 0, "In Progress", "Push notifications cu Firebase" },
                    { new Guid("33333333-3333-3333-3333-333333333320"), "user-member-4", new Guid("44444444-4444-4444-4444-444444444429"), null, null, 0, "To Do", "Dezvoltare Modul HR" }
                });

            migrationBuilder.InsertData(
                table: "ProjectTaskTag",
                columns: new[] { "TagsId", "TasksId" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555501"), new Guid("33333333-3333-3333-3333-333333333301") },
                    { new Guid("55555555-5555-5555-5555-555555555501"), new Guid("33333333-3333-3333-3333-333333333304") },
                    { new Guid("55555555-5555-5555-5555-555555555502"), new Guid("33333333-3333-3333-3333-333333333301") },
                    { new Guid("55555555-5555-5555-5555-555555555503"), new Guid("33333333-3333-3333-3333-333333333307") },
                    { new Guid("55555555-5555-5555-5555-555555555512"), new Guid("33333333-3333-3333-3333-333333333318") },
                    { new Guid("55555555-5555-5555-5555-555555555515"), new Guid("33333333-3333-3333-3333-333333333318") },
                    { new Guid("55555555-5555-5555-5555-555555555519"), new Guid("33333333-3333-3333-3333-333333333315") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoardColumns_BoardId",
                table: "BoardColumns",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_ProjectId",
                table: "Boards",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskTag_TasksId",
                table: "ProjectTaskTag",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ProjectId",
                table: "Tags",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedUserId",
                table: "Tasks",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ColumnId",
                table: "Tasks",
                column: "ColumnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "ProjectTaskTag");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "BoardColumns");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
