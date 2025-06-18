using UnityEngine;
namespace TV
{
    public class Unity_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUntilDestroy = 1f;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroy);
        }
    }
}
