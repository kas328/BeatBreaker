using UnityEngine;

namespace VRCollie
{
    public class XRPhysicsHand : MonoBehaviour
    {
        const float velocityPredictionFactor = 0.6f;
        const float angularVelocityDamping = 0.95f;

        #region Serialize Field
        [Header("Tracking Type")]
        [SerializeField] private TrackingType selectedTrackingType = TrackingType.VelocityUpdate;

        [Header("Update Target")]
        [SerializeField] private Transform target = null;
        [SerializeField] private float smoothAmount = 8f;
        #endregion

        #region Private Field
        private Rigidbody _rb = null;
        private Vector3 targetPosition = Vector3.zero;
        private Quaternion targetRotation = Quaternion.identity;
        #endregion

        private void Awake()
        {
            GetComponents();
        }

        #region Initialize
        private void GetComponents()
        {
            _rb = GetComponentInChildren<Rigidbody>();
        }
        #endregion

        private void Start()
        {
            SetRigidbody();
            TelePortToTarget();
        }

        #region Setup
        private void SetRigidbody()
        {
            _rb.isKinematic = selectedTrackingType == TrackingType.KinematicUpdate;
        }

        private void TelePortToTarget()
        {
            targetPosition = target.position;
            targetRotation = target.rotation;

            _rb.transform.position = targetPosition;
            _rb.transform.rotation = targetRotation;
        }
        #endregion

        private void Update()
        {
            SetTargetPosition();
            SetTargetRotation();
        }

        #region Update Target
        private void SetTargetPosition()
        {
            float time = smoothAmount * Time.unscaledDeltaTime;
            targetPosition = Vector3.Lerp(targetPosition, target.position, time);
        }

        private void SetTargetRotation()
        {
            float time = smoothAmount * Time.unscaledDeltaTime;
            targetRotation = Quaternion.Lerp(targetRotation, target.rotation, time);
        }
        #endregion

        private void FixedUpdate()
        {
            switch (selectedTrackingType)
            {
                case TrackingType.KinematicUpdate:
                    KinematicTrackPoision();
                    KinematicTrackRotation();
                    break;

                case TrackingType.VelocityUpdate:
                    VelocityTrackPosition();
                    VelocityTrackRotation();
                    break;
            }
        }

        #region Perform Kinematic Track
        private void KinematicTrackPoision()
        {
            Vector3 positionDelta = targetPosition - _rb.transform.position;

            _rb.velocity = Vector3.zero;
            _rb.MovePosition(this.transform.position + positionDelta);
        }

        private void KinematicTrackRotation()
        {
            _rb.angularVelocity = Vector3.zero;
            _rb.MoveRotation(targetRotation);
        }
        #endregion

        #region Perform Velocity Track
        private void VelocityTrackPosition()
        {
            _rb.velocity *= velocityPredictionFactor;
            var positoinDelta = targetPosition - _rb.worldCenterOfMass;
            var velocity = positoinDelta / Time.unscaledDeltaTime;

            if (!float.IsNaN(velocity.x))
            {
                _rb.velocity += velocity;
            }
        }

        private void VelocityTrackRotation()
        {
            _rb.angularVelocity *= velocityPredictionFactor;
            var rotationDelta = targetRotation * Quaternion.Inverse(_rb.rotation);
            float angleInDegrees; Vector3 rotationAxis;
            rotationDelta.ToAngleAxis(out angleInDegrees, out rotationAxis);
            if (angleInDegrees > 180)
            {
                angleInDegrees -= 360;
            }

            if (Mathf.Abs(angleInDegrees) > Mathf.Epsilon)
            {
                var angularVelocity = (rotationAxis * angleInDegrees * Mathf.Deg2Rad) / Time.unscaledDeltaTime;
                if (!float.IsNaN(angularVelocity.x))
                {
                    _rb.angularVelocity += angularVelocity * angularVelocityDamping;
                }
            }
        }
        #endregion

        private enum TrackingType { KinematicUpdate, VelocityUpdate }
    }
}