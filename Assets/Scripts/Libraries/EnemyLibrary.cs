using Core;
using Configs;

namespace Libraries
{
    public class EnemyLibrary : BaseLibrary<ActorConfig>
    {
        private const string CONFIG_PATH = "Configs/Actors/Enemies";

        protected override void Awake()
        {
            _paths.Add(CONFIG_PATH);
            base.Awake();
        }
    }
}