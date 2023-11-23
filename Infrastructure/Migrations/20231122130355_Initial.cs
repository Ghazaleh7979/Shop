using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Product");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", unicode: false, nullable: false),
                    Username = table.Column<string>(type: "text", unicode: false, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", unicode: false, nullable: false),
                    InsertTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProduceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ManufacturePhone = table.Column<string>(type: "text", nullable: false),
                    ManufactureEmail = table.Column<string>(type: "text", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ManufactureEmail",
                schema: "Product",
                table: "Product",
                column: "ManufactureEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProduceDate",
                schema: "Product",
                table: "Product",
                column: "ProduceDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_UserId",
                schema: "Product",
                table: "Product",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                schema: "User",
                table: "User",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "User",
                schema: "User");
        }
    }
}
