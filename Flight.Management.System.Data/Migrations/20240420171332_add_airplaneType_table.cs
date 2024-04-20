using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight.Management.System.Data.Migrations
{
    public partial class add_airplaneType_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AirplaneTypeId",
                table: "Airplane",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AirplaneType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SymbolInRegistrationNumber = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    NumberOfSeats = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirplaneType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airplane_AirplaneTypeId",
                table: "Airplane",
                column: "AirplaneTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airplane_AirplaneType_AirplaneTypeId",
                table: "Airplane",
                column: "AirplaneTypeId",
                principalTable: "AirplaneType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airplane_AirplaneType_AirplaneTypeId",
                table: "Airplane");

            migrationBuilder.DropTable(
                name: "AirplaneType");

            migrationBuilder.DropIndex(
                name: "IX_Airplane_AirplaneTypeId",
                table: "Airplane");

            migrationBuilder.DropColumn(
                name: "AirplaneTypeId",
                table: "Airplane");
        }
    }
}
