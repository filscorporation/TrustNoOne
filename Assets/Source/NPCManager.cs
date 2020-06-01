using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source.RulesManagement;
using Assets.Source.RulesManagement.Rules;
using UnityEngine;

namespace Assets.Source
{
    public class NPCManager : MonoBehaviour
    {
        private static NPCManager instance;
        public static NPCManager Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<NPCManager>())
            : instance;

        public List<NPC> NPCs { get; private set; }
        [SerializeField] private Transform NPCsPivot;
        [SerializeField] private List<GameObject> NPCPrefabs;
        private const float npcOffset = 1.7F;
        [SerializeField] private GameObject shootEffectPrefab;

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
            Vector2 position = NPCsPivot.position + new Vector3(i * npcOffset, 0);
            GameObject go = Instantiate(NPCPrefabs[npc.TypeIndex], position, Quaternion.identity, NPCsPivot);
            npc.GameObject = go;
            npc.ShootEffectPrefab = shootEffectPrefab;
            go.transform.localScale = new Vector3(-1, 1, 1);
            StartCoroutine(DrawNPCRules(npc));
        }

        private IEnumerator DrawNPCRules(NPC npc)
        {
            yield return new WaitForSeconds(0.5F);
            npc.Popup = RuleSpriteBuilder.Instance.CreateNPCPopup(npc.GameObject.transform);
            yield return new WaitForSeconds(1F);
            // TODO: multiple rules
            RuleData ruleData = npc.Rules.First();
            switch (ruleData.Name)
            {
                case nameof(CantMoveOn):
                    RuleSpriteBuilder.Instance.TypeCrossed(npc.Popup, ruleData.Parameters[CantMoveOn.TileTypeParam]);
                    break;
                case nameof(ShouldStartFrom):
                    RuleSpriteBuilder.Instance.TypeEnter(npc.Popup, ruleData.Parameters[ShouldStartFrom.TileTypeParam]);
                    break;
                case nameof(CantMoveDirection):
                    RuleSpriteBuilder.Instance.DirectionCrossed(npc.Popup, (Direction)ruleData.Parameters[CantMoveDirection.DirectionParam]);
                    break;
                case nameof(ShouldFollow):
                    RuleSpriteBuilder.Instance.TypeFollowed(
                        npc.Popup,
                        ruleData.Parameters[ShouldFollow.TileTypeStartParam],
                        ruleData.Parameters[ShouldFollow.TileTypeEndParam]);
                    break;
                case nameof(CantFollow):
                    RuleSpriteBuilder.Instance.TypeNotFollowed(
                        npc.Popup,
                        ruleData.Parameters[ShouldFollow.TileTypeStartParam],
                        ruleData.Parameters[ShouldFollow.TileTypeEndParam]);
                    break;
                default:
                    throw new NotSupportedException(ruleData.Name);
            }
        }

        /// <summary>
        /// Hides all npc's popups
        /// </summary>
        public void HideAllPopups()
        {
            foreach (NPC npc in NPCs)
            {
                Destroy(npc.Popup.gameObject);
            }
        }

        /// <summary>
        /// Checks if any rule was broken
        /// </summary>
        /// <returns></returns>
        public bool CheckAllRules()
        {
            foreach (RuleData ruleData in NPCs.Where(n => !n.IsLiar).SelectMany(n => n.Rules.ToList()))
            {
                if (rulesManager.CheckRule(ruleData.Name, ruleData.Parameters))
                    return true;
            }

            return false;
        }
    }
}
