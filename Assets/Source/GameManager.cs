using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source.UIManagement;
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
        public static GameManager Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<GameManager>())
            : instance;

        public GameStage GameStage;

        private void Start()
        {
            StartCoroutine(StartGame(LevelManager.LevelToLoad));
        }

        private IEnumerator StartGame(int levelIndex)
        {
            LevelManager.Instance.LoadAllLevels();
            Level currentLevel = LevelManager.Instance.Levels[levelIndex];
            FieldManager.Instance.Generate(currentLevel);
            NPCManager.Instance.Initialize(currentLevel);
            Player.Instance.MaxLife = currentLevel.PlayersLife;
            Player.Instance.Life = currentLevel.PlayersLife;

            GameStage = GameStage.Intro;
            StartCoroutine(ShowIntro());
            yield break;
        }

        private IEnumerator ShowIntro()
        {
            yield return new WaitForSeconds(2F);
            GameStage = GameStage.PlayerMovement;
        }

        public IEnumerator OnPlayerDead()
        {
            EndGame();
            yield break;
        }

        private void EndGame()
        {
            GameStage = GameStage.PlayerDead;
            StartCoroutine(UIManager.Instance.ShowPlayerDeadScreen());
        }

        public IEnumerator OnPlayerPassed()
        {
            GameStage = GameStage.ChooseLiar;
            StartCoroutine(UIManager.Instance.ShowChooseLiarScreen(1));
            yield break;
        }

        public IEnumerator OnAllLiarsChosen()
        {
            List<NPC> liarsLeft = NPCManager.Instance.NPCs.Where(n => !n.IsDead && n.IsLiar).ToList();
            yield return new WaitForSeconds(2F);
            foreach (NPC npc in liarsLeft)
            {
                StartCoroutine(npc.AnimateBetray());
            }
            yield return new WaitForSeconds(1.7F);
            if (liarsLeft.Any())
            {
                Player.Instance.AnimateDead();
                EndGame();
                yield break;
            }
            GameStage = GameStage.TakeReward;

            foreach (NPC npc in NPCManager.Instance.NPCs.Where(n => !n.IsDead && !n.IsLiar))
            {
                StartCoroutine(npc.AnimateWon());
            }

            StartCoroutine(UIManager.Instance.ShowWonScreen());
            LevelManager.Instance.IncreaseLevelsOpened();
            yield break;
        }
    }
}
