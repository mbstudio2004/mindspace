using UnityEditor;
using UnityEngine;

namespace Nocci.Utils.Editor
{
    public static class DuplicateComponentTool
    {
        private const string k_menuPath = "CONTEXT/Component/Duplicate";
        
        [MenuItem(k_menuPath, priority = 503)]
        public static void DuplicateMenuOption(MenuCommand command)
        {
            var sourceComponent = command.context as Component;
            var newComponent = Undo.AddComponent(sourceComponent.gameObject, sourceComponent.GetType());

            var source = new SerializedObject(sourceComponent);
            var dest = new SerializedObject(newComponent);
            var iterator = source.GetIterator();

            while (iterator.NextVisible(true))
            {
                dest.CopyFromSerializedProperty(iterator);
            }

            dest.ApplyModifiedProperties();
        }
        [MenuItem(k_menuPath, validate = true)]
        public static bool DuplicateMenuOptionValidation(MenuCommand command)
        {
            return DoesComponentAllowMultiples((Component)command.context);
        }

        static bool DoesComponentAllowMultiples(Component component)
        {
            var componentType = component.GetType();
            var attributes = (DisallowMultipleComponent[])componentType.GetCustomAttributes(typeof(DisallowMultipleComponent), true);
            return attributes.Length == 0;
        }
    }
}