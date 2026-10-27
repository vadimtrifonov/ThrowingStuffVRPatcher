using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Cache;
using Noggog;
using Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace ThrowingStuffVRPatcher
{
    public class Program
    {
        public static IEnumerable<DestructionStage>? destructionStages;

        // ThrowStuff_KID.ini marks these keywords with [D]: items that carry them disappear on impact, which needs destruction data.
        static readonly HashSet<string> destructionKeywords = new(StringComparer.OrdinalIgnoreCase)
        {
            "Alcohol", "MaterialGlass", "isSoup", "WhiteDustBombBig", "WhiteDustBombSmall", "BlackDustBombBig", "BlackDustBombSmall",
            "PurseLarge", "PurseMedium", "PurseSmall", "nullWhite", "FireBomb", "OilBomb", "FrostBomb", "ShockBomb", "PoisonBomb",
            "isPotion", "IsPoison",
        };

        record DestructionRule(string Keyword, string ItemType, string[] Names, string[] Exclusions);

        static Program()
        {
            /////////////////////////////////////////////////////////
            // Destruction Stages

            // 1
            var destructedStage1 = new DestructionStage();
            var destructedStage1Data = new DestructionStageData();
            destructedStage1.Data = destructedStage1Data;

            destructedStage1Data.HealthPercent = 88;
            destructedStage1Data.Index = 0;
            destructedStage1Data.ModelDamageStage = 1;
            destructedStage1Data.Flags = destructedStage1Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            // 2
            var destructedStage2 = new DestructionStage();
            var destructedStage2Data = new DestructionStageData();
            destructedStage2.Data = destructedStage2Data;

            destructedStage2Data.HealthPercent = 75;
            destructedStage2Data.Index = 1;
            destructedStage2Data.ModelDamageStage = 2;
            destructedStage2Data.Flags = destructedStage2Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            // 3
            var destructedStage3 = new DestructionStage();
            var destructedStage3Data = new DestructionStageData();
            destructedStage3.Data = destructedStage3Data;

            destructedStage3Data.HealthPercent = 63;
            destructedStage3Data.Index = 2;
            destructedStage3Data.ModelDamageStage = 3;
            destructedStage3Data.Flags = destructedStage3Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            // 4
            var destructedStage4 = new DestructionStage();
            var destructedStage4Data = new DestructionStageData();
            destructedStage4.Data = destructedStage4Data;

            destructedStage4Data.HealthPercent = 50;
            destructedStage4Data.Index = 3;
            destructedStage4Data.ModelDamageStage = 4;
            destructedStage4Data.Flags = destructedStage4Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            // 5
            var destructedStage5 = new DestructionStage();
            var destructedStage5Data = new DestructionStageData();
            destructedStage5.Data = destructedStage5Data;

            destructedStage5Data.HealthPercent = 38;
            destructedStage5Data.Index = 4;
            destructedStage5Data.ModelDamageStage = 5;
            destructedStage5Data.Flags = destructedStage5Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            // 6
            var destructedStage6 = new DestructionStage();
            var destructedStage6Data = new DestructionStageData();
            destructedStage6.Data = destructedStage6Data;

            destructedStage6Data.HealthPercent = 26;
            destructedStage6Data.Index = 5;
            destructedStage6Data.ModelDamageStage = 6;
            destructedStage6Data.Flags = destructedStage6Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            // 7
            var destructedStage7 = new DestructionStage();
            var destructedStage7Data = new DestructionStageData();
            destructedStage7.Data = destructedStage7Data;

            destructedStage7Data.HealthPercent = 13;
            destructedStage7Data.Index = 6;
            destructedStage7Data.ModelDamageStage = 7;
            destructedStage7Data.Flags = destructedStage7Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            // 8
            var destructedStage8 = new DestructionStage();
            var destructedStage8Data = new DestructionStageData();
            destructedStage8.Data = destructedStage8Data;

            destructedStage8Data.HealthPercent = 0;
            destructedStage8Data.Index = 7;
            destructedStage8Data.ModelDamageStage = 8;
            destructedStage8Data.Flags = destructedStage8Data.Flags.SetFlag(DestructionStageData.Flag.CapDamage, true);

            destructionStages = new[] { destructedStage1, destructedStage2, destructedStage3, destructedStage4, destructedStage5, destructedStage6, destructedStage7, destructedStage8 };

        }

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimVR, "ThrowingStuffVRPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (!state.LoadOrder.ContainsKey(ThrowStuff.ModKey))
                throw new Exception("ERROR: ThrowStuff.esp missing from load order. Check that the esp is enabled.");

#if DEBUG
            foreach (var mod in state.LoadOrder)
            {
                Console.WriteLine(mod.Value.ToString());
            }
#endif
            ////////////////////////////////////////////////////////
            // Read from ThrowStuff_KID.ini

            List<DestructionRule> rules;
            try
            {
                Console.WriteLine("\n\nTrying to read ThrowStuff_KID.ini from " + state.DataFolderPath);
                rules = ReadDestructionRules(state.DataFolderPath + "\\ThrowStuff_KID.ini");
                Console.WriteLine("Found ThrowStuff_KID.ini with " + rules.Count + " destruction rules");
            }
            catch (IOException e)
            {
                Console.WriteLine("Did not find ThrowStuff_KID.ini in data folder.");
                Console.WriteLine(e.Message);
                throw;
            }

            //////////////////////////////////////////////////////////
            // Begin patch
            // patch onmo coin purse
            foreach (var floraContext in state.LoadOrder.PriorityOrder.Flora().WinningContextOverrides())
            {
                if (floraContext.ModKey != "DropOnDeath.esp" && floraContext.ModKey != "JS Purses and Septims SE Patch.esp")
                {
                    continue;
                }

                var flora = AddDestruction(state.PatchMod, floraContext, "Flora");
                if (flora != null && floraContext.ModKey == "DropOnDeath.esp")
                {
                    flora.Keywords ??= new();
                    flora.Keywords.Add(ThrowStuff.Keyword.onmoPurse);
                    Console.WriteLine("Added keyword onmoPurse to \"" + flora.Name + "\" from " + floraContext.ModKey);
                }
            }

            // patch potions/food/drink
            foreach (var ingestibleContext in state.LoadOrder.PriorityOrder.Ingestible().WinningContextOverrides())
            {
                var ingestible = ingestibleContext.Record;
                if (ingestible.Destructible != null)
                {
                    continue;
                }

                if (IsPotionOrPoison(ingestible))
                {
                    AddDestruction(state.PatchMod, ingestibleContext, "Potion/Poison");
                    continue;
                }

                var keyword = FindDestructionKeyword(rules, "Potion", ingestible);
                if (keyword != null)
                {
                    AddDestruction(state.PatchMod, ingestibleContext, "Potion " + keyword);
                }
            }

            // patch ingredients
            foreach (var ingredientContext in state.LoadOrder.PriorityOrder.Ingredient().WinningContextOverrides())
            {
                if (ingredientContext.Record.Destructible == null &&
                    FindDestructionKeyword(rules, "Ingredient", ingredientContext.Record) is { } keyword)
                {
                    AddDestruction(state.PatchMod, ingredientContext, "Ingredient " + keyword);
                }
            }

            // patch misc items
            foreach (var miscItemContext in state.LoadOrder.PriorityOrder.MiscItem().WinningContextOverrides())
            {
                if (miscItemContext.Record.Destructible == null &&
                    FindDestructionKeyword(rules, "Misc Item", miscItemContext.Record) is { } keyword)
                {
                    AddDestruction(state.PatchMod, miscItemContext, "Misc Item " + keyword);
                }
            }
        }

        // Reads `Keyword = Keyword|Type|Filters|Traits|Chance` lines for destruction keywords.
        // Filters are display names: `Name` is exact, `*text` is contained, and a leading `-` excludes; commas separate alternatives.
        static List<DestructionRule> ReadDestructionRules(string path)
        {
            var rules = new List<DestructionRule>();
            foreach (var line in File.ReadLines(path))
            {
                var entry = line.Split('=', 2, StringSplitOptions.TrimEntries);
                if (entry.Length < 2 || !entry[0].Equals("Keyword", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var fields = entry[1].Split('|', StringSplitOptions.TrimEntries);
                if (!destructionKeywords.Contains(fields[0]))
                {
                    continue;
                }

                var filters = fields.Length > 2 ? fields[2].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) : [];
                var names = filters.Where(filter => !filter.StartsWith('-')).ToArray();
                var traits = fields.Length > 3 ? fields[3] : "";
                var chance = fields.Length > 4 ? fields[4] : "";
                if (names.Length == 0 || names.Any(IsOmitted) || filters.Any(filter => filter.Contains('+')) ||
                    !IsOmitted(traits) || !(IsOmitted(chance) || chance == "100"))
                {
                    Console.WriteLine("Skipping unsupported ThrowStuff_KID.ini rule: " + line.Trim());
                    continue;
                }

                var exclusions = filters.Where(filter => filter.StartsWith('-')).Select(filter => filter[1..]).ToArray();
                rules.Add(new DestructionRule(fields[0], fields[1], names, exclusions));
            }
            return rules;

            // KID treats empty and NONE fields as omitted.
            static bool IsOmitted(string field) => field.Length == 0 || field.Equals("NONE", StringComparison.OrdinalIgnoreCase);
        }

        static string? FindDestructionKeyword(List<DestructionRule> rules, string itemType, ITranslatedNamedGetter item)
        {
            var name = item.Name?.String;
            if (name == null)
            {
                return null;
            }

            return rules.FirstOrDefault(rule =>
                rule.ItemType.Equals(itemType, StringComparison.OrdinalIgnoreCase) &&
                rule.Names.Any(filter => NameMatches(name, filter)) &&
                !rule.Exclusions.Any(filter => NameMatches(name, filter)))?.Keyword;
        }

        static bool NameMatches(string name, string filter)
        {
            return filter.StartsWith('*')
                ? name.Contains(filter[1..], StringComparison.OrdinalIgnoreCase)
                : name.Equals(filter, StringComparison.OrdinalIgnoreCase);
        }

        static bool IsPotionOrPoison(IIngestibleGetter ingestible)
        {
            var name = ingestible.Name?.String ?? string.Empty;
            return name.Contains("potion", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("poison", StringComparison.OrdinalIgnoreCase) ||
                ingestible.Keywords.EmptyIfNull().Any(keyword =>
                    keyword.FormKey == Skyrim.Keyword.VendorItemPotion.FormKey ||
                    keyword.FormKey == Skyrim.Keyword.VendorItemPoison.FormKey) ||
                ingestible.Flags.HasFlag(Ingestible.Flag.Poison) ||
                (!ingestible.Flags.HasFlag(Ingestible.Flag.FoodItem) && !ingestible.Flags.HasFlag(Ingestible.Flag.Medicine));
        }

        // Returns the patched override, or null when the override would lose the record's name.
        static TMajor? AddDestruction<TMajor, TMajorGetter>(
            ISkyrimMod patchMod,
            IModContext<ISkyrimMod, ISkyrimModGetter, TMajor, TMajorGetter> context,
            string label)
            where TMajor : class, ISkyrimMajorRecord, TMajorGetter, IHasDestructible
            where TMajorGetter : class, ISkyrimMajorRecordGetter
        {
            var name = context.Record switch
            {
                ITranslatedNamedGetter named => named.Name,
                ITranslatedNamedRequiredGetter named => named.Name,
                _ => null,
            };

            // A localized name whose string could not be read would be written to the override as an empty name.
            if (name != null && name.String == null)
            {
                Console.WriteLine("Skipped " + label + " " + context.Record.FormKey + " from " + context.ModKey + " because its name could not be read.");
                return null;
            }

            var record = context.GetOrAddAsOverride(patchMod);
            record.Destructible = new Destructible
            {
                Data = new DestructableData { Health = 8, DESTCount = 8 },
            };
            record.Destructible.Stages.AddRange(destructionStages.EmptyIfNull());
            Console.WriteLine("Added destruction to " + label + " \"" + name?.String + "\" from " + context.ModKey);
            return record;
        }
    }
}
