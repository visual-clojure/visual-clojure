﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Collections.Generic;
using System.IO;
using VisualClojure.Utilities.IO.FileSystem;
using ICSharpCode.SharpZipLib.Zip;

namespace VisualClojure.Utilities.IO.Compression
{
	public static class CompressionExtensions
	{
		public static void ExtractZipToFreshSubDirectoryAndDelete(string zipFile)
		{
			string outputDirectory = FileSystemExtensions.GetFileDirectoryNameFromFileName(zipFile);
			FileSystemExtensions.ClearDirectory(outputDirectory);

			using (var fileStream = File.OpenRead(zipFile))
			{
				fileStream.ExtractStreamToFileSystem(new RelativePathFileSystemWriter(outputDirectory));
			}

			File.Delete(zipFile);
		}

		public static void ExtractStreamToFileSystem(this Stream compressedStream, IFileSystemWriter writer)
		{
			using (var zipInputStream = new ZipInputStream(compressedStream))
			{
				foreach (var currentEntry in zipInputStream.ToList())
				{
					string entryDirectory = Path.GetDirectoryName(currentEntry.Name);
					string entryFileName = Path.GetFileName(currentEntry.Name);
					if (entryDirectory.Length > 0) writer.CreateDirectory(entryDirectory);
					if (string.IsNullOrEmpty(entryFileName)) continue;
					writer.CreateFile(zipInputStream, Path.Combine(entryDirectory, entryFileName));
				}
			}
		}

		private static IEnumerable<ZipEntry> ToList(this ZipInputStream zipInputStream)
		{
			ZipEntry currentEntry = zipInputStream.GetNextEntry();

			while (currentEntry != null)
			{
				yield return currentEntry;
				currentEntry = zipInputStream.GetNextEntry();
			}
		}
	}
}
