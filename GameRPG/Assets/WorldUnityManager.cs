using UnityEngine;
namespace TV
{
    public class WorldUnityManager : MonoBehaviour
    {
       public static WorldUnityManager instance;

        [Header("Layers")]
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask environmentLayers;
        private void Awake()
         {
             if (instance == null)
              {
                    instance = this;
              }
             else
              {
                    Destroy(gameObject);
              }
          }
        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }
        public LayerMask GetEnvironmentLayers()
        {
            return environmentLayers;
        }

    }
}
