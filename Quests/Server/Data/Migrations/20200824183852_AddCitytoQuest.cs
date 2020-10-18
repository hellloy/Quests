using Microsoft.EntityFrameworkCore.Migrations;

namespace Quests.Server.Data.Migrations
{
    public partial class AddCitytoQuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Quests",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Quests");
        }
    }
}
