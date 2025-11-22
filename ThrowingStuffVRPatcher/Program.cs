using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Noggog;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using System.Text.RegularExpressions;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Strings;

namespace ThrowingStuffVRPatcher
{
    public class Program
    {
        public static IEnumerable<DestructionStage>? destructionStages;
        public static Dictionary<string, string> alcoholDict;
        public static Dictionary<string, string> glassDict;
        public static Dictionary<string, string> soupDict;
        public static Dictionary<string, string> whiteBombBigDict;
        public static Dictionary<string, string> whiteBombSmallDict;
        public static Dictionary<string, string> blackBombBigDict;
        public static Dictionary<string, string> blackBombSmallDict;
        public static Dictionary<string, string> purseDict;
        public static Dictionary<string, string> nullwhiteDict;
        public static Dictionary<string, string> firebombDict;
        public static Dictionary<string, string> oilbombDict;
        public static Dictionary<string, string> frostbombDict;
        public static Dictionary<string, string> shockbombDict;
        public static Dictionary<string, string> poisonbombDict;
        public static Dictionary<string, string> potionDict;
        public static Dictionary<string, string> poisonDict;
        public static Dictionary<string, string> exclusionsDict;

        static Program()
        {
            // Set the static members
            alcoholDict = new Dictionary<string, string>();
            glassDict = new Dictionary<string, string>();
            soupDict = new Dictionary<string, string>();
            whiteBombBigDict = new Dictionary<string, string>();
            whiteBombSmallDict = new Dictionary<string, string>();
            blackBombBigDict = new Dictionary<string, string>();
            blackBombSmallDict = new Dictionary<string, string>();
            purseDict = new Dictionary<string, string>();
            nullwhiteDict = new Dictionary<string, string>();
            firebombDict = new Dictionary<string, string>();
            oilbombDict = new Dictionary<string, string>();
            frostbombDict = new Dictionary<string, string>();
            shockbombDict = new Dictionary<string, string>();
            poisonbombDict = new Dictionary<string, string>();
            potionDict = new Dictionary<string, string>();
            poisonDict = new Dictionary<string, string>();
            exclusionsDict = new Dictionary<string, string>();

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

            try
            {
                Console.WriteLine("\n\nTrying to read ThrowStuff_KID.ini from " + state.DataFolderPath);
                using StreamReader reader = new(state.DataFolderPath + "\\ThrowStuff_KID.ini");
                Console.WriteLine("Found ThrowStuff_KID.ini");
                string text = reader.ReadToEnd();

                var sections = text.Split(";");
                foreach (var rawSection in sections)
                {
                    var section = rawSection.Trim();
                    if (section.Length == 0)
                    {
                        continue;
                    }

                    var itemsWithHeader = Regex.Split(
                        rawSection.Replace("\r\n", ""),
                        @"Keyword\s*=\s*",
                        RegexOptions.IgnoreCase);
                    var items = itemsWithHeader.Skip(1).Where(item => !string.IsNullOrWhiteSpace(item));

                    if (section.StartsWith("[D]Alcohol", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.alcoholDict, "Alcohol");
                        continue;
                    }
                    if (section.StartsWith("[D]MaterialGlass", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.glassDict, "MaterialGlass");
                        continue;
                    }
                    if (section.StartsWith("[D]isSoup", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.soupDict, "isSoup");
                        continue;
                    }
                    if (section.StartsWith("[D]WhiteDustBombBig", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.whiteBombBigDict, "WhiteDustBombBig");
                        continue;

                    }
                    if (section.StartsWith("[D]WhiteDustBombSmall", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.whiteBombSmallDict, "WhiteDustBombSmall");
                        continue;

                    }
                    if (section.StartsWith("[D]BlackDustBombBig", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.blackBombBigDict, "BlackDustBombBig");
                        continue;

                    }
                    if (section.StartsWith("[D]BlackDustBombSmall", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.blackBombSmallDict, "BlackDustBombSmall");
                        continue;

                    }
                    if (section.StartsWith("[D]PurseLarge", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.purseDict, "PurseLarge");
                        continue;

                    }
                    if (section.StartsWith("[D]nullWhite", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.nullwhiteDict, "nullWhite");
                        continue;

                    }
                    if (section.StartsWith("[D]FireBomb", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.firebombDict, "FireBomb");
                        continue;

                    }
                    if (section.StartsWith("[D]OilBomb", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.oilbombDict, "OilBomb");
                        continue;

                    }
                    if (section.StartsWith("[D]FrostBomb", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.frostbombDict, "FrostBomb");
                        continue;

                    }
                    if (section.StartsWith("[D]ShockBomb", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.shockbombDict, "ShockBomb");
                        continue;

                    }
                    if (section.StartsWith("[D]PoisonBomb", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.poisonbombDict, "PoisonBomb");
                        continue;

                    }
                    if (section.StartsWith("[D]isPotion", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.potionDict, "isPotion");
                        continue;

                    }
                    if (section.StartsWith("[D]IsPoison", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessIniSection(items, Program.poisonDict, "IsPoison");
                        continue;

                    }
                }
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
            foreach (var floraGetter in state.LoadOrder.PriorityOrder.Flora().WinningContextOverrides())
            {
                if (floraGetter.ModKey == "DropOnDeath.esp" || floraGetter.ModKey == "JS Purses and Septims SE Patch.esp")
                {
                    var flora = state.PatchMod.Florae.GetOrAddAsOverride(floraGetter.Record);

                    addDestruction(flora);

                    flora.Keywords ??= new();

                    if (floraGetter.ModKey == "DropOnDeath.esp")
                    {
                        flora.Keywords?.Add(ThrowStuff.Keyword.onmoPurse);
                        if (flora.Keywords.EmptyIfNull().Contains(ThrowStuff.Keyword.onmoPurse))
                        {
                            Console.WriteLine("Added keyword \"" + ThrowStuff.Keyword.onmoPurse.ResolveIdentifier(state.LinkCache) + "\" to " + flora?.Name);
                        }
                        else
                        {
                            Console.WriteLine("Failed to add keyword \"" + ThrowStuff.Keyword.onmoPurse.ToString() + "\" to " + flora?.Name);
                        }
                    }

                    if (flora?.Destructible?.Stages.Count == 8)
                    {
                        Console.WriteLine("Added destruction to \"" + flora?.Name + "\" from " + floraGetter.ModKey);
                    }
                    else
                    {
                        Console.WriteLine("Failed to add destruction to Potion/Poison \"" + flora?.Name + "\" from " + floraGetter.ModKey);
                    }

                }
            }

            // patch potions/food/drink
            foreach (var ingestibleGetter in state.LoadOrder.PriorityOrder.Ingestible().WinningContextOverrides())
            {
                var ingestibleRecord = ingestibleGetter.Record;
                if (ingestibleRecord.Destructible != null)
                {
                    continue;
                }

                var ingestibleName = GetNameString(ingestibleRecord);
                if (string.IsNullOrWhiteSpace(ingestibleName))
                {
                    Console.WriteLine("Name unresolved for ingestible " + ingestibleRecord.FormKey + " from " + ingestibleGetter.ModKey + "; falling back to keyword/flag detection.");
                }

                bool addedDestruction = false;
                foreach (var keyword in ingestibleRecord.Keywords.EmptyIfNull())
                {
                    if (!state.LinkCache.TryResolveIdentifier(Skyrim.Keyword.VendorItemPotion, out var potionKeyword) ||
                        !state.LinkCache.TryResolveIdentifier(Skyrim.Keyword.VendorItemPoison, out var poisonKeyword) ||
                        !state.LinkCache.TryResolveIdentifier(keyword, out var ourKeyword))
                    {
                        Console.WriteLine("Failed to find keywords while checking Ingestible " + ingestibleRecord.Name + " from " + ingestibleGetter.ModKey);
                        continue;
                    }

                    if ((ingestibleName?.Contains("potion", StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (ingestibleName?.Contains("poison", StringComparison.OrdinalIgnoreCase) ?? false) ||
                        ourKeyword == potionKeyword || ourKeyword == poisonKeyword ||
                        ingestibleRecord.Flags.HasFlag(Ingestible.Flag.Poison) ||
                        (!ingestibleRecord.Flags.HasFlag(Ingestible.Flag.FoodItem) && !ingestibleRecord.Flags.HasFlag(Ingestible.Flag.Medicine)))
                    {
                        var ingestible = state.PatchMod.Ingestibles.GetOrAddAsOverride(ingestibleRecord);
                        addDestruction(ingestible);

                        if (ingestible.Destructible?.Stages.Count == 8)
                        {
                            Console.WriteLine("Added destruction to Potion/Poison \"" + ingestible.Name + "\" from " + ingestibleGetter.ModKey);
                        }
                        else
                        {
                            Console.WriteLine("Failed to add destruction to Potion/Poison \"" + ingestible.Name + "\" from " + ingestibleGetter.ModKey);
                        }
                        addedDestruction = true;
                        break;
                    }

                }

                if (!addedDestruction)
                {
                    patchINIitems<IIngestibleGetter, IIngestible>(
                        ingestibleRecord,
                        ingestibleGetter.ModKey,
                        () => state.PatchMod.Ingestibles.GetOrAddAsOverride(ingestibleRecord));
                }
            }

            // patch ingredients
            foreach (var ingredientGetter in state.LoadOrder.PriorityOrder.Ingredient().WinningContextOverrides())
            {
                var ingredientRecord = ingredientGetter.Record;
                if (ingredientRecord.Destructible != null)
                {
                    continue;
                }

                if (!HasResolvableName(ingredientRecord))
                {
                    Console.WriteLine("Skipping ingredient " + ingredientRecord.FormKey + " from " + ingredientGetter.ModKey + " because its name could not be resolved.");
                    continue;
                }

                patchINIitems<IIngredientGetter, IIngredient>(
                    ingredientRecord,
                    ingredientGetter.ModKey,
                    () => state.PatchMod.Ingredients.GetOrAddAsOverride(ingredientRecord));
            }

            // patch misc items
            foreach (var miscItemGetter in state.LoadOrder.PriorityOrder.MiscItem().WinningContextOverrides())
            {
                var miscItemRecord = miscItemGetter.Record;
                if (miscItemRecord.Destructible != null)
                {
                    continue;
                }

                if (!HasResolvableName(miscItemRecord))
                {
                    Console.WriteLine("Skipping misc item " + miscItemRecord.FormKey + " from " + miscItemGetter.ModKey + " because its name could not be resolved.");
                    continue;
                }

                patchINIitems<IMiscItemGetter, IMiscItem>(
                    miscItemRecord,
                    miscItemGetter.ModKey,
                    () => state.PatchMod.MiscItems.GetOrAddAsOverride(miscItemRecord));
            }
        }

        static void addDestruction(dynamic myObject)
        {
            myObject.Destructible = new Destructible();
            myObject.Destructible.Data = new DestructableData();
            myObject.Destructible.Data.Health = 8;
            myObject.Destructible.Data.DESTCount = 8;
            myObject.Destructible.Stages.AddRange(Program.destructionStages.EmptyIfNull());

        }

        static void ProcessIniSection(IEnumerable<string> items, Dictionary<string, string> targetDict, string sectionName)
        {
            foreach (var item in items)
            {
                if (!TryParseIniEntry(item, out var itemName, out var itemType))
                {
                    continue;
                }

                if (itemName.StartsWith("-", StringComparison.Ordinal))
                {
                    Program.exclusionsDict[Regex.Replace(itemName, "-", "").ToLowerInvariant()] = sectionName;
                }
                else
                {
                    targetDict[itemName] = itemType;
                }
            }
        }

        static bool TryParseIniEntry(string rawItem, out string itemName, out string itemType)
        {
            itemName = string.Empty;
            itemType = string.Empty;

            if (string.IsNullOrWhiteSpace(rawItem))
            {
                return false;
            }

            var normalized = rawItem.Trim();
            var firstPipe = normalized.IndexOf("|", StringComparison.Ordinal);
            var lastPipe = normalized.LastIndexOf("|", StringComparison.Ordinal);

            if (firstPipe == -1 || lastPipe == -1 || lastPipe <= firstPipe)
            {
                return false;
            }

            itemType = normalized.Substring(firstPipe + 1, lastPipe - firstPipe - 1).Trim();
            itemName = normalized.Substring(lastPipe + 1).Trim();

            return itemType.Length > 0 && itemName.Length > 0;
        }

        static bool HasResolvableName(INamedGetter record)
        {
            return !string.IsNullOrWhiteSpace(GetNameString(record));
        }

        static string? GetNameString(INamedGetter record)
        {
            if (record is ITranslatedNamedGetter translatedNamed)
            {
                var translated = translatedNamed.Name;
                if (translated == null)
                {
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(translated.String))
                {
                    return translated.String;
                }

                if (translated.TryLookup(TranslatedString.DefaultLanguage, out var defaultLanguageName) &&
                    !string.IsNullOrWhiteSpace(defaultLanguageName))
                {
                    return defaultLanguageName;
                }

                if (translated.TryLookup(translated.TargetLanguage, out var targetLanguageName) &&
                    !string.IsNullOrWhiteSpace(targetLanguageName))
                {
                    return targetLanguageName;
                }
            }

            var nameValue = record.Name;
            if (!string.IsNullOrWhiteSpace(nameValue))
            {
                return nameValue;
            }

            if (record is IMajorRecordGetter major && !string.IsNullOrWhiteSpace(major.EditorID))
            {
                return major.EditorID;
            }

            return null;
        }

        static void patchINIitems<TGetter, TSetter>(TGetter myObject, ModKey modKey, Func<TSetter> getOrAddOverride)
            where TGetter : class, INamedGetter, ISkyrimMajorRecordGetter, IHasDestructibleGetter
            where TSetter : class, TGetter, IHasDestructible
        {

#if DEBUG
            if (myObject.Name?.Contains("flour", StringComparison.OrdinalIgnoreCase) ?? false)
                System.Diagnostics.Debugger.Break();
#endif
            string type = myObject switch
            {
                IIngestibleGetter => "Potion",
                IMiscItemGetter => "Misc Item",
                IIngredientGetter => "Ingredient",
                _ => myObject.GetType().Name
            };

            TSetter? editable = null;
            TSetter EnsureEditable()
            {
                editable ??= getOrAddOverride();
                return editable;
            }

            // Patch Alcohol
            if (Program.alcoholDict.Any(alcohol =>
                isValidObject(alcohol, myObject, type, "Alcohol")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Alcohol \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Alcohol \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Glass
            if (Program.glassDict.Any(glass =>
            isValidObject(glass, myObject, type, "MaterialGlass")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Glass Object \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Glass Object \"" + target?.Name + "\" from " + modKey);
                }
                return;
            }

            // Patch soups
            if (Program.soupDict.Any(soup =>
                isValidObject(soup, myObject, type, "isSoup")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Soup \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Soup \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch WhiteDustBombBig
            if (Program.whiteBombBigDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "WhiteDustBombBig")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " WhiteDustBombBig \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " WhiteDustBombBig \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch WhiteDustBombSmall
            if (Program.whiteBombSmallDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "WhiteDustBombSmall")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " WhiteDustBombSmall \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " WhiteDustBombSmall \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch BlackDustBombBig
            if (Program.blackBombBigDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "BlackDustBombBig")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " BlackDustBombBig \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " BlackDustBombBig \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch BlackDustBombSmall
            if (Program.blackBombSmallDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "BlackDustBombSmall")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " BlackDustBombSmall \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " BlackDustBombSmall \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Coin purses
            if (Program.purseDict.Any(purse =>
                isValidObject(purse, myObject, type, "PurseLarge")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Coin purse \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Coin purse \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch nullWhite
            if (Program.nullwhiteDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "nullWhite")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " nullWhite \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " nullWhite \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Firebomb
            if (Program.firebombDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "FireBomb")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Firebomb \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Firebomb \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Oilbomb
            if (Program.oilbombDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "OilBomb")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Oilbomb \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Oilbomb \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Frostbomb
            if (Program.frostbombDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "FrostBomb")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Frostbomb \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Frostbomb \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Shockbomb
            if (Program.shockbombDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "ShockBomb")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Shockbomb \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Shockbomb \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Poisonbomb
            if (Program.poisonbombDict.Any(bomb =>
                isValidObject(bomb, myObject, type, "PoisonBomb")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Poisonbomb \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Poisonbomb \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Potion
            if (Program.potionDict.Any(potion =>
                isValidObject(potion, myObject, type, "isPotion")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Potion \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Potion \"" + target?.Name + "\" from " + modKey);
                }
                return;

            }

            // Patch Poison
            if (Program.poisonDict.Any(poison =>
                isValidObject(poison, myObject, type, "IsPoison")))
            {
                var target = EnsureEditable();
                addDestruction(target);
                if (target?.Destructible?.Stages.Count == 8)
                {
                    Console.WriteLine("Added destruction to " + type + " Poison \"" + target.Name + "\" from " + modKey);
                }
                else
                {
                    Console.WriteLine("Failed to add destruction to " + type + " Poison \"" + target?.Name + "\" from " + modKey);
                }
            }
        }

        static bool isValidObject<T>(KeyValuePair<string, string> entry, T myObject, string type, string section)
            where T : INamedGetter
        {
            var objectName = GetNameString(myObject);
            if (string.IsNullOrWhiteSpace(objectName))
            {
                return false;
            }

            if (!string.Equals(entry.Value, type, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var normalizedName = objectName!.ToLowerInvariant();
            var normalizedEntry = entry.Key.ToLowerInvariant();
            var normalizedPattern = Regex.Replace(normalizedEntry, "-|\\+|\\*", "");
            var hasWildcard = entry.Key.IndexOf('*') >= 0;
            var matches = hasWildcard
                ? normalizedName.Contains(normalizedPattern, StringComparison.Ordinal)
                : normalizedEntry.Equals(normalizedName, StringComparison.Ordinal);

            if (!matches)
            {
                return false;
            }

            return !(Program.exclusionsDict.TryGetValue(normalizedName, out var exclusionType) &&
                string.Equals(exclusionType, section, StringComparison.OrdinalIgnoreCase));
        }
    }
}
