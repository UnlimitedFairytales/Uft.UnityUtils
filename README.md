# Getting started

## Install via git URL

https://github.com/UnlimitedFairytales/Uft.UnityUtils.git?path=Assets/Uft.UnityUtils

## Setup dependencies

- com.unity.ugui 2.0.0
    - Import TMP Essentials
- https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
- https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
    - Setup DOTween
    - Create ASMDEF
    - Project Settings > Player > Scripting Define Symbols > UNITASK_DOTWEEN_SUPPORT

# Option

## Disable features

Project Settings > Scripting Define Symbols

- DISABLE_UNITYUTILS_AUTO_AUDIO_LOAD_TYPE_SETTER
- DISABLE_UNITYUTILS_SCRIPTED_IMPORTER
- DISABLE_UNITYUTILS_JUST_BEFORE_UPDATE

## CsvHelper Option

Install

- https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
    - CsvHelper 33.1.0

Project Settings > Scripting Define Symbols

- UNITYUTILS_CSVHELPER_SUPPORT