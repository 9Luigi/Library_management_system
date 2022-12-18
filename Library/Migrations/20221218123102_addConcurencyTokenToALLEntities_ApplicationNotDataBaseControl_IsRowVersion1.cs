using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class addConcurencyTokenToALLEntitiesApplicationNotDataBaseControlIsRowVersion1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "coverImage",
                table: "Books",
                newName: "CoverImage");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Members",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[] { 0 },
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldDefaultValue: new byte[] { 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverImage",
                table: "Books",
                newName: "coverImage");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Members",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[] { 0 },
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldDefaultValue: new byte[] { 0 });
        }
    }
}
