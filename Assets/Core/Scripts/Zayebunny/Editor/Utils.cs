using System.Collections.Generic;
using Nocci.Zayebunny.GameDB.Base;
using Sirenix.Utilities;
using UnityEngine;

namespace Nocci.Zayebunny.Editor
{
    public static class Utils
    {
        public static bool SelectButtonModules(ref BaseModule selectedType, List<BaseModule> typesToDisplay)
        {
            var rect = GUILayoutUtility.GetRect(0, 25);
            for (var i = 0; i < typesToDisplay.Count; i++)
            {
                var name = typesToDisplay[i].DisplayName;
                var btnRect = rect.Split(i, typesToDisplay.Count);
                if (!MTK_EditorUtils.SelectButton(btnRect, name, typesToDisplay[i] == selectedType)) continue;
                selectedType = typesToDisplay[i];
                return true;
            }

            return false;
        }
    }
}