using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source
{
    /// <summary>
    /// Manages scene transition
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        private static SceneTransitionManager instance;
        public static SceneTransitionManager Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<SceneTransitionManager>())
            : instance;

        [SerializeField] private Image panel;
        private const float speed = 2F;

        private void Start()
        {
            StartCoroutine(FadeOut());
        }

        /// <summary>
        /// Slowly covers screen with a panel
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeIn()
        {
            panel.gameObject.SetActive(true);
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
            while (panel.color.a < 1 - Mathf.Epsilon)
            {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + Time.fixedDeltaTime * speed);
                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// Slowly covers screen with a panel
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeOut()
        {
            panel.gameObject.SetActive(true);
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1);
            while (panel.color.a > Mathf.Epsilon)
            {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - Time.fixedDeltaTime * speed);
                yield return new WaitForFixedUpdate();
            }
            panel.gameObject.SetActive(false);
        }
    }
}
