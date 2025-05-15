using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace UnityAssetBundlePatcher.AssetPatcherLib
{
    public class AssetPatcher
    {
        public static void PatchRawAsset(string fileToPatch, string datFile, string outputFile)
        {
            var manager = new AssetsManager();

            var afileInst = manager.LoadAssetsFile(fileToPatch, false);
            var afile = afileInst.file;

            var datFileNoExt = Path.GetFileNameWithoutExtension(datFile);
            var datFilePathIdStr = datFileNoExt[(datFileNoExt.LastIndexOf('-') + 1)..];
            var datFilePathId = long.Parse(datFilePathIdStr);

            var datFileInf = afile.GetAssetInfo(datFilePathId);
            datFileInf.SetNewData(File.ReadAllBytes(datFile));

            using var writer = new AssetsFileWriter(outputFile);
            afile.Write(writer);
        }
    }
}
