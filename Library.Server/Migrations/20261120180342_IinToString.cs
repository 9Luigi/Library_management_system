using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Server.Migrations
{
    /// <inheritdoc />
    public partial class IinToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IIN",
                table: "Members",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IIN",
                table: "Members",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12);
        }
    }
}
