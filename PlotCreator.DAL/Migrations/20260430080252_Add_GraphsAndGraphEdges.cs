using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PlotCreator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_GraphsAndGraphEdges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Graphs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerUserId = table.Column<int>(type: "integer", nullable: false),
                    WorldId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsSystemDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Kind = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Graphs_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Graphs_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GraphId = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    X = table.Column<double>(type: "double precision", nullable: false),
                    Y = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphNodes_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphNodes_Graphs_GraphId",
                        column: x => x.GraphId,
                        principalTable: "Graphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphEdges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GraphId = table.Column<int>(type: "integer", nullable: false),
                    FromNodeId = table.Column<int>(type: "integer", nullable: false),
                    ToNodeId = table.Column<int>(type: "integer", nullable: false),
                    Direction = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Label = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphEdges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphEdges_GraphNodes_FromNodeId",
                        column: x => x.FromNodeId,
                        principalTable: "GraphNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphEdges_GraphNodes_ToNodeId",
                        column: x => x.ToNodeId,
                        principalTable: "GraphNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GraphEdges_Graphs_GraphId",
                        column: x => x.GraphId,
                        principalTable: "Graphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraphEdges_FromNodeId",
                table: "GraphEdges",
                column: "FromNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphEdges_GraphId",
                table: "GraphEdges",
                column: "GraphId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphEdges_GraphId_FromNodeId_ToNodeId",
                table: "GraphEdges",
                columns: new[] { "GraphId", "FromNodeId", "ToNodeId" });

            migrationBuilder.CreateIndex(
                name: "IX_GraphEdges_ToNodeId",
                table: "GraphEdges",
                column: "ToNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphNodes_EntityId",
                table: "GraphNodes",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphNodes_GraphId_EntityId",
                table: "GraphNodes",
                columns: new[] { "GraphId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Graphs_DefaultPerUserWorld",
                table: "Graphs",
                columns: new[] { "OwnerUserId", "WorldId" },
                unique: true,
                filter: "\"IsSystemDefault\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_Graphs_WorldId_OwnerUserId",
                table: "Graphs",
                columns: new[] { "WorldId", "OwnerUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraphEdges");

            migrationBuilder.DropTable(
                name: "GraphNodes");

            migrationBuilder.DropTable(
                name: "Graphs");
        }
    }
}
