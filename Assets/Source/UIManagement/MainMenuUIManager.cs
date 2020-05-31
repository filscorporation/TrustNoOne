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
            foreach (Level level in LevelManager.Instance.Levels)
            {
                GameObject go = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity, levelSelectPanel.transform);
                go.GetComponent<Button>().onClick.AddListener(() => OnLevelClick(level.Index));
                go.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"Level {level.Index + 1}";
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(250, -100 - 200 * i);
                i++;
            }
        }

        private void OnLevelClick(int levelIndex)
        {
            LevelManager.LevelToLoad = levelIndex;
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
