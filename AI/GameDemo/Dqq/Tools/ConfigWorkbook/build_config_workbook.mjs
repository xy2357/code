import fs from "node:fs/promises";
import path from "node:path";
import { SpreadsheetFile, Workbook } from "@oai/artifact-tool";

const projectRoot = path.resolve(import.meta.dirname, "../..");
const configRoot = path.join(projectRoot, "Assets/DqqGame/Resources/Config");
const sourceOutput = path.join(projectRoot, "Assets/DqqGame/Config/DQQ_GameConfig.xlsx");
const deliveryOutput = path.join(projectRoot, "outputs/019fc843-43a4-73f0-8933-8ba80787ad71/DQQ_GameConfig.xlsx");

const readJson = async (name) => JSON.parse(await fs.readFile(path.join(configRoot, name), "utf8"));
const abilities = (await readJson("abilities.json")).abilities;
const heroes = (await readJson("heroes.json")).heroes;
const upgrades = (await readJson("upgrades.json")).upgrades;
const presentation = (await readJson("presentation.json")).abilities;

const heroModels = ["Viking_Male", "Ninja_Female", "Wizard", "Ninja_Sand", "Elf", "Knight_Golden_Female"];
heroes.forEach((hero, index) => {
  hero.modelResource = heroModels[index] || "Viking_Male";
});

const effectRows = [];
for (const ability of abilities) {
  (ability.effects ?? []).forEach((effect, index) => {
    effectRows.push({ abilityId: ability.abilityId, effectIndex: index + 1, ...effect });
  });
}

const workbook = Workbook.create();
workbook.comments.setSelf({ displayName: "DQQ Config Builder" });

const colors = {
  ink: "#172033",
  navy: "#202A44",
  gold: "#D6A84B",
  parchment: "#F7F1E3",
  paper: "#FFFDF8",
  line: "#D9D0BC",
  muted: "#6B7280",
  green: "#1F7A63",
  red: "#B84A4A",
};

function styleDataSheet(sheet, columnCount, rowCount, widths = {}) {
  sheet.showGridLines = false;
  sheet.freezePanes.freezeRows(2);
  const used = sheet.getRangeByIndexes(0, 0, rowCount + 2, columnCount);
  used.format.font = { name: "Microsoft YaHei", size: 10, color: colors.ink };
  used.format.verticalAlignment = "center";

  const keys = sheet.getRangeByIndexes(0, 0, 1, columnCount);
  keys.format.fill = colors.navy;
  keys.format.font = { name: "Consolas", size: 10, bold: true, color: "#FFFFFF" };
  keys.format.rowHeight = 25;
  keys.format.borders = { preset: "inside", style: "thin", color: "#39445F" };

  const labels = sheet.getRangeByIndexes(1, 0, 1, columnCount);
  labels.format.fill = colors.gold;
  labels.format.font = { name: "Microsoft YaHei", size: 10, bold: true, color: colors.ink };
  labels.format.wrapText = true;
  labels.format.rowHeight = 38;
  labels.format.borders = { preset: "inside", style: "thin", color: "#C5963E" };

  if (rowCount > 0) {
    const body = sheet.getRangeByIndexes(2, 0, rowCount, columnCount);
    body.format.fill = colors.paper;
    body.format.borders = {
      insideHorizontal: { style: "thin", color: colors.line },
      bottom: { style: "medium", color: colors.line },
    };
    body.format.rowHeight = 24;
  }

  for (let i = 0; i < columnCount; i++) {
    sheet.getRangeByIndexes(0, i, rowCount + 2, 1).format.columnWidth = widths[i] ?? 15;
  }
}

function addDataSheet({ name, columns, rows, widths = {}, validations = [] }) {
  const sheet = workbook.worksheets.getItem(name);
  sheet.getRangeByIndexes(0, 0, 2, columns.length).values = [
    columns.map((c) => c.key),
    columns.map((c) => c.label),
  ];
  if (rows.length) {
    sheet.getRangeByIndexes(2, 0, rows.length, columns.length).values = rows.map((row) =>
      columns.map((column) => row[column.key] ?? null),
    );
  }
  styleDataSheet(sheet, columns.length, rows.length, widths);
  for (const validation of validations) {
    sheet.getRange(validation.range).dataValidation = {
      rule: { type: "list", values: validation.values },
    };
  }
  return sheet;
}

