using UnityAssetBundlePatcher.AssetPatcherLib;

if (args.Length < 2)
{
    Console.WriteLine("Usage: patcher [file to patch] [dat file to import]");
    return;
}

string fileToPatch = args[0];
string datFile = args[1];
string outputFile = fileToPatch + ".patch";

var patcher = new AssetPatcher();
AssetPatcher.PatchRawAsset(fileToPatch, datFile, outputFile);
Console.WriteLine($"Patched file written to: {outputFile}");
