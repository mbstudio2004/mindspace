using Nocci.Zayebunny.GameDB.Base;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Internal.UIToolkitIntegration;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nocci.Zayebunny.Editor
{
    /// <summary>
    /// Custom editor for BaseObject.
    /// </summary>
    [CustomEditor(typeof(BaseObject))]
    public class BaseObjectEditor : OdinEditor
    {
        /// <summary>
        /// Serialized field for VisualTreeAsset.
        /// </summary>
        [field: SerializeField]
        public virtual VisualTreeAsset TreeAsset { get; private set; }

        /// <summary>
        /// Determines if the UI Toolkit should be used for drawing.
        /// </summary>
        public bool DrawWithUIToolkit => TreeAsset != null;

        /// <summary>
        /// Forces the use of custom Visual Element for drawing.
        /// </summary>
        public virtual bool ForceOverrideVisualElement { get; protected set; } = false;

        protected OdinImGuiElement OdinElement;
        protected VisualElement RootVisualElement;

        /// <summary>
        /// Method for drawing with UI Toolkit.
        /// </summary>
        /// <param name="root">The root VisualElement.</param>
        /// <returns>A VisualElement.</returns>
        public virtual VisualElement OnDrawUIToolkit(VisualElement root)
        {
            return root;
        }

        /// <summary>
        /// Method for drawing with UI Toolkit.
        /// </summary>
        /// <returns>A VisualElement.</returns>
        public virtual VisualElement OnDrawUIToolkit()
        {
            return default;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (DrawWithUIToolkit || ForceOverrideVisualElement)
            {
                VisualElement visualElement = null;

                if (ForceOverrideVisualElement)
                {
                    visualElement = OnDrawUIToolkit();
                }
                else
                {
                    RootVisualElement = new VisualElement();
                    TreeAsset.CloneTree(RootVisualElement);
                    visualElement = OnDrawUIToolkit(RootVisualElement);
                }

                OdinElement = new OdinImGuiElement(visualElement);
            }
            else
            {
                base.OnInspectorGUI();
            }
        }

        /// <summary>
        /// Overrides the OnInspectorGUI method.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (DrawWithUIToolkit || ForceOverrideVisualElement)
            {
                GUILayout.BeginVertical();
                ImguiElementUtils.EmbedVisualElementAndDrawItHere(OdinElement);

                GUILayout.EndVertical();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}