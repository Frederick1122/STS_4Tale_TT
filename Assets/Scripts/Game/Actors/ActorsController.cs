using System;
using System.Collections.Generic;
using Configs;
using Libraries;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Actors
{
    public interface IActorsControllerFacade
    {
        public List<Actor> GetEnemies();
        public Actor GetPlayer();
    }
    
    public class ActorsController : IActorsControllerFacade
    {
        private const int ENEMIES_MAX_COUNT = 3;

        public event Action<Actor> OnDeath = delegate {  };
        
        private ActorConfig _playerConfig;
        
        private Player _player;
        private List<Actor> _enemies = new();
        
        public void Init(ActorConfig playerConfig)
        {
            _playerConfig = playerConfig;
            
            for (int i = 0; i < ENEMIES_MAX_COUNT; i++)
            {
                Enemy newEnemy = new Enemy();
                newEnemy.Init(this);
                _enemies.Add(newEnemy);
            }
            
            _player = new Player();
            _player.Init(this);
            _player.Set(_playerConfig);
            Subscribe();
        }

        public void Terminate()
        {
            Unsubscribe();
        }

        public void GenerateRandomNumberOfEnemies()
        {
            GenerateNewEnemies((uint) Random.Range(0, ENEMIES_MAX_COUNT));
        }
        
        public void GenerateNewEnemies(uint quantity = 1)
        {
            Assert.IsTrue(quantity <= ENEMIES_MAX_COUNT, $"ActorsController cannot set {quantity} enemies. Max - {ENEMIES_MAX_COUNT}");
            
            for (int i = 0; i < ENEMIES_MAX_COUNT; i++)
            {
                if (_enemies[i].IsDead())
                {
                    GenerateNewEnemy(i);
                    quantity--;

                    if (quantity == 0)
                    {
                        return;
                    }
                }
            }
        }

        public void TurnEnemies()
        {
            foreach (Actor enemy in _enemies)
            {
                if (enemy.IsDead())
                {
                    continue;
                }

                enemy.ExecuteAction();
            }
        }

        public List<Actor> GetEnemies()
        {
            return _enemies;
        }

        public Actor GetPlayer()
        {
            return _player;
        }
        
        private void GenerateNewEnemy(int idx)
        {
            _enemies[idx].Set(EnemyLibrary.Instance.GetRandomConfig());
        }

        private void Subscribe()
        {
            Unsubscribe();
            _player.OnDeath += HandleDeath;

            foreach (var enemy in _enemies)
            {
                enemy.OnDeath += HandleDeath;
            }
        }
        
        private void Unsubscribe()
        {
            _player.OnDeath -= HandleDeath;

            foreach (var enemy in _enemies)
            {
                enemy.OnDeath -= HandleDeath;
            }
        }

        private void HandleDeath(Actor actor)
        {
            OnDeath?.Invoke(actor);
        }
    }
}