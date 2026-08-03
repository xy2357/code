import { FileBlob, SpreadsheetFile } from "@oai/artifact-tool";

const input = await FileBlob.load("Assets/DqqGame/Config/DQQ_GameConfig.xlsx");
const workbook = await SpreadsheetFile.importXlsx(input);
const expected = {
  使用说明: 12,
  英雄: 8,
  技能: 22,
  技能效果: 26,
  强化: 38,
  表现: 14,
  枚举参考: 6,
};
const errors = [];
const summary = {};
for (const [name, expectedRows] of Object.entries(expected)) {
  const sheet = workbook.worksheets.getItem(name);
  const used = sheet.getUsedRange();
  const values = used?.values ?? [];
  summary[name] = { rows: values.length, columns: values[0]?.length ?? 0 };
  if (values.length !== expectedRows) errors.push(`${name}: expected ${expectedRows} rows, got ${values.length}`);
  values.forEach((row, rowIndex) => row.forEach((value, columnIndex) => {
    if (typeof value === "string" && /#(?:REF!|DIV\/0!|VALUE!|NAME\?|N\/A)/i.test(value)) {
      errors.push(`${name}!R${rowIndex + 1}C${columnIndex + 1}=${value}`);
    }
  }));
}
const overview = workbook.worksheets.getItem("使用说明").getRange("A4:B8");
const heroes = workbook.worksheets.getItem("英雄").getRange("A1:R8");
const effects = workbook.worksheets.getItem("技能效果").getRange("A1:J26");
console.log(JSON.stringify({
  summary,
  overviewValues: overview.values,
  overviewFormulas: overview.formulas,
  heroModelResources: heroes.values.slice(2).map((row) => [row[0], row[1], row[17]]),
  effectRows: effects.values.length - 2,
  errors,
}, null, 2));
if (errors.length) process.exitCode = 1;
