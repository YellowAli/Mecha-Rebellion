// Designed by KINEMATION, 2024.

using KINEMATION.FPSAnimationFramework.Runtime.Core;
using KINEMATION.KAnimationCore.Runtime.Attributes;
using KINEMATION.KAnimationCore.Runtime.Core;
using KINEMATION.KAnimationCore.Runtime.Input;
using UnityEngine;

namespace KINEMATION.FPSAnimationFramework.Runtime.Camera
{
    public class FPSCameraController : MonoBehaviour
    {
        [SerializeField, InputProperty] protected string mouseInputProperty = FPSANames.MouseInput;
        [SerializeField] protected EaseMode fovEaseMode;
        [SerializeField] [Min(0f)] protected float fovSpeed;
        protected UserInputController _inputController;
        protected FPSCameraShake _activeShake;
        
        protected Vector3 _cameraShake;
        protected Vector3 _cameraShakeTarget;
        protected float _cameraShakePlayback;

        protected UnityEngine.Camera _camera;
        protected float _fovPlayback;
        protected float _cachedFov;
        protected float _targetFov;
        
        protected virtual void UpdateCameraShake()
        {
            if (_activeShake == null) return;

            float length = _activeShake.shakeCurve.GetCurveLength();
            _cameraShakePlayback += Time.deltaTime * _activeShake.playRate;
            _cameraShakePlayback = Mathf.Clamp(_cameraShakePlayback, 0f, length);

            float alpha = KMath.ExpDecayAlpha(_activeShake.smoothSpeed, Time.deltaTime);
            if (!KAnimationMath.IsWeightRelevant(alpha))
            {
                alpha = 1f;
            }

            Vector3 target = _activeShake.shakeCurve.GetValue(_cameraShakePlayback);
            target.x *= _cameraShakeTarget.x;
            target.y *= _cameraShakeTarget.y;
            target.z *= _cameraShakeTarget.z;
            
            _cameraShake = Vector3.Lerp(_cameraShake, target, alpha);
            transform.rotation *= Quaternion.Euler(_cameraShake);
        }

        protected virtual void UpdateFOV()
        {
            _fovPlayback = Mathf.Clamp01(_fovPlayback + Time.deltaTime * fovSpeed);
            _camera.fieldOfView = KCurves.Ease(_cachedFov, _targetFov, _fovPlayback, fovEaseMode);
        }
        
        public virtual void Initialize()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            _inputController = transform.root.gameObject.GetComponentInChildren<UserInputController>();
            _cachedFov = _targetFov = _camera.fieldOfView;
        }

        public virtual void UpdateCamera()
        {
            Vector4 input = _inputController.GetValue<Vector4>(mouseInputProperty);
            
            // Stabilize the camera by overriding the rotation.
            Transform root = transform.root;
            transform.rotation = root.rotation * Quaternion.Euler(0f, input.x, 0f) 
                                               * Quaternion.Euler(input.y, 0f, 0f);

            UpdateCameraShake();
            UpdateFOV();
        }

        public virtual void PlayCameraShake(FPSCameraShake newShake)
        {
            if (newShake == null) return;

            _activeShake = newShake;
            _cameraShakePlayback = 0f;

            _cameraShakeTarget.x = FPSCameraShake.GetTarget(_activeShake.pitch);
            _cameraShakeTarget.y = FPSCameraShake.GetTarget(_activeShake.yaw);
            _cameraShakeTarget.z = FPSCameraShake.GetTarget(_activeShake.roll);
        }

        public virtual void UpdateTargetFOV(float newFov)
        {
            _cachedFov = _camera.fieldOfView;
            _targetFov = newFov;
            _fovPlayback = 0f;
        }
    }
}