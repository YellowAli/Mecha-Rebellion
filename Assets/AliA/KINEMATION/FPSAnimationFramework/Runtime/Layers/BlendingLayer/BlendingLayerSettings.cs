using KINEMATION.FPSAnimationFramework.Runtime.Core;

namespace KINEMATION.FPSAnimationFramework.Runtime.Layers.BlendingLayer
{
    public class BlendingLayerSettings : FPSAnimatorLayerSettings
    {
        public override FPSAnimatorLayerState CreateState()
        {
            return new BlendingLayerState();
        }
    }
}