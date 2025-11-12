using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShippingAddress_EmailAddress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShippingAddress_AddressLine = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ShippingAddress_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingAddress_State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingAddress_ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BillingAddress_EmailAddress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BillingAddress_AddressLine = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BillingAddress_Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BillingAddress_State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BillingAddress_ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Payment_CardName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Payment_CardNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Payment_Expiration = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Payment_CVV = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "order",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "order",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderName",
                schema: "order",
                table: "Orders",
                column: "OrderName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "order");
        }
    }
}
