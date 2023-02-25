using System.Collections;
using AttackPatterns;
using DataModels;
using Managers;
using Player;
using UI;
using UnityEngine;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
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
                    ActivateDeathVfx();
                    EventManager.SendEvent(Constants.EnemyDied);
                    PlayerScores.AddScore(1);
                }
            }
        }
    
        public void Init(Transform bulletsParent)
        {
            CalculateStats();
            attackPattern.Init(bulletsParent, shipData.AttackInterval, _currentAttackPower, shipData.BulletSpeed);
            StartMoving();
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

        private void ActivateDeathVfx()      
        {
            attackPattern.StopAllCoroutines();
            StopAllCoroutines();
            shipView.SetActive(false);
            deathVfx.SetActive(true);
            Destroy(gameObject, 1.5f);
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
    }
}
