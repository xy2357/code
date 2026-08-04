# DQQ 四人匹配服务器

这是一个无第三方依赖的 ASP.NET Core 9 匹配服务。

## 规则

- 4 人组成一局；人数满 4 人时立即开局，等待时间达到配置值后人数不足会补机器人。
- 每轮两两配对，所有存活玩家先提交一项强化，再进入战斗。
- 失败扣 1 点生命，初始 10 点；最后一名存活玩家获胜。
- 同一对局的重复或迟到结果按幂等请求处理；首份结果到达 10 秒后仍缺失的对局由服务端兜底结算。
- 房间、排队、令牌、升级与排名数据目前保存在内存中。

## 启动

```powershell
dotnet run --project Server/Dqq.MatchServer
```

默认监听所有 IPv4 网卡的 `5077` 端口，健康检查为 `/health`。机器人补位等待时间由
`appsettings.json` 中的 `Matchmaking:BotFillDelaySeconds` 配置，局域网测试默认是 8 秒。
`ResultGraceSeconds` 控制首份结果到达后的结算宽限期，`BattleHardTimeoutSeconds` 防止所有客户端
都断线时房间永久停在战斗阶段。

在 Windows 上可直接运行：

```powershell
powershell -ExecutionPolicy Bypass -File Tools/Start-LanServer.ps1 -Restart
```

首次运行请使用管理员 PowerShell，以创建仅允许局域网来源访问 TCP 5077 的防火墙规则。
同事的客户端把 `server-url.txt` 放在 `Dqq.exe` 同级，并填写服务器地址，例如
`http://10.27.238.57:5077`。

发布版本位于 `Builds/Server/Dqq.MatchServer.exe`。

## 生产环境后续项

当前版本用于本机和局域网垂直切片。正式上线前需增加 Redis/PostgreSQL 持久化、账号鉴权、断线重连、服务发现、指标监控，并将战斗模拟提取为客户端与服务端共用的纯 .NET 程序集进行权威复算。
