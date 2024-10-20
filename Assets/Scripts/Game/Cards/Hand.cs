using System;
using System.Collections.Generic;
using System.Linq;
using Actors;
using UnityEngine;

namespace Game.Cards
{
    public interface IHandFacade
    {
        public int GetRemainingEnergy();
        
        public int GetMaxEnergy();
        
        public List<ICardFacade> GetAll();
        
        public void TryExecute(uint idx, Actor owner, Actor target);
        
        public void TryExecute(uint idx, Actor owner, List<Actor> targets);
    }
    
    public class Hand : IHandFacade
    {
        private const int PLAYER_ENERGY_MAX = 3;

        public event Action<int> OnPreExecute = delegate {  }; 
        public event Action<int> OnExecute = delegate {  }; 

        private List<Card> _cards;
        private int _energy;

        public void Set(List<Card> cards)
        {
            _cards = cards;

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].SetIdx(i);
            }
            
            Subscribe();
            _energy = PLAYER_ENERGY_MAX;
        }

        
        public void TryExecute(uint idx, Actor owner, Actor target)
        {
            List<Actor> targets = new() {target};
            TryExecute(idx, owner, targets);
        }
        
        public void TryExecute(uint idx, Actor owner, List<Actor> targets)
        {
            Card card = TryGet(idx);
            if (card == null || _energy < card.GetConfig().cost) 
            {
                return;            
            }

            _energy -= card.GetConfig().cost;
            card.Execute(owner, targets);
        }

        public int GetRemainingEnergy()
        {
            return _energy;
        }

        public int GetMaxEnergy()
        {
            return PLAYER_ENERGY_MAX;
        }

        public List<ICardFacade> GetAll()
        {
            return _cards.Cast<ICardFacade>().ToList();
        }
        
        public ICardFacade GetCard(uint idx)
        {
            return TryGet(idx);
        }

        public Card Remove(uint idx)
        {
            Card removeCard = TryGet(idx);
            removeCard.OnExecute -= OnExecute;
            removeCard.OnPreExecute -= OnPreExecute;
            _cards.Remove(removeCard);
            return removeCard;
        }

        public List<Card> RemoveAll()
        {
            var removeCards = _cards.ToList(); 
            _cards.Clear(); 
            return removeCards;
        }

        private Card TryGet(uint idx)
        {
            foreach (Card card in _cards)
            {
                if (card.GetIdx() == idx)
                    return card;
            }
            
            Debug.LogError($"Hand cannot find card with idx: {idx}");
            return null;
        }
        
        private void Subscribe()
        {
            Unsubscribe();
            foreach (Card card in _cards)
            {
                card.OnExecute += OnExecute;
                card.OnPreExecute += OnPreExecute;
            }
        }

        private void Unsubscribe()
        {
            foreach (Card card in _cards)
            {
                card.OnExecute -= OnExecute;
                card.OnPreExecute -= OnPreExecute;
            }
        }
    }
}