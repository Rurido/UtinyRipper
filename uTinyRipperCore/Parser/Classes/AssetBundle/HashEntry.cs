namespace uTinyRipper.Classes.AssetBundles
{
	public class HashEntry
	{
		public int Id { get; set; }
		public byte[] Bytes { get; set; }

		public HashEntry(int id, byte[] bytes)
		{
			Id = id;
			Bytes = bytes;
		}
	}
}
