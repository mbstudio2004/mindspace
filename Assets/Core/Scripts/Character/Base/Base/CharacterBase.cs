using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Nocci.Character
{
    public class CharacterBase : MonoBehaviour, ICharacter
    {
        private static readonly List<ICharacterModule> s_CachedModules = new(32);

        [SerializeField] [Required] [Tooltip("The view transform, you can think of it as the eyes of the character")]
        private Transform m_View;

        private Dictionary<Type, ICharacterModule> m_ModulesByType;


        protected virtual void Awake()
        {
            SetupModules();
            SetupBaseReferences();
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            gameObject.layer = LayerMask.NameToLayer("Character");
        }
#endif

        protected virtual void Start()
        {
            IsInitialized = true;
            Initialized?.Invoke();
        }

        public bool IsInitialized { get; private set; }

        public Transform ViewTransform => m_View;
        public Collider[] Colliders { get; private set; }
        //   public IAudioPlayer AudioPlayer { get; private set; }

        //     public IInventory Inventory { get; private set; }
        //    public IHealthManager HealthManager { get; private set; }

        /// <summary>
        ///     This message will be sent after all modules are created and initialized.
        /// </summary>
        public event UnityAction Initialized;


        /// <summary>
        ///     <para> Returns child module of specified type from this character. </para>
        ///     Use this if you are NOT sure this character has a module of the given type.
        /// </summary>
        public bool TryGetModule<T>(out T module) where T : class, ICharacterModule
        {
            if (m_ModulesByType != null && m_ModulesByType.TryGetValue(typeof(T), out var charModule))
            {
                module = (T)charModule;
                return true;
            }

            module = default;
            return false;
        }

        /// <summary>
        ///     <para> Returns child module of specified type from this character. </para>
        ///     Use this if you ARE sure this character has a module of the given type.
        /// </summary>
        public void GetModule<T>(out T module) where T : class, ICharacterModule
        {
            if (m_ModulesByType != null && m_ModulesByType.TryGetValue(typeof(T), out var charModule))
            {
                module = (T)charModule;
                return;
            }

            module = default;
        }

        /// <summary>
        ///     <para> Returns child module of specified type from this character. </para>
        ///     Use this if you ARE sure this character has a module of the given type.
        /// </summary>
        public T GetModule<T>() where T : class, ICharacterModule
        {
            if (m_ModulesByType != null && m_ModulesByType.TryGetValue(typeof(T), out var charModule))
                return (T)charModule;

            return default;
        }

        /// <summary>
        ///     Returns true if the passed collider is part of this character.
        /// </summary>
        public bool HasCollider(Collider collider)
        {
            for (var i = 0; i < Colliders.Length; i++)
                if (Colliders[i] == collider)
                    return true;

            return false;
        }

        protected virtual void SetupBaseReferences()
        {
            //        AudioPlayer = GetModule<IAudioPlayer>();

            //        Inventory = GetModule<IInventory>();
            //HealthManager = GetModule<IHealthManager>();

            Colliders = GetComponentsInChildren<Collider>(true);
        }

        public override string ToString()
        {
            return $"{name} ({GetType().Name})";
        }

        private void SetupModules()
        {
            GetComponentsInChildren(s_CachedModules);
            for (var i = 0; i < s_CachedModules.Count; i++)
            {
                var module = s_CachedModules[i];
                var interfaces = module.GetType().GetInterfaces();
                foreach (var interfaceType in interfaces)
                {
                    if (interfaceType.GetInterface(nameof(ICharacterModule)) == null) continue;
                    m_ModulesByType ??= new Dictionary<Type, ICharacterModule>();
                    m_ModulesByType.TryAdd(interfaceType, module);
                }
            }
        }
    }
}