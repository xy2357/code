# WindowPins

Windows 托盘置顶工具。左键单击托盘图标，鼠标变为十字后单击目标窗口即可置顶；按 Esc 或右键取消选择。标题栏靠右、窗口按钮左侧会出现透明图钉，单击图钉线条可取消置顶。图钉周围及内部空白处透明，可以点击下面的窗口。右键单击托盘图标也可以查看和取消已置顶的窗口。

程序退出时会恢复它置顶过的窗口；原本已经置顶的窗口保持原状。关闭目标窗口后，图钉会自动消失。

## 运行

双击 `publish/WindowPins.exe`。程序不会显示主窗口，请在 Windows 任务栏通知区域查找图钉图标。如果目标窗口以管理员身份运行，WindowPins 也需要以管理员身份运行。

## 从源码构建

需要 .NET 10 SDK 和 Windows。运行：

```powershell
dotnet publish .\WindowPins.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=false -o .\publish
```
