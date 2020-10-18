using Microsoft.EntityFrameworkCore.Migrations;

namespace Quests.Server.Data.Migrations
{
    public partial class fixCategoryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId1",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_QuestCategoryId1",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "QuestCategoryId1",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "QuestCategoryId",
                table: "Quests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestCategoryId",
                table: "Quests",
                column: "QuestCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId",
                table: "Quests",
                column: "QuestCategoryId",
                principalTable: "QuestCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_QuestCategoryId",
                table: "Quests");

            migrationBuilder.AlterColumn<string>(
                name: "QuestCategoryId",
                table: "Quests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestCategoryId1",
                table: "Quests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestCategoryId1",
                table: "Quests",
                column: "QuestCategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_QuestCategories_QuestCategoryId1",
                table: "Quests",
                column: "QuestCategoryId1",
                principalTable: "QuestCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
