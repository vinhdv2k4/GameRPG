using UnityEngine;
namespace TV
{
    [System.Serializable]
    public class CharacterSaveData 
    {
        [Header("Character Name")]
        public string characterName= "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;


    }
}
