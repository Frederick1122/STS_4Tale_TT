using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ActorConfig", menuName = "Configs/Actor Config")]
    public class ActorConfig : BaseConfig
    {
        public ActorType actorType;
        public int hp;
        public List<GameActionData> gameActionLoop = new();
    }


    public enum ActorType
    {
        Player, 
        Enemy
    }
}
