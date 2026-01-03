using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app_ensinai.Modules.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_InterestAreaANDUserInterestArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "interest_areas",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interest_areas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_interest_areas",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    interest_area_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_interest_areas", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_interest_areas_interest_areas_interest_area_id",
                        column: x => x.interest_area_id,
                        principalSchema: "auth",
                        principalTable: "interest_areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_interest_areas_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_interest_areas_interest_area_id",
                schema: "auth",
                table: "user_interest_areas",
                column: "interest_area_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_interest_areas_user_id_interest_area_id",
                schema: "auth",
                table: "user_interest_areas",
                columns: new[] { "user_id", "interest_area_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_interest_areas",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "interest_areas",
                schema: "auth");
        }
    }
}
