using System;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.Netcode;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.GameplayObjects
{
    public class ManaReceiver : NetworkBehaviour, IManaHaver
    {
        public event Action<ServerCharacter, int> ManaReceived;
        
        [SerializeField]
        NetworkLifeState m_NetworkLifeState;

        public void ReceiveMana(ServerCharacter inflicter, int manaPoints)
        {
            if (IsViable())
            {
                ManaReceived?.Invoke(inflicter, manaPoints);
            }
        }
        
        public bool IsViable()
        {
            return m_NetworkLifeState.LifeState.Value == LifeState.Alive;
        }
    }
}
