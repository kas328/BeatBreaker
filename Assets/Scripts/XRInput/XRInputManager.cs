using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRCollie
{
    public class XRInputManager : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField] private List<XRButtonHandler> buttonHandlersList = new List<XRButtonHandler>();
        [SerializeField] private List<XRAxis2DHandler> axis2DHandlerList = new List<XRAxis2DHandler>();
        [SerializeField] private List<XRAxisHandler> axisHandlerList = new List<XRAxisHandler>();
        #endregion

        #region Private Field
        private XRController _controller = null;
        #endregion

        private void Awake()
        {
            GetComponents();
        }

        #region Initialize
        private void GetComponents()
        {
            _controller = GetComponent<XRController>();
        }
        #endregion

        private void Update()
        {
            HandleButtonEvents();
            HandleAxis2DEvents();
            HandleAxisEvents();
        }

        #region Event Handlers
        private void HandleButtonEvents()
        {
            foreach (XRButtonHandler handler in buttonHandlersList)
            {
                handler.HandleState(_controller);
            }
        }

        private void HandleAxis2DEvents()
        {
            foreach (XRAxis2DHandler handler in axis2DHandlerList)
            {
                handler.HandleState(_controller);
            }
        }

        private void HandleAxisEvents()
        {
            foreach (XRAxisHandler handler in axisHandlerList)
            {
                handler.HandleState(_controller);
            }
        }
        #endregion
    }
}