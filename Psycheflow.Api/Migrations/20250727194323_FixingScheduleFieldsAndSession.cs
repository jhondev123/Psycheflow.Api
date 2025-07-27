using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Psycheflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixingScheduleFieldsAndSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Patients_PatientId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_PatientId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Schedules");

            migrationBuilder.AddColumn<int>(
                name: "SessionStatus",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleStatus",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleTypes",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionStatus",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ScheduleStatus",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ScheduleTypes",
                table: "Schedules");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Schedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_PatientId",
                table: "Schedules",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Patients_PatientId",
                table: "Schedules",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