const intro = workbook.worksheets.add("使用说明");
["英雄", "技能", "技能效果", "强化", "表现", "枚举参考"].forEach((name) => workbook.worksheets.add(name));
intro.showGridLines = false;
intro.getRange("A1:H2").merge();
intro.getRange("A1").values = [["电子斗蛐蛐 · 游戏配置中心"]];
intro.getRange("A1:H2").format = {
  fill: colors.navy,
  font: { name: "Microsoft YaHei", size: 22, bold: true, color: "#FFFFFF" },
  horizontalAlignment: "center",
  verticalAlignment: "center",
};
intro.getRange("A4:B8").values = [
  ["配置项", "当前数量"],
  ["英雄", null],
  ["技能", null],
  ["强化", null],
  ["表现配置", null],
];
intro.getRange("B5").formulas = [["=COUNTA('英雄'!A3:A102)"]];
intro.getRange("B6").formulas = [["=COUNTA('技能'!A3:A202)"]];
intro.getRange("B7").formulas = [["=COUNTA('强化'!A3:A202)"]];
intro.getRange("B8").formulas = [["=COUNTA('表现'!A3:A202)"]];
intro.getRange("A4:B4").format = { fill: colors.gold, font: { bold: true, color: colors.ink } };
intro.getRange("A5:B8").format = {
  fill: colors.paper,
  borders: { preset: "inside", style: "thin", color: colors.line },
};
intro.getRange("D4:H4").merge();
intro.getRange("D4").values = [["怎么改配置"]];
intro.getRange("D4:H4").format = { fill: colors.gold, font: { bold: true, color: colors.ink } };
intro.getRange("D5:H10").merge();
intro.getRange("D5").values = [[
  "1. 只编辑各分表第 3 行以后的数据。\n2. 第 1 行是程序字段名，不要改名；第 2 行是中文解释。\n3. 技能的多个效果在“技能效果”表中按 effectIndex 排序。\n4. 保存后回到 Unity，选择 DQQ > 从 Excel 导入配置；Windows 构建前也会自动导入。\n5. BP 表示万分比：1000 = 10%，10000 = 100%。",
]];
intro.getRange("D5:H10").format = {
  fill: colors.parchment,
  font: { name: "Microsoft YaHei", size: 11, color: colors.ink },
  wrapText: true,
  verticalAlignment: "top",
  borders: { preset: "outside", style: "medium", color: colors.gold },
};
intro.getRange("A11:H11").merge();
intro.getRange("A11").values = [["注意：英雄的 passiveAbilityId / ultimateAbilityId、强化的 addAbilityId 必须能在“技能”表中找到。"]];
intro.getRange("A11:H11").format = {
  fill: "#FCE8E6",
  font: { bold: true, color: colors.red },
  wrapText: true,
};
intro.getRange("A1:H12").format.font = { name: "Microsoft YaHei" };
intro.getRange("A1:H12").format.rowHeight = 25;
intro.getRange("A1:H2").format.rowHeight = 34;
intro.getRange("A1:A12").format.columnWidth = 19;
intro.getRange("B1:B12").format.columnWidth = 14;
intro.getRange("C1:C12").format.columnWidth = 3;
intro.getRange("D1:H12").format.columnWidth = 17;

