#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using DqqGame.Combat;
using DqqGame.Presentation;
using UnityEditor;
using UnityEngine;

namespace DqqGame.Editor
{
    public static class ExcelConfigImporter
    {
        private const string WorkbookPath = "Assets/DqqGame/Config/DQQ_GameConfig.xlsx";
        private const string OutputRoot = "Assets/DqqGame/Resources/Config";

        [MenuItem("DQQ/从 Excel 导入配置")]
        public static void ImportFromMenu()
        {
            Import(true);
        }

        public static void Import(bool refreshAssets)
        {
            if (!File.Exists(WorkbookPath))
                throw new FileNotFoundException("找不到游戏配置 Excel。", WorkbookPath);

            using (XlsxReader workbook = new XlsxReader(WorkbookPath))
            {
                HeroConfig[] heroes = ReadObjects<HeroConfig>(workbook.ReadTable("英雄")).ToArray();
                AbilityConfig[] abilities = ReadObjects<AbilityConfig>(workbook.ReadTable("技能")).ToArray();
                UpgradeConfig[] upgrades = ReadObjects<UpgradeConfig>(workbook.ReadTable("强化")).ToArray();
                AbilityPresentationConfig[] presentation =
                    ReadObjects<AbilityPresentationConfig>(workbook.ReadTable("表现")).ToArray();

                Dictionary<int, List<IndexedEffect>> effects = new Dictionary<int, List<IndexedEffect>>();
                foreach (Dictionary<string, string> row in workbook.ReadTable("技能效果"))
                {
                    int abilityId = ParseInt(Value(row, "abilityId"));
                    IndexedEffect item = new IndexedEffect
                    {
                        Index = ParseInt(Value(row, "effectIndex")),
                        Effect = ReadObject<EffectConfig>(row)
                    };
                    if (!effects.TryGetValue(abilityId, out List<IndexedEffect> list))
                    {
                        list = new List<IndexedEffect>();
                        effects.Add(abilityId, list);
                    }
                    list.Add(item);
                }

                foreach (AbilityConfig ability in abilities)
                {
                    ability.effects = effects.TryGetValue(ability.abilityId, out List<IndexedEffect> list)
                        ? list.OrderBy(item => item.Index).Select(item => item.Effect).ToArray()
                        : Array.Empty<EffectConfig>();
                }

                Validate(heroes, abilities, upgrades);
                Directory.CreateDirectory(OutputRoot);
                WriteJson("abilities.json", new AbilityConfigList { abilities = abilities });
                WriteJson("heroes.json", new HeroConfigList { heroes = heroes });
                WriteJson("upgrades.json", new UpgradeConfigList { upgrades = upgrades });
                WriteJson("presentation.json", new PresentationConfigList { abilities = presentation });
                Debug.Log($"DQQ_EXCEL_IMPORT_OK heroes={heroes.Length} abilities={abilities.Length} " +
                          $"effects={effects.Sum(pair => pair.Value.Count)} upgrades={upgrades.Length} " +
                          $"presentation={presentation.Length}");
            }

            if (refreshAssets) AssetDatabase.Refresh();
        }

        private static IEnumerable<T> ReadObjects<T>(IEnumerable<Dictionary<string, string>> rows) where T : new()
        {
            foreach (Dictionary<string, string> row in rows)
                yield return ReadObject<T>(row);
        }

