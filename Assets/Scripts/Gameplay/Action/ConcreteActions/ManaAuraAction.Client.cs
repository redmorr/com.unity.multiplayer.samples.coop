using Unity.BossRoom.Gameplay.Actions;
using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.BossRoom.VisualEffects;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    public partial class ManaAuraAction
    {
        private SpecialFXGraphic m_AuraGraphic;

        public override bool OnUpdateClient(ClientCharacter clientCharacter)
        {
            if (TimeRunning > Config.ExecTimeSeconds)
            {
                if (m_AuraGraphic == null)
                {
                    m_AuraGraphic = InstantiateSpecialFXGraphic(Config.Spawns[0], clientCharacter.transform, true);
                }
                
                if (TimeRunning <= Config.ExecTimeSeconds + Config.EffectDurationSeconds)
                {
                    m_Interval -= Time.deltaTime;
                    if (m_Interval <= 0)
                    {
                        m_Interval = 1f;
                        SpawnAuraEffectsInRadius(clientCharacter);
                    }
                }
                else
                {
                    return ActionConclusion.Stop;
                }
            }

            return ActionConclusion.Continue;
        }

        private void SpawnAuraEffectsInRadius(ClientCharacter parent)
        {
            int numResults = Physics.OverlapSphereNonAlloc(parent.transform.position, Config.Radius, m_colliders,
                LayerMask.GetMask("PCs"));
            for (int i = 0; i < numResults; i++)
            {
                IManaHaver player = m_colliders[i].GetComponent<IManaHaver>();
                if (player != null && player.IsViable())
                {
                    InstantiateSpecialFXGraphic(Config.Spawns[1], player.transform, true);
                }
            }
        }

        public override void CancelClient(ClientCharacter clientCharacter)
        {
            if (m_AuraGraphic)
            {
                m_AuraGraphic.Shutdown();
            }
        }
    }
}
