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

        public bool CanIDamageThisCharacter(ChacterGroup attackingCharacter, ChacterGroup targetCharacter)
        {
            if(attackingCharacter == ChacterGroup.Team01)
            {
                switch (targetCharacter)
                {
                    case ChacterGroup.Team01:  return false;               
                        
                    case ChacterGroup.Team02: return true;
                      
                    default:
                        break;
                }
            }
            else if(attackingCharacter == ChacterGroup.Team02)
            {
                switch (targetCharacter)
                {
                    case ChacterGroup.Team01: return true;
                       
                    case ChacterGroup.Team02: return false;
                     
                    default:
                        break;
                }
            }
            return false;
        }

        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

            if (cross.y < 0)
            {
                viewableAngle = -viewableAngle;
            }

            return viewableAngle;
        }

    }
}
