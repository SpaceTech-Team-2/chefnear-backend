using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChefNear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefundRequestedAtColumnInPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RefundRequestedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefundRequestedAt",
                table: "Payments");
        }
    }
}
