using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class SceneLoader : MonoBehaviour
    {
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}