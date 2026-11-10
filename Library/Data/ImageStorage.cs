namespace Library.Data
{
	internal enum ImageKind
	{
		Member,
		Author,
		Book
	}

	/// <summary>
	/// Images (member/author photos, book covers) are stored as files in %LocalAppData%\Library\Images\{Members|Authors|Books};
	/// the database keeps only the file name. Ids of different entities overlap, hence one folder per kind.
	/// </summary>
	internal static class ImageStorage
	{
		internal static readonly string Root = Path.Combine(DbProviderConfig.DataFolder, "Images");

		internal static string Folder(ImageKind kind) => Path.Combine(Root, kind switch
		{
			ImageKind.Member => "Members",
			ImageKind.Author => "Authors",
			ImageKind.Book => "Books",
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
		});

		/// <summary>
		/// Saves the image as {id}.png / {id}.jpg and returns the file name to store in the database.
		/// </summary>
		internal static string Save(ImageKind kind, int id, byte[] imageBytes)
		{
			string folder = Folder(kind);
			Directory.CreateDirectory(folder);
			Delete(kind, id); // the previous image may have another extension

			string fileName = $"{id}{GetExtension(imageBytes)}";
			File.WriteAllBytes(Path.Combine(folder, fileName), imageBytes);
			return fileName;
		}

		/// <summary>
		/// Returns the image bytes, or null if there is no image or the file is missing.
		/// </summary>
		internal static byte[]? Load(ImageKind kind, string? fileName)
		{
			if (string.IsNullOrEmpty(fileName)) return null;

			// GetFileName: the database value must never point outside the images folder
			string path = Path.Combine(Folder(kind), Path.GetFileName(fileName));
			return File.Exists(path) ? File.ReadAllBytes(path) : null;
		}

		internal static void Delete(ImageKind kind, int id)
		{
			string folder = Folder(kind);
			if (!Directory.Exists(folder)) return;

			foreach (var file in Directory.GetFiles(folder, $"{id}.*"))
			{
				File.Delete(file);
			}
		}

		/// <summary>
		/// JPEG starts with FF D8; everything else is saved as PNG (that is what PictureHelper produces).
		/// The same rule is used in the *ToFile migrations.
		/// </summary>
		private static string GetExtension(byte[] bytes) =>
			bytes.Length > 1 && bytes[0] == 0xFF && bytes[1] == 0xD8 ? ".jpg" : ".png";
	}
}
