using System.Collections.Generic;
using UnityEngine;

namespace Nocci.Zayebunny.GameDB.Base
{
    [CreateAssetMenu(fileName = "ZB_Settings", menuName = "Zayebunny/Settings")]
    public class GameDBSettings : ScriptableObject
    {
        [SerializeReference] public List<BaseModule> Modules = new();


        public List<BaseModule> GetModulesFlat()
        {
            var queue = new Queue<BaseModule>(Modules);
            var modules = new List<BaseModule>();

            while (queue.Count > 0)
            {
                var module = queue.Dequeue();
                if (module == null) continue;
                modules.Add(module);
                foreach (var child in module.SubModules) queue.Enqueue(child);
            }


            return modules;
        }
    }
}