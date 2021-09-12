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
        [SerializeField] private float pickMoveSensitivity;
        [SerializeField] private float pickRotateSensitivity;
        [SerializeField] private float minPickDepth;
        [SerializeField] private float maxPickDepth;

        private new Transform camera;
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

            if (Input.GetKey(KeyCode.LeftAlt))
                RotatePick(Input.GetAxis("Mouse X") * pickRotateSensitivity);
            else if (Input.GetKeyDown(KeyCode.A))
                MovePick(+1);
            else if (Input.GetKeyDown(KeyCode.D))
                MovePick(-1);
        }

        private void MovePick(int direction)
        {
            Vector3 pinMargin = currentLock.GetComponent<LockParts>().PinMargin;
            currentPick.localPosition += pinMargin * direction;
        }

        private void RotatePick(float amount)
        {
            currentLock.transform.Rotate(Vector3.up, amount, Space.Self);
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
