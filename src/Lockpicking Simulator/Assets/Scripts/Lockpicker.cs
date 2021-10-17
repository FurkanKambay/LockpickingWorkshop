using Game.Helpers;
using UnityEngine;

namespace Game
{
    public class Lockpicker : MonoBehaviour
    {
        [Header("Lock")]
        [SerializeField] private Vector3 lockOffset;
        [SerializeField] private Vector3 lockRotation;

        [Header("Pick")]
        [SerializeField] private Transform pickPrefab;
        [SerializeField] private Vector3 pickOffset;
        [SerializeField] private Vector3 pickRotation;

        [Header("Controls")]
        [SerializeField] private float lockRotateSensitivity;
        [SerializeField] private float lockZoomSensitivity;
        [SerializeField] private float pickRotateSensitivity;

        private new Transform camera;
        private GameObject aotCamera;
        private Interactor interactor;
        private LockParts currentLock;
        private Transform pickPivot;
        private Transform currentPick;
        private Vector3 lockOriginalPosition;
        private Quaternion lockOriginalRotation;

        private void Awake()
        {
            interactor = GetComponent<Interactor>();
            camera = GetComponentInChildren<Camera>().transform;
            aotCamera = camera.GetChild(0).gameObject;
        }

        private void Start()
        {
            pickPivot = new GameObject("Pick Pivot").transform;
            currentPick = Instantiate(pickPrefab, pickPivot);
            currentPick.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!currentPick.gameObject.activeSelf)
                return;

            if (Input.GetKeyDown(KeyCode.X))
                aotCamera.SetActive(!aotCamera.activeSelf);
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                RotateLock(Input.GetAxisRaw("Mouse X") * lockRotateSensitivity);
                Zoom(Input.mouseScrollDelta.y * lockZoomSensitivity);
            }
            else if (Input.GetKeyDown(KeyCode.W))
                MovePick(currentLock.PinMargin);
            else if (Input.GetKeyDown(KeyCode.S))
                MovePick(-currentLock.PinMargin);
            else
                RotatePick(Input.GetAxisRaw("Mouse Y") * pickRotateSensitivity);
        }

        private void MovePick(float amount)
        {
            // TODO should always move relative to the plug
            currentPick.Translate(Vector3.zero.With(z: amount));
        }

        private void RotatePick(float amount)
        {
            currentPick.Rotate(Vector3.right, amount);
        }

        private void RotateLock(float amount)
        {
            currentLock.transform.Rotate(Vector3.up, amount);
        }

        private void Zoom(float amount)
        {
            currentLock.transform.Translate(0, 0, amount, camera);
        }

        public void StartPicking(Transform lockTransform)
        {
            currentLock = lockTransform.GetComponent<LockParts>();
            lockOriginalPosition = lockTransform.position;
            lockOriginalRotation = lockTransform.rotation;

            lockTransform.position = camera.TransformPoint(lockOffset);
            lockTransform.rotation = camera.rotation * Quaternion.Euler(lockRotation);

            pickPivot.SetParent(lockTransform);
            pickPivot.localPosition = pickOffset;
            pickPivot.localRotation = Quaternion.Euler(pickRotation);

            currentPick.gameObject.SetActive(true);
        }

        public void StopPicking()
        {
            currentPick.gameObject.SetActive(false);
            pickPivot.parent = null;
            interactor.Interactee.position = lockOriginalPosition;
            interactor.Interactee.rotation = lockOriginalRotation;
            currentLock.ResetPlugRotation();
        }
    }
}
