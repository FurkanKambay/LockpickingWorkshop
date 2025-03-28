using Game.Helpers;
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
            camera = GetComponentInChildren<Camera>().transform;
            characterController = GetComponent<CharacterController>();
        }

        private void OnEnable() => Cursor.lockState = CursorLockMode.Locked;

        private void Update()
        {
            Move();
            Look();
        }

        private void Move()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Transform self = transform;
            Vector3 movement = (self.right * x) + (self.forward * y);
            characterController.SimpleMove(movement.normalized * MovementSpeed);
        }

        private void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY;
            transform.Rotate(Vector3.up * mouseX);

            float targetEuler = camera.eulerAngles.x - mouseY;
            if (!(targetEuler > 90 && targetEuler < 270))
                camera.eulerAngles = camera.eulerAngles.With(x: targetEuler > 90 ? targetEuler - 360 : targetEuler);
        }
    }
}
