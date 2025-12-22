using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class photoNullNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Members",
                type: "VARBINARY(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldDefaultValue: new byte[] { 0 });

            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverImage",
                table: "Books",
                type: "VARBINARY(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Authors",
                type: "VARBINARY(MAX)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Members",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[] { 0 },
                oldClrType: typeof(byte[]),
                oldType: "VARBINARY(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "CoverImage",
                table: "Books",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "VARBINARY(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Authors",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "VARBINARY(MAX)",
                oldNullable: true);
        }
    }
}
