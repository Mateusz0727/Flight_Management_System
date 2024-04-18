using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight.Management.System.Data.Migrations
{
    public partial class add_registrationNumber_to_airplane_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Airplane",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Airplane");
        }
    }
}
