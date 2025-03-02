using System;
using UnityEngine;

namespace Nocci.Input
{
    [CreateAssetMenu(menuName = "Terraknown/Input/Input Context", fileName = "(InputContext) ")]
    public sealed class InputContext : InputContextBase
    {
        internal override InputContextBase[] GetSubContexts()
        {
            return Array.Empty<InputContextBase>();
        }
    }
}