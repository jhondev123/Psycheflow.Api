using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Psycheflow.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixingDocumentFieldTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "DocumentFields");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "DocumentFields",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "DocumentFields",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FieldType",
                table: "DocumentFields",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
