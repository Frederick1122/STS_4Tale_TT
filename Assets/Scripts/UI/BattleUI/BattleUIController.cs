using System.Collections.Generic;
using Game;
using Game.Cards;
using UI.BattleUI.CardUI;
using UI.BattleUI.EndUI;
using UnityEngine;

namespace UI.BattleUI
{
    public class BattleUIController : UIController
    {
        private const string WIN_TEXT = "win";
        private const string DEFEAT_TEXT = "defeat";
        
        [SerializeField] private EndUIController _endUIController;
        [SerializeField] private List<CardUIController> _cardUIControllers;

        private ICardLoopFacade _cardLoop;

        private readonly BattleUIModel _battleUIModel = new();
        
        public override void Init()
        {
            _cardLoop = Battle.Instance.GetCardLoopFacade();
            Battle.Instance.OnNextTurn += HandleNextTurn;
            Battle.Instance.OnEndGame += HandleEndGame;
            
            GetView<BattleUIView>().OnEndTurn += HandleEndTurn;

            foreach (CardUIController cardUIController in _cardUIControllers)
            {
                cardUIController.Init();
                cardUIController.OnTrySelect += HandleCardTrySelect;
            }
            
            _endUIController.Init();
            _endUIController.Hide();
            
            base.Init();
        }

        public override void Terminate()
        {
            Battle.Instance.OnNextTurn -= HandleNextTurn;
            Battle.Instance.OnEndGame -= HandleEndGame;

            GetView<BattleUIView>().OnEndTurn -= HandleEndTurn;

            foreach (CardUIController cardUIController in _cardUIControllers)
            {
                cardUIController.OnTrySelect -= HandleCardTrySelect;
            }
            
            base.Terminate();
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

        private void HandleEndGame(bool isWin)
        {
            _endUIController.SetText(isWin ? WIN_TEXT : DEFEAT_TEXT);
            _endUIController.Show();
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