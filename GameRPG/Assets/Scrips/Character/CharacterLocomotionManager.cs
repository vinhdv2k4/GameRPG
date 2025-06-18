using UnityEngine;
namespace TV { 

public class CharaterLocomotionManager : MonoBehaviour
{
        CharacterManager character;

        [Header("Ground Check And Jump")]
        [SerializeField] protected Vector3 yVelocity; //juming and falling
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckRadius = 1;
        [SerializeField] protected float gravity = -5.5f;
        [SerializeField] protected float groundVelocity = -20;
        [SerializeField] protected float fallStartVelocity = -5;

        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;


        protected virtual void Awake()
    {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Update()
        {
            HandleGroundCheck();
            if (character.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundVelocity;
                }
            }
            else
            {
                if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartVelocity;
                }
                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);
                yVelocity.y += gravity*Time.deltaTime;
             

            }
            character.characterController.Move(yVelocity * Time.deltaTime);

        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position,groundCheckRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(character.transform.position, groundCheckRadius);
        }
    }
}
