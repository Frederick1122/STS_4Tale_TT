using Extensions;
using UnityEngine;

namespace Game.SceneEntryPoints
{
    public class BattleEntryPoint : SceneEntryPoint
    {
        private Battle _battle;
        
        protected override void InitializeManagers()
        {
            _battle = CreateNewGameObject<Battle>();
            _battle.Create();
            _battle.Init();
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