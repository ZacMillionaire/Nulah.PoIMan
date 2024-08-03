using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nulah.PoIMan.Data.Migrations
{
    /// <inheritdoc />
    public partial class feature_discrimination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "feature_type",
                table: "Features",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "feature_type",
                table: "Features");
        }
    }
}
