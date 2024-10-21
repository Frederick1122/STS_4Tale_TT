using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.EndUI
{
    public class EndUIView : UIView
    {
        public event Action OnRestart = delegate {  };
        
        [SerializeField] private TMP_Text _endText;
        [SerializeField] private Button _restartButton;

        public override void Init(UIModel uiModel)
        {
            base.Init(uiModel);
            _restartButton.onClick.AddListener(OnRestart.Invoke);
        }

        public override void Terminate()
        {
            _restartButton.onClick.RemoveAllListeners();
            base.Terminate();
        }

        public override void UpdateView(UIModel uiModel)
        {
            EndUIModel castData = (EndUIModel) uiModel;

            _endText.text = castData.endText;
        }
    }

    public class EndUIModel : UIModel
    {
        public string endText;
    }
}