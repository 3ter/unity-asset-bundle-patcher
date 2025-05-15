# unity-asset-bundle-patcher

## Abstract

A small endeavor to automate the `import raw` action from UABEA: https://github.com/nesrak1/UABEA/issues/47#issuecomment-2879837557.

## Usage

```pwsh
> .\UnityAssetBundlePatcher\bin\Debug\net9.0\UnityAssetBundlePatcher.exe --help
UnityAssetBundlePatcher 1.0.0+3be4f4ec0a68f9ae9eef2b2b4d052d9acc0aef75
Copyright (C) 2025 UnityAssetBundlePatcher

  --overwrite            Overwrite the input file instead of creating a .patch.

  --help                 Display this help screen.

  --version              Display version information.

  input-file (pos. 0)    Required. The asset bundle file to patch (e.g. `resources.assets`).

  dat-file (pos. 1)      Required. The .dat file to import (overwrite an existing object). It is of the form `{asset display name}-{source file name}-{asset path id}.{file extension}` where currently only the path  
                         id is used to identify the asset to patch.
```
