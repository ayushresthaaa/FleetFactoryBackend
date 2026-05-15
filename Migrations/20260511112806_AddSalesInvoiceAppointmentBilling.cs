using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetFactoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesInvoiceAppointmentBilling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_CustomerProfiles_CustomerId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Vehicles_VehicleId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_PartRequest_CustomerProfiles_CustomerId",
                table: "PartRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_PartRequest_Vehicles_VehicleId",
                table: "PartRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_CustomerProfiles_CustomerId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartRequest",
                table: "PartRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "PartRequest",
                newName: "PartRequests");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Review_CustomerId",
                table: "Reviews",
                newName: "IX_Reviews_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_PartRequest_VehicleId",
                table: "PartRequests",
                newName: "IX_PartRequests_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_PartRequest_CustomerId",
                table: "PartRequests",
                newName: "IX_PartRequests_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_VehicleId",
                table: "Appointments",
                newName: "IX_Appointments_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CustomerId",
                table: "Appointments",
                newName: "IX_Appointments_CustomerId");

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "SalesInvoices",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCharge",
                table: "SalesInvoices",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ServiceDescription",
                table: "SalesInvoices",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartRequests",
                table: "PartRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_AppointmentId",
                table: "SalesInvoices",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_CustomerProfiles_CustomerId",
                table: "Appointments",
                column: "CustomerId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Vehicles_VehicleId",
                table: "Appointments",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PartRequests_CustomerProfiles_CustomerId",
                table: "PartRequests",
                column: "CustomerId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartRequests_Vehicles_VehicleId",
                table: "PartRequests",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_CustomerProfiles_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Appointments_AppointmentId",
                table: "SalesInvoices",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_CustomerProfiles_CustomerId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Vehicles_VehicleId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_PartRequests_CustomerProfiles_CustomerId",
                table: "PartRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PartRequests_Vehicles_VehicleId",
                table: "PartRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_CustomerProfiles_CustomerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Appointments_AppointmentId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_AppointmentId",
                table: "SalesInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartRequests",
                table: "PartRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "ServiceCharge",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "ServiceDescription",
                table: "SalesInvoices");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "PartRequests",
                newName: "PartRequest");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CustomerId",
                table: "Review",
                newName: "IX_Review_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_PartRequests_VehicleId",
                table: "PartRequest",
                newName: "IX_PartRequest_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_PartRequests_CustomerId",
                table: "PartRequest",
                newName: "IX_PartRequest_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_VehicleId",
                table: "Appointment",
                newName: "IX_Appointment_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointment",
                newName: "IX_Appointment_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartRequest",
                table: "PartRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_CustomerProfiles_CustomerId",
                table: "Appointment",
                column: "CustomerId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Vehicles_VehicleId",
                table: "Appointment",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartRequest_CustomerProfiles_CustomerId",
                table: "PartRequest",
                column: "CustomerId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartRequest_Vehicles_VehicleId",
                table: "PartRequest",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_CustomerProfiles_CustomerId",
                table: "Review",
                column: "CustomerId",
                principalTable: "CustomerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
