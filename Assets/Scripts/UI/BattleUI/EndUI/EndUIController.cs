using Services;

namespace UI.BattleUI.EndUI
{
    public class EndUIController : UIController
    {
        private EndUIModel _model = new();

        public override void Init()
        {
            GetView<EndUIView>().OnRestart += HandleRestartGame;
            base.Init();
        }

        public override void Terminate()
        {
            GetView<EndUIView>().OnRestart -= HandleRestartGame;
            base.Terminate();
        }

        public void SetText(string endText)
        {
            _model.endText = endText;
            UpdateView();
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }

        private void HandleRestartGame()
        {
            ScenesService.Instance.StartBattleScene();
        }
    }
}