using MyGames;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class NPCAnimator : MonoBehaviour
    {
        [SerializeField] private int numberOfWavingClips;
        [SerializeField] private int numberOfVibingClips;
        [SerializeField] private int numberOfDancingClips;

        private NPC npc;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            npc = GetComponent<NPC>();
        }

        private void Start()
        {
            npc.OnStateChanged += Npc_OnStateChanged;
            HandleStateChange(npc.GetState());
        }

        private void Npc_OnStateChanged(NPC.State npcState)
        {
            HandleStateChange(npcState);
        }

        private void HandleStateChange(NPC.State npcState)
        {
            animator.SetInteger(NPCAnimatorKeys.State, (int)npcState);
            UpdateSubState(npcState);
        }

        private void UpdateSubState(NPC.State npcState)
        {
            if(npcState == NPC.State.Waving)
            {
                int wavingStateIndex = (int)Utils.GetRandomValueBetween(0, numberOfWavingClips);
                animator.SetInteger(NPCAnimatorKeys.Wave_State, wavingStateIndex);
            }
            else if(npcState == NPC.State.Dancing)
            {
                int dancingStateIndex = (int)Utils.GetRandomValueBetween(0, numberOfDancingClips);
                animator.SetInteger(NPCAnimatorKeys.Dance_Emote_Index, dancingStateIndex);
            }
            else if(npcState == NPC.State.Vibing)
            {
                int vibingStateIndex = (int)Utils.GetRandomValueBetween(0, numberOfVibingClips);
                animator.SetInteger(NPCAnimatorKeys.Vibe_State, vibingStateIndex);
            }
        }

        private void OnDestroy()
        {
            npc.OnStateChanged -= Npc_OnStateChanged;
        }
    }
}
