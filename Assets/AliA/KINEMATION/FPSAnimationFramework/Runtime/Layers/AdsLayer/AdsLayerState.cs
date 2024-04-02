// Designed by KINEMATION, 2024.

using KINEMATION.FPSAnimationFramework.Runtime.Core;
using KINEMATION.FPSAnimationFramework.Runtime.Layers.WeaponLayer;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.FPSAnimationFramework.Runtime.Layers.AdsLayer
{
    public class AdsLayerState : WeaponLayerState
    {
        private FPSAnimatorEntity _entity;
        private AdsLayerSettings _settings;
        
        private Transform _aimTargetBone;

        private float _aimingWeight;
        private KTransform _additivePose = KTransform.Identity;

        // Returns an additive ads pose in Component space.
        private KTransform GetAdsPose()
        {
            KTransform componentBone =
                new KTransform(_owner.transform).GetRelativeTransform(new KTransform(WeaponIkBone), false);
            
            KTransform aimBone =
                new KTransform(_owner.transform).GetRelativeTransform(new KTransform(_aimTargetBone), false);
            
            KTransform result = new KTransform()
            {
                position = aimBone.position - componentBone.position,
                rotation = Quaternion.Inverse(componentBone.rotation)
            };

            return result;
        }

        private Vector3 GetEuler(Quaternion rotation)
        {
            Vector3 result = rotation.eulerAngles;

            result.x = KMath.NormalizeEulerAngle(result.x);
            result.y = KMath.NormalizeEulerAngle(result.y);
            result.z = KMath.NormalizeEulerAngle(result.z);

            return result;
        }

        public override void InitializeState(FPSAnimatorLayerSettings newSettings)
        {
            base.InitializeState(newSettings);
            
            _settings = (AdsLayerSettings) newSettings;
            _aimTargetBone = _rigComponent.GetRigTransform(_settings.aimTargetBone);

            _additivePose = GetAdsPose();
        }

        public override void OnEntityUpdated(FPSAnimatorEntity newEntity)
        {
            _entity = newEntity;
        }

        public override void OnGameThreadUpdate()
        {
            bool isAiming = _inputController.GetValue<bool>(_settings.isAimingProperty);

            _aimingWeight += _settings.aimingSpeed * (isAiming ? Time.deltaTime : -Time.deltaTime);
            _aimingWeight = Mathf.Clamp01(_aimingWeight);
            
            _inputController.SetValue(FPSANames.AimingWeight, _aimingWeight);
        }

        protected override void OnEvaluateWeaponPose()
        {
            float weight = KCurves.Ease(0f, 1f, _aimingWeight, _settings.aimingEaseMode) * _settings.alpha;
            
            AdsBlend blend = _settings.positionBlend;
            KTransform pose = GetAdsPose();

            pose.position.x = Mathf.Lerp(pose.position.x, _additivePose.position.x, blend.x);
            pose.position.y = Mathf.Lerp(pose.position.y, _additivePose.position.y, blend.y);
            pose.position.z = Mathf.Lerp(pose.position.z, _additivePose.position.z, blend.z);

            blend = _settings.rotationBlend;
            Vector3 absQ = GetEuler(pose.rotation);
            Vector3 addQ = GetEuler(_additivePose.rotation);

            absQ.x = Mathf.Lerp(absQ.x, addQ.x, blend.x);
            absQ.y = Mathf.Lerp(absQ.y, addQ.y, blend.y);
            absQ.z = Mathf.Lerp(absQ.z, addQ.z, blend.z);

            pose.rotation = Quaternion.Euler(absQ);

            KAnimationMath.RotateInSpace(_owner.transform, WeaponIkBone, pose.rotation, weight);
            KAnimationMath.MoveInSpace(_owner.transform, WeaponIkBone, pose.position, weight);
            
            if (_entity == null || _entity.defaultAimPoint == null) return;

            Quaternion socketRotation = Quaternion.Inverse(WeaponIkBone.rotation) * _entity.defaultAimPoint.rotation;
            Vector3 socketPosition = -WeaponIkBone.InverseTransformPoint(_entity.defaultAimPoint.position);

            KAnimationMath.MoveInSpace(_owner.transform, WeaponIkBone, socketRotation * socketPosition, weight);
            KAnimationMath.RotateInSpace(_owner.transform, WeaponIkBone, socketRotation, weight);
        }
    }
}