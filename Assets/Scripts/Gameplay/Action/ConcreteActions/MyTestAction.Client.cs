using Unity.BossRoom.Gameplay.Actions;
using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.BossRoom.VisualEffects;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    public partial class MyTestAction
    {
        SpecialFXGraphic m_AuraGraphics;

        public bool OnUpdateClient2(ClientCharacter clientCharacter)
        {
            if (TimeRunning >= Config.ExecTimeSeconds && m_AuraGraphics == null)
            {
                m_AuraGraphics = InstantiateSpecialFXGraphic(Config.Spawns[0], clientCharacter.transform, true);
            }

            // if (TimeRunning > Config.ExecTimeSeconds && TimeRunning <= Config.ExecTimeSeconds + Config.EffectDurationSeconds)
            // {
            //     if (m_Interval <= 0)
            //     {
            //         for (int i = 0; i < hits; i++)
            //         {
            //             InstantiateSpecialFXGraphic(Config.Spawns[1], m_colliders[i].transform, true);
            //         }
            //     }
            // }

            //Debug.LogFormat("TimeRunning:{0} ExecTimeSeconds:{1} EffectDurationSeconds:{2}", TimeRunning, Config.ExecTimeSeconds, Config.EffectDurationSeconds);
            return TimeRunning <= Config.ExecTimeSeconds + Config.EffectDurationSeconds;
        }

        public override bool OnUpdateClient(ClientCharacter clientCharacter)
        {
            if (TimeRunning >= Config.ExecTimeSeconds && m_AuraGraphics == null)
            {
                m_AuraGraphics = InstantiateSpecialFXGraphic(Config.Spawns[0], clientCharacter.transform, true);
            }
            
            if (TimeRunning > Config.ExecTimeSeconds)
            {
                if (TimeRunning <= Config.ExecTimeSeconds + Config.EffectDurationSeconds)
                {
                    m_Interval -= Time.deltaTime;
                    if (m_Interval <= 0)
                    {
                        m_Interval = 1f;
                        PerformAoE2(clientCharacter);
                    }
                }
                else
                {
                    return ActionConclusion.Stop;
                }
            }

            return ActionConclusion.Continue;
        }
        
        private void PerformAoE2(ClientCharacter parent)
        {
            int hits = Physics.OverlapSphereNonAlloc(parent.transform.position, Config.Radius, m_colliders,
                LayerMask.GetMask("PCs"));
            for (var i = 0; i < hits; i++)
            {
                //if (colliders[i].gameObject == parent.gameObject) continue; // Ignore self ?
                IManaHaver friend = m_colliders[i].GetComponent<IManaHaver>();
                if (friend != null && friend.IsViable())
                {
                    InstantiateSpecialFXGraphic(Config.Spawns[1], friend.transform, true);
                }
            }
        }

        public override bool OnStartClient(ClientCharacter clientCharacter)
        {
            Debug.Log("OnStartClient");
            base.OnStartClient(clientCharacter);
            return true;
        }

        public override void CancelClient(ClientCharacter clientCharacter)
        {
            Debug.Log("CancelClient");
            if (m_AuraGraphics)
            {
                m_AuraGraphics.Shutdown();
            }
        }
    }
}
