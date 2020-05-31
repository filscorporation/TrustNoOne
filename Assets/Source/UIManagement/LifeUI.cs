using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.UIManagement
{
    /// <summary>
    /// Players lifes UI
    /// </summary>
    public class LifeUI : MonoBehaviour
    {
        [SerializeField] private GameObject heartPrefab;
        private List<GameObject> hearts;

        private const float heartWidth = 100F;

        /// <summary>
        /// Sets max life in UI
        /// </summary>
        /// <param name="maxLife"></param>
        public void SetMaxLife(int maxLife)
        {
            hearts = new List<GameObject>();
            for (int i = 0; i < maxLife; i++)
            {
                hearts.Add(Instantiate(heartPrefab, Vector3.zero, Quaternion.identity, transform));
                Vector2 position = new Vector2(i * heartWidth, 0);
                hearts[i].GetComponent<RectTransform>().anchoredPosition = position;
            }
        }

        /// <summary>
        /// Sets life in UI
        /// </summary>
        /// <param name="life"></param>
        public void SetLife(int life)
        {
            if (life < hearts.Count)
            {
                for (int i = hearts.Count - 1; i >= life; i--)
                {
                    Destroy(hearts[i]);
                    hearts.RemoveAt(i);
                }
            }
        }
    }
}
