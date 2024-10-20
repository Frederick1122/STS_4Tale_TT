using UnityEngine;

namespace Visualizers
{
    public abstract class BaseVisualizer : MonoBehaviour
    {
        public abstract void Init();
        
        public abstract void Terminate();
        
        public abstract void Show();
        
        public abstract void Hide();
        
        public abstract void UpdateVisualize();
    }
}