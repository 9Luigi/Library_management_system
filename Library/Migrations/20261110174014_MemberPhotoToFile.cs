using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <summary>
    /// Member photos move from the Photo BLOB column to files (PhotoStorage); the database keeps the file name.
    /// IMPORTANT: on a database with photos, export them to files BEFORE applying this migration —
    /// the Photo column is dropped here and its content is lost.
    /// </summary>
    public partial class MemberPhotoToFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Members",
                type: "TEXT",
                maxLength: 260,
                nullable: true);

            // Same naming rule as PhotoStorage.Save: {Id}.jpg for JPEG (starts with FF D8), otherwise {Id}.png
            migrationBuilder.Sql(
                """
                UPDATE "Members"
                SET "PhotoPath" = CAST("Id" AS TEXT) ||
                    CASE WHEN hex(substr("Photo", 1, 2)) = 'FFD8' THEN '.jpg' ELSE '.png' END
                WHERE "Photo" IS NOT NULL AND length("Photo") > 0;
                """);

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Members");
        }

        /// <inheritdoc />
        /// <remarks>Restores the column only; photo bytes are not copied back from the files.</remarks>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Members",
                type: "BLOB",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Members");
        }
    }
}
