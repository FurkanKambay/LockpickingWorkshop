using System.Collections.Generic;
using Game.Helpers;
using UnityEngine;

namespace Game
{
    public class LockParts : MonoBehaviour
    {
        public float PlugRotation
        {
            get => plug.localEulerAngles.y;
            set => plug.localEulerAngles = plug.localEulerAngles.With(y: value);
        }

        [SerializeField] private Transform plug;

        [Header("Pin Generation")]
        [SerializeField] Transform pinContainer;
        [SerializeField] private Transform pinPrefab;
        [SerializeField] private int pinCount;
        [SerializeField] private float pinMargin;

        private float initialPlugRotation;
        private List<Transform> pins;

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
            for (int i = 0; i < pinCount; i++)
            {
                if (i >= pins.Count)
                    pins.Add(Instantiate(pinPrefab, pinContainer));
                pins[i].localPosition = Vector3.up * (i * pinMargin);
            }
        }

        public void ResetPlugRotation() => PlugRotation = initialPlugRotation;
    }
}
