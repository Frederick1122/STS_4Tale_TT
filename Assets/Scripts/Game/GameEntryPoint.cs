using Extensions;
using Game.SceneEntryPoints;
using Libraries;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameEntryPoint : MonoBehaviour
    {
        private static GameEntryPoint _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StartGame()
        {
            _instance = CreateNewGameObject<GameEntryPoint>();
            _instance.Init();
        }

        private void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            
        //    LoadConfiguration();
            InitializeManagers();
            SetupUI();
        }

        // private void LoadConfiguration()
        // {
        //     
        // }

        private void InitializeManagers()
        {
            EnemyLibrary enemyLibrary = CreateNewGameObject<EnemyLibrary>();
            enemyLibrary.Create();
        }
        
        private void SetupUI()
        {
            
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var sceneName = scene.name;
            
            if (sceneName == ScenesService.SCENE_BATTLE)
            {
                StartBattle();
            }
        }

        private void StartBattle()
        {
            BattleEntryPoint entryPoint = FindObjectOfType<BattleEntryPoint>();
            entryPoint.Process();
        }

        private static T CreateNewGameObject<T>() where T : MonoBehaviour
        {
            T newComponent = GameObjectExtensions.CreateNewGameObject<T>();
            DontDestroyOnLoad(newComponent.gameObject);
            return newComponent;
        }
    }
}