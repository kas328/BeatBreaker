using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRCollie
{
    [CreateAssetMenu(fileName = "NewAxis2DHandler")]
    public class XRAxis2DHandler : XRInputHandler, ISerializationCallbackReceiver
    {
        #region Events
        public delegate void ValueChange(XRController controller, Vector2 value);
        public event ValueChange OnValueChange;
        #endregion

        #region Serialize Field
        [SerializeField] private Axis2D axis = Axis2D.None;
        #endregion

        #region Private Field
        private InputFeatureUsage<Vector2> _inputFeature;
        private Vector2 _previousValue = Vector2.zero;
        #endregion

        #region Properties
        public Vector2 Value { get => _previousValue; }
        #endregion

        #region Serialization Callback
        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            _inputFeature = new InputFeatureUsage<Vector2>(axis.ToString());
        }
        #endregion

        public override void HandleState(XRController controller)
        {
            Vector2 value = GetValue(controller);

            if (value != _previousValue)
            {
                _previousValue = value;
                OnValueChange?.Invoke(controller, value);
            }
        }

        public Vector2 GetValue(XRController controller)
        {
            if (controller.inputDevice.TryGetFeatureValue(_inputFeature, out Vector2 value))
            {
                return value;
            }
            return Vector2.zero;
        }
    }

    public enum Axis2D { None, Primary2DAxis, Secondary2DAxis }
}