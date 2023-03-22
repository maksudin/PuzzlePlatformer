using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.PixelCrew.Components.UI.LevelsLoader
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _transitionTime;
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad() => InitLoader();

        private void Awake() => DontDestroyOnLoad(gameObject);

        private static void InitLoader() =>
            SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive);

        public void LoadLevel(string sceneName) =>
            StartCoroutine(StartAnimation(sceneName));

        private IEnumerator StartAnimation(string sceneName)
        {
            _animator.SetBool(Enabled, true);
            SceneManager.LoadScene(sceneName);
            yield return new WaitForSeconds(_transitionTime);
            _animator.SetBool(Enabled, false);
        }
    }
}