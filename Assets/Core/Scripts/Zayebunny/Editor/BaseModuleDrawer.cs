using Nocci.Zayebunny.GameDB.Base;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Internal.UIToolkitIntegration;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Zayebunny.Editor
{
    [CustomEditor(typeof(BaseModule))]
    public class BaseModuleDrawer : OdinEditor
    {
        [field: SerializeField]
        public virtual VisualTreeAsset TreeAsset { get; private set; }
        public bool DrawWithUIToolkit => TreeAsset != null;

        protected OdinImGuiElement OdinElement;
        protected VisualElement RootVisualElement;

        protected override void OnEnable()
        {
            if (DrawWithUIToolkit)
            {
                RootVisualElement = new VisualElement();
                TreeAsset.CloneTree(RootVisualElement);
                OdinElement = new OdinImGuiElement(RootVisualElement);
            }
        }
        
        public virtual void DrawUIToolkit(Rect visualElementRect)
        {
        }


        public override void OnInspectorGUI()
        {
            if (DrawWithUIToolkit)
            {
                GUILayout.BeginVertical();
                DrawUIToolkit(ImguiElementUtils.EmbedVisualElementAndDrawItHere(OdinElement));
                GUILayout.EndVertical();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}