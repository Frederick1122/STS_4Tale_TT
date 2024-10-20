using System;
using Actors;
using GameActions;

namespace Game.GameActions
{
    [Serializable]
    public class AttackAction : GameAction
    {
        public override void Execute(Actor owner, Actor target, int power)
        {
            target.UpdateHp(-power);
            base.Execute(owner, target, power);
        }
    }
}