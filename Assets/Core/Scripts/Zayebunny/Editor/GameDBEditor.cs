using System;
using System.Collections.Generic;
using System.IO;
using Nocci.Zayebunny.GameDB.Base;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

namespace Nocci.Zayebunny.Editor
{
    public class GameDBEditor : OdinMenuEditorWindow
    {
        private const string ZB_ICON_PATH = "Assets/_Core/Scripts/Zayebunny/Editor/Resources/zayebunnyWhite.png";

        #region Initialization

        /// <summary>
        ///     Initializes the editor window and sets up modules.
        /// </summary>
        protected override void Initialize()
        {
            SetupModules();
        }

        #endregion

        #region State Management

        /// <summary>
        ///     Changes the state of the module tree and updates the menu configuration.
        /// </summary>
        private void StateChange()
        {
            _treeRebuild = true;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        ///     Handles the event when the assembly is reloaded.
        /// </summary>
        public void OnAfterAssemblyReload()
        {
            var window = GetWindow<GameDBEditor>("Zayebunny");
            var icon = EditorGUIUtility.Load(ZB_ICON_PATH) as Texture;
            window.titleContent = new GUIContent("Zayebunny", icon);

            if (window.Settings.Modules.Count <= 0) return;
            window.AddModuleToTree(window.Settings.Modules[0]);
            window.ChangeSelectedModule(0);
        }

        #endregion

        #region Fields

        public GameDBSettings Settings;
        private bool _treeRebuild;
        private readonly List<DrawModuleTree> _drawModules = new();
        private DrawModuleTree _selectedDrawModule;
        [SerializeReference] private BaseModule _selectedModule;
        private int _selectedModuleIndex;
        private List<BaseModule> _moduleTree = new();

        #endregion


        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }

        #endregion

        #region Menu Items

        [MenuItem("MonadGames/GameDB Manager/Open Window")]
        public static void OpenWindow()
        {
            var window = GetWindow<GameDBEditor>("Zayebunny", true);
            var icon = EditorGUIUtility.Load(ZB_ICON_PATH) as Texture;
            window.titleContent = new GUIContent("Zayebunny", icon);
            window.DrawMenuSearchBar = true;

            window.Show();
            if (window.Settings.Modules.Count <= 0) return;
            window.AddModuleToTree(window.Settings.Modules[0]);
            window.ChangeSelectedModule(0);
        }

        private static void ClearNullsObjects(IEnumerable<BaseModule> modules)
        {
            foreach (var module in modules)
            {
                module.Objects.RemoveAll(x => x == null);
                if (module.SubModules.Count <= 0) continue;
                ClearNullsObjects(module.SubModules);
            }
        }

        #endregion

        #region Module Management

        /// <summary>
        ///     Sets up the module tree.
        /// </summary>
        private void SetupModules()
        {
            _moduleTree = new List<BaseModule>();
            foreach (var module in Settings.Modules) AddModulesRecursive(module);
        }

        /// <summary>
        ///     Adds modules and their submodules recursively to the module tree.
        /// </summary>
        /// <param name="module">The module to add.</param>
        private void AddModulesRecursive(BaseModule module)
        {
            var drawModule = new DrawModuleTree();
            drawModule.SetModule(module);
            _drawModules.Add(drawModule);

            foreach (var subModule in module.SubModules) AddModulesRecursive(subModule);
        }

        /// <summary>
        ///     Adds a module to the module tree.
        /// </summary>
        /// <param name="module">The module to add.</param>
        private void AddModuleToTree(BaseModule module)
        {
            _moduleTree.Add(module);
            if (module.SubModules.Count > 0) AddModuleToTree(module.SubModules[0]);
        }

        /// <summary>
        ///     Changes the selected module.
        /// </summary>
        /// <param name="module">The module to select.</param>
        public void ChangeSelectedModule(BaseModule module)
        {
            _selectedModule = module;
            _selectedDrawModule = _drawModules.Find(x => x.module == module);
            _selectedDrawModule.SetSelected(MenuTree.Selection.SelectedValue);
            _selectedModuleIndex = Settings.Modules.IndexOf(module);
        }

        /// <summary>
        ///     Changes the selected module by index.
        /// </summary>
        /// <param name="index">The index of the module to select.</param>
        public void ChangeSelectedModule(int index)
        {
            _selectedModuleIndex = index;
            _selectedModule = Settings.Modules[index];
            _selectedDrawModule = _drawModules[index];
        }

        /// <summary>
        ///     Selects a module and its submodules recursively.
        /// </summary>
        /// <param name="module">The module to select.</param>
        private void SelectModuleRecursive(BaseModule module)
        {
            ChangeSelectedModule(module);
            if (module.SubModules.Count > 0) SelectModuleRecursive(module.SubModules[0]);
        }

        #endregion

        #region GUI Drawing

        protected override void OnImGUI()
        {
            if (_treeRebuild && Event.current.type == EventType.Layout)
            {
                ForceMenuTreeRebuild();
                _treeRebuild = false;
            }

            SirenixEditorGUI.Title("Zayebunny", "Game Database system working under Zayebunny framework",
                TextAlignment.Center,
                true);
            EditorGUILayout.Space();
            //SirenixEditorGUI.BeginLegendBox();

            DrawModules();
            //SirenixEditorGUI.EndLegendBox();

            try
            {
                base.OnImGUI();
            }
            catch (Exception)
            {
                //ignored
            }
            // GUIUtility.ExitGUI(); 
        }

