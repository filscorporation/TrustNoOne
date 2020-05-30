using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.InputManagement
{
    /// <summary>
    /// Controls all input and notifies subscribers about clicks or touches
    /// </summary>
    public abstract class InputManagerBase : MonoBehaviour
    {
        public bool IsNeedToCheckForInput = true;

        private readonly List<IInputSubscriber> subs = new List<IInputSubscriber>();

        /// <summary>
        /// Subscribe to get input notifications
        /// </summary>
        /// <param name="sub"></param>
        public void Subscribe(IInputSubscriber sub)
        {
            subs.Add(sub);
        }

        public void Update()
        {
            if (IsNeedToCheckForInput)
                CheckForInput();
        }

        protected abstract void CheckForInput();

        protected bool ProcessInput(Vector2 inputPoint)
        {
            if (IsPointerOverUIObject(inputPoint))
                // Ignore input when on UI
                return false;

            var wp = Camera.main.ScreenToWorldPoint(inputPoint);
            var position = new Vector2(wp.x, wp.y);

            if (EventSystem.current.currentSelectedGameObject != null)
                return false;
            Collider2D[] hits = Physics2D.OverlapPointAll(position);
            if (hits == null || !hits.Any())
                return false;
            Collider2D hit = hits.First();
            foreach (IInputSubscriber subscriber in subs)
            {
                subscriber.Handle(hit.gameObject.GetComponent<Tile>());
            }

            return true;
        }

        private bool IsPointerOverUIObject(Vector2 inputPoint)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = inputPoint;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
