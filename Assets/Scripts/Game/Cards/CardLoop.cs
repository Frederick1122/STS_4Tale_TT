using System.Collections.Generic;
using Configs;

namespace Game.Cards
{
    
    public interface ICardLoopFacade
    {
        public IHandFacade GetHand();
        public int GetRemainingCardsInDeck();
        public int GetRemainingCardsInDump();
    }
    
    public class CardLoop : ICardLoopFacade
    {
        private const int CARD_COUNT_IN_HAND = 5;
        
        private Deck _deck = new();
        private Dump _dump = new();
        private Hand _hand = new();
        
        public void Init(List<CardStack> cardStacks)
        {
            SetDeck(cardStacks);
            
            _hand.OnExecute += ExecuteCardHandler;
        }

        public void Terminate()
        {
            _hand.OnExecute -= ExecuteCardHandler;
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
        }

        public void Refresh()
        {
            _deck.Refresh();
            _dump.Clear();
        }
        
        public IHandFacade GetHand()
        {
            return _hand;
        }

        public int GetRemainingCardsInDeck()
        {
            return _deck.GetQuantity();
        }
        
        public int GetRemainingCardsInDump()
        {
            return _dump.GetQuantity();
        }

        private void ExecuteCardHandler(int idx)
        {
            Card card = _hand.Remove((uint) idx);
            _dump.Add(card);
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