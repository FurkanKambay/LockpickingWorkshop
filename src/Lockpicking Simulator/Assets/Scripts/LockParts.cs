using System.Collections.Generic;
using Game.Helpers;
using UnityEngine;

namespace Game
{
    public class LockParts : MonoBehaviour
    {
        public int PinCount => pinCount;
        public Vector3 PinMargin => pinMargin;

        public float PlugRotation
        {
            get => plug.localEulerAngles.y;
            set => plug.localEulerAngles = plug.localEulerAngles.With(y: value);
        }

        [SerializeField] private Transform plug;

        [Header("Pin Generation")]
        [SerializeField] Transform pinContainer;
        [SerializeField] private Transform pinPrefab;
        [SerializeField] private Vector3 pinMargin;
        [SerializeField, Range(1, 10)] private int pinCount;

        private float initialPlugRotation;
        private List<Transform> pins;
        private bool pinsUpdated;

        private void Awake()
        {
            initialPlugRotation = PlugRotation;
            pins = new List<Transform>(pinCount);
        }

        private void Start()
        {
            for (int i = 0; i < pinCount; i++)
                pins.Add(Instantiate(pinPrefab, pinContainer));
        }

        private void Update()
        {
            if (pinsUpdated)
            {
                pins.ForEach(p => Destroy(p.gameObject));
                pins.Clear();

                for (int i = 0; i < pinCount; i++)
                {
                    var pin = Instantiate(pinPrefab, pinContainer);
                    pin.localPosition = pinMargin * i;
                    pins.Add(pin);
                }

                pinsUpdated = false;
            }
        }

        private void OnValidate() => pinsUpdated = true;

        public void ResetPlugRotation() => PlugRotation = initialPlugRotation;
    }
}
