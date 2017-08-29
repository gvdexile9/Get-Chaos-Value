﻿using Newtonsoft.Json;
using PoeHUD.Controllers;
using PoeHUD.Models;
using PoeHUD.Models.Enums;
using PoeHUD.Plugins;
using PoeHUD.Poe;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.EntityComponents;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GetValue
{
    public class GetValuePlugin : BaseSettingsPlugin<GetValueSettings>
    {
        string NinjaDirectory;
        public poe_ninja_api.Currency.RootObject Currency;
        public poe_ninja_api.DivinationCards.RootObject DivinationCards;
        public poe_ninja_api.Essences.RootObject Essences;
        public poe_ninja_api.Fragments.RootObject Fragments;
        public poe_ninja_api.Prophecies.RootObject Prophecies;
        public poe_ninja_api.UniqueAccessories.RootObject UniqueAccessories;
        public poe_ninja_api.UniqueArmours.RootObject UniqueArmours;
        public poe_ninja_api.UniqueFlasks.RootObject UniqueFlasks;
        public poe_ninja_api.UniqueJewels.RootObject UniqueJewels;
        public poe_ninja_api.UniqueMaps.RootObject UniqueMaps;
        public poe_ninja_api.UniqueWeapons.RootObject UniqueWeapons;
        public poe_ninja_api.WhiteMaps.RootObject WhiteMaps;
        public bool DownloadDone = false;
        public bool InitJsonDone = false;

        public override void Initialise()
        {
            NinjaDirectory = PluginDirectory + $"\\NinjaData\\";

            // Make folder if it doesnt exist
            FileInfo file = new FileInfo(NinjaDirectory);
            file.Directory.Create(); // If the directory already exists, this method does nothing.

            var Leagues = new List<string>() { "Standard", "Hardcore", "Harbinger", "Hardcore Harbinger" };
            string currentLeague = Leagues[0];
            Settings.LeagueList.SetListValues(Leagues);

            // display default league in setting
            if (Settings.LeagueList.Value == null)
                Settings.LeagueList.Value = currentLeague;
            // set wanted league
            currentLeague = Settings.LeagueList.Value;

            Task mytask = Task.Run(() =>
            {
                LogMessage("Gathering Data from Poe.Ninja.", 5);
                DownloadPoeNinjaAPI(currentLeague);
                DownloadDone = true;
                LogMessage("Finished Gathering Data from Poe.Ninja.", 5);
            });
        }

        private void InitPoeNinjaData_Harbinger()
        {
            using (StreamReader r = new StreamReader(NinjaDirectory + "Currency.json")) { string json = r.ReadToEnd(); Currency = JsonConvert.DeserializeObject<poe_ninja_api.Currency.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "DivinationCards.json")) { string json = r.ReadToEnd(); DivinationCards = JsonConvert.DeserializeObject<poe_ninja_api.DivinationCards.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "Essences.json")) { string json = r.ReadToEnd(); Essences = JsonConvert.DeserializeObject<poe_ninja_api.Essences.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "Fragments.json")) { string json = r.ReadToEnd(); Fragments = JsonConvert.DeserializeObject<poe_ninja_api.Fragments.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "Prophecies.json")) { string json = r.ReadToEnd(); Prophecies = JsonConvert.DeserializeObject<poe_ninja_api.Prophecies.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "UniqueAccessories.json")) { string json = r.ReadToEnd(); UniqueAccessories = JsonConvert.DeserializeObject<poe_ninja_api.UniqueAccessories.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "UniqueArmours.json")) { string json = r.ReadToEnd(); UniqueArmours = JsonConvert.DeserializeObject<poe_ninja_api.UniqueArmours.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "UniqueFlasks.json")) { string json = r.ReadToEnd(); UniqueFlasks = JsonConvert.DeserializeObject<poe_ninja_api.UniqueFlasks.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "UniqueJewels.json")) { string json = r.ReadToEnd(); UniqueJewels = JsonConvert.DeserializeObject<poe_ninja_api.UniqueJewels.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "UniqueMaps.json")) { string json = r.ReadToEnd(); UniqueMaps = JsonConvert.DeserializeObject<poe_ninja_api.UniqueMaps.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "UniqueWeapons.json")) { string json = r.ReadToEnd(); UniqueWeapons = JsonConvert.DeserializeObject<poe_ninja_api.UniqueWeapons.RootObject>(json); }
            using (StreamReader r = new StreamReader(NinjaDirectory + "WhiteMaps.json")) { string json = r.ReadToEnd(); WhiteMaps = JsonConvert.DeserializeObject<poe_ninja_api.WhiteMaps.RootObject>(json); }

        }

        private void DownloadPoeNinjaAPI(string league)
        {

            poe_ninja_api.API.SaveJSON(NinjaDirectory + "Currency.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetCurrencyOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "DivinationCards.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetDivinationCardsOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "Essences.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetEssenceOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "Fragments.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetFragmentOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "Prophecies.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetProphecyOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "UniqueAccessories.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetUniqueAccessoryOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "UniqueArmours.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetUniqueArmourOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "UniqueFlasks.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetUniqueFlaskOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "UniqueJewels.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetUniqueJewelOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "UniqueMaps.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetUniqueMapOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "UniqueWeapons.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetUniqueWeaponOverview?league={league}"));
            poe_ninja_api.API.SaveJSON(NinjaDirectory + "WhiteMaps.json", poe_ninja_api.API.DownloadAPI($"http://cdn.poe.ninja/api/Data/GetMapOverview?league={league}"));
        }

        public override void Render()
        {
            base.Render();
            // Do Stuff Every Frame
            if (DownloadDone && !InitJsonDone)
            {
                InitPoeNinjaData_Harbinger();
                InitJsonDone = true;
            }
            if (DownloadDone && InitJsonDone)
            {
                try
                {
                    var lineCount = 0;
                    var window = GameController.Window.GetWindowRectangle();
                    Vector2 drawPos = window.TopLeft;
                    window.Width = 400;
                    window.Height = 15;

                    Element uiHover = GameController.Game.IngameState.UIHover;
                    var inventoryItemIcon = uiHover.AsObject<HoverItemIcon>();
                    if (inventoryItemIcon == null)
                        return;
                    Element tooltip = inventoryItemIcon.Tooltip;
                    Entity poeEntity = inventoryItemIcon.Item;

                    if (tooltip == null || poeEntity.Address == 0 || !poeEntity.IsValid) { return; }

                    if (inventoryItemIcon == null)
                        return;

                    var item = inventoryItemIcon.Item;

                    var BaseItemType = GameController.Files.BaseItemTypes.Translate(item.Path);
                    if (BaseItemType == null)
                        return;
                    var dropLevel = BaseItemType.DropLevel;
                    string className = GetClassName(BaseItemType);
                    var itemBase = item.GetComponent<Base>();
                    var mods = item.GetComponent<Mods>();
                    var path = item.Path;


                    var identified = mods.Identified;
                    var UniqueItemName = mods.UniqueName;
                    var BaseItemName = BaseItemType.BaseName;
                    var isMap = item.HasComponent<PoeHUD.Poe.Components.Map>();
                    var itemRarity = mods.ItemRarity;
                    ShowChaosValue(window, drawPos, item, className, path, identified, UniqueItemName, BaseItemName, isMap, itemRarity, lineCount);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private string GetClassName(BaseItemType BaseItemType)
        {
            ItemClass tmp;
            string className;
            if (GameController.Files.itemClasses.contents.TryGetValue(BaseItemType.ClassName, out tmp))
                className = tmp.ClassName;
            else
                className = BaseItemType.ClassName;
            return className;
        }

        private void ShowChaosValue(RectangleF window, Vector2 TextPos, Entity item, string className, string path, bool identified, string UniqueItemName, string BaseItemName, bool isMap, ItemRarity itemRarity, int lineCount)
        {
            #region Non-Unique Maps
            if (itemRarity != ItemRarity.Unique && isMap)
            {
                if (item != null)
                {
                    if (WhiteMaps.lines.Find(x => x.name == BaseItemName && x.variant == "Atlas") == null)
                        return;

                    var _item = WhiteMaps.lines.Find(x => x.name == BaseItemName && x.variant == "Atlas");
                    var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                    DrawText(ref TextPos, ref lineCount, _text);
                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Unique Maps
            else if (itemRarity == ItemRarity.Unique && isMap)
            {
                if (item != null)
                {
                    if (UniqueMaps.lines.Find(x => x.baseType == BaseItemName) == null)
                        return;

                    var _item = UniqueMaps.lines.Find(x => x.baseType == BaseItemName);
                    var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                    DrawText(ref TextPos, ref lineCount, _text);
                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Currency
            else if (path.Contains("Currency") && BaseItemName != "Chaos Orb" && !BaseItemName.Contains("Shard") && !BaseItemName.Contains("Essence") && !BaseItemName.Contains("Remnant of") && !BaseItemName.Contains("Wisdom") && BaseItemName != "Prophecy")
            {
                if (item != null)
                {
                    if (Currency.lines.Find(x => x.currencyTypeName == BaseItemName) == null)
                        return;

                    var _item = Currency.lines.Find(x => x.currencyTypeName == BaseItemName);
                    var _text = $"Chaos: {_item.chaosEquivalent} || Change last 7 days: {_item.receiveSparkLine.totalChange}%";

                    DrawText(ref TextPos, ref lineCount, _text);
                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Essences
            else if (BaseItemName.Contains("Essence") || BaseItemName.Contains("Remnant of"))
            {
                if (item != null)
                {
                    if (Essences.lines.Find(x => x.name == BaseItemName) == null)
                        return;

                    var _item = Essences.lines.Find(x => x.name == BaseItemName);
                    var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                    DrawText(ref TextPos, ref lineCount, _text);
                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Divination Cards
            else if (className.Contains("Divination"))
            {
                if (item != null)
                {
                    if (DivinationCards.lines.Find(x => x.name == BaseItemName) == null)
                        return;

                    var _item = DivinationCards.lines.Find(x => x.name == BaseItemName);
                    var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                    DrawText(ref TextPos, ref lineCount, _text);
                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Fragments
            else if (className == "Map Fragments" || BaseItemName == "Offering to the Goddess")
            {
                if (item != null)
                {
                    if (Fragments.lines.Find(x => x.currencyTypeName == BaseItemName) == null)
                        return;

                    var _item = Fragments.lines.Find(x => x.currencyTypeName == BaseItemName);
                    var _text = $"Chaos: {_item.chaosEquivalent} || Change last 7 days: {_item.receiveSparkLine.totalChange}%";

                    DrawText(ref TextPos, ref lineCount, _text);
                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Unique Accessories
            else if (itemRarity == ItemRarity.Unique && (className == "Amulets" || className == "Rings" || className == "Belts") && identified)
            {
                if (item != null)
                {
                    var _taliosSignCorrect = "Tasalio's Sign";
                    var _taliosSignIncorrect = "Tasalio’s Sign";

                    if (UniqueAccessories.lines.Find(x => x.name == UniqueItemName) == null)
                        return;
                    if (UniqueAccessories.lines.Find(x => x.name == _taliosSignCorrect) == null)
                        return;

                    if (UniqueAccessories.lines.Find(x => x.name == UniqueItemName) != null)
                    {
                        var _item = UniqueAccessories.lines.Find(x => x.name == UniqueItemName);
                        var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }
                    else if (UniqueItemName == _taliosSignIncorrect)
                    {
                        var _item = UniqueAccessories.lines.Find(x => x.name == _taliosSignCorrect);
                        var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }

                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Unique Armours
            else if (itemRarity == ItemRarity.Unique && (item.HasComponent<Armour>() || className == "Quivers") && identified)
            {
                if (item != null)
                {
                    var _victariosFlightCorrect = "Victario's Flight";
                    var _victariosFlightIncorrect = "Ondar's Flight";
                    
                    if (UniqueItemName == _victariosFlightIncorrect && UniqueArmours.lines.Find(x => x.name == _victariosFlightCorrect) != null)
                    {
                        var _item = UniqueArmours.lines.Find(x => x.name == _victariosFlightCorrect);
                        var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }
                    else
                    {
                        if (UniqueArmours.lines.Find(x => x.name == UniqueItemName && x.links == 0) != null)
                        {
                            var _item = UniqueArmours.lines.Find(x => x.name == UniqueItemName && x.links == 0);
                            var _text = $"Links: 0 || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                            DrawText(ref TextPos, ref lineCount, _text);
                        }
                        if (UniqueArmours.lines.Find(x => x.name == UniqueItemName && x.links == 5) != null)
                        {
                            var _item = UniqueArmours.lines.Find(x => x.name == UniqueItemName && x.links == 5);
                            var _text = $"Links: 5 || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                            DrawText(ref TextPos, ref lineCount, _text);
                        }
                        if (UniqueArmours.lines.Find(x => x.name == UniqueItemName && x.links == 6) != null)
                        {
                            var _item = UniqueArmours.lines.Find(x => x.name == UniqueItemName && x.links == 6);
                            var _text = $"Links: 6 || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                            DrawText(ref TextPos, ref lineCount, _text);
                        }
                    }

                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Unique Flasks
            else if (itemRarity == ItemRarity.Unique && item.HasComponent<Flask>() && identified)
            {
                if (item != null)
                {
                    if (UniqueItemName == "Vessel of Vinktar")
                    {
                        if (UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Added Attacks") != null)
                        {
                            var _item = UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Added Attacks");
                            var _text = $"{_item.variant} || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                            DrawText(ref TextPos, ref lineCount, _text);
                        }
                        if (UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Penetration") != null)
                        {
                            var _item = UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Penetration");
                            var _text = $"{_item.variant} || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                            DrawText(ref TextPos, ref lineCount, _text);
                        }
                        if (UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Added Spells") != null)
                        {
                            var _item = UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Added Spells");
                            var _text = $"{_item.variant} || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                            DrawText(ref TextPos, ref lineCount, _text);
                        }
                        if (UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Conversion") != null)
                        {
                            var _item = UniqueFlasks.lines.Find(x => x.name == UniqueItemName && x.variant == "Conversion");
                            var _text = $"{_item.variant} || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                            DrawText(ref TextPos, ref lineCount, _text);
                        }
                    }
                    else
                    {
                        if (UniqueFlasks.lines.Find(x => x.name == UniqueItemName) == null)
                            return;

                        var _item = UniqueFlasks.lines.Find(x => x.name == UniqueItemName);
                        var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }

                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Unique Jewels
            else if (className == "Jewel" && identified && itemRarity == ItemRarity.Unique)
            {
                if (item != null)
                {
                    var _CorrectOne = "Fortified Legion";
                    var _IncorrectOne = "Bulwark Legion";

                    if (UniqueJewels.lines.Find(x => x.name == UniqueItemName) != null)
                    {
                        var _item = UniqueJewels.lines.Find(x => x.name == UniqueItemName);
                        var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }
                    else if (UniqueItemName == _IncorrectOne && UniqueJewels.lines.Find(x => x.name == _CorrectOne) != null)
                    {
                        var _item = UniqueJewels.lines.Find(x => x.name == _CorrectOne);
                        var _text = $"Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }

                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Unique Weapons
            else if (item.HasComponent<Weapon>() && identified && itemRarity == ItemRarity.Unique)
            {
                if (item != null)
                {
                    if (UniqueWeapons.lines.Find(x => x.name == UniqueItemName && x.links == 0) != null)
                    {
                        var _item = UniqueWeapons.lines.Find(x => x.name == UniqueItemName && x.links == 0);
                        var _text = $"Links: 0 || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }
                    if (UniqueWeapons.lines.Find(x => x.name == UniqueItemName && x.links == 5) != null)
                    {
                        var _item = UniqueWeapons.lines.Find(x => x.name == UniqueItemName && x.links == 5);
                        var _text = $"Links: 5 || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }
                    if (UniqueWeapons.lines.Find(x => x.name == UniqueItemName && x.links == 6) != null)
                    {
                        var _item = UniqueWeapons.lines.Find(x => x.name == UniqueItemName && x.links == 6);
                        var _text = $"Links: 6 || Chaos: {_item.chaosValue} || Change last 7 days: {_item.sparkline.totalChange}%";

                        DrawText(ref TextPos, ref lineCount, _text);
                    }

                    BackgroundBox(window, lineCount);
                }
            }
            #endregion
            #region Breachstones
            else if (BaseItemName.Contains("Breachstone"))
            {
                if (item != null)
                {
                    if (Fragments.lines.Find(x => x.currencyTypeName == BaseItemName) == null)
                        return;

                    var _item = Fragments.lines.Find(x => x.currencyTypeName == BaseItemName);
                    var _text = $"Chaos: {_item.chaosEquivalent} || Change last 7 days: {_item.receiveSparkLine.totalChange}%";
                    DrawText(ref TextPos, ref lineCount, _text);
                    BackgroundBox(window, lineCount);
                }
            } 
            #endregion
        }

        private void DrawText(ref Vector2 TextPos, ref int lineCount, string _text)
        {
            Graphics.DrawText(_text, 15, TextPos, FontDrawFlags.Left);
            lineCount++;
            TextPos.Y += 15;
        }

        private void BackgroundBox(RectangleF window, int lineCount)
        {
            window.Height *= lineCount;
            Graphics.DrawBox(window, new Color(0, 0, 0, 240));
        }
    }
}
