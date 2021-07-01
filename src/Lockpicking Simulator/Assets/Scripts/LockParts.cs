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
        private float defaultPlugRotation;

        private void Awake() => defaultPlugRotation = PlugRotation;

        public void ResetPlugRotation() => PlugRotation = defaultPlugRotation;
    }
}
