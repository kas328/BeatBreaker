using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRCollie
{
    public class XRInputConsole : MonoBehaviour
    {
        #region Serialize Field
        [Header("Inputs")]
        [SerializeField] private XRAxis2DHandler primaryAxisLeft = null;
        [SerializeField] private XRAxis2DHandler primaryAxisRight = null;
        [SerializeField] private XRAxisHandler triggerLeft = null;
        [SerializeField] private XRAxisHandler triggerRight = null;
        [SerializeField] private XRAxisHandler gripLeft = null;
        [SerializeField] private XRAxisHandler gripRight = null;
        [SerializeField] private XRButtonHandler primaryAxisClick = null;
        [SerializeField] private XRButtonHandler primaryButtonLeft = null;
        [SerializeField] private XRButtonHandler primaryButtonRight = null;
        [SerializeField] private XRButtonHandler SecondaryButtonLeft = null;
        [SerializeField] private XRButtonHandler SecondaryButtonRight = null;

        [Header("Outputs")]
        [SerializeField] private TextMeshProUGUI output = null;
        #endregion

        private void OnEnable()
        {
            primaryAxisLeft.OnValueChange += PrimaryAxisLeft;
            primaryAxisRight.OnValueChange += PrimaryAxisRight;
            triggerLeft.OnValueChange += TriggerLeft;
            triggerRight.OnValueChange += TriggerRight;
            gripLeft.OnValueChange += GripLeft;
            gripRight.OnValueChange += GripRight;
            primaryAxisClick.OnButtonDown += PrimaryClickDown;
            primaryAxisClick.OnButtonUp += PrimaryClickUp;
            primaryButtonLeft.OnButtonDown += PrimaryButtonDownLeft;
            primaryButtonLeft.OnButtonUp += PrimaryButtonUpLeft;
            primaryButtonRight.OnButtonDown += PrimaryButtonDownRight;
            primaryButtonRight.OnButtonUp += PrimaryButtonUpRight;
            SecondaryButtonLeft.OnButtonDown += SecondaryButtonDownLeft;
            SecondaryButtonRight.OnButtonUp += SecondaryButtonDownRight;
            SecondaryButtonLeft.OnButtonDown += SecondaryButtonUpLeft;
            SecondaryButtonRight.OnButtonUp += SecondaryButtonUpRight;
        }

        #region Evnets Test
        private void PrimaryAxisLeft(XRController controller, Vector2 value)
        {
            print("[Event] PrimaryAxisLeft");
        }

        private void PrimaryAxisRight(XRController controller, Vector2 value)
        {
            print("[Event] PrimaryAxisRight");
        }

        private void TriggerLeft(XRController controller, float value)
        {
            print("[Event] TriggerLeft");
        }

        private void TriggerRight(XRController controller, float value)
        {
            print("[Event] TriggerRight");
        }

        private void GripLeft(XRController controller, float value)
        {
            print("[Event] GripLeft");
        }

        private void GripRight(XRController controller, float value)
        {
            print("[Event] GripRight");
        }

        private void PrimaryClickDown(XRController controller)
        {
            print("[Event] PrimaryClickDown");
        }

        private void PrimaryClickUp(XRController controller)
        {
            print("[Event] PrimaryClickUp");
        }

        private void PrimaryButtonDownLeft(XRController controller)
        {
            print("[Event] PrimaryButtonDownLeft");
        }

        private void PrimaryButtonDownRight(XRController controller)
        {
            print("[Event] PrimaryButtonDownRight");
        }

        private void PrimaryButtonUpLeft(XRController controller)
        {
            print("[Event] PrimaryButtonUpLeft");
        }

        private void PrimaryButtonUpRight(XRController controller)
        {
            print("[Event] PrimaryButtonUpRight");
        }

        private void SecondaryButtonDownLeft(XRController controller)
        {
            print("[Event] SecondaryButtonDownLeft");
        }

        private void SecondaryButtonDownRight(XRController controller)
        {
            print("[Event] SecondaryButtonDownRight");
        }

        private void SecondaryButtonUpLeft(XRController controller)
        {
            print("[Event] SecondaryButtonUpLeft");
        }

        private void SecondaryButtonUpRight(XRController controller)
        {
            print("[Event] SecondaryButtonUpRight");
        }
        #endregion

        private void Update()
        {
            output.text = "Primary Axis Left : " + primaryAxisLeft.Value + "\n" +
                "Primary Axis Right : " + primaryAxisRight.Value + "\n" +
                "Trigger Left : " + triggerLeft.Value + "\n" +
                "Trigger Right : " + triggerRight.Value + "\n" +
                "Grip Left : " + gripLeft.Value + "\n" +
                "Grip Right : " + gripRight.Value + "\n" +
                "Primary Axis Click : " + primaryAxisClick.IsPressed + "\n" +
                "Primary Button Left : " + primaryButtonLeft.IsPressed + "\n" +
                "Primary Button Right : " + primaryButtonRight.IsPressed + "\n" +
                "Secondary Button Left : " + SecondaryButtonLeft.IsPressed + "\n" +
                "Secondary Button Right : " + SecondaryButtonRight.IsPressed + "\n";
        }

        private void OnDisable()
        {
            primaryAxisLeft.OnValueChange -= PrimaryAxisLeft;
            primaryAxisRight.OnValueChange -= PrimaryAxisRight;
            triggerLeft.OnValueChange -= TriggerLeft;
            triggerRight.OnValueChange -= TriggerRight;
            gripLeft.OnValueChange -= GripLeft;
            gripRight.OnValueChange -= GripRight;
            primaryAxisClick.OnButtonDown -= PrimaryClickDown;
            primaryAxisClick.OnButtonUp -= PrimaryClickUp;
            primaryButtonLeft.OnButtonDown -= PrimaryButtonDownLeft;
            primaryButtonLeft.OnButtonUp -= PrimaryButtonUpLeft;
            primaryButtonRight.OnButtonDown -= PrimaryButtonDownRight;
            primaryButtonRight.OnButtonUp -= PrimaryButtonUpRight;
            SecondaryButtonLeft.OnButtonDown -= SecondaryButtonDownLeft;
            SecondaryButtonRight.OnButtonUp -= SecondaryButtonDownRight;
            SecondaryButtonLeft.OnButtonDown -= SecondaryButtonUpLeft;
            SecondaryButtonRight.OnButtonUp -= SecondaryButtonUpRight;
        }
    }
}