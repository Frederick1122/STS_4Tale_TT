using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "DeckConfig", menuName = "Configs/Deck Config")]

    public class DeckConfig : BaseConfig
    {
        public List<CardStack> cardStacks;
    }

    [Serializable]
    public class CardStack
    {
        public CardConfig card;
        public int count;
    }
}