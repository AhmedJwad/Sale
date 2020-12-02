using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sale.web.Migrations
{
    public partial class productimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "productImages",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "productImages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
