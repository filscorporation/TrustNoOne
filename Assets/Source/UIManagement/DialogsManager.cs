using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.UIManagement
{
    /// <summary>
    /// Controls methods to show dialogs
    /// </summary>
    public class DialogsManager : MonoBehaviour
    {
        private static DialogsManager instance;
        public static DialogsManager Instance => (instance == null || !instance.isActiveAndEnabled)
            ? (instance = FindObjectOfType<DialogsManager>())
            : instance;

        [SerializeField] private GameObject dialogPrefab;
        private const float dialogXOffset = 0F;
        private const float dialogYOffset = 1.5F;

        /// <summary>
        /// Shows dialog window about parent with text
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerator ShowDialog(Transform parent, string text)
        {
            yield return new WaitForEndOfFrame();
            Vector2 position = Camera.main.WorldToScreenPoint(parent.position + new Vector3(dialogXOffset, dialogYOffset));
            GameObject go = Instantiate(dialogPrefab, Vector3.zero, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            go.GetComponent<RectTransform>().anchoredPosition = position;
            go.transform.SetAsFirstSibling();
            yield return new WaitForSeconds(0.4F);
            Text textComponent = go.GetComponentInChildren<Text>();
            textComponent.text = text;
            yield return new WaitForSeconds(2F);
            Destroy(go);
        }
    }
}
