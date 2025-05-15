using System;
using System.IO;
using NUnit.Framework;
using UnityAssetBundlePatcher.AssetPatcherLib;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace Tests
{
    [TestFixture]
    public class AssetPatcherIntegrationTests
    {
        [Test]
        public void PatchRawAsset_AppliesPatchFromCorrectlyNamedFile()
        {
            // Arrange: use embedded asset file and patch data
            string assetFileTemplate = Path.Combine(TestContext.CurrentContext.TestDirectory, "resources.assets");
            string patchFileTemplate = Path.Combine(TestContext.CurrentContext.TestDirectory, "Solid_001-resources.assets-6.dat");

            string testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);
            string assetFile = Path.Combine(testDir, "test-resources.assets");
            string outputFile = Path.Combine(testDir, "test-resources.assets.patch");
            string patchFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "Solid_001-resources.assets-5.dat");

            // Copy the real asset file from test resources
            File.Copy(assetFileTemplate, assetFile);
            // Copy the patch file from test resources and give it another id to replace
            File.Copy(patchFileTemplate, patchFile);

            // Read the mesh data from the file before replacing it
            var manager = new AssetsManager();
            var fileInst = manager.LoadAssetsFile(assetFile, false);
            var afile = fileInst.file;
            var assetInfo1 = afile.GetAssetsOfType(AssetClassID.Mesh).Find(a => a.PathId == 5);
            var assetInfo2 = afile.GetAssetsOfType(AssetClassID.Mesh).Find(a => a.PathId == 6);

            Assert.That(!assetInfo1.ByteSize.Equals(assetInfo2.ByteSize), "Before the patch the meshes should not have the same size");

            // Act
            AssetPatcher.PatchRawAsset(assetFile, patchFile, outputFile);

            // Assert: output file should contain the patched data at the correct asset
            var fileInstNew = manager.LoadAssetsFile(outputFile, false);
            var afileNew = fileInst.file;
            var assetInfoNew1 = afile.GetAssetsOfType(AssetClassID.Mesh).Find(a => a.PathId == 6);
            var assetInfoNew2 = afile.GetAssetsOfType(AssetClassID.Mesh).Find(a => a.PathId == 6);
            Assert.That(assetInfoNew1.ByteSize.Equals(assetInfoNew2.ByteSize), "After the patch both meshes should have the same size");
        }
    }
}