addDataSheet({
  name: "英雄",
  rows: heroes,
  columns: [
    ["heroId", "英雄ID"], ["heroName", "英雄名称"], ["title", "英雄称号"], ["school", "流派"],
    ["accent", "主题色（十六进制）"], ["passiveName", "被动名称"], ["passiveDescription", "被动说明"],
    ["passiveAbilityId", "被动技能ID"], ["ultimateName", "大招名称"], ["ultimateDescription", "大招说明"],
    ["ultimateAbilityId", "大招技能ID"], ["baseHealth", "基础生命"], ["baseAttack", "基础攻击"],
    ["baseDefense", "基础防御"], ["attackIntervalMs", "攻击间隔（毫秒）"], ["dodgeBP", "闪避率（BP）"],
    ["critBP", "暴击率（BP）"], ["modelResource", "3D 模型资源名"],
  ].map(([key, label]) => ({ key, label })),
  widths: { 1: 17, 2: 17, 3: 13, 5: 18, 6: 34, 8: 18, 9: 34, 17: 24 },
  validations: [{ range: "D3:D102", values: ["Basic", "Critical", "Ultimate", "Dodge", "Frost", "Burn"] }],
});

addDataSheet({
  name: "技能",
  rows: abilities.map(({ effects, ...ability }) => ability),
  columns: [
    ["abilityId", "技能ID"], ["abilityName", "技能名称"], ["description", "技能说明"], ["triggerEvent", "触发事件"],
    ["triggerCount", "触发计数"], ["triggerChanceBP", "触发概率（BP）"], ["internalCooldownMs", "内部冷却（毫秒）"],
    ["condition", "触发条件"], ["targetRule", "目标规则"], ["tags", "标签（| 分隔）"],
    ["maxTriggersPerChain", "单链最大触发数"], ["energyCost", "能量消耗"], ["isUltimate", "是否大招"],
  ].map(([key, label]) => ({ key, label })),
  widths: { 1: 18, 2: 42, 3: 21, 7: 22, 8: 17, 9: 30, 10: 20 },
  validations: [
    { range: "D3:D202", values: ["BattleStart", "AfterBasicAttack", "DodgeSucceeded", "DamageResolved", "EnergyFull", "AfterUltimate"] },
    { range: "H3:H202", values: ["Always", "EventTargetIsOwner", "EventSourceIsOwner", "EventWasCritical"] },
    { range: "I3:I202", values: ["Enemy", "EventSource", "Self"] },
    { range: "M3:M202", values: [true, false] },
  ],
});

addDataSheet({
  name: "技能效果",
  rows: effectRows,
  columns: [
    ["abilityId", "所属技能ID"], ["effectIndex", "效果顺序"], ["effectType", "效果类型"],
    ["coefficientBP", "系数（BP）"], ["flatValue", "固定数值"], ["element", "元素"], ["buffId", "状态ID"],
    ["repeatCount", "重复次数"], ["durationMs", "持续时间（毫秒）"], ["guaranteedCritical", "必定暴击"],
  ].map(([key, label]) => ({ key, label })),
  widths: { 2: 22, 5: 16, 6: 16, 8: 21, 9: 16 },
  validations: [
    { range: "C3:C302", values: ["Damage", "RepeatDamage", "HealFromEvent", "AddBurn", "AddFrost", "GainEnergy", "DetonateBurn", "TemporaryDodge"] },
    { range: "J3:J302", values: [true, false] },
  ],
});

addDataSheet({
  name: "强化",
  rows: upgrades,
  columns: [
    ["upgradeId", "强化ID"], ["upgradeName", "强化名称"], ["description", "强化说明"], ["icon", "图标文字"],
    ["accent", "主题色"], ["school", "流派"], ["rarity", "稀有度"], ["attackBP", "攻击（BP）"],
    ["healthBP", "生命（BP）"], ["attackSpeedBP", "攻速（BP）"], ["dodgeBP", "闪避（BP）"], ["critBP", "暴击（BP）"],
    ["defenseFlat", "固定防御"], ["addAbilityId", "添加技能ID"], ["unique", "是否唯一"], ["critDamageBP", "暴伤（BP）"],
    ["basicPowerBP", "普攻伤害（BP）"], ["ultimatePowerBP", "大招伤害（BP）"], ["energyGainBP", "能量获取（BP）"],
    ["burnPowerBP", "灼烧伤害（BP）"], ["frostPowerBP", "寒霜效率（BP）"], ["counterPowerBP", "反击伤害（BP）"],
  ].map(([key, label]) => ({ key, label })),
  widths: { 0: 22, 1: 18, 2: 38, 5: 13, 6: 13 },
  validations: [
    { range: "F3:F202", values: ["Basic", "Critical", "Ultimate", "Dodge", "Frost", "Burn"] },
    { range: "G3:G202", values: ["Common", "Rare", "Epic"] },
    { range: "O3:O202", values: [true, false] },
  ],
});

