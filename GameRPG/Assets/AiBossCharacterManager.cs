
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TV {
    public class AiBossCharacterManager : CharacterAIManager
    {
        public int bossID = 0;
        [SerializeField] bool hasBeenDefeated = false;
        [SerializeField] bool hasBeenAwakened = false;
        [SerializeField] List<FogWallInteractable> fogWalls;

        [Header("DEBug")]
        [SerializeField] bool wakeBossUp = false;

        protected override void Update()
        {
            base.Update();
            if (wakeBossUp)
            {
                wakeBossUp = false;
                WakeBoos();
            }
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                if (!WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldGameSaveManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                else
                {
                    hasBeenDefeated = WorldGameSaveManager.instance.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened = WorldGameSaveManager.instance.currentCharacterData.bossesAwakened[bossID];             
                }
                StartCoroutine(GetFogWWallsFromWorldObjectManager());
                if (hasBeenAwakened)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = true;
                    }
                }

                if (hasBeenDefeated)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = false;
                    }
                    aiCharacterNetworkManager.isActive.Value = false;
                }
                

            
            }
        }
        public IEnumerator GetFogWWallsFromWorldObjectManager()
        {
            while(WorldObjectManager.instance.fogWalls.Count ==0)
                yield return new WaitForEndOfFrame();
            fogWalls = new List<FogWallInteractable>();
            foreach (var fogWall in WorldObjectManager.instance.fogWalls)
            {
                if (fogWall.fogWallID == bossID)
                {
                    fogWalls.Add(fogWall);
                }
            }

        }
        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
                }
                hasBeenDefeated = true;
                if (!WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldGameSaveManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                else
                {
                    WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldGameSaveManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                    WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldGameSaveManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }

                WorldGameSaveManager.instance.SaveGame();
            }
            
            yield return new WaitForSeconds(5);
        }

        public void WakeBoos()
        {
            hasBeenAwakened = true;
            if (!WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }
            else
            {
                WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldGameSaveManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
             }
            for(int i =0; i< fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = true;
            }
        }
    }
}
