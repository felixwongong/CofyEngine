using System.Collections.Generic;
using UnityEditor;

namespace CofyEngine.Editor
{
    public class TabEditorWindow<TKey, TContent> : EditorWindow
    {
        private static Dictionary<TKey, TContent> _tabContents = new ();
        
        private static TKey _activeTab;
    }
}