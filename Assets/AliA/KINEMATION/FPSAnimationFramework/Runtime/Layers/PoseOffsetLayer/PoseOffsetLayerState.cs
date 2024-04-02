// Designed by KINEMATION, 2024.

using KINEMATION.FPSAnimationFramework.Runtime.Core;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.FPSAnimationFramework.Runtime.Layers.PoseOffsetLayer
{
    public class PoseOffsetLayerState : FPSAnimatorLayerState
    {
        private PoseOffsetLayerSettings _settings;
        private Transform[] _boneReferences;
        
        public override void InitializeState(FPSAnimatorLayerSettings newSettings)
        {
            _settings = (PoseOffsetLayerSettings) newSettings;
            _boneReferences = new Transform[_settings.poseOffsets.Count];

            int count = _boneReferences.Length;

            for (int i = 0; i < count; i++)
            {
                _boneReferences[i] = _rigComponent.GetRigTransform(_settings.poseOffsets[i].pose.element);
            }
        }

        public override void OnEvaluatePose()
        {
            int count = _boneReferences.Length;

            for (int i = 0; i < count; i++)
            {
                PoseOffset poseOffset = _settings.poseOffsets[i];
                KAnimationMath.ModifyTransform(_owner.transform, _boneReferences[i], poseOffset.pose,
                    Weight * GetCurveBlendValue(poseOffset.blend));
            }
        }
    }
}