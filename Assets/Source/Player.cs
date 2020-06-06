using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Source.InputManagement;
using Assets.Source.RulesManagement;
using Assets.Source.UIManagement;
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
        public static Player Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<Player>())
            : instance;

        public PlayerState State = PlayerState.Idle;

        public int MaxLife;
        public int Life;

        public readonly List<Step> Steps = new List<Step>();

        private Tile currentTile;
        [SerializeField] [Range(0.1F, 20F)] private float speed = 1F;
        private Transform reward;
        private const string idolTag = "Idol";

        [SerializeField] private LifeUI lifeUI;

        private Animator animator;
        private const string shootAnimatorParam = "Shoot";
        private const string deadAnimatorParam = "Dead";
        private const string walkAnimatorParam = "Walk";

        [SerializeField] private Transform gunPoint;
        [SerializeField] private GameObject shootEffectPrefab;
        [SerializeField] private AudioClip onIdolPickupSound;
        [SerializeField] private List<AudioClip> stepSounds;
        private const float stepSoundDelay = 0.2F;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            AutoInputInitializer.InputManager.Subscribe(this);
            lifeUI.SetMaxLife(MaxLife);
            animator = GetComponentInChildren<Animator>();
            StartCoroutine(DialogsManager.Instance.ShowDialog(transform.GetChild(0), "Lets go!"));
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (State == PlayerState.Moving)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position, currentTile.transform.position, Time.deltaTime * speed);
                if (Vector2.Distance(transform.position, currentTile.transform.position) < Mathf.Epsilon)
                {
                    animator.SetBool(walkAnimatorParam, false);
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
                    animator.SetBool(walkAnimatorParam, false);
                    State = PlayerState.Waiting;
                    OnMoveToReward();
                }
            }
        }

        private IEnumerator PlayStepsSound()
        {
            while (State == PlayerState.Moving || State == PlayerState.MovingToReward)
            {
                audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Count)]);
                yield return new WaitForSeconds(stepSoundDelay);
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
                StartCoroutine(FieldManager.Instance.ShowTrap(currentTile));

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
            audioSource.PlayOneShot(onIdolPickupSound);
            StartCoroutine(GameManager.Instance.OnPlayerPassed());
        }

        private void TryMoveTo(Tile tile)
        {
            if (State != PlayerState.Idle)
                return;

            if (!CanMoveTo(tile))
                return;

            State = tile == null ? PlayerState.MovingToReward : PlayerState.Moving;
            StartCoroutine(PlayStepsSound());
            animator.SetBool(walkAnimatorParam, true);
            currentTile = tile;
        }

        private bool CanMoveTo(Tile tile)
        {
            if (GameManager.Instance.GameStage != GameStage.PlayerMovement)
                return false;

            if (tile == null)
            {
                // Move to idol
                return currentTile.X == FieldManager.Instance.Field.GetLength(0) - 1;
            }

            if (tile.TypeIndex == 0)
                return false;

            if (currentTile == null)
                return tile.X == 0;

            return (currentTile.X == tile.X && Mathf.Abs(currentTile.Y - tile.Y) == 1)
                || (currentTile.Y == tile.Y && Mathf.Abs(currentTile.X - tile.X) == 1);
        }

        public void Handle(Tile input)
        {
            TryMoveTo(input);
        }

        public IEnumerator AnimateShoot()
        {
            transform.GetChild(0).localScale = new Vector3(1, 1 ,1);
            StartCoroutine(DialogsManager.Instance.ShowDialog(transform.GetChild(0), "Die liar!"));
            yield return new WaitForSeconds(1.5F);
            animator.SetTrigger(shootAnimatorParam);
            Destroy(Instantiate(shootEffectPrefab, gunPoint.transform.position, Quaternion.identity), 2F);
        }

        public void AnimateDead()
        {
            animator.SetTrigger(deadAnimatorParam);
        }
    }
}
