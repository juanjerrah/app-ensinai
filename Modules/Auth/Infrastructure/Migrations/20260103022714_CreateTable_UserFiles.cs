using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app_ensinai.Modules.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_UserFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "auth",
                table: "users",
                newName: "id");

            migrationBuilder.CreateTable(
                name: "user_files",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    purpose = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_files_File_file_id",
                        column: x => x.file_id,
                        principalSchema: "media",
                        principalTable: "files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_files_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_auth_user_files_user",
                schema: "auth",
                table: "user_files",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_auth_user_files_user_purpose",
                schema: "auth",
                table: "user_files",
                columns: new[] { "user_id", "purpose" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_files_file_id",
                schema: "auth",
                table: "user_files",
                column: "file_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_files",
                schema: "auth");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "auth",
                table: "users",
                newName: "Id");
        }
    }
}
