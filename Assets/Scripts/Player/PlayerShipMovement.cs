using Managers;
using UI.JoyStick;
using UnityEngine;

namespace Player
{
    public class PlayerShipMovement : MonoBehaviour
    {
        [SerializeField] private Joystick movementJoyStick;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform shipAssetTransform;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float turnSpeed = 5;
        [SerializeField] private bool useJoystickControls;

        private float horizontal;
        private float vertical;
        private Vector3 _movementVector;
        

        private void Awake()
        {
#if UNITY_ANDROID
            useJoystickControls = true;
#endif
            movementJoyStick.Horizontal += OnHorizontalMovement;
            movementJoyStick.Vertical += OnVerticalMovement;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleRotation();
            CheckPlayerInScreenBounds();
        }

        private void HandleMovement()
        {
            if (!useJoystickControls)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }

            _movementVector = new Vector3(horizontal, vertical, 0);
        
            if(!PauseManager.IsPause)
                rb.velocity = _movementVector.normalized * speed;
            else
                rb.velocity = Vector3.zero;
        }

        private void HandleRotation()
        {
            if (horizontal != 0)
            {
                shipAssetTransform.rotation *= Quaternion.Euler(new Vector3(0, 0, -horizontal) * turnSpeed);
                Vector3 rot = shipAssetTransform.rotation.eulerAngles;
                rot.y = rot.y > 180 ? rot.y - 360 : rot.y;
                rot.y = Mathf.Clamp(rot.y, -45, 45);
                shipAssetTransform.rotation = Quaternion.Euler(rot);
            }
            else
            {
                shipAssetTransform.rotation = Quaternion.Slerp(shipAssetTransform.rotation, Quaternion.Euler(-90, 0, 180), turnSpeed * Time.deltaTime);
            }
        }

        private void CheckPlayerInScreenBounds()
        {
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, Screen.height));
            Vector3 playerPos = transform.position;
            playerPos.x = Mathf.Clamp(playerPos.x, -screenBounds.x, screenBounds.x);
            playerPos.y = Mathf.Clamp(playerPos.y, -screenBounds.y, screenBounds.y);
            transform.position = playerPos;
        }
        
        private void OnHorizontalMovement(float input)
        {
            horizontal = input;
        }
        
        private void OnVerticalMovement(float input)
        {
            vertical = input;
        } 
    }
}
