using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRCollie
{
    [CreateAssetMenu(fileName = "NewButtonHandler")]
    public class XRButtonHandler : XRInputHandler
    {
        #region Events
        public delegate void StateChange(XRController controller);
        public event StateChange OnButtonDown;
        public event StateChange OnButtonUp;
        #endregion

        #region Serialize Field
        [SerializeField] private InputHelpers.Button button = InputHelpers.Button.None;
        #endregion

        #region Private Field
        private bool _previousPress = false;
        #endregion

        #region Properties
        public bool IsPressed { get { return _previousPress; } }
        #endregion

        #region Invoke Event
        public override void HandleState(XRController controller)
        {
            if (controller.inputDevice.IsPressed(button, out bool pressed, controller.axisToPressThreshold))
            {
                if (_previousPress != pressed)
                {
                    _previousPress = pressed;
                    if (pressed)
                    {
                        OnButtonDown?.Invoke(controller);
                    }
                    else
                    {
                        OnButtonUp?.Invoke(controller);
                    }
                }
            }
        }
        #endregion
    }
}