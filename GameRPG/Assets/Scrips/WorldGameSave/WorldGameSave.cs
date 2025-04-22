using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TV
{
    public class WorldGameSave : MonoBehaviour
    {
        public static WorldGameSave instance;
        [SerializeField] int worldSceneIndex = 1;
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
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public IEnumerator loadNewGame()
        {

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }


    }
}