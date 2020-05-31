using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source.UIManagement
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<UIManager>())
            : instance;

        [SerializeField] private GameObject playerDeadScreen;
        [SerializeField] private GameObject chooseLiarScreen;
        [SerializeField] private GameObject wonScreen;
        [SerializeField] private ChooseLiarUI chooseLiarUI;

        public void OnRestartButtonClick()
        {
            StartCoroutine(SceneTransitionManager.Instance.FadeIn());
            StartCoroutine(ReloadGameScene());
        }

        private IEnumerator ReloadGameScene()
        {
            yield return new WaitForSeconds(0.5F);
            SceneManager.LoadScene(MainMenuUIManager.GameSceneName);
        }

        public void OnToMenuButtonClick()
        {
            StartCoroutine(SceneTransitionManager.Instance.FadeIn());
            StartCoroutine(LoadMainMenuScene());
        }

        private IEnumerator LoadMainMenuScene()
        {
            yield return new WaitForSeconds(0.5F);
            SceneManager.LoadScene(MainMenuUIManager.MainMenuSceneName);
        }

        public void OnToNextLevelButtonClick()
        {
            StartCoroutine(SceneTransitionManager.Instance.FadeIn());
            StartCoroutine(LoadNextLevel());
        }

        private IEnumerator LoadNextLevel()
        {
            yield return new WaitForSeconds(0.5F);
            if (LevelManager.LevelToLoad + 1 == LevelManager.Instance.Levels.Count)
            {
                SceneManager.LoadScene(MainMenuUIManager.MainMenuSceneName);
            }
            else
            {
                LevelManager.LevelToLoad++;
                SceneManager.LoadScene(MainMenuUIManager.GameSceneName);
            }
        }

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

        public IEnumerator ShowWonScreen()
        {
            yield return new WaitForSeconds(2F);
            wonScreen.SetActive(true);
        }
    }
}
