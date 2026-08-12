荆棘司祭动画帧

内容
- 27 张独立透明 PNG，单张 72×72 px。
- 1 张 6×6 总图，尺寸 432×432 px；每格 72×72 px。
- frame_manifest.csv 记录动作、帧数和总图位置。

动作数量
- 待机 4 帧
- 横移 6 帧
- 举炮瞄准 3 帧
- 充能 3 帧
- 射击后坐 2 帧
- 恢复 2 帧
- 受击 2 帧
- 死亡 5 帧

Unity 导入建议
- Texture Type：Sprite (2D and UI)
- Sprite Mode：独立帧选 Single；总图选 Multiple
- Pixels Per Unit：72（也可按项目统一值修改）
- Filter Mode：Point (no filter)
- Compression：None
- Wrap Mode：Clamp
- 总图切片：Grid by Cell Size，X=72，Y=72
- Pivot：Bottom Center；同一动作保持统一脚底基准

总图排布
- 第 1 行：待机 1~4，空，空
- 第 2 行：横移 1~6
- 第 3 行：瞄准 1~3，充能 1~3
- 第 4 行：后坐 1~2，恢复 1~2，受击 1~2
- 第 5 行：死亡 1~5，空
- 第 6 行：全空
