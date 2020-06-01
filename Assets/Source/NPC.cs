using System;
using System.Collections;
using System.Linq;
using Assets.Source.UIManagement;
using UnityEngine;
using Object = UnityEngine.Object;

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

        [NonSerialized]
        public bool IsDead = false;

        private const string deadTrigger = "Dead";
        private const string betrayTrigger = "Shoot";
        private const string gunPointName = "GunPoint";
        [NonSerialized]
        public GameObject ShootEffectPrefab;

        [NonSerialized]
        public Transform Popup;

        /// <summary>
        /// Plays animation of NPC death
        /// </summary>
        public IEnumerator AnimateDead()
        {
            yield return new WaitForSeconds(1.7F);
            GameObject.GetComponent<Animator>().SetTrigger(deadTrigger);
        }

        /// <summary>
        /// Plays animation of NPC shooting player
        /// </summary>
        public IEnumerator AnimateBetray()
        {
            NPCManager.Instance.StartCoroutine(DialogsManager.Instance.ShowDialog(GameObject.transform, "Mistake!"));
            yield return new WaitForSeconds(1.3F);
            GameObject.GetComponent<Animator>().SetTrigger(betrayTrigger);
            Transform gunPoint = GameObject.GetComponentsInChildren<Transform>().First(c => c.gameObject.name == gunPointName);
            Object.Destroy(Object.Instantiate(ShootEffectPrefab, gunPoint.transform.position, Quaternion.identity), 2F);
        }

        /// <summary>
        /// Plays animation of NPC winning with a player
        /// </summary>
        public IEnumerator AnimateWon()
        {
            NPCManager.Instance.StartCoroutine(DialogsManager.Instance.ShowDialog(GameObject.transform, "Yay!"));
            yield break;
        }
    }
}
