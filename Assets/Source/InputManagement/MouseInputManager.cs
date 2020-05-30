using UnityEngine;

namespace Assets.Source.InputManagement
{
    public class MouseInputManager : InputManagerBase
    {
        protected override void CheckForInput()
        {
            if (Input.GetMouseButtonUp(0))
            {
                ProcessInput(Input.mousePosition);
            }
        }
    }
}
