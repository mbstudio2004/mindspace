using Nocci.Zayebunny.Attributes;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Nocci.Zayebunny.Editor.Editor
{
    public class ShowInheritedPropertiesOnlyDrawer : OdinAttributeDrawer<ShowInheritedPropertiesOnlyAttribute>
    {
        private PropertyTree propertyTree;

        protected override void Initialize()
        {
            base.Initialize();
            propertyTree = PropertyTree.Create(Property.ValueEntry.WeakSmartValue);
            propertyTree.PropertyResolverLocator = new DefaultOdinPropertyResolverLocator();
        }


        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (Property.ValueEntry.BaseValueType.BaseType != null)
                propertyTree.Draw(false);
            else
                CallNextDrawer(label);
        }

        private bool FilterInheritedPropertiesOnly(InspectorProperty property)
        {
            // Filter out properties declared by the base type
            return property.ParentValueProperty != null && property.ParentValueProperty.ValueEntry.BaseValueType !=
                property.Info.GetMemberInfo().DeclaringType;
        }
    }
}