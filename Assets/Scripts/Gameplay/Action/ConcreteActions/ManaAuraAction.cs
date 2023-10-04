using Unity.BossRoom.Gameplay.Actions;
using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.BossRoom.VisualEffects;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Mage Mana Aura")]
    public partial class ManaAuraAction : Action
    {
        private Collider[] m_colliders = new Collider[4];
        private float m_Interval;

        public override bool OnStart(ServerCharacter serverCharacter)
        {
            Debug.Log("OnStart");
            m_Interval = 0f;
            serverCharacter.serverAnimationHandler.NetworkAnimator.SetTrigger(Config.Anim);
            serverCharacter.clientCharacter.RecvDoActionClientRPC(Data);
            return true;
        }

        public override bool OnUpdate(ServerCharacter clientCharacter)
        {
            if (TimeRunning > Config.ExecTimeSeconds)
            {
                if (TimeRunning <= Config.ExecTimeSeconds + Config.EffectDurationSeconds)
                {
                    m_Interval -= Time.deltaTime;
                    if (m_Interval <= 0)
                    {
                        m_Interval = 1f;
                        RegenerateManaInRadius(clientCharacter);
                    }
                }
                else
                {
                    return ActionConclusion.Stop;
                }
            }

            return ActionConclusion.Continue;
        }

        private void RegenerateManaInRadius(ServerCharacter parent)
        {
            int numResults = Physics.OverlapSphereNonAlloc(parent.physicsWrapper.DamageCollider.transform.position, Config.Radius, m_colliders,
                LayerMask.GetMask("PCs"));
            
            for (var i = 0; i < numResults; i++)
            {
                IManaHaver player = m_colliders[i].GetComponent<IManaHaver>();
                if (player != null && player.IsViable())
                {
                    player.ReceiveMana(parent, Config.Amount);
                }
            }
        }

        public override void Reset()
        {
            Debug.Log("Reset");
            base.Reset();
            m_AuraGraphic = null;
        }
    }
}
