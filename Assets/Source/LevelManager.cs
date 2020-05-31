using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Manages levels
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager instance;
        public static LevelManager Instance => instance ?? (instance = FindObjectOfType<LevelManager>());

        public List<Level> Levels;

        private const string levelsFilePath = "Levels";

        public static int LevelToLoad;

        /// <summary>
        /// Load all levels data from file
        /// </summary>
        public void LoadAllLevels()
        {
            TextAsset file = Resources.Load(levelsFilePath) as TextAsset;
            if (file == null)
                throw new Exception("Level file not found");
            Levels = JsonConvert.DeserializeObject<Level[]>(file.text).ToList();
        }
    }
}
