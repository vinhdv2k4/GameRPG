using UnityEngine;
namespace TV
{
    [System.Serializable]
    public class CharacterSaveData 
    {
        [Header("Scene Index")]
        public int sceneIndex=1;
        [Header("Character Name")]
        public string characterName= "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float curremtStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;

    }
}
