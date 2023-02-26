using AttackPatterns;
using DataModels;
using Enemy;
using Managers;
using PoolFactories;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerShip : MonoBehaviour
    {
        [SerializeField] private AttackPattern attackPattern;
        [SerializeField] private BulletsPoolFactory bulletsParent;
        [SerializeField] private Healthbar healthBar;
        [SerializeField] private GameObject deathVfx;
        [SerializeField] private GameObject playerView;
        [SerializeField] private ShipData shipData;

        private float _currentHP;

        public float CurrentHP
        {
            get => _currentHP;
            set
            {
                _currentHP = value;
                if (_currentHP <= 0)
                {
                    ActivateDeathVfx(true);
                    EventManager.SendEvent(Constants.PlayerDiedEvent);
                }
            }
        }

        public void Init()
        {
            _currentHP = shipData.InitialHP;
            healthBar.Init(_currentHP);
        }

        private void Awake()
        {
            attackPattern.Init(bulletsParent, shipData.AttackInterval, shipData.InitialAttackPower, shipData.BulletSpeed, true);
            attackPattern.SetAttackDirection(Vector3.up);
            attackPattern.Attack();
            EventManager.StartListening(Constants.StartGame, Revive);
            EventManager.StartListening(Constants.RevivePlayer, Revive);
        }

        private void ActivateDeathVfx(bool activate)                           
        {
            healthBar.gameObject.SetActive(!activate);
            deathVfx.SetActive(activate);
            playerView.SetActive(!activate);
        }

        public void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            healthBar.TakeDamage(damage);
            EventManager.SendEvent(Constants.PlayerHit);
        }
    
        private void Revive()
        {
            Init();
            ActivateDeathVfx(false);
        }
    
        private void OnTriggerEnter(Collider collision)
        {
            GetCollidedObject(collision);
        }
    
        private void GetCollidedObject(Collider collidedObj)
        {
            if (collidedObj.CompareTag(Constants.EnemyTag))
            {
                collidedObj.GetComponentInParent<EnemyAI>().Kill();
                TakeDamage(75);
            }
        }
    }
}
