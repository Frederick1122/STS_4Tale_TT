using System;
using System.Collections.Generic;
using Actors;
using Configs;
using Core;

namespace Game.Cards
{
    public interface ICardFacade
    {
        public CardConfig GetConfig();
        public int GetIdx();
    }
    
    public class Card :  ICardFacade, IPoolObject
    {
        public event Action<int> OnExecute = delegate {  };
        public event Action<int> OnPreExecute = delegate {  };
        
        private CardConfig _currentConfig;

        private int _idx;
        
        public void SetConfig(CardConfig config)
        {
            _currentConfig = config;
        }
        public CardConfig GetConfig()
        {
            return _currentConfig;
        }

        public void SetIdx(int idx)
        {
            _idx = idx;
        }
        
        public int GetIdx()
        {
            return _idx;
        }

        public void Execute(Actor owner, List<Actor> targets)
        {
            foreach (var actionConfig in _currentConfig.actions)
            {
                actionConfig.config.gameAction.Execute(owner, targets, actionConfig.power);
            }

            OnPreExecute?.Invoke(_idx);
            OnExecute?.Invoke(_idx);
        }
    }
}