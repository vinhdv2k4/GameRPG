using TV;
using Unity.Netcode;
using UnityEngine;
namespace TV
{
    public class NetworkObjectSpawner : MonoBehaviour
    {

        [Header("Object")]
        [SerializeField] GameObject networkGameObject;
        [SerializeField] GameObject instantiateGameObject;

        private void Awake()
        {

        }
        private void Start()
        {
            WorldObjectManager.instance.SpawnObject(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnObjects()
        {
            if (networkGameObject != null)
            {
                instantiateGameObject = Instantiate(networkGameObject);
                instantiateGameObject.transform.position = transform.position;
                instantiateGameObject.transform.rotation = transform.rotation;
                instantiateGameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
