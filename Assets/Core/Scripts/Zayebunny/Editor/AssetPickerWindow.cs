using Nocci.Zayebunny.GameDB.Base;
using UnityEditor;
using UnityEngine;

namespace Nocci.Zayebunny.Editor
{
    public class AssetPickerWindow : EditorWindow
    {
        public System.Action<Object> OnAssetSelected;
        private Object selectedAsset;

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select an asset", EditorStyles.boldLabel);

            selectedAsset = EditorGUILayout.ObjectField("Asset", selectedAsset, typeof(ZB_ObjectTemplate), false);

            if (GUILayout.Button("Select Asset"))
            {
                OnAssetSelected?.Invoke(selectedAsset);
                Close();
            }
        }

        [MenuItem("Window/Asset Picker")]
        public static AssetPickerWindow ShowWindow()
        {
            var window = GetWindow<AssetPickerWindow>("Asset Picker");
            window.Show();
            return window;
        }
    }
}