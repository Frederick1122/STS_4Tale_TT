using System.Collections.Generic;
using Actors;
using Configs;
using Game;
using Game.Cards;
using UnityEngine;

namespace Visualizers
{
    public class ActorsVisualizer : BaseVisualizer
    {
        [SerializeField] private ActorVisualizer _playerVisualizer;
        [SerializeField] private List<ActorVisualizer> _enemyVisualizers;

        private IActorsControllerFacade _actorsController;
        private ICardLoopFacade _cardLoop;

        private ICardFacade _currentCard;
        
        public override void Init()
        {
            _actorsController = Battle.Instance.GetActorsControllerFacade();
            _cardLoop = Battle.Instance.GetCardLoopFacade();
            
            _playerVisualizer.Init();
            
            foreach (ActorVisualizer enemyVisualizer in _enemyVisualizers)
            {
                enemyVisualizer.Init();
            }
            
            Subscribe();
        }

        public override void Terminate()
        {
            Unsubscribe();
        }

        public override void Show()
        {
            _playerVisualizer.Show();
            
            foreach (ActorVisualizer enemyVisualizer in _enemyVisualizers)
            {
                enemyVisualizer.Show();
            }
        }

        public override void Hide()
        {
            _playerVisualizer.Hide();
            
            foreach (ActorVisualizer enemyVisualizer in _enemyVisualizers)
            {
                enemyVisualizer.Hide();
            }
        }

        public override void UpdateVisualize()
        {
            _playerVisualizer.UpdateVisualize();
            
            foreach (ActorVisualizer enemyVisualizer in _enemyVisualizers)
            {
                enemyVisualizer.UpdateVisualize();
            }
        }

        private void Subscribe()
        {
            Unsubscribe();
            _actorsController.OnChangeActors += HandleChangeActors;
            _cardLoop.OnCurrentCardChanged += HandleSelectCard;
            
            _playerVisualizer.OnTarget += HandleSelectTarget;
            
            foreach (ActorVisualizer enemyVisualizer in _enemyVisualizers)
            {
                enemyVisualizer.OnTarget += HandleSelectTarget;
            }
        }
        
        private void Unsubscribe()
        {
            _actorsController.OnChangeActors -= HandleChangeActors;
            _cardLoop.OnCurrentCardChanged -= HandleSelectCard;
            
            _playerVisualizer.OnTarget -= HandleSelectTarget;
            
            foreach (ActorVisualizer enemyVisualizer in _enemyVisualizers)
            {
                enemyVisualizer.OnTarget -= HandleSelectTarget;
            }
        }

        private void HandleSelectTarget(Actor target)
        {
            _cardLoop.TrySelectTarget(target);
        } 
        
        private void HandleSelectCard(ICardFacade card)
        {
            _playerVisualizer.SetTargetPossibility(card != null && card.GetConfig().targetType == TargetType.Self);

            foreach (ActorVisualizer enemyVisualizer in _enemyVisualizers)
            {
                enemyVisualizer.SetTargetPossibility(card != null && card.GetConfig().targetType == TargetType.Other);
            }
        }
        
        private void HandleChangeActors()
        {
            Debug.Log("HandleChangeActors");
            _playerVisualizer.Set(_actorsController.GetPlayer());

            List<Actor> enemyActors = _actorsController.GetEnemies();
            for (int i = 0; i < _enemyVisualizers.Count; i++)
            {
                ActorVisualizer enemyVisualizer = _enemyVisualizers[i];
                enemyVisualizer.Set(enemyActors[i]);
            }
        }
    }
}