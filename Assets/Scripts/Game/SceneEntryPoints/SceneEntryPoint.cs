using UnityEngine;

namespace Game.SceneEntryPoints
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        public void Process()
        {
            InitializeManagers();
            StartScene();
        }

        protected abstract void InitializeManagers();
        protected abstract void StartScene();
    }
}