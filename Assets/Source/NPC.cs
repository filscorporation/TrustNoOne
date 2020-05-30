using System;
using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// NPC in game that will give player advice (rules)
    /// </summary>
    [Serializable]
    public class NPC
    {
        public int TypeIndex;

        public RuleData[] Rules;

        public bool IsLiar;

        [NonSerialized]
        public GameObject GameObject;
    }
}
