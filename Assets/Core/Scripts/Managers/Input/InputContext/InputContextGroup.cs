using System;
using UnityEngine;

namespace Nocci.Input
{
    [CreateAssetMenu(menuName = "Terraknown/Input/Input Context Group", fileName = "(InputContextGroup) ")]
    public sealed class InputContextGroup : InputContextBase
    {
        [SerializeField /*, ScriptablesList*/] private InputContext[] m_Subcontexts = Array.Empty<InputContext>();


        internal override void EnableContext()
        {
            foreach (var subContext in m_Subcontexts)
                subContext.EnableContext();

            base.EnableContext();
        }

        internal override void DisableContext(InputContextBase newContext)
        {
            foreach (var subContext in m_Subcontexts)
                if (!newContext.ContainsSubContext(subContext))
                    subContext.DisableContext(newContext);

            base.DisableContext(newContext);
        }

        internal override InputContextBase[] GetSubContexts()
        {
            return m_Subcontexts;
        }
    }
}