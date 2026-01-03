using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app_ensinai.Modules.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndex_Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_active",
                schema: "auth",
                table: "users",
                column: "active");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "auth",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_profile_type",
                schema: "auth",
                table: "users",
                column: "profile_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_active",
                schema: "auth",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_email",
                schema: "auth",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_profile_type",
                schema: "auth",
                table: "users");
        }
    }
}
