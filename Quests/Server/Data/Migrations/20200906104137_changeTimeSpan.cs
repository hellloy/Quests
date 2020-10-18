using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quests.Server.Data.Migrations
{
    public partial class changeTimeSpan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TravelTime",
                table: "QuestSteps",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TravelTime",
                table: "Quests",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TravelTime",
                table: "QuestSteps",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TravelTime",
                table: "Quests",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
