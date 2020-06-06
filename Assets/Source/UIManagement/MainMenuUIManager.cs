using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Source.UIManagement
{
    /// <summary>
    /// Controlls main menu UI
    /// </summary>
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelSelectPanel;
        [SerializeField] private GameObject levelButtonPrefab;

        public const string GameSceneName = "GameScene";
        public const string MainMenuSceneName = "MainMenuScene";
        private const int levelButtonsInACollumn = 5;

        private void Start()
        {
            GenerateLevelSelect();
        }

        public void OnPlayButtonClick()
        {
            levelSelectPanel.SetActive(true);
        }

        public void OnBackButtonClick()
        {
            levelSelectPanel.SetActive(false);
        }

        public void OnExitButtonClick()
        {
            Application.Quit();
        }

        private void GenerateLevelSelect()
        {
            LevelManager.Instance.LoadAllLevels();
            int i = 0;
            int column = 0;
            int maxOpenedLevel = LevelManager.Instance.GetLevelsOpened();
            foreach (Level level in LevelManager.Instance.Levels)
            {
                if (i == levelButtonsInACollumn)
                {
                    i = 0;
                    column++;
                }
                GameObject go = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity, levelSelectPanel.transform);
                Button button = go.GetComponent<Button>();
                button.onClick.AddListener(() => OnLevelClick(level.Index));
                if (i > maxOpenedLevel)
                {
                    button.interactable = false;
                }
                go.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"Level {level.Index + 1}";
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + column * 450, -100 - 200 * i);
                i++;
            }
        }

        private void OnLevelClick(int levelIndex)
        {
            StartCoroutine(SceneTransitionManager.Instance.FadeIn());
            StartCoroutine(LoadLevel(levelIndex));
        }

        private IEnumerator LoadLevel(int levelIndex)
        {
            yield return new WaitForSeconds(0.5F);
            LevelManager.LevelToLoad = levelIndex;
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
