namespace uTinyRipper.Classes.AssetBundles
{
	public class FilenameEntry
	{
		public int Id { get; set; }
		public string Path { get; set; }

		public FilenameEntry(int id, string path) {
			Id = id;
			Path = path;
		}
	}
}
