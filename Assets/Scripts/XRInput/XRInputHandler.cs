using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRCollie
{
    public class XRInputHandler : ScriptableObject
    {
        public virtual void HandleState(XRController controller)
        {
            // this is empty.
        }
    }

}