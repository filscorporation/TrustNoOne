using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source
{
    /// <summary>
    /// Controls UI over NPC to choose a liar(s)
    /// </summary>
    public class ChooseLiarUI : MonoBehaviour
    {
        [SerializeField] private GameObject onLiarUI;
        private List<GameObject> onLiarUIs;

        private int liarsToChoose;

        public IEnumerator ShowChooseLiarUI(int _liarsToChoose)
        {
            liarsToChoose = _liarsToChoose;
            foreach (NPC npc in NPCManager.Instance.NPCs)
            {
                Vector2 position = Camera.main.WorldToScreenPoint(npc.GameObject.transform.position);
                GameObject go = Instantiate(onLiarUI, Vector3.zero, Quaternion.identity, transform);
                go.GetComponent<RectTransform>().anchoredPosition = position;
                go.GetComponent<Button>().onClick.AddListener(() => OnChooseLiarClicked(npc));
            }

            yield break;
        }

        private void OnChooseLiarClicked(NPC npc)
        {
            Destroy(npc.GameObject);
            liarsToChoose--;
            if (liarsToChoose == 0)
            {
                StartCoroutine(GameManager.Instance.OnAllLiarsChosen());
                gameObject.SetActive(false);
            }
        }
    }
}
