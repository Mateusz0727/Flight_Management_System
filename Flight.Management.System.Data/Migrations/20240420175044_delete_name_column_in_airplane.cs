﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight.Management.System.Data.Migrations
{
    public partial class delete_name_column_in_airplane : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Airplane");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Airplane",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
