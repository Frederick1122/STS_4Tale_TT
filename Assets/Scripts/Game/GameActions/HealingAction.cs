using System;
using Actors;

namespace GameActions
{
    [Serializable]
    public class HealingAction : GameAction
    {
        public override void Execute(Actor owner, Actor target, int power)
        {
            target.UpdateHp(power);
            base.Execute(owner, target, power);
        }
    }
}