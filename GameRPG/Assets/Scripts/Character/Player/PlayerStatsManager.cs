using UnityEngine;
namespace TV
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

        }
        protected override void Start()
        {
            base.Start();
            CaculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CaculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        }
    }
}
