﻿using UnityEngine;

namespace Game.SceneEntryPoints
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        public void Process()
        {
            InitializeManagers();
            InitializeUI();
            InitializeGameObjects();
            StartScene();
        }

        public abstract void Unload(); 
        
        protected abstract void InitializeManagers();

        protected abstract void InitializeUI();
        protected abstract void InitializeGameObjects();
        
        protected abstract void StartScene();
    }
}