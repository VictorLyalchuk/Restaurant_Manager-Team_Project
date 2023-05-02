using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Entity.Migrations
{
    public partial class table_active : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Tables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 1,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 2,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 3,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 4,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 5,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 6,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 7,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 8,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 9,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "ID",
                keyValue: 10,
                column: "Active",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Tables");
        }
    }
}
