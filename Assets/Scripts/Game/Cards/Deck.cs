using System.Collections.Generic;
using Core;

namespace Game.Cards
{
    public class Deck : Pool<Card>
    {
        public void Init(List<Card> cards)
        {
            Add(cards);
        }

        public void Refresh()
        {
            _quantity = _PoolObjects.Count;    
            Extensions.ListExtensions.Shuffle(_PoolObjects);
        }
    }
}