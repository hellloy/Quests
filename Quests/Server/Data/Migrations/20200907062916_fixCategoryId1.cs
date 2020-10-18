using Microsoft.EntityFrameworkCore.Migrations;

namespace Quests.Server.Data.Migrations
{
    public partial class fixCategoryId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "QuestCategoryId",
                table: "Quests",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId",
                table: "Quests",
                column: "QuestCategoryId",
                principalTable: "QuestCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "QuestCategoryId",
                table: "Quests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId",
                table: "Quests",
                column: "QuestCategoryId",
                principalTable: "QuestCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
