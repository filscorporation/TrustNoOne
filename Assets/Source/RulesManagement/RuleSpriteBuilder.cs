using System;
using Assets.Source.RulesManagement.Rules;
using UnityEngine;

namespace Assets.Source.RulesManagement
{
    public enum SpriteType
    {
        Cross,
        Followed,
        Direction,
        Enter,
    }

    public class RuleSpriteBuilder : MonoBehaviour
    {
        private static RuleSpriteBuilder instance;
        public static RuleSpriteBuilder Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<RuleSpriteBuilder>())
            : instance;

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private GameObject NPCPopupPrefab;
        private const float popupOffset = 1.4F;
        private const float spritesYOffset = -0.16F;

        /// <summary>
        /// Creates popup sprite above npc
        /// </summary>
        /// <param name="npcTransform"></param>
        /// <returns></returns>
        public Transform CreateNPCPopup(Transform npcTransform)
        {
            GameObject go = Instantiate(
                NPCPopupPrefab,
                npcTransform.position + new Vector3(0, popupOffset, 0),
                Quaternion.identity,
                npcTransform);
            return go.transform;
        }

        /// <summary>
        /// Generates sprite of tile type crossed
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="typeIndex"></param>
        public void TypeCrossed(Transform parent, int typeIndex)
        {
            GameObject go1 = new GameObject("Type");
            SpriteRenderer sr1 = go1.AddComponent<SpriteRenderer>();
            sr1.sprite = FieldManager.Instance.TileTypes[typeIndex];
            sr1.sortingOrder = 10;
            sr1.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go1.transform.SetParent(parent);
            go1.transform.localPosition = new Vector3(0, spritesYOffset, 0);
            GameObject go2 = new GameObject("Cross");
            SpriteRenderer sr2 = go2.AddComponent<SpriteRenderer>();
            sr2.sprite = sprites[(int)SpriteType.Cross];
            sr2.sortingOrder = 11;
            sr2.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go2.transform.SetParent(parent);
            go2.transform.localPosition = new Vector3(0, spritesYOffset, 0);
        }

        /// <summary>
        /// Generates sprite of tile type followed by another tile type
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="typeIndex"></param>
        /// <param name="typeFollowedIndex"></param>
        public void TypeFollowed(Transform parent, int typeIndex, int typeFollowedIndex)
        {
            GameObject go1 = new GameObject("Type1");
            SpriteRenderer sr1 = go1.AddComponent<SpriteRenderer>();
            sr1.sprite = FieldManager.Instance.TileTypes[typeIndex];
            sr1.sortingOrder = 10;
            sr1.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go1.transform.SetParent(parent);
            go1.transform.localPosition = new Vector3(-1, spritesYOffset, 0);
            GameObject go2 = new GameObject("Followed");
            SpriteRenderer sr2 = go2.AddComponent<SpriteRenderer>();
            sr2.sprite = sprites[(int)SpriteType.Followed];
            sr2.sortingOrder = 10;
            sr2.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go2.transform.SetParent(parent);
            go2.transform.localPosition = new Vector3(0, spritesYOffset, 0);
            GameObject go3 = new GameObject("Type2");
            SpriteRenderer sr3 = go3.AddComponent<SpriteRenderer>();
            sr3.sprite = FieldManager.Instance.TileTypes[typeFollowedIndex];
            sr3.sortingOrder = 10;
            sr3.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go3.transform.SetParent(parent);
            go3.transform.localPosition = new Vector3(1, spritesYOffset, 0);
        }

        /// <summary>
        /// Generates sprite of crossed direction
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="direction"></param>
        public void DirectionCrossed(Transform parent, Direction direction)
        {
            GameObject go1 = new GameObject("Direction");
            SpriteRenderer sr1 = go1.AddComponent<SpriteRenderer>();
            sr1.sprite = sprites[(int)SpriteType.Direction];
            sr1.sortingOrder = 10;
            sr1.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go1.transform.SetParent(parent);
            go1.transform.localPosition = new Vector3(0, spritesYOffset, 0);
            float angle;
            switch (direction)
            {
                case Direction.Up:
                    angle = 0;
                    break;
                case Direction.Down:
                    angle = 180;
                    break;
                case Direction.Left:
                    angle = 270;
                    break;
                case Direction.Right:
                    angle = 90;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            go1.transform.eulerAngles = new Vector3(0, 0, angle);
            GameObject go2 = new GameObject("Cross");
            SpriteRenderer sr2 = go2.AddComponent<SpriteRenderer>();
            sr2.sprite = sprites[(int)SpriteType.Cross];
            sr2.sortingOrder = 11;
            sr2.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go2.transform.SetParent(parent);
            go2.transform.localPosition = new Vector3(0, spritesYOffset, 0);
        }

        /// <summary>
        /// Generates sprite of entering field from some tile
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="typeIndex"></param>
        public void TypeEnter(Transform parent, int typeIndex)
        {
            GameObject go1 = new GameObject("Enter");
            SpriteRenderer sr1 = go1.AddComponent<SpriteRenderer>();
            sr1.sprite = sprites[(int)SpriteType.Enter];
            sr1.sortingOrder = 10;
            sr1.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go1.transform.SetParent(parent);
            go1.transform.localPosition = new Vector3(-0.5F, spritesYOffset, 0);
            GameObject go2 = new GameObject("Type");
            SpriteRenderer sr2 = go2.AddComponent<SpriteRenderer>();
            sr2.sprite = FieldManager.Instance.TileTypes[typeIndex];
            sr2.sortingOrder = 10;
            sr2.material = parent.gameObject.GetComponent<SpriteRenderer>().material;
            go2.transform.SetParent(parent);
            go2.transform.localPosition = new Vector3(0.5F, spritesYOffset, 0);
        }
    }
}
