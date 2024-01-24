# [English](README.md), [中文](README_zh.md)
# Subtitle file translation tool

## What is this?

### This is a tool for translating subtitle files and currently supports srt format translation.

## Screenshot

<img width="868" alt="CleanShot 2024-01-24 at 16 31 45@2x" src="https://github.com/eeee0717/SubtitleTranslator/assets/70054568/c6c22132-2227-45e6-8161-5ce0332b682c">

## Installation

> Go to the [Release]() to download the package.

### MacOS

Since there is no Apple developer account, if xxx.app is damaged when opening, please execute the following instructions:

```bash
sudo xattr -rd com.apple.quarantine /Applications/SubtitleTranslator.app
```

## Provider Config
### Tencent Cloud
1. Log in[Tencent Cloud](https://cloud.tencent.com/)
2. Search Access management -> User -> User list -> New User
![CleanShot 2024-01-24 at 16 39 02@2x](https://github.com/eeee0717/SubtitleTranslator/assets/70054568/561d5437-d59d-4520-9c5a-4905c5df862d)
3. Click Add User -> API Key -> Add Key
4. Copy SecretId and SecretKey and paste them into Settings
5. Click the test button to test

### Youdao
1. Log in[Youdao](https://ai.youdao.com/console/#/)
2. Application Overview -> Create Application -> Select Text Translation Service and API access
3. Copy AppId and AppKey and paste them into Settings
4. Click the test button to test


## Development guide
The project was developed using Avalonia, [Avalonia](https://avaloniaui.net/) is a cross-platform framework that allows. NET developers are better at creating cross-platform apps.

Due to the use of experimental features in the Community Toolkit Lab during development, a new nuget package needs to be added to nuget

```bash
https://pkgs.dev.azure.com/dotnet/CommunityToolkit/_packaging/CommunityToolkit-Labs/nuget/v3/index.json
```

If you encounter any problems in development or have any ideas, please feel free to raise issues and pr.

