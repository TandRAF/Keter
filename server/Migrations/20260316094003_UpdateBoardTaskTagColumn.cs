using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBoardTaskTagColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Boards_BoardId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "Tasks",
                newName: "ColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_BoardId",
                table: "Tasks",
                newName: "IX_Tasks_ColumnId");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ColorHex = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "266b0ca5-bc8f-4801-8127-4e55a74b573a", "AQAAAAIAAYagAAAAEFP3jl+a2B90QeNITqNc+I6sKLY4AoZg8PU3Gh9YuNpJj7OZAYGuthIXHmGJaTWbeQ==", "18206443-e478-4391-bf9d-2d513265b3db" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c6083cf-c99c-47da-8555-9ce339b841da", "AQAAAAIAAYagAAAAENmGd4zFirIqAt575N2ye24miisf/lCl2JWf6TT0wT8QOU5Iu7epRbQwJ1IGxLGxRQ==", "4ae1db07-a152-47f6-ac48-ca4a8d3dfbef" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c4febbe3-683c-49f6-a3da-de0590fa4b54", "AQAAAAIAAYagAAAAEEUgDxTmY6nR87aA7gOTi/cKcqq1oWUf67ymF16ZXYCuJTAHU8PIvrhgA1PXW3f3Qw==", "2e7a44ce-aaa6-482d-9c0a-68249e925a43" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "daa76403-1231-4e0e-8a52-7cbf7f531ab8", "AQAAAAIAAYagAAAAEKr1dInUQW9kuFgybcG+Wtb4Bs/KAP1a2I921atX3mi2KeC2Nnv4Jj/pq5mHz6bL/Q==", "7aadf32c-27bb-4f09-b677-66ab763bc6bf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6299ce2a-aa22-495b-b2ad-a5021f4e157d", "AQAAAAIAAYagAAAAEFC7/OySRM4eF3ran984lRjHhzKAqmwa9u2YQJfbJ3r9tApqyPBc4tqSoiaDVkU/Fg==", "75d17af1-6b78-405a-b509-0adca77103c1" });

            migrationBuilder.InsertData(
                table: "BoardColumns",
                columns: new[] { "Id", "BoardId", "Order", "Title" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444411"), new Guid("22222222-2222-2222-2222-222222222221"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444412"), new Guid("22222222-2222-2222-2222-222222222221"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444413"), new Guid("22222222-2222-2222-2222-222222222221"), 2, "Done" },
                    { new Guid("44444444-4444-4444-4444-444444444421"), new Guid("22222222-2222-2222-2222-222222222222"), 0, "To Do" },
                    { new Guid("44444444-4444-4444-4444-444444444422"), new Guid("22222222-2222-2222-2222-222222222222"), 1, "In Progress" },
                    { new Guid("44444444-4444-4444-4444-444444444423"), new Guid("22222222-2222-2222-2222-222222222222"), 2, "Done" }
                });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 16, 9, 40, 3, 414, DateTimeKind.Utc).AddTicks(1290));

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ColorHex", "Name" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555551"), "#3b82f6", "Feature" },
                    { new Guid("55555555-5555-5555-5555-555555555552"), "#ef4444", "Bug" },
                    { new Guid("55555555-5555-5555-5555-555555555553"), "#10b981", "DevOps" }
                });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333331"),
                columns: new[] { "ColumnId", "Order" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444413"), 0 });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333332"),
                columns: new[] { "ColumnId", "Order" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444412"), 0 });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ColumnId", "Order" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444421"), 0 });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "ColumnId", "Order" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444421"), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_BoardColumns_BoardId",
                table: "BoardColumns",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskTag_TasksId",
                table: "ProjectTaskTag",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_BoardColumns_ColumnId",
                table: "Tasks",
                column: "ColumnId",
                principalTable: "BoardColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_BoardColumns_ColumnId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "BoardColumns");

            migrationBuilder.DropTable(
                name: "ProjectTaskTag");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ColumnId",
                table: "Tasks",
                newName: "BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ColumnId",
                table: "Tasks",
                newName: "IX_Tasks_BoardId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f655376f-852f-48ae-a8c9-f20cf1537518", "AQAAAAIAAYagAAAAEEC3ho9PBa13GirDHJiGgM3oNKVtFtVEqJXBAUc+RIYFnBHE8CcM33iZqxUnpMOpYQ==", "cca7afad-bc7a-42b0-9845-b37e234c9fd9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4356a288-c4ab-4ab3-8201-7a7be9f594b5", "AQAAAAIAAYagAAAAEJUUKDfawQnH0x5rpBBzq1mPdj256s8OjahXAbytcSQgoF4YdJYVF5elhBf9fFscLw==", "819f1c27-d70a-44cb-8ea2-ef1bf41ab086" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9e68242-0011-436e-a9ef-c6379d61b0af", "AQAAAAIAAYagAAAAEFOAf3RqnM/e2TRxUj7LoBWSCVqKOJejt7KuUsbAC4+DXCsurNINTdpcbR2lHnAEQQ==", "50622e43-fa91-42dd-8714-9a32fe5ff930" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d010aeac-d5a8-457d-a691-ea3debc4041f", "AQAAAAIAAYagAAAAENermC68HTBeugMXt3OSoGDtmXCKelWnahv59X56hmJ8EWT1MJ1L7X2AVSH7xgHogw==", "491079c1-eb36-4f34-abaf-1292e020d9aa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-member-4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9cbfd700-bfef-42a9-941f-72db09a25737", "AQAAAAIAAYagAAAAEJhwPWT6aOBRO74Vr7rnwQu79JRpEAUBhmD+aNvpQNwU/hmcKjYf9iHFQ/V7Q7A4RQ==", "3a083c4f-a8ea-463f-a708-3ddc12fbc788" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 3, 11, 13, 36, 8, 636, DateTimeKind.Utc).AddTicks(6290));

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333331"),
                columns: new[] { "BoardId", "Status" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222221"), "Done" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333332"),
                columns: new[] { "BoardId", "Status" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222221"), "InProgress" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "BoardId", "Status" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "Todo" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"),
                columns: new[] { "BoardId", "Status" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "Todo" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Boards_BoardId",
                table: "Tasks",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
