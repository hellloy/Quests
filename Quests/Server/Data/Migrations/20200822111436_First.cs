using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quests.Server.Data.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveQuest",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Confidentiality",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Free = table.Column<bool>(nullable: false),
                    Img = table.Column<string>(nullable: false),
                    Distance = table.Column<int>(nullable: false),
                    TravelTime = table.Column<TimeSpan>(nullable: false),
                    MapCode = table.Column<string>(nullable: true),
                    VideoCode = table.Column<string>(nullable: true),
                    QuestCategoryId = table.Column<string>(nullable: true),
                    QuestCategoryId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_QuestCategories_QuestCategoryId1",
                        column: x => x.QuestCategoryId1,
                        principalTable: "QuestCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Img = table.Column<string>(nullable: false),
                    Distance = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    TravelTime = table.Column<TimeSpan>(nullable: false),
                    Hint = table.Column<string>(nullable: false),
                    Answer = table.Column<string>(nullable: false),
                    VideoCode = table.Column<string>(nullable: true),
                    MapCode = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: false),
                    StepNumber = table.Column<int>(nullable: false),
                    QuestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestSteps_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestCategoryId1",
                table: "Quests",
                column: "QuestCategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_QuestSteps_QuestId",
                table: "QuestSteps",
                column: "QuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestSteps");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "QuestCategories");

            migrationBuilder.DropColumn(
                name: "ActiveQuest",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Confidentiality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "AspNetUsers");
        }
    }
}
