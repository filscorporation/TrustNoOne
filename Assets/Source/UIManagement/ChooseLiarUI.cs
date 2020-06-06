using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.UIManagement
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
                GameObject go = Instantiate(onLiarUI, Vector3.zero, Quaternion.identity);
                go.GetComponent<RectTransform>().anchoredPosition = position;
                go.transform.SetParent(transform);
                go.transform.localScale = Vector3.one;
                Button button = go.GetComponent<Button>();
                button.onClick.AddListener(() => OnChooseLiarClicked(npc));
                button.onClick.AddListener(() => FindObjectOfType<Canvas>().GetComponent<AudioSource>().Play());
            }

            yield break;
        }

        private void OnChooseLiarClicked(NPC npc)
        {
            // Starting courutine from GM couse otherwise deactivation will stop it
            GameManager.Instance.StartCoroutine(Player.Instance.AnimateShoot());
            npc.IsDead = true;
            GameManager.Instance.StartCoroutine(npc.AnimateDead());
            liarsToChoose--;
            if (liarsToChoose == 0)
            {
                NPCManager.Instance.HideAllPopups();
                GameManager.Instance.StartCoroutine(GameManager.Instance.OnAllLiarsChosen());
                gameObject.SetActive(false);
            }
        }
    }
}
