using System.Collections.Generic;
using System.Linq;
using Assets.Source.RulesManagement;
using UnityEngine;

namespace Assets.Source
{
    public class NPCManager : MonoBehaviour
    {
        private static NPCManager instance;
        public static NPCManager Instance => instance ?? (instance = FindObjectOfType<NPCManager>());

        public List<NPC> NPCs { get; private set; }
        [SerializeField] private Transform NPCsPivot;
        [SerializeField] private GameObject NPCPrefab;
        [SerializeField] private List<Sprite> NPCsSprites;

        private RulesManager rulesManager;

        /// <summary>
        /// Initialize NPCs
        /// </summary>
        /// <param name="level"></param>
        public void Initialize(Level level)
        {
            rulesManager = new RulesManager();
            rulesManager.LoadRulesFromSoltion();
            NPCs = level.NPCs.ToList();
            int i = 0;
            foreach (NPC npc in NPCs)
            {
                CreateNPC(npc, i);
                i++;
            }
        }

        private void CreateNPC(NPC npc, int i)
        {
            Vector2 position = NPCsPivot.position + new Vector3(i * 1.5F, 0);
            GameObject go = Instantiate(NPCPrefab, position, Quaternion.identity, NPCsPivot);
            npc.GameObject = go;
            go.GetComponent<SpriteRenderer>().sprite = NPCsSprites[npc.TypeIndex];
        }

        /// <summary>
        /// Checks if any rule was broken
        /// </summary>
        /// <returns></returns>
        public bool CheckAllRules()
        {
            foreach (RuleData ruleData in NPCs.SelectMany(n => n.Rules.ToList()))
            {
                if (rulesManager.CheckRule(ruleData.Name, ruleData.Parameters))
                    return true;
            }

            return false;
        }
    }
}
