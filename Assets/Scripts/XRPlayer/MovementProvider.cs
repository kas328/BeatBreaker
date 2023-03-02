using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRCollie
{
    public class MovementProvider : LocomotionProvider
    {
        #region Serialize Field
        [SerializeField] private List<XRController> controllers = null;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float gravityMultiplier = 1f;
        [SerializeField] private CharacterController _characterController = null;
        #endregion

        #region Private Field
        private GameObject _head = null;
        #endregion

        protected override void Awake()
        {
            GetComponents();
        }

        #region Initialize
        private void GetComponents()
        {
            _head = GetComponent<XRRig>().cameraGameObject;
        }
        #endregion

        private void Start()
        {
            PositionCharacter();
        }

        private void Update()
        {
            PositionCharacter();
            CheckForInput();
            ApplyGravity();
        }

        #region Check For Updates
        private void CheckForInput()
        {
            foreach (XRController controller in controllers)
            {
                if (controller.enableInputActions)
                {
                    CheckForMovement(controller.inputDevice);
                }
            }
        }

        private void CheckForMovement(InputDevice device)
        {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            {
                StartMove(position);
            }
        }
        #endregion

        #region Apply Movement
        private void StartMove(Vector2 position)
        {
            Vector3 direction = new Vector3(position.x, 0, position.y);
            Vector3 headRotation = new Vector3(0, _head.transform.eulerAngles.y, 0);

            direction = Quaternion.Euler(headRotation) * direction;

            Vector3 movement = direction * speed;
            _characterController.Move(movement * Time.fixedDeltaTime);
        }

        private void PositionCharacter()
        {
            float headHeight = Mathf.Clamp(_head.transform.localPosition.y, 1, 2);
            _characterController.height = headHeight;

            Vector3 newCenter = Vector3.zero;
            newCenter.y = _characterController.height / 2;
            newCenter.y += _characterController.skinWidth;

            newCenter.x = _head.transform.localPosition.x;
            newCenter.z = _head.transform.localPosition.z;

            _characterController.center = newCenter;
        }

        private void ApplyGravity()
        {
            Vector3 gravity = new Vector3(0, Physics.gravity.y * gravityMultiplier, 0);
            gravity.y *= Time.deltaTime;

            _characterController.Move(gravity * Time.deltaTime);
        }
        #endregion
    }
}
