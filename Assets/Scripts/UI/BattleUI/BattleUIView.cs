using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI
{
    public class BattleUIView : UIView
    {
        private const string WAVE_TEXT = "Wave";
        private const string DECK_TEXT = "Card in deck";
        private const string DUMP_TEXT = "Card in dump";
        private const string ENERGY_TEXT = "Energy";
        
        public event Action OnEndTurn = delegate {  };
        
        [SerializeField] private Text _wave;
        [SerializeField] private Text _deck;
        [SerializeField] private Text _dump;
        [SerializeField] private Text _energy;

        [SerializeField] private Button _endTurnButton;

        public override void Init(UIModel uiModel)
        {
            _endTurnButton.onClick.AddListener(OnEndTurn.Invoke);
            base.Init(uiModel);
        }

        private void OnDestroy()
        {
            _endTurnButton?.onClick.RemoveAllListeners();
        }

        public override void UpdateView(UIModel uiModel)
        {
            BattleUIModel castData = (BattleUIModel) uiModel;

            _wave.text = $"{WAVE_TEXT} {castData.currentWave} / {castData.maxWave}";
            _deck.text = $"{DECK_TEXT} {castData.deckRemainingCards}";
            _dump.text = $"{DUMP_TEXT} {castData.dumpRemainingCards}";
            _energy.text = $"{ENERGY_TEXT} {castData.currentEnergy} / {castData.maxEnergy}";
        }
    }

    public class BattleUIModel : UIModel
    {
        public int maxWave;
        public int currentWave;

        public int deckRemainingCards;
        public int dumpRemainingCards;

        public int maxEnergy;
        public int currentEnergy;
    }
}