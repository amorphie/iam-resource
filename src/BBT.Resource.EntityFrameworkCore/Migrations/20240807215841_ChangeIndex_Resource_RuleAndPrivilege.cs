using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBT.Resource.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIndex_Resource_RuleAndPrivilege : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceRules",
                table: "ResourceRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourcePrivileges",
                table: "ResourcePrivileges");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "ResourceRules",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "ResourcePrivileges",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceRules",
                table: "ResourceRules",
                columns: new[] { "ResourceId", "RuleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourcePrivileges",
                table: "ResourcePrivileges",
                columns: new[] { "ResourceId", "Priority" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceRules",
                table: "ResourceRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourcePrivileges",
                table: "ResourcePrivileges");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "ResourceRules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "ResourcePrivileges",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceRules",
                table: "ResourceRules",
                columns: new[] { "ResourceId", "RuleId", "ClientId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourcePrivileges",
                table: "ResourcePrivileges",
                columns: new[] { "ResourceId", "Priority", "ClientId" });
        }
    }
}
