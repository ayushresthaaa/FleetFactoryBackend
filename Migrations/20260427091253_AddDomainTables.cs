using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetFactoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CreditBalance = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PartCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    HiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Make = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_CustomerProfiles_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    CostPrice = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    StockQty = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parts_PartCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PartCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parts_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedById = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvoiceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: true),
                    Subtotal = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    DiscountPct = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DiscountReason = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedById = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_CustomerProfiles_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoices_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    MovementType = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovements_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoiceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseInvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    ReceivedQuantity = table.Column<int>(type: "integer", nullable: true),
                    ActualUnitCost = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceItems_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceItems_PurchaseInvoices_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoiceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesInvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceItems_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceItems_SalesInvoices_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartCategories_Name",
                table: "PartCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CategoryId",
                table: "Parts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_Sku",
                table: "Parts",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_VendorId",
                table: "Parts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceItems_PartId",
                table: "PurchaseInvoiceItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceItems_PurchaseInvoiceId",
                table: "PurchaseInvoiceItems",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_CreatedById",
                table: "PurchaseInvoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_InvoiceNo",
                table: "PurchaseInvoices",
                column: "InvoiceNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_VendorId",
                table: "PurchaseInvoices",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceItems_PartId",
                table: "SalesInvoiceItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceItems_SalesInvoiceId",
                table: "SalesInvoiceItems",
                column: "SalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_CreatedById",
                table: "SalesInvoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_CustomerId",
                table: "SalesInvoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_InvoiceNo",
                table: "SalesInvoices",
                column: "InvoiceNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_VehicleId",
                table: "SalesInvoices",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_UserId",
                table: "StaffProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CreatedById",
                table: "StockMovements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_PartId",
                table: "StockMovements",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CustomerId",
                table: "Vehicles",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleNumber",
                table: "Vehicles",
                column: "VehicleNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PurchaseInvoiceItems");

            migrationBuilder.DropTable(
                name: "SalesInvoiceItems");

            migrationBuilder.DropTable(
                name: "StaffProfiles");

            migrationBuilder.DropTable(
                name: "StockMovements");

            migrationBuilder.DropTable(
                name: "PurchaseInvoices");

            migrationBuilder.DropTable(
                name: "SalesInvoices");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "PartCategories");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "CustomerProfiles");
        }
    }
}
