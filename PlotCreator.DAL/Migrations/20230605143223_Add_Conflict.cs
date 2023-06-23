using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlotCreator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_Conflict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Conflict",
                table: "Characters",
                type: "NText",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conflict",
                table: "Characters");
        }
    }
}
