using Game.Helpers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class PlayerInteracting : MonoBehaviour
    {
        public float MaxInteractDistance = 5f;

        [SerializeField] private float HoldOffset = 1f;
        [SerializeField] private Vector3 HoldingEuler;
        [SerializeField] private RectTransform cursor;

        private PlayerMovement playerMovement;
        private new Transform camera;

        private bool isInteracting;
        private Transform interactingObject;
        private Rigidbody interactingRigidbody;
        private Vector3 interactingObjectOriginalPosition;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            camera = GetComponentInChildren<Camera>().transform;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (isInteracting)
            {
                if (Input.GetButtonDown("Interact"))
                    CancelInteract();
            }
            else
            {
                (bool ok, RaycastHit hit) = Raycast();
                if (Input.GetButtonDown("Interact") && ok)
                    Interact(hit);
            }
        }

        private void Interact(RaycastHit hitInfo)
        {
            Assert.IsFalse(isInteracting);

            interactingObject = hitInfo.transform;
            interactingObjectOriginalPosition = interactingObject.position;
            interactingRigidbody = hitInfo.rigidbody;
            interactingRigidbody.isKinematic = true;

            interactingObject.position = camera.TransformPoint(Vector3.zero.With(z: HoldOffset));
            interactingObject.rotation = camera.rotation * Quaternion.Euler(HoldingEuler);

            cursor.gameObject.SetActive(false);
            playerMovement.enabled = false;
            isInteracting = true;
        }

        private void CancelInteract()
        {
            Assert.IsTrue(isInteracting);

            interactingObject.eulerAngles = Vector3.zero.With(x: 90f);
            interactingObject.position = interactingObjectOriginalPosition;
            interactingRigidbody.isKinematic = false;

            cursor.gameObject.SetActive(true);
            playerMovement.enabled = true;
            isInteracting = false;
        }

        private (bool ok, RaycastHit hit) Raycast()
        {
            bool ok = Physics.Raycast(camera.position, camera.forward, out RaycastHit hitInfo,
                    MaxInteractDistance, LayerMask.GetMask("Interactable"));
            return (ok, hitInfo);
        }
    }
}
