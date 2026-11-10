using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <summary>
    /// Author photos and book covers move from BLOB columns to files (ImageStorage); the database keeps the file name.
    /// IMPORTANT: on a database with images, export them to files BEFORE applying this migration —
    /// the Photo / CoverImage columns are dropped here and their content is lost.
    /// </summary>
    public partial class AuthorBookImagesToFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Authors",
                type: "TEXT",
                maxLength: 260,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImagePath",
                table: "Books",
                type: "TEXT",
                maxLength: 260,
                nullable: true);

            // Same naming rule as ImageStorage.Save: {Id}.jpg for JPEG (starts with FF D8), otherwise {Id}.png
            migrationBuilder.Sql(
                """
                UPDATE "Authors"
                SET "PhotoPath" = CAST("Id" AS TEXT) ||
                    CASE WHEN hex(substr("Photo", 1, 2)) = 'FFD8' THEN '.jpg' ELSE '.png' END
                WHERE "Photo" IS NOT NULL AND length("Photo") > 0;
                """);

            migrationBuilder.Sql(
                """
                UPDATE "Books"
                SET "CoverImagePath" = CAST("Id" AS TEXT) ||
                    CASE WHEN hex(substr("CoverImage", 1, 2)) = 'FFD8' THEN '.jpg' ELSE '.png' END
                WHERE "CoverImage" IS NOT NULL AND length("CoverImage") > 0;
                """);

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Books");
        }

        /// <inheritdoc />
        /// <remarks>Restores the columns only; image bytes are not copied back from the files.</remarks>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Authors",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImage",
                table: "Books",
                type: "BLOB",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CoverImagePath",
                table: "Books");
        }
    }
}
