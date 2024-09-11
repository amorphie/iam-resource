using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBT.Resource.Migrations
{
    /// <inheritdoc />
    public partial class Policy_Feature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Effect = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Permissions = table.Column<string[]>(type: "text[]", nullable: true),
                    EvaluationOrder = table.Column<string[]>(type: "text[]", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ConflictResolution = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "N"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyConditions",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Roles = table.Column<string[]>(type: "text[]", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Time_Timezone = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Rules = table.Column<string[]>(type: "text[]", nullable: false),
                    Context = table.Column<string>(type: "jsonb", nullable: false),
                    Attributes = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyConditions", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_PolicyConditions_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourcePolicies",
                columns: table => new
                {
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcePolicies", x => new { x.ResourceId, x.PolicyId });
                    table.ForeignKey(
                        name: "FK_ResourcePolicies_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourcePolicies_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourcePolicies_PolicyId",
                table: "ResourcePolicies",
                column: "PolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyConditions");

            migrationBuilder.DropTable(
                name: "ResourcePolicies");

            migrationBuilder.DropTable(
                name: "Policies");
        }
    }
}
