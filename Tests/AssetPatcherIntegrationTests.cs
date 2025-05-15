using System;
using System.IO;
using NUnit.Framework;
using UnityAssetBundlePatcher.AssetPatcherLib;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Collections.Generic;

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
            File.Copy(assetFileTemplate, assetFile, true);
            // Copy the patch file from test resources and give it another id to replace
            File.Copy(patchFileTemplate, patchFile, true);

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

        [Test]
        public void PatchRawAsset_AppliesPatchByName()
        {
            // Arrange: use embedded asset file and patch data for byName case
            string assetFileTemplate = Path.Combine(TestContext.CurrentContext.TestDirectory, "resources.assets");
            string patchFileTemplate = Path.Combine(TestContext.CurrentContext.TestDirectory, "Solid_001-resources.assets-6.dat");

            const string ASSET_NAME_1 = "tea mug";
            const string ASSET_NAME_2 = "Solid_001";

            string testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);
            string assetFile = Path.Combine(testDir, "test-resources.assets");
            string outputFile = Path.Combine(testDir, "test-resources.assets.patch");
            string patchFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "random-patch-file-name.dat");

            // Copy the real asset file from test resources
            File.Copy(assetFileTemplate, assetFile, true);
            // Copy the patch file from test resources (by name)
            File.Copy(patchFileTemplate, patchFile, true);

            // Read the mesh data from the file before replacing it
            var manager = new AssetsManager();
            // Load the class database for Unity 2019 (or the version matching your asset files)
            manager.LoadClassPackage(Path.Combine(TestContext.CurrentContext.TestDirectory, "classdata.tpk"));
            var fileInst = manager.LoadAssetsFile(assetFile, false);
            var afile = fileInst.file;
            manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);
            var assetInfo1 = afile.GetAssetsOfType(AssetClassID.Mesh).Find(a =>
            {
                var texBase = manager.GetBaseField(fileInst, a);
                var name = texBase["m_Name"].AsString;
                return name == ASSET_NAME_1;
            });
            List<AssetFileInfo> assetInfo2 = afile.GetAssetsOfType(AssetClassID.Mesh).FindAll(a =>
            {
                var texBase = manager.GetBaseField(fileInst, a);
                var name = texBase["m_Name"].AsString;
                return name == ASSET_NAME_2;
            });

            Assert.That(assetInfo2.Count == 1, "Before the patch there should be exactly 1 asset with the name " + ASSET_NAME_2);
            Assert.That(!assetInfo1.ByteSize.Equals(assetInfo2[0]), "Before the patch the meshes should not have the same size");

            // Act
            AssetPatcher.PatchRawAsset(assetFile, patchFile, outputFile, ASSET_NAME_1);

            // Assert: output file should contain the patched data at the correct asset
            var fileInstNew = manager.LoadAssetsFile(outputFile, false);
            var afileNew = fileInstNew.file;
            var assetInfoNew1 = afileNew.GetAssetsOfType(AssetClassID.Mesh).Find(a =>
            {
                var texBase = manager.GetBaseField(fileInstNew, a);
                var name = texBase["m_Name"].AsString;
                return name == ASSET_NAME_1;
            });
            List<AssetFileInfo> assetInfosNew = afileNew.GetAssetsOfType(AssetClassID.Mesh).FindAll(a =>
            {
                var texBase = manager.GetBaseField(fileInstNew, a);
                var name = texBase["m_Name"].AsString;
                return name == ASSET_NAME_2;
            });
            Assert.That(assetInfoNew1 == null, "After the patch there should be no asset with the name " + ASSET_NAME_1 + " anymore.");
            Assert.That(assetInfosNew.Count == 2, "After the patch there should be exactly 2 assets with the name " + ASSET_NAME_2);
        }
    }
}
