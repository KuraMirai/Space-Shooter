using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerShipMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 2f;

        private float horizontal;
        private float vertical;
        private Vector3 _movementVector;

        private void FixedUpdate()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            _movementVector = new Vector3(horizontal, vertical, 0);
        
            if(!PauseManager.IsPause)
                rb.velocity = _movementVector.normalized * speed;
            else
                rb.velocity = Vector3.zero;
        
            CheckPlayerInScreenBounds();
        }

        private void CheckPlayerInScreenBounds()
        {
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, Screen.height));
            Vector3 playerPos = transform.position;
            playerPos.x = Mathf.Clamp(playerPos.x, -screenBounds.x, screenBounds.x);
            playerPos.y = Mathf.Clamp(playerPos.y, -screenBounds.y, screenBounds.y);
            transform.position = playerPos;
        }
    }
}
