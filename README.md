AzureArchiveHDTextureFix
版本: 1.0.0
授权协议: MIT License

1. 项目简介
本模组用于修复《AzureArchive》游戏中原生高分辨率（4K/8K）Spine 立绘显示模糊的问题。通过拦截游戏底层的纹理生成逻辑，强制禁用有损压缩并使用最高级别的采样过滤。

2. 技术背景与问题根因
通过逆向分析游戏底层代码 (Assembly-CSharp.dll)，发现引擎在处理高分辨率 Spine 纹理时存在以下机制：
- 有损压缩：为控制显存占用，引擎默认将无损的 PNG 图像强制压缩为有损的 DXT5 格式，导致高对比度边缘出现色块与细节丢失。
- 缓存污染：压缩后的低画质纹理被通过 QueueWrite 写入本地缓存目录 (LocalLow)，导致后续加载时直接读取旧缓存，形成永久性模糊。
- 过滤算法：默认采用双线性过滤 (Bilinear)，在纹理缩放时产生了进一步的模糊。

3. 模组功能与实现原理
- 拦截 RuntimeAtlasTexture.CanLoadPngAsDxt5 方法，强制返回 false，阻止引擎对原图进行 DXT5 有损压缩。
- 拦截 RuntimeAtlasTexture.TryReadCache 与 RuntimeAtlasTexture.WriteCache 方法，绕过本地低画质缓存，且不再生成新的缓存数据（直接在内存层面拦截，不修改或删除硬盘上的现有存档及配置文件）。
- 拦截 RuntimeAtlasTexture.Load 方法，在纹理生成后强制将 FilterMode 修改为 Trilinear (三线性过滤)，并设置 AnisoLevel 为 16 (最高级别各向异性过滤)。

4. 性能警告
由于禁用了纹理压缩，游戏将以无损格式 (RGBA32) 将原图直接加载至显存。单张 8K 分辨率立绘的显存占用将从约 20MB 激增至 80MB 以上。
在低配设备（显存 < 4GB，或系统内存 < 8GB）上使用时，可能引发显存溢出 (OOM) 导致游戏崩溃或掉帧。请在安装前评估硬件性能。

5. 安装方法
1. 确保游戏已安装并正常运行 BepInEx 6 (IL2CPP) 框架。
2. 将解压后的 AzureArchiveHDTextureFix 文件夹放入游戏根目录下的 mods 文件夹中。
3. 启动游戏。模组将在底层自动加载并生效，无需额外配置。

6. 开源协议
本项目遵循 MIT License 协议。允许任何个人或组织自由使用、修改、合并、发布或分发本代码，只需保留原始版权声明。欢迎其他 Mod 直接集成或引用本项目的补丁逻辑

7. 致谢
特别感谢 DeepSeek AI 在本项目开发过程中提供的底层代码解析、逻辑推演与架构设计建议。