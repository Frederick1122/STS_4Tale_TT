using System;
using Actors;
using Configs;
using UI.Actor;
using UnityEngine;

namespace Visualizers
{
    public class ActorVisualizer : BaseVisualizer
    {
        public event Action<Actor> OnTarget = delegate {  };
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _targetMarker;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private ActorUIController _actorUIController;
        
        private Actor _actor;

        private bool _isPossibleTarget;
        
        public override void Init()
        {
            _actorUIController.Init();
            SetTargetPossibility(false);
        }

        public override void Terminate()
        {
            
        }
        
        public override void Show()
        {
            if (_actor == null)
            {
                return;
            }
            
            _spriteRenderer.enabled = true;
            UpdateVisualize();
            _actorUIController.Show();
        }
        
        public override void Hide()
        {
            _actor = null;
            _spriteRenderer.enabled = false;
            _actorUIController.Hide();
        }

        public override void UpdateVisualize()
        {
            if (_actor == null)
            {
                return;
            }
            
            ActorConfig config = _actor.GetConfig();
            _spriteRenderer.sprite = config.sprite;
            _actorUIController.UpdateModel();
        }

        public void SetTargetPossibility(bool isPossibleTarget)
        {
            _isPossibleTarget = isPossibleTarget;
            _targetMarker.enabled = _isPossibleTarget && _actor != null && !_actor.IsDead();
        }
        
        public void Set(Actor actor)
        {
            if (actor.GetConfig() == null || actor.IsDead())
            {
                Hide();
                return;
            }
            
            Debug.Log($"Set Actor visualizer {actor.GetConfig().configName}");

            if (_actor != null)
            {
                _actor.OnExecute -= HandleActorExecute;
                _actor.OnUpdate -= HandleActorUpdate;
                _actor.OnDeath -= HandleActorDeath;
            }
            
            _actor = actor;
            _actorUIController.Set(_actor);

            _actor.OnExecute += HandleActorExecute;
            _actor.OnUpdate += HandleActorUpdate;
            _actor.OnDeath += HandleActorDeath;

            Show();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider == _collider)
                {
                    TryToTarget();
                }
            }
        }

        private void TryToTarget()
        {
            if (!_isPossibleTarget || _actor.IsDead())
            {
                return;
            }
            
            OnTarget?.Invoke(_actor);
        }
        
        private void HandleActorExecute(GameActionData currentData, Actor target)
        {
            UpdateVisualize();
        }

        private void HandleActorUpdate()
        {
            UpdateVisualize();
        }

        private void HandleActorDeath(Actor actor)
        {
            if (actor.IsDead())
            {
                Hide();
            }
        }
    }
}