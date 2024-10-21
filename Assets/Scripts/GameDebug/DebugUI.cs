using System.Collections.Generic;
using Actors;
using Configs;
using Game;
using Game.Cards;
using UnityEngine;

namespace GameDebug
{
    public class DebugUI : MonoBehaviour
    {
        private static readonly Rect TEXT_SIZE = new(10,10,150,100);
        private static readonly Rect BUTTON_SIZE = new(850, 10, 250, 120);

        private static readonly int TEXT_STEP = 25;
        private static readonly int BUTTON_STEP = 140;
        
        [SerializeField] private bool _isTextEnabled;
        [SerializeField] private bool _isButtonsEnabled; 
        
        private ICardLoopFacade _cardLoop;
        private IActorsControllerFacade _actorsController;

        private IHandFacade _hand;
        private ICardFacade _currentCardConfig;

        private bool _isWin;
        private bool _isDefeat;

        private void Start()
        {
            _cardLoop = Battle.Instance.GetCardLoopFacade();
            _actorsController = Battle.Instance.GetActorsControllerFacade();
            _hand = _cardLoop.GetHand();

            Battle.Instance.OnEndGame += (isWin) =>
            {
                if (isWin)
                {
                    _isWin = true;
                }
                else
                {
                    _isDefeat = true;
                }
            };
        }

        void OnGUI()
        {
           UpdateButtons();
           UpdateText();
        }

        private void UpdateButtons()
        {
            if (_isDefeat || _isWin || !_isButtonsEnabled)
            {
                return;
            }
            
            GUIStyle style = new(GUI.skin.button)
            {
                fontSize = 25
            };

            Rect buttonsRect = BUTTON_SIZE;
            if (GUI.Button (buttonsRect, "End turn", style)) {
                Battle.Instance.EndTurn();
            }

            UpdateRectPosition(ref buttonsRect, BUTTON_STEP);
            int remainingEnergy = _hand.GetRemainingEnergy();

            foreach (ICardFacade handCart in _hand.GetAll())
            {
                CardConfig cardConfig = handCart.GetConfig();

                GUI.enabled = cardConfig.cost <= remainingEnergy;

                string cardText = cardConfig.configName;
                cardText += "\n Actions:";
                
                foreach (GameActionData action in cardConfig.actions)
                {
                    cardText += $"{action.config.configName} {action.power} \n";
                }
                    
                cardText += $"Target : {cardConfig.targetType} \n";
                cardText += $"Cost : {cardConfig.cost}";
                
                if (GUI.Button (buttonsRect, cardText, style)) {
                    if (_currentCardConfig == null || _currentCardConfig != handCart)
                    {
                        _currentCardConfig = handCart;
                    }
                    else
                    {
                        _currentCardConfig = null;
                    }
                }

                GUI.enabled = true; 
                
                UpdateRectPosition(ref buttonsRect, BUTTON_STEP);
            }
            
            buttonsRect.y = 10;
            UpdateRectPosition(ref buttonsRect, 0, BUTTON_STEP * 2);

            if (_currentCardConfig == null)
                return;
            
            bool isCardUsed = false;
            if (_currentCardConfig.GetConfig().targetType == TargetType.Self)
            {
                if (GUI.Button (buttonsRect, "Use it on yourself", style)) 
                {
                    Actor player = _actorsController.GetPlayer();
                    _hand.TryExecute((uint)_currentCardConfig.GetIdx(), player, player);
                    isCardUsed = true;
                }   
            }
            else
            {
                List<Actor> enemies = _actorsController.GetEnemies();
                for (int i = 0; i < enemies.Count; i++)
                {
                    Actor enemy = enemies[i];
                    if (enemy == null || enemy.IsDead())
                        continue;
                        
                    if (GUI.Button(buttonsRect, $"({i}) {enemy.GetConfig().configName}", style))
                    {
                        Actor player = _actorsController.GetPlayer();
                        _hand.TryExecute((uint)_currentCardConfig.GetIdx(), player, enemy);
                        isCardUsed = true;
                    }
                    
                    UpdateRectPosition(ref buttonsRect, BUTTON_STEP);
                }
            }

            if (isCardUsed)
            {
                _currentCardConfig = null;
            }
        }

        private void UpdateText()
        {
            if (!_isTextEnabled)
            {
                return;
            }
            
            GUIStyle style = new()
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 25,
                normal =
                {
                    textColor = Color.white
                }
            };
            
            Rect textsRect = TEXT_SIZE;
            
            if (_isDefeat || _isWin)
            {
                string text = _isDefeat ? "You defeat" : "You win";
                GUI.Label(textsRect, text, style);
                return;
            }

            string cardInfo = "";
            cardInfo += $"Card in deck: {_cardLoop.GetRemainingCardsInDeck()} ";
            cardInfo += $"Card in dump: {_cardLoop.GetRemainingCardsInDump()}";
            GUI.Label(textsRect, cardInfo, style);

            UpdateRectPosition(ref textsRect, TEXT_STEP * 2);

            Actor player = _actorsController.GetPlayer();
            ActorConfig playerConfig = player.GetConfig(); 
            string playerInfo = $"{playerConfig.configName}: \n";
            playerInfo += $"Hp: {player.GetHp()} / {playerConfig.hp}\n";
            playerInfo += $"Defence: {player.GetDefence()}\n";
            playerInfo += $"Energy: {_hand.GetRemainingEnergy()} / {_hand.GetMaxEnergy()}\n";
            if (playerConfig.gameActionLoop.Count > 0)
            {
                playerInfo += $"Next action: {playerConfig.gameActionLoop[player.GetCurrentActionIdx()].config.configName} {playerConfig.gameActionLoop[player.GetCurrentActionIdx()].power}";
            }
            GUI.Label(textsRect, playerInfo, style);

            UpdateRectPosition(ref textsRect, TEXT_STEP * 5);

            foreach (Actor enemy in _actorsController.GetEnemies())
            {
                ActorConfig enemyConfig = enemy.GetConfig();
                string enemyInfo = "";
                
                if (enemyConfig == null)
                {
                    enemyInfo += "Enemy slot is empty";
                }
                else
                {
                    enemyInfo += $"{enemyConfig.configName}. ";
                    enemyInfo += $"Hp: {enemy.GetHp()}. ";
                    enemyInfo += $"Defence: {enemy.GetDefence()}. ";
                    enemyInfo += $"Next action: {enemyConfig.gameActionLoop[enemy.GetCurrentActionIdx()].config.configName} {enemyConfig.gameActionLoop[enemy.GetCurrentActionIdx()].power}";
                }
                
                GUI.Label(textsRect, enemyInfo, style);

                UpdateRectPosition(ref textsRect, TEXT_STEP);
            }

            UpdateRectPosition(ref textsRect, TEXT_STEP);
        }
        
        private void UpdateRectPosition(ref Rect rect, int yStep = 0, int xStep = 0)
        {
            rect.x += xStep;
            rect.y += yStep;
        }
    }
}