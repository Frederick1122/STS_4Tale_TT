using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static T CreateNewGameObject<T>() where T : MonoBehaviour
        {
            GameObject newGameObject = new(typeof(T).Name);
            T newComponent = newGameObject.AddComponent<T>();
            return newComponent;
        }
    }
}