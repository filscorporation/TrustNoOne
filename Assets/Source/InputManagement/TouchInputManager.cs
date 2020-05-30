using System.Linq;
using UnityEngine;

namespace Assets.Source.InputManagement
{
    public class TouchInputManager : InputManagerBase
    {
        protected override void CheckForInput()
        {
            foreach (Touch touch in Input.touches.Where(t => t.phase == TouchPhase.Ended))
            {
                ProcessInput(touch.position);
            }
        }
    }
}
