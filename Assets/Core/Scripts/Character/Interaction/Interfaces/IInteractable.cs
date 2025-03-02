using Nocci.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Nocci.Interaction
{
    public interface IInteractable
    {
        GameObject gameObject { get; }
        Transform transform { get; }

        bool InteractionEnabled { get; set; }
        float HoldDuration { get; }

        event UnityAction<ICharacter> Interacted;
        event UnityAction InteractionEnabledChanged;


        /// <summary>
        ///     Called when a character interacts with this object.
        /// </summary>
        void OnInteract(ICharacter character);
    }
}