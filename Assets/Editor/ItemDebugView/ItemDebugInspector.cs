using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ItemDebugView
{
    public class ItemDebugInspector : OdinEditorWindow
    { 
         
        
        
        [MenuItem("GameTools/ItemDebug/Inspector")]
        private static void ShowWindow()
        {
            if (Inst != null)
                Inst.Close();
            
            var window = GetWindow<ItemDebugInspector>();
            window.titleContent = new GUIContent("ItemDebugInspector");
            window.Show();
            Inst = window;
        }

        public static ItemDebugInspector Inst;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            Inst = this;
        }

        [ShowIf("TargetItemObject")]
        [InlineEditor(InlineEditorModes.GUIOnly,InlineEditorObjectFieldModes.Hidden)]
        public ItemScriptableObject TargetItemObject;

        private bool ItemIsNull => TargetItemObject == null;

        
        protected override void OnGUI()
        {
            base.OnGUI();
            if (ItemIsNull)
            {
                this.ShowNotification(new GUIContent("万物皆空"));
            }
        }
    }
}