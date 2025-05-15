using System;
using System.IO;
using NUnit.Framework;
using UnityAssetBundlePatcher.AssetPatcherLib;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace UnityAssetBundlePatcher.Tests
{
    [TestFixture]
    public class AssetPatcherIntegrationTests
    {
        [Test]
        public void PatchRawAsset_AppliesPatchFromCorrectlyNamedFile()
        {
            // Arrange: use embedded asset file and patch data
            string testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);
            string assetFile = Path.Combine(testDir, "test-asset.assets");
            string patchFile = Path.Combine(testDir, "test-12345.dat");
            string outputFile = Path.Combine(testDir, "output.assets");

            // Copy the real asset file from test resources
            File.Copy(Path.Combine(TestContext.CurrentContext.TestDirectory, "test-asset.assets"), assetFile);

            // Write patch data (example: 4 bytes)
            byte[] patchData = { 0xDE, 0xAD, 0xBE, 0xEF };
            File.WriteAllBytes(patchFile, patchData);

            // Act
            AssetPatcher.PatchRawAsset(assetFile, patchFile, outputFile);

            // Assert: output file should contain the patched data at the correct asset
            var manager = new AssetsManager();
            var fileInst = manager.LoadAssetsFile(outputFile, false);
            var afile = fileInst.file;
            // Find the asset with pathId 12345
            var assetInfo = afile.GetAssetInfo(12345);
            // Read the raw bytes from the asset's data using BaseField
            var baseField = manager.GetBaseField(fileInst, assetInfo);
            byte[] data = null;
            if (baseField != null)
            {
                // Try common field names for raw byte data
                var scriptField = baseField["m_Script"];
                var bytesField = baseField["m_Bytes"];
                if (scriptField != null && scriptField.Value != null && scriptField.Value.ValueType == AssetValueType.ByteArray)
                {
                    data = scriptField.Value.AsByteArray;
                }
                else if (bytesField != null && bytesField.Value != null && bytesField.Value.ValueType == AssetValueType.ByteArray)
                {
                    data = bytesField.Value.AsByteArray;
                }
            }
            Assert.That(data, Is.EqualTo(patchData), "Patched data does not match expected bytes.");
        }
    }
}
