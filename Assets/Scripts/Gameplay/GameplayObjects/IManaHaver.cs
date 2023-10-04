using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.GameplayObjects
{
    public interface IManaHaver
    {
        void ReceiveMana(ServerCharacter inflicter, int manaPointsChange);
        
        bool IsViable();
        
        // TODO: check if required, if shared between multiple interfaces should it be inherited?
        // ulong NetworkObjectId { get; }
        Transform transform { get; }
    }
}
