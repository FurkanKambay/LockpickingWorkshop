using Game.Helpers;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        public float MovementSpeed = 1f;
        public float LookSensitivityX = 1f;
        public float LookSensitivityY = 1f;

        private new Transform camera;
        private CharacterController characterController;

        private void Awake()
        {
            Debug.Assert(Camera.main != null, "Camera.main is null");
            camera = Camera.main.transform;
            characterController = GetComponent<CharacterController>();
        }

        private void OnEnable() => Cursor.lockState = CursorLockMode.Locked;
        private void OnDisable() => Cursor.lockState = CursorLockMode.None;

        private void Update()
        {
            Move();
            Look();
        }

        private void Move()
        {
            float speed = MovementSpeed ;
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Transform self = transform;
            Vector3 movement = (self.right * x) + (self.forward * y);
            characterController.SimpleMove(movement.normalized * speed);
        }

        private void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY;
            transform.Rotate(Vector3.up * mouseX);

            float targetEuler = camera.eulerAngles.x - mouseY;
            if (!(targetEuler > 90 && targetEuler < 270))
                camera.eulerAngles = camera.eulerAngles.With(x: targetEuler > 90 ? targetEuler - 360 : targetEuler);

            // camera.Rotate(Vector3.left * mouseY);
        }
    }
}
