using Configs;

namespace UI.Actor
{
    public class ActorUIController : UIController
    {
        private Actors.Actor _actor;
        private readonly ActorUIModel _model = new();
        
        public void Set(Actors.Actor actor)
        {
            _actor = actor;
            UpdateModel();
        }

        public void UpdateModel()
        {
            if (_actor == null)
            {
                return;
            }

            ActorConfig config = _actor.GetConfig();
            _model.name = config.configName;
            _model.maxHp = config.hp;
            _model.hp = _actor.GetHp();
            _model.defence = _actor.GetDefence();

            if (config.gameActionLoop.Count == 0)
            {
                _model.nextActionName = "";    
            }
            else
            {
                GameActionData actionData = config.gameActionLoop[_actor.GetCurrentActionIdx()];
                _model.nextActionName = actionData.config.configName;
                _model.nextActionPower = actionData.power;
            }
            
           
            UpdateView();
        }
        
        protected override UIModel GetViewData()
        {
            return _model;
        }
    }
}