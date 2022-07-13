using Assets.PixelCrew.Components.UI.LevelsLoader;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
       [SerializeField] private string _sceneName;

       public void Exit()
       {
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
       }
    }
}