        /// <summary>
        ///     Draws the module selection buttons.
        /// </summary>
        private void DrawModules()
        {
            if (_moduleTree.Count <= 0) return;
            GUILayout.BeginHorizontal();
            var zbBaseModule = _moduleTree[0];
            if (Utils.SelectButtonModules(ref zbBaseModule, Settings.Modules))
            {
                SelectModuleRecursive(zbBaseModule);
                StateChange();
                _moduleTree[0] = zbBaseModule;
            }

            GUILayout.EndHorizontal();
            if (_moduleTree.Count == 1) return;

            var moduleIndex = 1;
            var prevModule = _moduleTree[0];
            //Debug.Log(moduleIndex); 
            while (moduleIndex < _moduleTree.Count)
            {
                var module = _moduleTree[moduleIndex];
                if (prevModule.SubModules.Count == 0)
                {
                    moduleIndex++;
                    continue;
                }

                GUILayout.BeginHorizontal();
                if (Utils.SelectButtonModules(ref module, prevModule.SubModules))
                {
                    SelectModuleRecursive(module);
                    StateChange();
                    _moduleTree[moduleIndex] = module;
                }

                GUILayout.EndHorizontal();
                prevModule = module;
                moduleIndex++;
            }
        }

        protected override void DrawEditors()
        {
            if (_selectedDrawModule == null) return;
            if (MenuTree.Selection.SelectedValue == null) return;

            _selectedDrawModule.SetSelected(MenuTree.Selection.SelectedValue);
            _selectedModuleIndex = _selectedDrawModule.GetSelectedIndex();
            try
            {
                DrawEditor(_selectedModule.IsTree ? _selectedModuleIndex : 0);
            }
            catch
            {
                //ignored
            }
        }

        private void SetupMenu()
        {
            MenuTree.Config.DrawSearchToolbar = true;
            MenuTree.Config.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            MenuTree.Config.DrawScrollView = true;
            MenuTree.Config.UseCachedExpandedStates = true;
            MenuTree.Config.AutoHandleKeyboardNavigation = true;
            MenuTree.DefaultMenuStyle.Borders = true;
            MenuTree.Config.DrawScrollView = true;
        }

        protected override void DrawMenu()
        {
            if (_selectedModule == null) return;
            if (!_selectedModule.IsTree) return;
            SetupMenu();
            base.DrawMenu();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                _selectedModule.AddObject();
                EditorUtility.SetDirty(_selectedModule);
                StateChange();
            }

            if (GUILayout.Button("From template"))
            {
                var window = AssetPickerWindow.ShowWindow();
                window.OnAssetSelected = obj =>
                {
                    var template = obj as ZB_ObjectTemplate;
                    if (template == null) return;
                    if (!template.IsProperType(_selectedModule.InheritorType))
                    {
                        UnityEngine.Debug.LogError("Template is not of proper type!");
                        return;
                    }

                    var newObj = template.InstantiateObject();
                    _selectedModule.Objects.Add(newObj);
                    EditorUtility.SetDirty(_selectedModule);
                    StateChange();
                };
            }

            if (GUILayout.Button("Remove"))
            {
                _selectedModule.RemoveObject(_selectedDrawModule.selected);
                EditorUtility.SetDirty(_selectedModule);
                StateChange();
            }

            if (GUILayout.Button("Refresh"))
            {
                ClearNullsObjects(Settings.Modules);
                RefreshObjects();
                foreach (var module in Settings.Modules)
                {
                    EditorUtility.SetDirty(module);
                }

                StateChange();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            /*if (GUILayout.Button("Clear nulls"))
            {
                ClearNullsObjects(Settings.Modules);
                EditorUtility.SetDirty(_selectedModule);
                StateChange();
            }*/

            if (GUILayout.Button("Save"))
            {
                ClearNullsObjects(Settings.Modules);
                foreach (var module in Settings.Modules)
                {
                    EditorUtility.SetDirty(module);
                }

                StateChange();

                AssetDatabase.SaveAssets();
            }

            GUILayout.EndHorizontal();
        }

        public void RefreshObjects()
        {
            foreach (var module in Settings.GetModulesFlat())
            {
                foreach (var obj in module.Objects)
                {
                    if (obj == null) continue;
                    var correctFileName = $"({module.DisplayName}) {obj.Name}";
                    if (obj.name != correctFileName)
                    {
                        obj.name = correctFileName;
                        EditorUtility.SetDirty(obj);
                        AssetDatabase.SaveAssetIfDirty(obj);
                    }
                }
            }
        }

        #endregion

        #region Target Management

        protected override IEnumerable<object> GetTargets()
        {
            var targets = new List<object>();

            if (_selectedModule == null) return targets;

            if (_selectedModule.IsTree)
                foreach (var zbObject in _selectedModule.Objects)
                    targets.Add(zbObject);
            else
                targets.Add(_selectedModule);

            return targets;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            if (_selectedModule == null) return tree;
            foreach (var zbObject in _selectedModule.Objects)
            {
                if (zbObject == null) continue;
                try
                {
                    tree.Add(zbObject.folderPath + zbObject.Name, zbObject, zbObject.icon);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            return tree;
        }

        #endregion
    }
}