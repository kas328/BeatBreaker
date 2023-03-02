using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRCollie
{
    [CreateAssetMenu(fileName = "NewAxisHandler")]
    public class XRAxisHandler : XRInputHandler, ISerializationCallbackReceiver
    {
        #region Events
        public delegate void ValueChange(XRController controller, float value);
        public event ValueChange OnValueChange;
        #endregion

        #region Serialize Field
        [SerializeField] private Axis axis = Axis.None;
        #endregion

        #region Private Field
        private InputFeatureUsage<float> _inputFeature;
        private float _previousValue = 0f;
        #endregion

        #region Properties
        public float Value { get => _previousValue; }
        #endregion

        #region Serialization Callback
        /* [ISerializationCallbackReceiver]
         * This interface is used to receive callbacks upon serialization and deserialization.
         * This can be used to serialize datatypes that Unity can't serialize.
         * For example, Unity doesn't know how to serialize a Dictionary. */
        public void OnBeforeSerialize()
        {
            // this is empty.
        }

        public void OnAfterDeserialize()
        {
            _inputFeature = new InputFeatureUsage<float>(axis.ToString());
        }
        #endregion

        public override void HandleState(XRController controller)
        {
            float value = GetValue(controller);

            if (value != _previousValue)
            {
                _previousValue = value;
                OnValueChange?.Invoke(controller, value);
            }
        }

        public float GetValue(XRController controller)
        {
            if (controller.inputDevice.TryGetFeatureValue(_inputFeature, out float value))
            {
                return value;
            }
            return 0f;
        }
    }

    public enum Axis { None, Trigger, Grip }
}