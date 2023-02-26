using System;
using System.Collections;
using AttackPatterns;
using DataModels;
using Managers;
using Player;
using PoolFactories;
using PoolFactories.Interfaces;
using UI;
using UnityEngine;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour, IPullable
    {
        [SerializeField] private AttackPattern attackPattern;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Healthbar healthBar;
        [SerializeField] private GameObject deathVfx;
        [SerializeField] private GameObject shipView;
        [SerializeField] private Transform shipAssetTransform;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float turnSpeed = 5;
        [SerializeField] private float attackTime = 5;
        [SerializeField] private ShipData shipData;

        private float _currentHP;
        private float _currentAttackSpd;
        private float _currentAttackPower;
        private Coroutine _moveRoutine;
        
        public event Action<EnemyAI> Pushed; 


        #region Constants
        private const float HealthCoeff = 25f;
        private const float AttackPowerCoeff = 0.5f;
        #endregion
    
        private float CurrentHP
        {
            get => _currentHP;
            set
            {
                _currentHP = value;
                if (_currentHP <= 0)
                {
                    EventManager.SendEvent(Constants.EnemyDied);
                    PlayerScores.AddScore(1);
                    Push();
                }
            }
        }
    
        public void Init(BulletsPoolFactory bulletsPoolFactory)
        {
            SetActive(true);
            CalculateStats();
            attackPattern.Init(bulletsPoolFactory, shipData.AttackInterval, _currentAttackPower, shipData.BulletSpeed);
            StartMoving();
            ActivateVfx(true);
        }
    
        
        private void CalculateStats()
        {
            _currentHP = shipData.InitialHP + (PlayerScores.ExpansionLevel * HealthCoeff);
            _currentAttackPower = shipData.InitialAttackPower + (PlayerScores.ExpansionLevel * AttackPowerCoeff);
            healthBar.Init(_currentHP);
        }

        private void StartMoving()
        {
            if (_moveRoutine == null)
            {
                Vector3 screenBounds = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, Screen.height));
                Vector3 destinationPosition = new Vector3(UnityEngine.Random.Range(-screenBounds.x, screenBounds.x), UnityEngine.Random.Range(screenBounds.y / 2, screenBounds.y));
                _moveRoutine = StartCoroutine(MovingRoutine(destinationPosition));
            }
        }

        private void StopMoving()
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
                _moveRoutine = null;
                StartMoving();
            }
        }

        private IEnumerator MovingRoutine(Vector3 destinationPosition)
        {
            Vector3 direction = (destinationPosition - transform.position).normalized;
            while ((destinationPosition - transform.position).magnitude >= 0.1f)
            {
                if (!PauseManager.IsPause)
                    rb.velocity = direction * movementSpeed;
                else
                    rb.velocity = Vector3.zero;
                yield return null;
            }
            rb.velocity = Vector3.zero;
            attackPattern.SetAttackDirection(Vector3.down);
            attackPattern.Attack();
            yield return new WaitForSeconds(attackTime);
            attackPattern.StopAttacking();
            StopMoving();
        }


        private void FixedUpdate()
        {
            HandleRotation();
        }

        private void HandleRotation()
        {
            if (rb.velocity.x != 0)
            {
                shipAssetTransform.rotation *= Quaternion.Euler(new Vector3(0, -rb.velocity.x, 0) * turnSpeed);
                Vector3 rot = shipAssetTransform.rotation.eulerAngles;
                rot.y = rot.y > 180 ? rot.y - 360 : rot.y;
                rot.y = Mathf.Clamp(rot.y, -45, 45);
                shipAssetTransform.rotation = Quaternion.Euler(rot);
            }
            else
            {
                shipAssetTransform.rotation = Quaternion.Slerp(shipAssetTransform.rotation, Quaternion.Euler(0, 0, 0), turnSpeed * Time.deltaTime);
            }
        }

        private void ActivateVfx(bool activate)      
        {
            shipView.SetActive(activate);
            deathVfx.SetActive(!activate);
        }

        private void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        
        public void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            healthBar.TakeDamage(damage);
        }

        public void Kill()
        {
            CurrentHP = 0;
        }

        public void Push()
        {
            Pushed?.Invoke(this);
            Pushed = null;
            ActivateVfx(false);
            attackPattern.StopAttacking();
            StopMoving();
        }
    }
}
