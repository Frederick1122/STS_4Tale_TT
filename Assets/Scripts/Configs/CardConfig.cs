using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "Configs/Card Config")]

    public class CardConfig : BaseConfig
    {
        public int cost;
        public TargetType targetType;
        public List<GameActionData> actions = new();
    }
}