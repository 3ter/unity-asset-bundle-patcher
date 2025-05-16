# unity-asset-bundle-patcher

## Abstract

A small endeavor to automate the `import raw` action from UABEA: https://github.com/nesrak1/UABEA/issues/47#issuecomment-2879837557.

## Usage

> ⚠ You need to have the `classdata.tpk` next to the executable (can be downloaded from [AssetRipper type tree tpk](https://github.com/AssetRipper/Tpk/actions/workflows/type_tree_tpk.yml) → click the latest workflow run and download `uncompressed_file` and copy the unzipped file next to the executable and rename it to `classdata.tpk`).

```
.\UnityAssetBundlePatcher\bin\Debug\net9.0\UnityAssetBundlePatcher.exe Tests\resources.assets Tests\Solid_001-resources.assets-6.dat --by-name "tea mug"
```

```
> .\UnityAssetBundlePatcher\bin\Debug\net9.0\UnityAssetBundlePatcher.exe --help   
UnityAssetBundlePatcher 1.0.0+b86cc2dab1b9167029288b9abf360e40f27a1dcf
Copyright (C) 2025 UnityAssetBundlePatcher

  --overwrite            Overwrite the input file instead of creating a .patch.

  --by-name              Ignore dat file name and use the asset name instead. If there are multiple assets with the same name, the one with the largest file size will be overwritten.

  --help                 Display this help screen.

  --version              Display version information.

  input-file (pos. 0)    Required. The asset bundle file to patch (e.g. `resources.assets`).

  dat-file (pos. 1)      Required. The .dat file to import (overwrite an existing object). It is of the form `{asset display name}-{source file name}-{asset path id}.{file extension}` where currently only the path
                         id is used to identify the asset to patch.
```

## Known Limitations
- Currently only looks for replacing Meshes when using `--by-name`.
