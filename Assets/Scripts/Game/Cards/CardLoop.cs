using System;
using System.Collections.Generic;
using Actors;
using Configs;
using UnityEngine;

namespace Game.Cards
{
    public interface ICardLoopFacade
    {
        public event Action<int> OnHandChanged;
        
        public event Action<ICardFacade> OnCurrentCardChanged;
        
        public IHandFacade GetHand();
        public ICardFacade GetCurrentCard();
        
        public void TrySelectCard(int idx);
        
        public void TrySelectTarget(Actor target);
        
        public int GetRemainingCardsInDeck();
        
        public int GetRemainingCardsInDump();
    }
    
    public class CardLoop : ICardLoopFacade
    {
        public event Action<int> OnHandChanged = delegate {  };
        public event Action<ICardFacade> OnCurrentCardChanged = delegate {  }; 

        private const int CARD_COUNT_IN_HAND = 5;
        
        private readonly Deck _deck = new();
        private readonly Dump _dump = new();
        private readonly Hand _hand = new();

        private Actor _player;
        private ICardFacade _currentCard;
        
        public void Init(List<CardStack> cardStacks)
        {
            SetDeck(cardStacks);

            _player = Battle.Instance.GetActorsControllerFacade().GetPlayer();
            _hand.OnPreExecute += HandleExecuteCard;
        }

        public void Terminate()
        {
            _hand.OnPreExecute -= HandleExecuteCard;
        }

        public void StartTurn()
        {
            if (_deck.GetQuantity() < CARD_COUNT_IN_HAND)
            {
                Refresh();
            }
            
            _hand.Set(_deck.Get(CARD_COUNT_IN_HAND));
        }

        public void EndTurn()
        {
            _dump.Add(_hand.RemoveAll());
            ClearCurrentCard();
        }

        public void Refresh()
        {
            _deck.Refresh();
            _dump.Clear();
            ClearCurrentCard();
        }
        
        
        public void TrySelectCard(int idx)
        {
            Debug.Log($"Select new card {idx}");
            ICardFacade card = _hand.GetCard((uint) idx);
            if (_currentCard != null && _currentCard.GetIdx() == idx || 
                card.GetConfig().cost > _hand.GetRemainingEnergy())
            {
                _currentCard = null;
            }
            else
            {
                _currentCard = card;
            }
            
            OnCurrentCardChanged?.Invoke(_currentCard);
        }

        public void TrySelectTarget(Actor target)
        {
            Debug.Log($"Select new target {target.GetConfig().configName}");

            if (_currentCard == null)
            {
                return;
            }
            
            _hand.TryExecute((uint)_currentCard.GetIdx(), _player, target);
            _currentCard = null;
            OnCurrentCardChanged?.Invoke(_currentCard);
        }
        
        public IHandFacade GetHand()
        {
            return _hand;
        }
        
        public ICardFacade GetCurrentCard()
        {
            return _currentCard;
        }
        
        public int GetRemainingCardsInDeck()
        {
            return _deck.GetQuantity();
        }
        
        public int GetRemainingCardsInDump()
        {
            return _dump.GetQuantity();
        }

        private void ClearCurrentCard()
        {
            _currentCard = null;
            OnCurrentCardChanged?.Invoke(_currentCard);
        }
        
        private void HandleExecuteCard(int idx)
        {
            Card card = _hand.Remove((uint) idx);
            _dump.Add(card);
            OnHandChanged?.Invoke(idx);
        }

        private void SetDeck(List<CardStack> cardStacks)
        {
            List<Card> cards = new();
            foreach (CardStack deckCard in cardStacks)
            {
                for (int i = 0; i < deckCard.count; i++)
                {
                    Card newCard = new();
                    newCard.SetConfig(deckCard.card);
                    cards.Add(newCard);
                }
            }
            
            _deck.Init(cards);
        }
    }
}