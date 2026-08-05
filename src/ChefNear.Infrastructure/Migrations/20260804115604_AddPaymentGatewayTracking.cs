using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChefNear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentGatewayTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
                name: "IX_Payments_GatewayTransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GatewayTransactionId",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayTransactionId",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdempotencyKey",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayOrderId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_GatewayTransactionId",
                table: "Payments",
                column: "GatewayTransactionId",
                unique: true,
                filter: "[GatewayTransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IdempotencyKey",
                table: "Payments",
                column: "IdempotencyKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_GatewayTransactionId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_IdempotencyKey",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GatewayTransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IdempotencyKey",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayOrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Payments");
        }
    }
}
