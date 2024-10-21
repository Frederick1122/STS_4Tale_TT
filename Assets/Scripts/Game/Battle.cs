using System;
using System.Collections.Generic;
using System.Linq;
using Actors;
using Core;
using Configs;
using Game.Cards;
using UnityEngine;

namespace Game
{
    public class Battle : Singleton<Battle>
    {
        private const int WAVE_COUNT = 3;
        
        private const string DECK_CONFIG = "Configs/DeckConfig";
        private const string PLAYER_CONFIG_PATH = "Configs/Actors/Player";

        public event Action<bool> OnEndGame = delegate {  };
        public event Action OnNextTurn = delegate {  };
        
        private readonly CardLoop _cardLoop = new();
        private readonly ActorsController _actorsController = new();
        
        private int _waveCounter = 0;
        
        public void Init()
        {
            DeckConfig deckConfig = Resources.Load<DeckConfig>(DECK_CONFIG);
            ActorConfig playerConfig = Resources.Load<ActorConfig>(PLAYER_CONFIG_PATH);
            
            _actorsController.Init(playerConfig);
            _actorsController.OnDeath += HandleDeath;
            
            _cardLoop.Init(deckConfig.cardStacks);
        }

        public void Terminate()
        {
            _actorsController.OnDeath -= HandleDeath;
            
            _cardLoop.Terminate();
            _actorsController.Terminate();
        }

        public void StartBattle()
        {
            _cardLoop.Refresh();
            _actorsController.GenerateRandomNumberOfEnemies();
            _waveCounter = 1;
            StartTurn();
        }

        public void StartTurn()
        {
            _cardLoop.StartTurn();
            OnNextTurn?.Invoke();
        }
        
        public void EndTurn()
        {
            _actorsController.TurnEnemies();
            _cardLoop.EndTurn();
            StartTurn();
        }

        public int GetWaveCount()
        {
            return _waveCounter;
        }

        public int GetMaxWaveCount()
        {
            return WAVE_COUNT;
        }
        
        public ICardLoopFacade GetCardLoopFacade()
        {
            return _cardLoop;
        }
        
        public IActorsControllerFacade GetActorsControllerFacade()
        {
            return _actorsController;
        }

        private void HandleDeath(Actor actor)
        {
            Debug.Log($"{actor.GetConfig().configName} is dead");
            
            if (actor.GetConfig().actorType == ActorType.Player)
            {
                OnEndGame?.Invoke(false);
            }
            else
            {
                List<Actor> enemies = _actorsController.GetEnemies();
                
                Actor livingEnemy = enemies.FirstOrDefault(enemy => !enemy.IsDead());
                if (livingEnemy != null)
                {
                    return;
                }

                if (_waveCounter < WAVE_COUNT)
                {
                    _waveCounter++;
                    _cardLoop.Refresh();
                    _actorsController.GenerateRandomNumberOfEnemies();
                    StartTurn();
                }
                else
                {
                    OnEndGame?.Invoke(true);
                }
            }
        }
    }
}