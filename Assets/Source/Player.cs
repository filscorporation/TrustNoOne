using System.Collections.Generic;
using System.Linq;
using Assets.Source.InputManagement;
using Assets.Source.RulesManagement;
using UnityEngine;

namespace Assets.Source
{
    public enum PlayerState
    {
        Idle,
        Moving,
        Dead,
        Waiting,
        MovingToReward,
    }

    /// <summary>
    /// Player
    /// </summary>
    public class Player : MonoBehaviour, IInputSubscriber
    {
        private static Player instance;
        public static Player Instance => instance ?? (instance = FindObjectOfType<Player>());

        public PlayerState State = PlayerState.Idle;

        public int MaxLife;
        public int Life;

        public readonly List<Step> Steps = new List<Step>();

        private Tile currentTile;
        [SerializeField] [Range(0.1F, 20F)] private float speed = 1F;
        private Transform reward;
        private const string idolTag = "Idol";

        [SerializeField] private LifeUI lifeUI;

        private void Start()
        {
            AutoInputInitializer.InputManager.Subscribe(this);
            lifeUI.SetMaxLife(MaxLife);
        }

        private void Update()
        {
            if (State == PlayerState.Moving)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position, currentTile.transform.position, Time.deltaTime * speed);
                if (Vector2.Distance(transform.position, currentTile.transform.position) < Mathf.Epsilon)
                {
                    State = PlayerState.Idle;
                    OnMoveToTile();
                }
            }

            if (State == PlayerState.MovingToReward)
            {
                if (reward == null)
                    reward = GameObject.FindWithTag(idolTag).transform;

                transform.position = Vector2.MoveTowards(
                    transform.position, reward.transform.position, Time.deltaTime * speed);
                if (Vector2.Distance(transform.position, reward.transform.position) < Mathf.Epsilon)
                {
                    State = PlayerState.Waiting;
                    OnMoveToReward();
                }
            }
        }

        private void OnMoveToTile()
        {
            if (Steps.Count == 0)
            {
                Steps.Add(new Step(currentTile.X, currentTile.Y));
            }
            else
            {
                Step lastStep = Steps.Last();
                Steps.Add(new Step(lastStep.To.x, lastStep.To.y, currentTile.X, currentTile.Y));
            }

            if (NPCManager.Instance.CheckAllRules())
            {
                Life--;
                lifeUI.SetLife(Life);

                if (Life == 0)
                {
                    Debug.Log("Dead");
                    State = PlayerState.Dead;
                    StartCoroutine(GameManager.Instance.OnPlayerDead());
                    return;
                }

                Debug.Log("Broke rule");
            }
        }

        private void OnMoveToReward()
        {
            StartCoroutine(GameManager.Instance.OnPlayerPassed());
        }

        private void TryMoveTo(Tile tile)
        {
            if (State != PlayerState.Idle)
                return;

            if (!CanMoveTo(tile))
                return;

            State = tile == null ? PlayerState.MovingToReward : PlayerState.Moving;
            currentTile = tile;
        }

        private bool CanMoveTo(Tile tile)
        {
            if (tile == null)
            {
                // Move to idol
                return currentTile.X == FieldManager.Instance.Field.GetLength(0) - 1;
            }

            if (currentTile == null)
                return tile.X == 0;

            return (currentTile.X == tile.X && Mathf.Abs(currentTile.Y - tile.Y) == 1)
                || (currentTile.Y == tile.Y && Mathf.Abs(currentTile.X - tile.X) == 1);
        }

        public void Handle(Tile input)
        {
            TryMoveTo(input);
        }
    }
}
