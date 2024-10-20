using Configs;

namespace Actors
{
    public class Enemy : Actor
    {
        protected override Actor GetTarget(TargetType targetType)
        {
            if (targetType == TargetType.Self)
            {
                return this;
            }

            return _actorsController.GetPlayer();
        }
    }
}