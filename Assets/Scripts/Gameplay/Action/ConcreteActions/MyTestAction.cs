using Unity.BossRoom.Gameplay.Actions;
using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.BossRoom.VisualEffects;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/My Test Action")]
    public partial class MyTestAction : Action
    {
        private float m_Interval;
        private Collider[] m_colliders = new Collider[4];

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
                        PerformAoE(clientCharacter);
                    }
                }
                else
                {
                    return ActionConclusion.Stop;
                }
            }

            return ActionConclusion.Continue;
        }

        private void PerformAoE(ServerCharacter parent)
        {
            int hits = Physics.OverlapSphereNonAlloc(parent.transform.position, Config.Radius, m_colliders,
                LayerMask.GetMask("PCs"));
            for (var i = 0; i < hits; i++)
            {
                //if (colliders[i].gameObject == parent.gameObject) continue; // Ignore self ?
                IManaHaver friend = m_colliders[i].GetComponent<IManaHaver>();
                if (friend != null && friend.IsViable())
                {
                    //InstantiateSpecialFXGraphic(Config.Spawns[1], friend.transform, true);
                    friend.ReceiveMana(parent, Config.Amount);
                }
            }
        }

        public override void Reset()
        {
            Debug.Log("Reset");
            base.Reset();
            m_AuraGraphics = null;
        }
    }
}
