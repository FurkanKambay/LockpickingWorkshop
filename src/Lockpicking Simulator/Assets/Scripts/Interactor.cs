using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public class Interactor : MonoBehaviour
    {
        public float MaxDistance = 5f;
        public Transform Interactee { get; private set; }

        [SerializeField] private RectTransform cursor;

        private PlayerMovement playerMovement;
        private Lockpicker lockpicker;
        private new Transform camera;
        private bool isInteracting;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            lockpicker = GetComponent<Lockpicker>();
            camera = GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            bool interactButton = Input.GetButtonDown("Interact");
            if (isInteracting && interactButton)
                StopInteract();
            else
            {
                RaycastHit hit = Raycast(LayerMask.GetMask("Interactable"));
                if (interactButton && hit.transform && hit.transform.CompareTag("Pickable"))
                    StartInteract(hit);
            }
        }

        private void StartInteract(RaycastHit hitInfo)
        {
            Assert.IsFalse(isInteracting);
            playerMovement.enabled = false;
            cursor.gameObject.SetActive(false);
            Interactee = hitInfo.transform;
            isInteracting = true;
            lockpicker.StartPicking(Interactee);
        }

        private void StopInteract()
        {
            Assert.IsTrue(isInteracting);
            lockpicker.StopPicking();
            playerMovement.enabled = true;
            cursor.gameObject.SetActive(true);
            isInteracting = false;
            Interactee = null;
        }

        private RaycastHit Raycast(LayerMask layerMask)
        {
            Vector3 cameraPosition = camera.position;
            Physics.Linecast(cameraPosition, cameraPosition + (camera.forward * MaxDistance), out RaycastHit hitInfo, layerMask);
            return hitInfo;
        }
    }
}
