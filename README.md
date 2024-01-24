# 字幕文件翻译工具

## 这是什么？

### 这是一个用于翻译字幕文件的工具，目前支持srt格式的翻译。

## 项目截图

<img width="868" alt="CleanShot 2024-01-24 at 16 31 45@2x" src="https://github.com/eeee0717/SubtitleTranslator/assets/70054568/c6c22132-2227-45e6-8161-5ce0332b682c">

## 安装指南

> 前往(Release)[]界面下载对应的安装包

### MacOS

由于没有Apple开发者账号，打开时如出现 xxx.app 已损坏，请执行如下指令：

```bash
sudo xattr -rd com.apple.quarantine /Applications/SubtitleTranslator.app
```

## 服务商配置
### 腾讯云配置
1. 登录[腾讯云](https://cloud.tencent.com/)
2. 搜索访问管理 -> 用户 -> 用户列表 -> 新增用户
![CleanShot 2024-01-24 at 16 39 02@2x](https://github.com/eeee0717/SubtitleTranslator/assets/70054568/561d5437-d59d-4520-9c5a-4905c5df862d)
3. 点击新增的用户 -> API密钥 -> 新增密钥
4. 复制 SecretId 和 SecretKey粘贴到设置中
5. 点击测试按钮进行测试

### 有道云配置
1. 登录[有道智云](https://ai.youdao.com/console/#/)
2. 应用总览 -> 创建应用 -> 选择文本翻译服务和API接入
3. 复制 AppId 和 AppKey粘贴到设置中
4. 点击测试按钮进行测试


## 开发指南
该项目使用Avalonia开发，[Avalonia](https://avaloniaui.net/)是一个跨平台框架，可以让.NET开发人员更好的创建跨平台App。

由于开发中使用到了Community Toolkit Lab中的实验特性，因此需要在nuget中添加新的nuget包

```bash
https://pkgs.dev.azure.com/dotnet/CommunityToolkit/_packaging/CommunityToolkit-Labs/nuget/v3/index.json
```

开发中遇到任何问题或者您有任何想法，欢迎提issue和pr.

