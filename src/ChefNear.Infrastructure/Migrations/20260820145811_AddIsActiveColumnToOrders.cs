using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChefNear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumnToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Orders",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE\r\n    WHEN [Status] <> 'Delivered'\r\n     AND [Status] <> 'Cancelled'\r\n     AND [DeliveredAt] IS NULL\r\n     AND [CanceledAt] IS NULL\r\n    THEN CAST(1 AS bit)\r\n    ELSE CAST(0 AS bit)\r\nEND");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Orders");
        }
    }
}
