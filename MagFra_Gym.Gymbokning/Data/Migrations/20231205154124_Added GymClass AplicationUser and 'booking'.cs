using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFra_Gym.Gymbokning.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedGymClassAplicationUserandbooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GymClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserGymClass",
                columns: table => new
                {
                    gymClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    applicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserGymClass", x => new { x.gymClassId, x.applicationUserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserGymClass_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserGymClass_GymClass_gymClassId",
                        column: x => x.gymClassId,
                        principalTable: "GymClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGymClass_applicationUserId",
                table: "ApplicationUserGymClass",
                column: "applicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserGymClass");

            migrationBuilder.DropTable(
                name: "GymClass");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
