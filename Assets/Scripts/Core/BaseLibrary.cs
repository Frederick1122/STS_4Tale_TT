using System.Collections.Generic;
using System.Linq;
using Configs;
using UnityEngine;

namespace Core
{
    public class BaseLibrary<T> : Singleton<BaseLibrary<T>> where T : BaseConfig
    {
        protected List<string> _paths = new();
        private Dictionary<string, T> _allConfigs = new();

        protected override void Awake()
        {
            base.Awake();

            List<object> allConfigs = new();

            foreach (string path in _paths)
            {
                allConfigs.AddRange(Resources.LoadAll(path));
            }

            foreach (object config in allConfigs)
            {
                T castConfig = (T) config;
                if (_allConfigs.ContainsKey(castConfig.configKey))
                {
                    Debug.LogAssertion(
                        $"{castConfig.configKey} is duplicated. Check {castConfig.name} and {_allConfigs[castConfig.configKey].name}");
                    return;
                }

                _allConfigs.Add(castConfig.configKey, castConfig);
            }
        }

        public T GetConfig(string configKey)
        {
            T config = _allConfigs[configKey];

            if (config == null)
                Debug.LogAssertion($"CarLibrary not founded config with this key: {configKey}");

            return config;
        }

        public T GetRandomConfig(string excludedKey = "")
        {
            if (!_allConfigs.ContainsKey(excludedKey))
                return _allConfigs.ElementAt(Random.Range(0, _allConfigs.Count)).Value;

            List<KeyValuePair<string, T>> allConfigsWithoutExcludedKey = _allConfigs.Where(p => p.Key != excludedKey).ToList();
            return allConfigsWithoutExcludedKey.ElementAt(Random.Range(0, allConfigsWithoutExcludedKey.Count)).Value;
        }

        public List<T> GetRandomsConfigs(int count, bool withoutDuplicate = false, string excludedKey = "")
        {
            List<T> randomConfigs = new List<T>();

            if (withoutDuplicate)
            {
                if (_allConfigs.Count < count)
                {
                    Debug.LogError(
                        $"{this} can't execute GetRandomConfig. all configs - {_allConfigs.Count}, need configs - {count}. Add new configs or change parameter 'without duplicates'");
                }

                List<KeyValuePair<string, T>> allConfigsWithoutExcludedKey = 
                    _allConfigs.Where(p => p.Key != excludedKey).ToList();

                if (allConfigsWithoutExcludedKey.Count < count)
                {
                    Debug.LogError(
                        $"{this} can't execute GetRandomConfig. all configs - {_allConfigs.Count}, need configs - {count}. Add new configs or change parameter 'without duplicates'");
                }

                if (allConfigsWithoutExcludedKey.Count == count)
                {
                    randomConfigs.AddRange(_allConfigs.Values);
                }
                else
                {
                    Dictionary<string, T> allConfigCopy = _allConfigs;
                    while (randomConfigs.Count < count)
                    {
                        KeyValuePair<string, T> randomConfig =
                            allConfigsWithoutExcludedKey.ElementAt(Random.Range(0, allConfigsWithoutExcludedKey.Count));
                        randomConfigs.Add(randomConfig.Value);
                        allConfigCopy.Remove(randomConfig.Key);
                    }
                }
            }
            else
                while (randomConfigs.Count < count)
                    randomConfigs.Add(GetRandomConfig());

            return randomConfigs;
        }

        public virtual IReadOnlyList<T> GetAllConfigs()
        {
            return _allConfigs.Values.ToList();
        }
    }
}