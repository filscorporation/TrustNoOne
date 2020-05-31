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
        private const string levelsOpenedPref = "LevelsOpened";

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

        /// <summary>
        /// Returns how many levels player opened
        /// </summary>
        /// <returns></returns>
        public int GetLevelsOpened()
        {
            return PlayerPrefs.GetInt(levelsOpenedPref, 0);
        }

        /// <summary>
        /// Increase how many levels player opened by one
        /// </summary>
        /// <returns></returns>
        public void IncreaseLevelsOpened()
        {
            int currentOpened = GetLevelsOpened();
            if (currentOpened > LevelToLoad)
                return;
            PlayerPrefs.SetInt(levelsOpenedPref, LevelToLoad + 1);
            PlayerPrefs.Save();
        }
    }
}
