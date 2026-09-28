using UnityEngine;
using UnityEngine.SceneManagement;

namespace MicrogameCourse.Framework
{
    public sealed class SceneNavigator : MonoBehaviour
    {
        private const string MenuScene = "MainMenu";
        private const string PracticeScene = "PracticeTap";
        private bool isLoading;

        public void OpenPractice()
        {
            Load(PracticeScene);
        }

        public void OpenMenu()
        {
            Load(MenuScene);
        }

        public void Replay()
        {
            Load(SceneManager.GetActiveScene().name);
        }

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