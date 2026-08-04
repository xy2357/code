# 电子斗蛐蛐 · Unity MVP

这是一个可直接运行的 Unity 6 四人自动战斗构筑原型。玩家选择英雄、进入四人房间，每轮从三个改造中选择一个，随后两两自动战斗；失败扣除一点生命，最后存活者获胜。

## 运行

- Unity 版本：`6000.2.14f1`
- 主场景：`Assets/DqqGame/Scenes/Main.unity`
- Windows 构建：`Builds/Windows/Dqq.exe`
- 匹配服务器：`Builds/Server/Dqq.MatchServer.exe`
- 编辑器菜单：`DQQ/Verify Combat Framework`、`DQQ/Build Windows`
- Excel 导入菜单：`DQQ/从 Excel 导入配置`

直接运行 `Dqq.exe` 即可。未配置远程地址时，客户端会检测并自动启动同级 `Builds/Server`
中的本地匹配服务器；不足四名玩家时自动补机器人。

### 局域网匹配

服务器电脑以管理员 PowerShell 运行：

```powershell
powershell -ExecutionPolicy Bypass -File Tools/Start-LanServer.ps1 -Restart
```

客户端服务器地址按以下优先级读取：命令行参数 `--server-url`、环境变量
`DQQ_MATCH_SERVER_URL`、`Dqq.exe` 同级的 `server-url.txt`，最后才回退到本机
`http://127.0.0.1:5077`。当前 Windows 包的配置文件已指向 `http://10.27.238.57:5077`。
把整个 `Builds/Windows` 文件夹发给同事即可；如果服务器电脑的 IP 改变，只需修改
`server-url.txt` 后重新分发。服务器满 4 人立即开局，默认等待 8 秒后用机器人补足空位。

## 已实现的框架边界

```text
Assets/DqqGame/Config/DQQ_GameConfig.xlsx（唯一可编辑源）
        ↓ Unity 构建前自动导入
Resources/Config/*.json（运行时生成文件）
        ↓
CombatWorld（纯逻辑模拟）
        ↓
BattleViewEvent 列表（Tick + Sequence + ActionInstanceId）
        ↓
BattlePresenter（表现调度）
        ↓
FighterView / UI / 飘字 / 动画
```

- 统一 `Ability`：触发器 → 条件 → 目标选择 → 原子效果。
- 配置与运行时分离：`AbilityConfig` 不保存计数器和冷却；`AbilityRuntime` 保存单场状态。
- 表现层不计算命中、闪避、暴击、伤害或死亡，只播放已经确定的结果。
- 战斗逻辑不依赖 `MonoBehaviour`、协程或动画帧，可用于加速、跳过、录像和服务端复算。
- 随机数使用传入种子；同一构筑、轮次和种子会得到同一事件日志。

## 配置文件

请直接编辑 `Assets/DqqGame/Config/DQQ_GameConfig.xlsx`。工作簿包含使用说明、英雄、技能、技能效果、强化、表现和枚举参考七张表；保存后从 Unity 菜单导入，Windows 构建前也会自动导入并校验技能引用。

| 文件 | 对应表 | 作用 |
|---|---|---|
| `abilities.json` | Ability / Condition / Target / Effect | 技能触发、条件、目标和效果链 |
| `upgrades.json` | Upgrade / Modifier | 三选一强化与构筑修改 |
| `presentation.json` | AbilityPresentation / EffectPresentation | 技能颜色、标签、时序和受击表现 |

当前包含 6 名英雄、6 个流派、36 张构筑强化、8 个通用技能模块、12 个英雄技能，以及寒霜、冻结、灼烧、引爆、能量、暴击、闪避和反击机制。四人匹配初始生命为 10 点。

英雄采用 Quaternius 的 CC0 `Ultimate Animated Character Pack` 3D 角色，界面采用 Kenney 的 CC0 `UI Pack - Adventure`。来源和许可证见 `Assets/DqqGame/ThirdParty/ART_ASSETS_NOTICE.md`。

## 主要代码

- `Assets/DqqGame/Scripts/Combat/CombatCore.cs`：数据结构、运行时与战斗模拟。
- `Assets/DqqGame/Scripts/Presentation/BattlePresenter.cs`：事件到表现的映射。
- `Assets/DqqGame/Scripts/Presentation/FighterView.cs`：角色动画和生命显示。
- `Assets/DqqGame/Scripts/Game/GameApp.cs`：英雄选择、四人淘汰循环、三选一和界面。
- `Assets/DqqGame/Scripts/Network/MatchClient.cs`：服务器启动、排队、房间、选牌与战果同步。
- `Assets/DqqGame/Editor/ProjectSetup.cs`：工程配置、自检与 Windows 构建。
- `Server/Dqq.MatchServer`：ASP.NET Core 9 四人匹配服务。

## 扩展技能

一般技能只需在 Excel 的“技能”和“技能效果”表增加配置。若需要新的原子效果，再扩展 `EffectType` 和 `ExecuteEffect`；不要为每个技能创建新的 C# 类。
