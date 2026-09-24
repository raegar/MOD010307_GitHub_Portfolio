using UnityEngine;
using UnityEngine.SceneManagement;

namespace MicrogameCourse.Framework
{
    /// <summary>Loads one complete scene at a time. Add each scene to Build Profiles.</summary>
    public sealed class SceneNavigator : MonoBehaviour
    {
        private const string MenuScene = "MainMenu";
        private const string PracticeScene = "PracticeTap";
        private bool isLoading;

        public void OpenPractice() => Load(PracticeScene);
        public void OpenMenu() => Load(MenuScene);
        public void Replay() => Load(SceneManager.GetActiveScene().name);

        private void Load(string sceneName)
        {
            if (isLoading) return;

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError($"Add {sceneName} to File > Build Profiles > Scene List.");
                return;
            }

            isLoading = true;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
    }
}
