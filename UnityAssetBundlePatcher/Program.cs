using UnityAssetBundlePatcher;
using UnityAssetBundlePatcher.AssetPatcherLib;
using CommandLine;

Parser.Default.ParseArguments<CliOptions>(args).WithParsed(options =>
    {
        string outputPath = options.Overwrite
                ? options.FileToPatch
                : options.FileToPatch + ".patch";

        AssetPatcher.PatchRawAsset(options.FileToPatch, options.DatFile, outputPath);
        Console.WriteLine($"Patched file written to: {outputPath}");
    }).WithNotParsed(errors =>
    {
        if (errors.IsHelp())
        {
            return;
        }
        Console.WriteLine("Invalid arguments. Use --help for usage info.");
    });
