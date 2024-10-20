﻿using System;
using Configs;

namespace Actors
{
    public abstract class Actor
    {
        public event Action<GameActionData, Actor> OnExecute = delegate {};
        public event Action<Actor> OnDeath = delegate {  };
        
        protected ActorsController _actorsController;
        
        protected ActorConfig _config;

        protected int _actionIdx = 0;
        protected int _hp = 0;
        protected int _defence = 0;

        public void Init(ActorsController actorsController)
        {
            _actorsController = actorsController;
        } 
        
        public void Set(ActorConfig config)
        {
            _config = config;
            _hp = _config.hp;
            _actionIdx = 0;
        }

        public void ExecuteAction()
        {
            GameActionData currentAction = _config.gameActionLoop[_actionIdx];
            Actor target = GetTarget(currentAction.config.targetType);
            currentAction.config.gameAction.Execute(this, target, currentAction.power);
            OnExecute?.Invoke(currentAction, target);

            _actionIdx = (_actionIdx + 1) % _config.gameActionLoop.Count;
        }

        public void UpdateHp(int modifier)
        {
            if (modifier < 0 && _defence > 0)
            {
                modifier += _defence;
                _defence = modifier > 0 ? modifier : 0;
                modifier = modifier > 0 ? 0 : modifier;
            }
            
            _hp = _hp + modifier > _config.hp ? _config.hp : _hp + modifier; 

            if (IsDead())
            {
                OnDeath?.Invoke(this);
            }
        }

        public int GetCurrentActionIdx()
        {
            return _actionIdx;
        }

        public void AddDefence(int modifier)
        {
            _defence += modifier;
        }
        
        public int GetHp()
        {
            return _hp;
        }

        public int GetDefence()
        {
            return _defence;
        }
        
        public ActorConfig GetConfig()
        {
            return _config;
        } 
        
        public bool IsDead()
        {
            return _hp <= 0;
        }
        
        protected abstract Actor GetTarget(TargetType targetType);
    }
}