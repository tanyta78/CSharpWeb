using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Intro.Data.Migrations
{
    public partial class ResourseLisensesChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Resources_ResourceId",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "ResourseId",
                table: "Licenses");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceId",
                table: "Licenses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Resources_ResourceId",
                table: "Licenses",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Resources_ResourceId",
                table: "Licenses");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceId",
                table: "Licenses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ResourseId",
                table: "Licenses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Resources_ResourceId",
                table: "Licenses",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
