using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight.Management.System.Data.Migrations
{
    public partial class update_flight_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalPoint",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "DeparturePoint",
                table: "Flight");

            migrationBuilder.AddColumn<int>(
                name: "ArrivalPointId",
                table: "Flight",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeparturePointId",
                table: "Flight",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Airport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airport_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_ArrivalPointId",
                table: "Flight",
                column: "ArrivalPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_DeparturePointId",
                table: "Flight",
                column: "DeparturePointId");

            migrationBuilder.CreateIndex(
                name: "IX_Airport_CityId",
                table: "Airport",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Airport_ArrivalPointId",
                table: "Flight",
                column: "ArrivalPointId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Airport_DeparturePointId",
                table: "Flight",
                column: "DeparturePointId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Airport_ArrivalPointId",
                table: "Flight");

            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Airport_DeparturePointId",
                table: "Flight");

            migrationBuilder.DropTable(
                name: "Airport");

            migrationBuilder.DropIndex(
                name: "IX_Flight_ArrivalPointId",
                table: "Flight");

            migrationBuilder.DropIndex(
                name: "IX_Flight_DeparturePointId",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "ArrivalPointId",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "DeparturePointId",
                table: "Flight");

            migrationBuilder.AddColumn<string>(
                name: "ArrivalPoint",
                table: "Flight",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeparturePoint",
                table: "Flight",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
