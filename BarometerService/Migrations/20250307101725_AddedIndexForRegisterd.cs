using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarometerService.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexForRegisterd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Measures_Registered",
                table: "Measures",
                column: "Registered");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Measures_Registered",
                table: "Measures");
        }
    }
}
