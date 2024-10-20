using System;
using Configs;
using Game.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.CardUI
{
    public class CardUIView : UIView
    {
        private const string COST_TEXT = "Cost";
        private const string ACTIONS_TEXT = "Actions";
        
        public event Action OnTrySelect = delegate {  };

        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _cost;

        [SerializeField] private Image _background;
        [SerializeField] private Color _openedColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _closedColor;

        [SerializeField] private Button _button;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            _button.onClick.AddListener(OnTrySelect.Invoke);
        }

        private void OnDestroy()
        {
            _button?.onClick.RemoveAllListeners();
        }

        public override void UpdateView(UIModel uiModel)
        {
            CardUIModel castData = (CardUIModel) uiModel;

            CardConfig cardConfig = castData.card.GetConfig();
            
            _title.text = cardConfig.configName;

            string description = $"{ACTIONS_TEXT}: \n";
            
            foreach (GameActionData action in cardConfig.actions)
            {
                description += $"{action.config.configName} {action.power} \n";
            }
                    
            description += $"Target : {cardConfig.targetType}";
            
            _description.text = description;
            _cost.text = $"{COST_TEXT} {cardConfig.cost}";

            if (cardConfig.cost > castData.remainingEnergy)
            {
                _background.color = _closedColor;
            }
            else if (castData.currentCardIdx == castData.card.GetIdx())
            {
                _background.color = _selectedColor;
            }
            else
            {
                _background.color = _openedColor;
            }
        }
    }

    public class CardUIModel : UIModel
    {
        public int remainingEnergy;
        public int currentCardIdx;
        public ICardFacade card;
    }
}