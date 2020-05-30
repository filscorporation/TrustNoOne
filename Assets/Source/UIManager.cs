using System.Collections;
using UnityEngine;

namespace Assets.Source
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance ?? (instance = FindObjectOfType<UIManager>());

        [SerializeField] private GameObject playerDeadScreen;
        [SerializeField] private GameObject chooseLiarScreen;
        [SerializeField] private ChooseLiarUI chooseLiarUI;

        public IEnumerator ShowPlayerDeadScreen()
        {
            yield return new WaitForSeconds(1F);
            playerDeadScreen.SetActive(true);
        }

        public IEnumerator ShowChooseLiarScreen(int liarsToChoose)
        {
            yield return new WaitForSeconds(1F);
            chooseLiarScreen.SetActive(true);
            StartCoroutine(chooseLiarUI.ShowChooseLiarUI(liarsToChoose));
        }
    }
}
