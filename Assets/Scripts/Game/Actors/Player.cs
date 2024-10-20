using Configs;

namespace Actors
{
    public class Player : Actor
    {
        protected override Actor GetTarget(TargetType targetType)
        {
            if (targetType == TargetType.Self)
            {
                return this;
            }

            return _actorsController.GetEnemies()[1];
        }
    }
}