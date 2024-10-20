using System;
using Actors;
using GameActions;

namespace Game.GameActions
{
    [Serializable]
    public class DefenceAction : GameAction
    {
        public override void Execute(Actor owner, Actor target, int power)
        {
            target.AddDefence(power);
            base.Execute(owner, target, power);
        }
    }
}