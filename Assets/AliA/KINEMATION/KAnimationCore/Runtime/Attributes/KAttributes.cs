// Designed by KINEMATION, 2024.

using System;
using KINEMATION.KAnimationCore.Runtime.Rig;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Attributes
{
    public class CurveSelectorAttribute : PropertyAttribute
    {
        public bool UseAnimator;
        public bool UsePlayables;
        public bool UseInput;
        
        public CurveSelectorAttribute(bool useAnimator = true, bool usePlayables = true, bool useInput = true)
        {
            UseAnimator = useAnimator;
            UsePlayables = usePlayables;
            UseInput = useInput;
        }
    }

    public class InputProperty : PropertyAttribute { }

    public class ElementChainSelectorAttribute : PropertyAttribute
    {
        public string TargetAssetName;
        
        public ElementChainSelectorAttribute(string rigName = "")
        {
            TargetAssetName = rigName;
        }
    }

    public class ReadOnlyAttribute : PropertyAttribute { }

    public class UnfoldAttribute : PropertyAttribute { }
    
    public class KAttributes { }
}