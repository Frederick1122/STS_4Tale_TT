using System;
using Game;
using Game.Cards;

namespace UI.BattleUI.CardUI
{
    public class CardUIController : UIController
    {
        public event Action<int> OnTrySelect = delegate {  };

        private ICardLoopFacade _cardLoop;
        
        private ICardFacade _card;
        private readonly CardUIModel _model = new();

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();    
        }

        public override void Init()
        {
            _cardLoop = Battle.Instance.GetCardLoopFacade();

            GetView<CardUIView>().OnTrySelect += HandleTrySelect;
            _cardLoop.OnHandChanged += HandleHandChanged;
            _cardLoop.OnCurrentCardChanged += HandleSelect;

            base.Init();
        }

        private void OnDestroy()
        {
            CardUIView view = GetView<CardUIView>();
            if (view != null)
            {
                view.OnTrySelect -= HandleTrySelect;
            }

            if (_cardLoop != null)
            {
                _cardLoop.OnHandChanged -= HandleHandChanged;
                _cardLoop.OnCurrentCardChanged -= HandleSelect;
            }
        }

        public void Set(ICardFacade cardFacade)
        {
            _card = cardFacade;
            UpdateModel();
            UpdateView();
        }
        
        protected override UIModel GetViewData()
        {
            return _model;
        }

        private void HandleTrySelect()
        {
            OnTrySelect?.Invoke(_card.GetIdx());
            UpdateModel();
            UpdateView();
        }

        private void HandleSelect(ICardFacade card)
        {
            if (_card == null)
            {
                return;
            }
            
            UpdateModel();
            UpdateView();
        }

        private void HandleHandChanged(int idx)
        {
            if (_card.GetIdx() == idx)
            {
                Hide();
                return;
            }
            
            UpdateModel();
            UpdateView();
        }

        private void UpdateModel()
        {
            _model.card = _card;
            _model.remainingEnergy = _cardLoop.GetHand().GetRemainingEnergy();
            _model.currentCardIdx = _cardLoop.GetCurrentCard()?.GetIdx() ?? -1;
        }
    }
}