using Extensions;
using UI.BattleUI;
using UnityEngine;
using Visualizers;

namespace Game.SceneEntryPoints
{
    public class BattleEntryPoint : SceneEntryPoint
    {
        [SerializeField] private ActorsVisualizer _actorsVisualizer;
        [SerializeField] private BattleUIController _battleUIController;
        
        private Battle _battle;
        
        protected override void InitializeManagers()
        {
            _battle = CreateNewGameObject<Battle>();
            _battle.Create();
            _battle.Init();
        }

        protected override void InitializeUI()
        {
            _battleUIController.Init();
        }

        protected override void InitializeGameObjects()
        {
            _actorsVisualizer.Init();
        }

        protected override void StartScene()
        {
            _battle.StartBattle();
        }

        private T CreateNewGameObject<T>() where T : MonoBehaviour
        {
            T newComponent = GameObjectExtensions.CreateNewGameObject<T>();
            newComponent.gameObject.transform.parent = gameObject.transform;
            return newComponent;
        }
    }
}