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
        [SerializeField] private float minPickDepth;
        [SerializeField] private float maxPickDepth;

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
            currentPick = Instantiate(pickPrefab);
            currentPick.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (currentPick.gameObject.activeSelf)
                MovePick(Input.GetAxis("Mouse Y") * pickMoveSensitivity);
        }

        private void MovePick(float amount)
        {
            Vector3 pickPosition = currentPick.localPosition;
            pickPosition = pickPosition.With(y: Mathf.Clamp(pickPosition.y + amount, minPickDepth, maxPickDepth));
            currentPick.localPosition = pickPosition;
        }

        public void StartPicking(Transform lockTransform)
        {
            lockOriginalPosition = lockTransform.position;
            lockOriginalRotation = lockTransform.rotation;

            Vector3 lockPosition = camera.TransformPoint(lockOffset);
            Quaternion lockRotation = camera.rotation * Quaternion.Euler(this.lockRotation);
            lockTransform.position = lockPosition;
            lockTransform.rotation = lockRotation;

            currentPick.SetParent(lockTransform);
            currentPick.localPosition = pickOffset;
            currentPick.localRotation = Quaternion.Euler(pickRotation);
            currentPick.localScale = Vector3.one;
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
