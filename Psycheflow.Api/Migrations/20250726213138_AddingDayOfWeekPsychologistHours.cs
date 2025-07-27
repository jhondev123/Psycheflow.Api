using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Psycheflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingDayOfWeekPsychologistHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "PsychologistsHours",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "PsychologistsHours",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "PsychologistsHours");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "PsychologistsHours",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
