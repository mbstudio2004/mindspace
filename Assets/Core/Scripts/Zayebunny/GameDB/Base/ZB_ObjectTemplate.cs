using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nocci.Zayebunny.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Nocci.Zayebunny.GameDB.Base
{
    [CreateAssetMenu(fileName = "New ObjectTemplate", menuName = "Zayebunny/ObjectTemplate")]
    public class ZB_ObjectTemplate : ScriptableObject
    {
        [SerializeField] [ValueDropdown(nameof(GetInheritorTypes))] [OnValueChanged(nameof(CreateTemplateInstance))]
        private string selectedInheritorType;

        [InlineProperty] [HideLabel] [HideReferenceObjectPicker] [SerializeReference]
        public BaseObject templateInstance;

        private static List<Type> InheritorTypes =>
            StaticHelpers.GetEnumerableOfType<BaseObject>().Select(x => x.GetType()).ToList();

        public Type SelectedInheritor => InheritorTypes.Find(type => type.FullName == selectedInheritorType);

        private IEnumerable GetInheritorTypes()
        {
            return InheritorTypes.Select(type => type.FullName);
        }

        private void CreateTemplateInstance()
        {
            if (SelectedInheritor != null) templateInstance = (BaseObject)Activator.CreateInstance(SelectedInheritor);
        }

        public bool IsProperType(Type type)
        {
            return type == templateInstance.GetType();
        }

        public BaseObject InstantiateObject()
        {
            if (templateInstance == null) return null;

            var newInstance = SerializationUtility.CreateCopy(templateInstance) as BaseObject;
            return newInstance;
        }
    }
}