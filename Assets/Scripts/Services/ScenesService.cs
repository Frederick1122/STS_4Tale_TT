using System;
using Core;
using UnityEngine.SceneManagement;

namespace Services
{
    public class ScenesService : Singleton<ScenesService>
    {
        public const string SCENE_BATTLE = "Battle";

        public event Action OnPreUnload = delegate {  };

        public void StartBattleScene()
        {
            LoadScene(SCENE_BATTLE);
        }

        private void LoadScene(string sceneName)
        {
            OnPreUnload?.Invoke();
            SceneManager.LoadScene(sceneName);
        }
    }
}