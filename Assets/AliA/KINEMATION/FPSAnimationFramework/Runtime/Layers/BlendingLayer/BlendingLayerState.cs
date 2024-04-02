using KINEMATION.FPSAnimationFramework.Runtime.Core;

namespace KINEMATION.FPSAnimationFramework.Runtime.Layers.BlendingLayer
{
    public class BlendingLayerState : FPSAnimatorLayerState
    {
        private BlendingLayerSettings _settings;

        public override void InitializeState(FPSAnimatorLayerSettings newSettings)
        {
            base.InitializeState(newSettings);
            _settings = (BlendingLayerSettings) newSettings;
        }
    }
}