using System.Collections.Generic;
using Game;
using Game.Cards;
using UI.BattleUI.CardUI;
using UnityEngine;

namespace UI.BattleUI
{
    public class BattleUIController : UIController
    {
        [SerializeField] private List<CardUIController> _cardUIControllers;

        private ICardLoopFacade _cardLoop;

        private readonly BattleUIModel _battleUIModel = new();
        
        public override void Init()
        {
            _cardLoop = Battle.Instance.GetCardLoopFacade();
            Battle.Instance.OnNextTurn += HandleNextTurn;
            GetView<BattleUIView>().OnEndTurn += HandleEndTurn;

            foreach (CardUIController cardUIController in _cardUIControllers)
            {
                cardUIController.Init();
                cardUIController.OnTrySelect += HandleCardTrySelect;
            }
            
            base.Init();
        }

        private void OnDestroy()
        {
            if (Battle.Instance != null)
            {
                Battle.Instance.OnNextTurn -= HandleNextTurn;
            }

            BattleUIView view = GetView<BattleUIView>();

            if (view != null)
            {
                view.OnEndTurn -= HandleEndTurn;
            }
            
            foreach (CardUIController cardUIController in _cardUIControllers)
            {
                if (cardUIController != null)
                {
                    cardUIController.OnTrySelect += HandleCardTrySelect;
                }
            }
        }

        protected override UIModel GetViewData()
        {
            return _battleUIModel;
        }

        private void HandleNextTurn()
        {
            List<ICardFacade> cards = _cardLoop.GetHand().GetAll();

            for (int i = 0; i < _cardUIControllers.Count; i++)
            {
                _cardUIControllers[i].Set(cards[i]);
                _cardUIControllers[i].Show();
            }

            UpdateModel();
            UpdateView();
        }

        private void HandleEndTurn()
        {
            Battle.Instance.EndTurn();
            UpdateModel();
            UpdateView();
        }

        private void HandleCardTrySelect(int idx)
        {
            _cardLoop.TrySelectCard(idx);
        }
        
        private void UpdateModel()
        {
            _battleUIModel.maxWave = Battle.Instance.GetMaxWaveCount();
            _battleUIModel.currentWave = Battle.Instance.GetWaveCount();

            _battleUIModel.deckRemainingCards = _cardLoop.GetRemainingCardsInDeck();
            _battleUIModel.dumpRemainingCards = _cardLoop.GetRemainingCardsInDump();
            _battleUIModel.maxEnergy = _cardLoop.GetHand().GetMaxEnergy();
            _battleUIModel.currentEnergy = _cardLoop.GetHand().GetRemainingEnergy();
        }
    }
}