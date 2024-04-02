// Designed by KINEMATION, 2024.

using KINEMATION.FPSAnimationFramework.Runtime.Core;
using KINEMATION.FPSAnimationFramework.Runtime.Layers.WeaponLayer;
using KINEMATION.FPSAnimationFramework.Runtime.Recoil;
using KINEMATION.KAnimationCore.Runtime.Core;

using UnityEngine;

namespace KINEMATION.FPSAnimationFramework.Runtime.Layers.AdditiveLayer
{
    // Applies curve animations and recoil.
    public class AdditiveLayerState : WeaponLayerState
    {
        private AdditiveLayerSettings _settings;
        private RecoilAnimation _recoilAnimation;

        private Transform _additiveBone;
        private KTransform _ikMotion = KTransform.Identity;
        
        public override void InitializeState(FPSAnimatorLayerSettings newSettings)
        {
            base.InitializeState(newSettings);
            
            _settings = (AdditiveLayerSettings) newSettings;
            _recoilAnimation = _owner.GetComponent<RecoilAnimation>();

            _additiveBone = _rigComponent.GetRigTransform(_settings.additiveBone);
        }

        protected override void OnEvaluateWeaponPose()
        {
            float weight = Weight;
            
            Quaternion recoilR = Quaternion.Euler(_recoilAnimation.OutRot);
            Vector3 recoilT = _recoilAnimation.OutLoc;

            KAnimationMath.MoveInSpace(WeaponIkBone, WeaponIkBone, recoilT, weight);
            KAnimationMath.RotateInSpace(WeaponIkBone, WeaponIkBone, recoilR, weight);
            
            float t = KMath.ExpDecayAlpha(_settings.interpSpeed, Time.deltaTime);
            if (Mathf.Approximately(_settings.interpSpeed, 0f))
            {
                t = 1f;
            }
            
            float aimingWeight = _inputController.GetValue<float>(_settings.aimingInputProperty);
            weight *= Mathf.Lerp(1f, _settings.adsScalar, aimingWeight);
            _ikMotion = KTransform.Lerp(_ikMotion, new KTransform(_additiveBone.transform, false), t);

            KAnimationMath.MoveInSpace(_owner.transform, WeaponIkBone, _ikMotion.position, weight);
            KAnimationMath.RotateInSpace(_owner.transform, WeaponIkBone, _ikMotion.rotation, weight);
        }
    }
}