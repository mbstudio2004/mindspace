using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Zayebunny.GameDB.Base
{
    /// <summary>
    /// Base class for all game objects.
    /// </summary>
    [Serializable]
    public abstract class BaseObject : SerializedScriptableObject, IComparable<BaseObject>
    {
        // Constants for Odin Inspector groups
        protected const string SPLIT = "Split";
        protected const string SPLIT_LEFT_VERTICAL = "Split/Left";
        protected const string SPLIT_RIGHT_VERTICAL = "Split/Right";
        protected const string STATS_GROUP = "Split/Left/Stats";
        protected const string GENERAL_BOX = "Split/Left/General";
        protected const string GENERAL_BOX_HORIZONTAL = "Split/Left/General/Horizontal";
        protected const string GENERAL_GROUP_VALUES = "Split/Left/General/Horizontal/Right";

        /// <summary>
        /// Icon of the game object.
        /// </summary>
        [HorizontalGroup(SPLIT, 0.5f, MarginLeft = 5, LabelWidth = 130)]
        [VerticalGroup(SPLIT_LEFT_VERTICAL)]
        [BoxGroup(GENERAL_BOX)]
        [HorizontalGroup(GENERAL_BOX_HORIZONTAL, 55, LabelWidth = 67)]
        [HideLabel]
        [PreviewField(55)]
        public Sprite icon;

        /// <summary>
        /// Description of the game object.
        /// </summary>
        [VerticalGroup(SPLIT_RIGHT_VERTICAL)] [BoxGroup("Split/Right/Description")] [HideLabel] [TextArea(4, 14)]
        public string Description;

        /// <summary>
        /// Folder path of the game object.
        /// </summary>
        [VerticalGroup(GENERAL_GROUP_VALUES)] public string folderPath = "";

        /// <summary>
        /// Name of the game object.
        /// </summary>
        [VerticalGroup(GENERAL_GROUP_VALUES)] public string Name;

        /// <summary>
        /// Key of the game object. Can only be set before the object is created.
        /// </summary>
        [VerticalGroup(GENERAL_GROUP_VALUES)] [EnableIf("@!_isCreated")]
        public string key;

        /// <summary>
        /// Indicates if the game object is created.
        /// </summary>
        [HideInInspector]
        [Space] public bool _isCreated;

        /// <summary>
        /// List of data associated with the game object.
        /// </summary>
        [ShowIf("@Data.Count > 0")]
        [PropertyOrder(997)]
        [BoxGroup(SPLIT_RIGHT_VERTICAL + "/Data definitions", centerLabel: true)]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false, ShowPaging = false, ShowIndexLabels = false, HideAddButton = true, HideRemoveButton = false)]
        [SerializeReference]
        public List<BaseObjectData> Data = new();

        // Private fields for adding data
        [ValueDropdown(nameof(DataTypes))]
        [ShowIf("@dataToAddIsSet")]
        [HorizontalGroup(SPLIT_RIGHT_VERTICAL + "/Data definitions/Horizontal")]
        [PropertyOrder(999)]
        [SerializeField]
        [LabelWidth(100)]
        private string dataToAdd;
        
        private bool dataToAddIsSet;

        /// <summary>
        /// Indicates if the game object is created.
        /// </summary>
        protected bool IsCreated => _isCreated;

        /// <summary>
        /// Indicates if the game object is static.
        /// </summary>
        public virtual bool IsStatic { get; set; } = false;

        /// <summary>
        /// List of data types that can be added to the game object.
        /// </summary>
        private List<string> DataTypes => BaseObjectData.DataNames.Where(x => Data.All(y => x != y.GetType().ToString())).ToList();

        /// <summary>
        /// Compares this game object to another game object.
        /// </summary>
        public int CompareTo(BaseObject other)
        {
            return 0;
        }

        /// <summary>
        /// Method called when the game object is created.
        /// </summary>
        protected virtual void OnObjectCreated()
        {
        }

        /// <summary>
        /// Method for custom code generation.
        /// </summary>
        public virtual void CustomCodeGen(ref StringBuilder stringBuilder)
        {
        }

        /// <summary>
        /// Creates the game object.
        /// </summary>
        [InfoBox("Once object created, key cannot be changed", InfoMessageType.Warning)]
        [ShowIf("@!_isCreated")]
        public void Create()
        {
            if (string.IsNullOrEmpty(key))
            {
                UnityEngine.Debug.LogError("Key cannot be empty");
                return;
            }

            _isCreated = true;
            OnObjectCreated();
        }

        /// <summary>
        /// Adds data to the game object.
        /// </summary>
        [Button]
        [PropertySpace(10)]
        [HorizontalGroup(SPLIT_RIGHT_VERTICAL + "/Data definitions/Horizontal")]
        [PropertyOrder(998)]
        private void AddData()
        {
            if (dataToAddIsSet)
            {
                if (!string.IsNullOrEmpty(dataToAdd))
                {
                    var type = BaseObjectData.GetTypes.First(x => x.GetType().Name == dataToAdd).GetType();
                    Data.Add(Activator.CreateInstance(type) as BaseObjectData);
                }

                dataToAdd = "";
                dataToAddIsSet = false;
            }
            else
            {
                dataToAddIsSet = true;
            }
        }

        /// <summary>
        /// Gets data of a specific type from the game object.
        /// </summary>
        public T GetData<T>() where T : BaseObjectData
        {
            var detail = Data.FirstOrDefault(x => x?.GetType() == typeof(T));
            return detail as T;
        }

        /// <summary>
        /// Tries to get data of a specific type from the game object.
        /// </summary>
        public bool TryGetDataOfType<T>(out T data) where T : BaseObjectData
        {
            data = GetData<T>();
            return data != null;
        }
    }
}