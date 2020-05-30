using System.Collections;
using UnityEngine;

namespace Assets.Source
{
    public enum GameStage
    {
        Intro,
        PlayerMovement,
        PlayerDead,
        ChooseLiar,
        TakeReward,
    }

    /// <summary>
    /// Controls main game process
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());

        public GameStage GameStage;

        private void Awake()
        {
            StartGame(0);
        }

        private void StartGame(int levelIndex)
        {
            LevelManager levelManager = new LevelManager();
            levelManager.LoadAllLevels();
            Level currentLevel = levelManager.GetLevel(levelIndex);
            FieldManager.Instance.Generate(currentLevel);
            NPCManager.Instance.Initialize(currentLevel);
            Player.Instance.MaxLife = currentLevel.PlayersLife;
            Player.Instance.Life = currentLevel.PlayersLife;

            GameStage = GameStage.Intro;
            StartCoroutine(ShowIntro());
        }

        private IEnumerator ShowIntro()
        {
            yield return new WaitForSeconds(1F);
            GameStage = GameStage.PlayerMovement;
        }

        public IEnumerator OnPlayerDead()
        {
            GameStage = GameStage.PlayerDead;
            StartCoroutine(UIManager.Instance.ShowPlayerDeadScreen());
            yield break;
        }

        public IEnumerator OnPlayerPassed()
        {
            GameStage = GameStage.ChooseLiar;
            StartCoroutine(UIManager.Instance.ShowChooseLiarScreen(1));
            yield break;
        }

        public IEnumerator OnAllLiarsChosen()
        {
            GameStage = GameStage.TakeReward;
            yield break;
        }
    }
}
