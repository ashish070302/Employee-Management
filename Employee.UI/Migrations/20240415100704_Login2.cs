using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee.UI.Migrations
{
    /// <inheritdoc />
    public partial class Login2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemember",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Mobile",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Mobile",
                table: "Users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemember",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
