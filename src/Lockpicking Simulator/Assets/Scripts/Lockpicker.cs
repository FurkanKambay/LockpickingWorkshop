using UnityEngine;

namespace Game
{
    public class Lockpicker : MonoBehaviour
    {
        [Header("Lock")]
        [SerializeField] private Vector3 lockHoldOffset;
        [SerializeField] private Vector3 lockHoldRotation;

        [Header("Pick")]
        [SerializeField] private Transform hookPickPrefab;
        [SerializeField] private Vector3 pickHoldOffset;
        [SerializeField] private Vector3 pickHoldRotation;

        private new Transform camera;
        private Interactor interactor;
        private Vector3 lockOriginalPosition;
        private Quaternion lockOriginalRotation;
        private Transform currentPick;

        private void Awake()
        {
            interactor = GetComponent<Interactor>();
            camera = GetComponentInChildren<Camera>().transform;
        }

        private void Start()
        {
            currentPick = Instantiate(hookPickPrefab);
            currentPick.gameObject.SetActive(false);
        }

        public void StartPicking(Transform lockTransform)
        {
            lockOriginalPosition = lockTransform.position;
            lockOriginalRotation = lockTransform.rotation;

            Vector3 lockPosition = camera.TransformPoint(lockHoldOffset);
            Quaternion lockRotation = camera.rotation * Quaternion.Euler(lockHoldRotation);
            lockTransform.position = lockPosition;
            lockTransform.rotation = lockRotation;

            currentPick.SetParent(lockTransform);
            currentPick.localPosition = pickHoldOffset;
            // currentPick.position = lockTransform.TransformPoint(pickHoldOffset);
            // Vector3(0.0028,0.028,-0.144)
            // Vector3(0,90,-90)
            currentPick.rotation = lockRotation * Quaternion.Euler(pickHoldRotation);
            currentPick.gameObject.SetActive(true);
        }

        public void StopPicking()
        {
            currentPick.gameObject.SetActive(false);
            currentPick.parent = null;
            interactor.Interactee.position = lockOriginalPosition;
            interactor.Interactee.rotation = lockOriginalRotation;
        }
    }
}
