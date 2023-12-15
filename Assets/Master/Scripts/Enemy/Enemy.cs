using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Master.Scripts.Common;
using Master.Scripts.SO;
using PlayerComponent = Master.Scripts.Player.Player;

namespace Master.Scripts.Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        public static Action<Enemy> OnAwake;
        
        [Header("Pattern")]
        [SerializeField] private List<CommandSO> _commands;

        [Header("Statistics")]
        [SerializeField] private float _initialMaxHP;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private int _power;

        // Properties
        
        public PlayerComponent PlayerReference { get; private set; }
        public bool HasCollidedWithPlayer { get; private set; }

        public float MaxHealth { get; private set; }
        public float Health { get; set; }
        public float Speed => _speed;
        public int Power => _power;
        
        // Individual events

        public Action<DmgType, Enemy> OnHit;
        public Action<int> OnAttack;
        public Action<Enemy> OnKill;
        
        // Other fields
        
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public Dictionary<(CommandSO, string), object> Memory = new();
        private CommandSO _currentCommand;
        private int _currentCommandIndex;
        
        // Methods
        
        private void Awake()
        {
            PlayerReference = GameObject.Find("Player").GetComponent<PlayerComponent>();
            MaxHealth = _initialMaxHP;
            Health = MaxHealth;
            
            OnAwake.Invoke(this);
        }

        private void Update() {
            // Control to check if commands list is empty
            if (_commands.Count == 0)
                throw new Exception("Enemy " + name + " commands is empty");
            // If no command then fetch first command
            if (!_currentCommand) {
                _currentCommand = _commands[0];
                _currentCommand.Setup(this);
            }
            // If current command is finished then get the next
            if (_currentCommand.IsFinished()) {
                _currentCommand.CleanUp();
                _currentCommandIndex++;
                // If index is higher than the number of elements
                if (_currentCommandIndex >= _commands.Count) 
                    _currentCommandIndex = 0;
                // Fetch the next command
                _currentCommand = _commands[_currentCommandIndex];
                _currentCommand.Setup(this);
            }
            _currentCommand.Execute();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($">> Enemy {gameObject.name} triggered with tagged object : {other.tag}");
            switch (other.tag)
            {
                case "Player":
                    HandlePlayerState(other.gameObject);
                    break;
                
                case "Projectile":
                    OnHit.Invoke(DmgType.Projectile, this);
                    break;
            }
        }
        
        // Utils //

        private void HandlePlayerState(GameObject ctx)
        {
            PlayerComponent player = ctx.GetComponent<PlayerComponent>();

            if (player.IsDashing) {
                OnHit.Invoke(DmgType.Dash, this);
            }
            else {
                OnAttack.Invoke(this.Power);
            }
        }
    }
}
