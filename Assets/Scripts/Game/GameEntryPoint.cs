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

        private SceneEntryPoint _currentSceneEntryPoint;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StartGame()
        {
            _instance = CreateNewGameObject<GameEntryPoint>();
            _instance.Init();
        }

        private void Init()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            EnemyLibrary enemyLibrary = CreateNewGameObject<EnemyLibrary>();
            enemyLibrary.Create();

            ScenesService scenesService = CreateNewGameObject<ScenesService>();
            scenesService.OnPreUnload += HandleScenePreUnloaded;
            scenesService.Create();
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name;
            
            if (sceneName == ScenesService.SCENE_BATTLE)
            {
                StartBattle();
            }
        }

        private void HandleScenePreUnloaded()
        {
            if (_currentSceneEntryPoint == null)
            {
                return;
            }
            
            _currentSceneEntryPoint.Unload();
        }
        
        private void StartBattle()
        {
            BattleEntryPoint entryPoint = FindObjectOfType<BattleEntryPoint>();
            _currentSceneEntryPoint = entryPoint;
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