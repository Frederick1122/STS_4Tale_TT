using System;
using GameActions;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameActionConfig", menuName = "Configs/Game Action Config")]
    public class GameActionConfig : BaseConfig
    {
        public TargetType targetType;
        public GameAction gameAction;
    }

    [Serializable]
    public class GameActionData
    {
        public GameActionConfig config;
        public int power;
    }

    public enum TargetType
    {
        Self,
        Other
    }
}