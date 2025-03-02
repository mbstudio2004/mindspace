using UnityEditor;
using UnityEngine;

namespace Nocci.Utils.Editor
{
    public static class MoveComponentTool
    {
        
        const string k_menuMoveToTop = "CONTEXT/Component/Move To Top";
        const string k_menuMoveToBottom = "CONTEXT/Component/Move To Bottom";
        
        [MenuItem(k_menuMoveToTop, priority = 501)]
        public static void MoveComponentToTopMenuItem(MenuCommand command)
        {
            while (UnityEditorInternal.ComponentUtility.MoveComponentUp((Component)command.context)) ;
        }
        
        [MenuItem(k_menuMoveToTop, validate = true)]
        public static bool MoveComponentToTopMenuItemValidate(MenuCommand command)
        {
            var components = ((Component)command.context).gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == ((Component)command.context))
                {
                    if (i == 1) return false;
                }
            }

            return true;
        }
        
        
        [MenuItem(k_menuMoveToBottom, priority = 502)]
        public static void MoveComponentToBottomMenuItem(MenuCommand command)
        {
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown((Component)command.context)) ;
        }
        
        [MenuItem(k_menuMoveToBottom, validate = true)]
        public static bool MoveComponentToBottomMenuItemValidate(MenuCommand command)
        {
            var components = ((Component)command.context).gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == ((Component)command.context))
                {
                    if (i == components.Length-1) return false;
                }
            }

            return true;
        }
    }
    
    
    
}