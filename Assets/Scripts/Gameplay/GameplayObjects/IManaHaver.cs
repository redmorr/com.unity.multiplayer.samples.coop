using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.GameplayObjects
{
    public interface IManaHaver
    {
        void ReceiveMana(ServerCharacter inflicter, int manaPointsChange);
        
        bool IsViable();
        
        Transform transform { get; }
    }
}
