using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Manages levels
    /// </summary>
    public class LevelManager
    {
        private Level[] levels;

        private const string levelsFilePath = "Levels";

        /// <summary>
        /// Load all levels data from file
        /// </summary>
        public void LoadAllLevels()
        {
            TextAsset file = Resources.Load(levelsFilePath) as TextAsset;
            if (file == null)
                throw new Exception("Level file not found");
            levels = JsonConvert.DeserializeObject<Level[]>(file.text);
        }

        /// <summary>
        /// Get level data by its index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Level GetLevel(int index) => levels[index];
    }
}
