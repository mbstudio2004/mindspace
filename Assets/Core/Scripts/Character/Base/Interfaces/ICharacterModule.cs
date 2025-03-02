using UnityEngine;

namespace Nocci.Character
{
    public interface ICharacterModule
    {
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }

        // ReSharper disable once InconsistentNaming
        Transform transform { get; }
    }
}