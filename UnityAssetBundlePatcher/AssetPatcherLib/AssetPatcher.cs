using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace UnityAssetBundlePatcher.AssetPatcherLib
{
    public class AssetPatcher
    {
        public static void PatchRawAsset(string fileToPatch, string datFile, string outputFile, string? byName = "")
        {
            var manager = new AssetsManager();

            manager.LoadClassPackage(Path.Combine(AppContext.BaseDirectory, "classdata.tpk"));

            var afileInst = manager.LoadAssetsFile(fileToPatch, false);
            var afile = afileInst.file;

            manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);

            AssetFileInfo datFileInf;
            if (byName != "")
            {
                List<AssetFileInfo> assetsWithName = afile.GetAssetsOfType(AssetClassID.Mesh).FindAll(a =>
                {
                    var texBase = manager.GetBaseField(afileInst, a);
                    var name = texBase["m_Name"].AsString;
                    return name == byName;
                });
                if (assetsWithName.Count == 0)
                {
                    throw new InvalidOperationException($"No asset with name '{byName}' found.");
                }
                datFileInf = assetsWithName.MaxBy(a =>
                {
                    return a.ByteSize;
                })!;
            }
            else
            {
                var datFileNoExt = Path.GetFileNameWithoutExtension(datFile);
                var datFilePathIdStr = datFileNoExt[(datFileNoExt.LastIndexOf('-') + 1)..];
                var datFilePathId = long.Parse(datFilePathIdStr);

                datFileInf = afile.GetAssetInfo(datFilePathId);
            }
            datFileInf.SetNewData(File.ReadAllBytes(datFile));

            using var writer = new AssetsFileWriter(outputFile);
            afile.Write(writer);
        }
    }
}