        private static T ReadObject<T>(Dictionary<string, string> row) where T : new()
        {
            T target = new T();
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!row.TryGetValue(field.Name, out string raw) || string.IsNullOrWhiteSpace(raw)) continue;
                if (field.FieldType == typeof(string)) field.SetValue(target, raw);
                else if (field.FieldType == typeof(int)) field.SetValue(target, ParseInt(raw));
                else if (field.FieldType == typeof(bool)) field.SetValue(target, ParseBool(raw));
            }
            return target;
        }

        private static int ParseInt(string value)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int integer)) return integer;
            return (int)Math.Round(double.Parse(value, CultureInfo.InvariantCulture));
        }

        private static bool ParseBool(string value)
        {
            return value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   value.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private static string Value(Dictionary<string, string> row, string key)
        {
            return row.TryGetValue(key, out string value) ? value : string.Empty;
        }

        private static void Validate(HeroConfig[] heroes, AbilityConfig[] abilities, UpgradeConfig[] upgrades)
        {
            RequireUnique(heroes.Select(item => item.heroId), "heroId");
            RequireUnique(abilities.Select(item => item.abilityId), "abilityId");
            RequireUnique(upgrades.Select(item => item.upgradeId), "upgradeId");
            HashSet<int> ids = new HashSet<int>(abilities.Select(item => item.abilityId));
            foreach (HeroConfig hero in heroes)
            {
                if (!ids.Contains(hero.passiveAbilityId) || !ids.Contains(hero.ultimateAbilityId))
                    throw new InvalidDataException($"英雄 {hero.heroName} 引用了不存在的技能。 ");
            }
            foreach (UpgradeConfig upgrade in upgrades)
            {
                if (upgrade.addAbilityId != 0 && !ids.Contains(upgrade.addAbilityId))
                    throw new InvalidDataException($"强化 {upgrade.upgradeName} 引用了不存在的技能。 ");
            }
        }

        private static void RequireUnique<T>(IEnumerable<T> values, string label)
        {
            T[] list = values.ToArray();
            if (list.Distinct().Count() != list.Length)
                throw new InvalidDataException($"Excel 中存在重复的 {label}。 ");
        }

        private static void WriteJson(string name, object value)
        {
            File.WriteAllText(Path.Combine(OutputRoot, name), JsonUtility.ToJson(value, true), new UTF8Encoding(false));
        }

        private sealed class IndexedEffect
        {
            public int Index;
            public EffectConfig Effect;
        }

        private sealed class XlsxReader : IDisposable
        {
            private static readonly XNamespace Main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
            private static readonly XNamespace Rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
            private static readonly XNamespace PackageRel = "http://schemas.openxmlformats.org/package/2006/relationships";
            private readonly FileStream stream;
            private readonly ZipArchive archive;
            private readonly Dictionary<string, string> sheetPaths = new Dictionary<string, string>();
            private readonly List<string> sharedStrings = new List<string>();

            public XlsxReader(string path)
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                archive = new ZipArchive(stream, ZipArchiveMode.Read);
                LoadSharedStrings();
                LoadSheetPaths();
            }

            public List<Dictionary<string, string>> ReadTable(string sheetName)
            {
                if (!sheetPaths.TryGetValue(sheetName, out string path))
                    throw new InvalidDataException($"Excel 中缺少工作表：{sheetName}");
                XDocument document = LoadXml(path);
                Dictionary<int, string> keys = new Dictionary<int, string>();
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                foreach (XElement row in document.Descendants(Main + "row"))
                {
                    int rowIndex = (int?)row.Attribute("r") ?? 0;
                    Dictionary<int, string> cells = new Dictionary<int, string>();
                    foreach (XElement cell in row.Elements(Main + "c"))
                    {
                        int column = ColumnIndex((string)cell.Attribute("r"));
                        cells[column] = CellValue(cell);
                    }
                    if (rowIndex == 1)
                    {
                        foreach (KeyValuePair<int, string> pair in cells)
                            if (!string.IsNullOrWhiteSpace(pair.Value)) keys[pair.Key] = pair.Value.Trim();
                    }
                    else if (rowIndex >= 3 && cells.Values.Any(value => !string.IsNullOrWhiteSpace(value)))
                    {
                        Dictionary<string, string> item = new Dictionary<string, string>(StringComparer.Ordinal);
                        foreach (KeyValuePair<int, string> pair in cells)
                            if (keys.TryGetValue(pair.Key, out string key)) item[key] = pair.Value.Trim();
                        if (item.Count > 0) result.Add(item);
                    }
                }
                return result;
            }

            private void LoadSharedStrings()
            {
                ZipArchiveEntry entry = archive.GetEntry("xl/sharedStrings.xml");
                if (entry == null) return;
                XDocument document = LoadXml(entry);
                foreach (XElement item in document.Descendants(Main + "si"))
                    sharedStrings.Add(string.Concat(item.Descendants(Main + "t").Select(node => node.Value)));
            }

            private void LoadSheetPaths()
            {
                XDocument workbook = LoadXml("xl/workbook.xml");
                XDocument relationships = LoadXml("xl/_rels/workbook.xml.rels");
                Dictionary<string, string> targets = relationships.Descendants(PackageRel + "Relationship")
                    .ToDictionary(node => (string)node.Attribute("Id"), node => (string)node.Attribute("Target"));
                foreach (XElement sheet in workbook.Descendants(Main + "sheet"))
                {
                    string name = (string)sheet.Attribute("name");
                    string id = (string)sheet.Attribute(Rel + "id");
                    if (!targets.TryGetValue(id, out string target)) continue;
                    target = target.Replace('\\', '/').TrimStart('/');
                    sheetPaths[name] = target.StartsWith("xl/", StringComparison.Ordinal) ? target : "xl/" + target;
                }
            }

            private string CellValue(XElement cell)
            {
                string type = (string)cell.Attribute("t") ?? string.Empty;
                if (type == "inlineStr") return string.Concat(cell.Descendants(Main + "t").Select(node => node.Value));
                string raw = cell.Element(Main + "v")?.Value ?? string.Empty;
                if (type == "s" && int.TryParse(raw, out int index) && index >= 0 && index < sharedStrings.Count)
                    return sharedStrings[index];
                return raw;
            }

            private static int ColumnIndex(string reference)
            {
                int value = 0;
                foreach (char character in reference)
                {
                    if (!char.IsLetter(character)) break;
                    value = value * 26 + char.ToUpperInvariant(character) - 'A' + 1;
                }
                return value - 1;
            }

            private XDocument LoadXml(string path)
            {
                ZipArchiveEntry entry = archive.GetEntry(path) ?? throw new InvalidDataException($"XLSX 内缺少 {path}");
                return LoadXml(entry);
            }

            private static XDocument LoadXml(ZipArchiveEntry entry)
            {
                using (Stream input = entry.Open()) return XDocument.Load(input);
            }

            public void Dispose()
            {
                archive.Dispose();
                stream.Dispose();
            }
        }
    }
}
#endif
