using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetItDoneBro.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddedTasks : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ProjectUsers_KeycloakId",
            table: "ProjectUsers");

        migrationBuilder.DropIndex(
            name: "IX_ProjectUsers_ProjectId_KeycloakId",
            table: "ProjectUsers");

        migrationBuilder.DropColumn(
            name: "KeycloakId",
            table: "ProjectUsers");

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "ProjectUsers",
            type: "uuid",
            nullable: false,
            defaultValue: Guid.Empty);

        migrationBuilder.CreateTable(
            name: "TaskColumns",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                OrderIndex = table.Column<int>(type: "integer", nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TaskColumns", x => x.Id);
                table.ForeignKey(
                    name: "FK_TaskColumns_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ProjectTasks",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                TaskColumnId = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: false),
                AssignedToKeycloakId = table.Column<string>(type: "text", nullable: true),
                ImageUrl = table.Column<string>(type: "text", nullable: true),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectTasks", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProjectTasks_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProjectTasks_TaskColumns_TaskColumnId",
                    column: x => x.TaskColumnId,
                    principalTable: "TaskColumns",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProjectUsers_ProjectId_UserId",
            table: "ProjectUsers",
            columns: columnsArray,
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ProjectUsers_UserId",
            table: "ProjectUsers",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectTasks_ProjectId",
            table: "ProjectTasks",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectTasks_TaskColumnId",
            table: "ProjectTasks",
            column: "TaskColumnId");

        migrationBuilder.CreateIndex(
            name: "IX_TaskColumns_ProjectId",
            table: "TaskColumns",
            column: "ProjectId");
    }

    private static readonly string[] columns = new[] { "ProjectId", "KeycloakId" };
    private static readonly string[] columnsArray = new[] { "ProjectId", "UserId" };

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProjectTasks");

        migrationBuilder.DropTable(
            name: "TaskColumns");

        migrationBuilder.DropIndex(
            name: "IX_ProjectUsers_ProjectId_UserId",
            table: "ProjectUsers");

        migrationBuilder.DropIndex(
            name: "IX_ProjectUsers_UserId",
            table: "ProjectUsers");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "ProjectUsers");

        migrationBuilder.AddColumn<string>(
            name: "KeycloakId",
            table: "ProjectUsers",
            type: "character varying(255)",
            maxLength: 255,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectUsers_KeycloakId",
            table: "ProjectUsers",
            column: "KeycloakId");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectUsers_ProjectId_KeycloakId",
            table: "ProjectUsers",
            columns: columns,
            unique: true);
    }
}
