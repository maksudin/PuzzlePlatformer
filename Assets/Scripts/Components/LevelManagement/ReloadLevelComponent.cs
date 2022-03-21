using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}

