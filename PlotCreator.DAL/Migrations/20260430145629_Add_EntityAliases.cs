using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlotCreator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_EntityAliases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Aliases",
                table: "Entities",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aliases",
                table: "Entities");
        }
    }
}
