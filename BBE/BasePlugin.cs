﻿using BepInEx;
using HarmonyLib;
using System;
using BBE.Creators;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI;
using System.Collections;
using BBE.Patches;
using BBE.CustomClasses;
using MTM101BaldAPI.OptionsAPI;
using BepInEx.Logging;
using UnityEngine;
using System.IO;
using PlusLevelFormat;
using TMPro;
using System.Collections.Generic;
using System.Reflection;
using BBE.Extensions;
using BBE.Compats;
using UnityEngine.UI;
using System.Linq;
using BBE.Helpers;
using BBE.API;
using BBE.NPCs.Chess;
using BBE.Rooms;
using BBE.CustomClasses.OptionsMenu;
using MTM101BaldAPI.Registers.Buttons;
using UnityEngine.SceneManagement;

namespace BBE
{
    [BepInPlugin("rost.moment.baldiplus.extramod", "Baldi's Basics Extra", "2.2.0.2")]
    [BepInDependency("mtm101.rulerp.baldiplus.levelloader", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]

    // Compats
    [BepInDependency("mtm101.rulerp.baldiplus.leveleditor", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("rost.moment.baldiplus.achievements", BepInDependency.DependencyFlags.SoftDependency)]
    public class BasePlugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        public static AssetManager Asset { get; private set; }
        public static Harmony Harmony { get; private set; }
        public static BasePlugin Instance { get; private set; }
        public static string CurrentFloor
        {
            get
            {
                if (CoreGameManager.Instance == null)
                    return "None";
                return CoreGameManager.Instance.sceneObject.levelTitle;
            }
        }
        public static FloorData CurrentFloorData => FloorData.Get(CurrentFloor);

        public void RegisterDataToGenerator(string floorName, int floorNumber, SceneObject scene)
        {
            FloorData floorData = FloorData.Get(floorName);
            if (floorData == null)
                return;
            if (File.Exists(Path.Combine(AssetsHelper.ModPath, "disabled.txt")))
            {
                string[] lines = File.ReadAllLines(Path.Combine(AssetsHelper.ModPath, "disabled.txt"));
                foreach (string line in lines)
                {
                    if (line.TryParseToEnum<ModdedItems>(out ModdedItems item))
                    {
                        floorData.forcedItems.RemoveAll(x => x.itemType.Is(item));
                        floorData.shopItems.RemoveAll(x => x.selection.itemType.Is(item));
                        floorData.potentialItems.RemoveAll(x => x.selection.itemType.Is(item));
                        floorData.partyEventItems.RemoveAll(x => x.selection.itemType.Is(item));
                    }
                    if (line.TryParseToEnum<ModdedRandomEvent>(out ModdedRandomEvent randomEvent))
                        floorData.randomEvents.RemoveAll(x => x.selection.Type.Is(randomEvent));
                    if (line.TryParseToEnum<ModdedCharacters>(out ModdedCharacters character))
                    {
                        floorData.forcedNPCs.RemoveAll(x => x.Character.Is(character));
                        floorData.potentialNPCs.RemoveAll(x => x.selection.Character.Is(character));
                    }
                }
            }
            scene.shopItems = scene.shopItems.AddRangeToArray(floorData.shopItems.ToArray());
            scene.potentialNPCs.AddRange(floorData.potentialNPCs);
            scene.forcedNpcs = scene.forcedNpcs.AddRangeToArray(floorData.forcedNPCs.ToArray());

            foreach (CustomLevelObject lvl in scene.GetCustomLevelObjects())
                RegisterDataToGeneratorLvl(lvl, floorData);
        }

        private void RegisterDataToGeneratorLvl(CustomLevelObject lvl, FloorData floorData)
        {
            lvl.potentialItems = lvl.potentialItems.AddRangeToArray(floorData.potentialItems.ToArray());
            lvl.forcedItems.AddRange(floorData.forcedItems);
            lvl.randomEvents.AddRange(floorData.randomEvents);
            lvl.potentialSpecialRooms = lvl.potentialSpecialRooms.AddRangeToArray(floorData.specialRooms.ToArray());
            foreach (RoomGroup roomGroup in floorData.roomGroups)
            {
                if (lvl.roomGroup.IfExists(x => x.name == roomGroup.name, out RoomGroup group))
                    group.potentialRooms = group.potentialRooms.AddRangeToArray(roomGroup.potentialRooms);
                else
                    lvl.roomGroup = lvl.roomGroup.AddToArray(roomGroup);
            }
            StructureWithParameters structure = lvl.forcedStructures.Where(x => x.prefab.name == "SwingingDoorConstructor").FirstOrDefault();
            if (structure != null)
            {
                structure.parameters.prefab = structure.parameters.prefab.AddRangeToArray(floorData.customSwingDoors.ToArray());
                lvl.forcedStructures.ReplaceWhere(x => x.prefab.name == "SwingingDoorConstructor", structure);
            }
            lvl.forcedStructures = lvl.forcedStructures.AddRangeToArray(floorData.forcedStructures.ToArray());
            lvl.posters = lvl.posters.AddRangeToArray(floorData.posters.ToArray());
        }

        public IEnumerator LoadAssets()
        {
            yield return 14;
            if (!AssetsHelper.AssetsAreInstalled())
            {
                MTM101BaldiDevAPI.CauseCrash(Info, new Exception("Baldi's Basics Extra assets not installed! Try check if you have put folder rost.moment.baldiplus.extramod into Modded"));
                yield break;
            }
            yield return "Creating compats...";
            BaseCompat.CreateCompats();
            yield return "Creating pre-datas...";
            new FloorData("F1");
            new FloorData("F2");
            new FloorData("F3");
            new FloorData("F4");
            new FloorData("F5");
            new FloorData("END");
            yield return "Loading save file...";
            new BBESave().Initialize().Update();
            yield return "Calling compats prefixes...";
            BaseCompat.CallPrefixes();
            yield return "Creating textures...";
            Creator.CreateTextures();
            yield return "Creating sounds...";
            Creator.CreateSounds();
            yield return "Creating prefabs...";
            Creator.CreatePrefabs();
            yield return "Creating new school rules...";
            RulesCreator.CreateRules();
            yield return "Creating items...";
            ItemsCreator.CreateItems();
            yield return "Creating events...";
            EventsCreator.CreateEvents();
            yield return "Creating NPCs...";
            NPCCreator.CreateNPCs();
            yield return "Creating rooms...";
            RoomsCreator.CreateRooms();
            yield return "Creating structures...";
            StructuresCreator.CreateStructures();
            yield return "Creating config...";
            BBEConfigs.Initialize();
            yield return "Adding custom meta tags...";
            CustomMetaTags.AddTags();
            yield return "Calling compats postfixes...";
            BaseCompat.CallPostfixes();
            yield break;
        }

        private void Awake()
        {
            Harmony = new Harmony("rost.moment.baldiplus.extramod");
            Harmony.TryPatchAll();
            Asset = new AssetManager();
            Logger = BepInEx.Logging.Logger.CreateLogSource("Baldi's Basics Extra");
            if (Instance == null)
            {
                Instance = this;
            }
            LoadingEvents.RegisterOnAssetsLoaded(Info, LoadAssets(), LoadingEventOrder.Pre);
            GeneratorManagement.Register(this, GenerationModType.Addend, RegisterDataToGenerator);
            AssetLoader.LoadLocalizationFolder(Path.Combine(AssetsHelper.ModPath, "Language", "English"), Language.English);
            CustomOptionsCore.OnMenuInitialize += (x, y) =>
            {
                y.AddCategory<BBEOptions>("BBE_BBEOption");
            };
            CustomOptionsCore.OnMenuClose += (x, y) =>
            {
                BBEOptions.Instance?.Save();
            };
        }
    }
}