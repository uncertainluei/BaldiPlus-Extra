﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BBE.Compats;
using BBE.Extensions;
using MTM101BaldAPI.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace BBE.CustomClasses
{
    public class BBESave
    {
        [JsonIgnore]
        public string SavePath => Path.Combine(Application.persistentDataPath, "BBESave.json");
        [JsonIgnore]
        public static BBESave Instance;

        public Dictionary<string, string> keyBindings;
        public List<string> unlockedFunSettings;
        public List<string> customAttributes;

        public BBESave Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
                keyBindings = new Dictionary<string, string>();
                unlockedFunSettings = new List<string>();
                customAttributes = new List<string>();
            }
            return Instance;
        }

        public void Save()
        {
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(BBESave.Instance, Formatting.Indented));
        }
        public void Update()
        {
            Load();
            Save();
            Load();
        }
        public void AddAttribute(string attribute, bool allowDuplicate = false)
        {
            if (!customAttributes.Contains(attribute) || allowDuplicate)
                customAttributes.Add(attribute);
            Save();
        }
        public void Load()
        {
            if (File.Exists(SavePath))
            {
                JsonConvert.PopulateObject(File.ReadAllText(SavePath), BBESave.Instance);
                return;
            }
            Save();
        }
        public void PerfectSave()
        {
            //BaseCompat.Get<AchievementsCompat>()?.UnlockAll();
        }
    }
}