addDataSheet({
  name: "表现",
  rows: presentation,
  columns: [
    ["abilityId", "技能ID"], ["accent", "表现主题色"], ["castLabel", "施法提示"], ["hitTimeMs", "命中时间（毫秒）"],
    ["totalTimeMs", "总演出时间（毫秒）"], ["faceTarget", "是否朝向目标"], ["hitReaction", "受击动作"], ["floatingTextType", "飘字类型"],
  ].map(([key, label]) => ({ key, label })),
  widths: { 1: 18, 2: 22, 3: 20, 4: 22, 6: 18, 7: 22 },
  validations: [
    { range: "F3:F202", values: [true, false] },
    { range: "G3:G202", values: ["None", "HitLight", "HitHeavy"] },
  ],
});

const enums = workbook.worksheets.getItem("枚举参考");
enums.showGridLines = false;
enums.getRange("A1:D1").values = [["字段", "可用值", "中文含义", "备注"]];
const enumRows = [
  ["school", "Basic / Critical / Ultimate / Dodge / Frost / Burn", "普攻 / 暴击 / 大招 / 闪避 / 冰霜 / 燃烧", "六大流派"],
  ["rarity", "Common / Rare / Epic", "普通 / 稀有 / 史诗", "影响卡牌框与抽取权重"],
  ["targetRule", "Enemy / EventSource / Self", "敌方 / 事件来源 / 自身", "技能效果目标"],
  ["effectType", "Damage / RepeatDamage / HealFromEvent / AddBurn / AddFrost / GainEnergy / DetonateBurn / TemporaryDodge", "伤害 / 多段 / 按事件治疗 / 灼烧 / 寒霜 / 回能 / 引爆 / 临时闪避", "效果执行器"],
  ["BP", "100 = 1%", "万分比单位", "10000 = 100%"],
];
enums.getRange("A2:D6").values = enumRows;
enums.getRange("A1:D1").format = { fill: colors.navy, font: { bold: true, color: "#FFFFFF" } };
enums.getRange("A2:D6").format = { fill: colors.paper, wrapText: true, borders: { preset: "inside", style: "thin", color: colors.line } };
enums.getRange("A1:A6").format.columnWidth = 18;
enums.getRange("B1:B6").format.columnWidth = 72;
enums.getRange("C1:C6").format.columnWidth = 48;
enums.getRange("D1:D6").format.columnWidth = 28;
enums.getRange("A1:D6").format.rowHeight = 34;
enums.freezePanes.freezeRows(1);

for (const outputPath of [sourceOutput, deliveryOutput]) {
  await fs.mkdir(path.dirname(outputPath), { recursive: true });
  const file = await SpreadsheetFile.exportXlsx(workbook);
  await file.save(outputPath);
}

const inspect = await workbook.inspect({
  kind: "workbook,sheet,formula",
  maxChars: 8000,
  tableMaxRows: 8,
  tableMaxCols: 8,
});
await fs.mkdir(path.join(projectRoot, "Temp/ConfigWorkbookPreview"), { recursive: true });
await fs.writeFile(path.join(projectRoot, "Temp/ConfigWorkbookPreview/inspect.json"), inspect.ndjson ?? JSON.stringify(inspect, null, 2));

for (const sheetName of ["使用说明", "英雄", "技能", "技能效果", "强化", "表现", "枚举参考"]) {
  const preview = await workbook.render({ sheetName, autoCrop: "all", scale: 1, format: "png" });
  await fs.writeFile(
    path.join(projectRoot, `Temp/ConfigWorkbookPreview/${sheetName}.png`),
    new Uint8Array(await preview.arrayBuffer()),
  );
}

console.log(`CONFIG_WORKBOOK_OK ${sourceOutput}`);
