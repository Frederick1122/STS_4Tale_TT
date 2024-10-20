using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Actor
{
    public class ActorUIView : UIView
    {
        [SerializeField] private Text _name;
        
        [SerializeField] private Text _maxHp;
        [SerializeField] private Text _hp;
        
        [SerializeField] private Text _defence;

        [SerializeField] private Slider _healthBar;
        
        [SerializeField] private Text _nextAction;
        
        public override void UpdateView(UIModel uiModel)
        {
            base.UpdateView(uiModel);

            ActorUIModel castData = (ActorUIModel) uiModel;

            _name.text = castData.name;
            _maxHp.text = castData.maxHp.ToString();
            _hp.text = castData.hp.ToString();
            _defence.text = castData.defence.ToString();
            
            _nextAction.enabled = castData.nextActionName != ""; 
            if (castData.nextActionName != "")
            {
                _nextAction.text = $"{castData.nextActionName} {castData.nextActionPower}";
            }
            
            _healthBar.maxValue = castData.maxHp;
            _healthBar.value = castData.hp;
        }
    }

    public class ActorUIModel : UIModel
    {
        public string name;
        
        public int maxHp;
        public int hp;

        public int defence;

        public string nextActionName;
        public int nextActionPower;
    }
}