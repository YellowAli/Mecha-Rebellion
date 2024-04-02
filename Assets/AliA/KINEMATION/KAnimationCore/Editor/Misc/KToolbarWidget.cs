using UnityEngine;

namespace KINEMATION.KAnimationCore.Editor.Misc
{
    public struct KToolbarTab
    {
        public delegate void KOnTabRendered();
        
        public string Name;
        public KOnTabRendered OnTabRendered;
    }
    
    public class KToolbarWidget : IEditorTool
    {
        private int _toolbarIndex = 0;
        private string[] _toolbarTabNames;
        private KToolbarTab[] _toolbarTabs;

        public KToolbarWidget(KToolbarTab[] tabs)
        {
            _toolbarTabs = tabs;
            _toolbarTabNames = new string[_toolbarTabs.Length];

            for (int i = 0; i < _toolbarTabs.Length; i++)
            {
                _toolbarTabNames[i] = _toolbarTabs[i].Name;
            }
        }
        
        public void Render()
        {
           _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _toolbarTabNames);
           _toolbarTabs[_toolbarIndex].OnTabRendered?.Invoke();
        }
    }
}