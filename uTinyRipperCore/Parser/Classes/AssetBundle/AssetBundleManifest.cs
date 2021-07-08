using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uTinyRipper.Classes;
using uTinyRipper.Classes.AssetBundles;
using uTinyRipper.Converters;
using uTinyRipper.Layout;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes
{
	public sealed class AssetBundleManifest : NamedObject
	{

		public AssetBundleManifest(AssetInfo assetInfo) : base(assetInfo) { }

		public override void Read(AssetReader reader)
		{
			base.Read(reader);

			int fileCount = reader.ReadInt32();
			FilenameEntry[] files = new FilenameEntry[fileCount];
			for (int i = 0; i < fileCount; i++)
			{
				int fileId = reader.ReadInt32();
				string file = reader.ReadString();
				files[i] = new FilenameEntry(fileId, file);
			}

			int unknown = reader.ReadInt32();

			int hashCount = reader.ReadInt32();
			HashEntry[] hashes = new HashEntry[hashCount];
			for (int i = 0; i < hashCount; i++)
			{
				int fileId = reader.ReadInt32();
				byte[] hashBytes = reader.ReadBytes(16);

				int trailer = reader.ReadInt32();
				if (trailer != 0)
				{
					throw new Exception($"Expected trailer (0), got {trailer}");
				}

				hashes[i] = new HashEntry(fileId, hashBytes);
			}

			FileEntries = files;
			HashEntries = hashes;

		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);

			YAMLSequenceNode fileEntriesNode = new YAMLSequenceNode();
			foreach (FilenameEntry fileEntry in FileEntries)
			{
				YAMLMappingNode fileNode = new YAMLMappingNode();
				fileNode.Add("m_Id", fileEntry.Id);
				fileNode.Add("m_Path", fileEntry.Path);
				fileEntriesNode.Add(fileNode);
			}
			node.Add("m_Files", fileEntriesNode);

			YAMLSequenceNode hashEntriesNode = new YAMLSequenceNode();
			foreach (HashEntry hashEntry in HashEntries)
			{
				YAMLMappingNode hashNode = new YAMLMappingNode();
				hashNode.Add("m_Id", hashEntry.Id);
				YAMLSequenceNode hashBytesNode = new YAMLSequenceNode();
				foreach (byte hashEntryByte in hashEntry.Bytes)
				{
					hashBytesNode.Add(hashEntryByte);
				}
				hashNode.Add("m_Bytes", hashBytesNode);
				hashEntriesNode.Add(hashNode);
			}
			node.Add("m_Hashes", hashEntriesNode);

			return node;
		}

		public FilenameEntry[] FileEntries { get; set; } = new FilenameEntry[0];
		public HashEntry[] HashEntries { get; set; } = new HashEntry[0];

		public override string ExportExtension => "assetbundlemanifest.yaml";

	}
}
