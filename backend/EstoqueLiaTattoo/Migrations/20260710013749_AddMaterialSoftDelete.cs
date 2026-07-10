using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstoqueLiaTattoo.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Material",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAtivo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsAtivo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsAtivo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsAtivo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsAtivo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Material",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsAtivo",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Material");
        }
    }
}
