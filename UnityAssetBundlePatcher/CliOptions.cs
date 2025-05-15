using CommandLine;

namespace UnityAssetBundlePatcher
{
    public class CliOptions
    {
        [Value(0, MetaName = "input-file", Required = true, HelpText = "The asset bundle file to patch (e.g. `resources.assets`).")]
        public required string FileToPatch { get; set; }

        [Value(1, MetaName = "dat-file", Required = true, HelpText = "The .dat file to import (overwrite an existing object). It is of the form `{asset display name}-{source file name}-{asset path id}.{file extension}` where currently only the path id is used to identify the asset to patch.")]
        public required string DatFile { get; set; }

        [Option("overwrite", Required = false, HelpText = "Overwrite the input file instead of creating a .patch.")]
        public bool Overwrite { get; set; }
    }
}
