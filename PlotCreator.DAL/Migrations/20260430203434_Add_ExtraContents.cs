using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlotCreator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_ExtraContents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtraContents",
                table: "Entities",
                type: "jsonb",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraContents",
                table: "Entities");
        }
    }
}
