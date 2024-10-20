using System;
using System.Collections.Generic;
using Actors;
using UnityEngine;

namespace GameActions
{
    [Serializable]
    public abstract class GameAction : MonoBehaviour
    {
        public void Execute(Actor owner, List<Actor> targets, int power)
        {
            foreach (Actor t in targets)
            {
                Execute(owner, t, power);
            }
        }

        public virtual void Execute(Actor owner, Actor target, int power)
        {
            Debug.Log($"{owner.GetConfig().configName} make {GetType()} to {target.GetConfig().configName} with a power of {power}");
        }
    }
}