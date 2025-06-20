using UnityEngine;

namespace AI
{
    public class StateSettings : MonoBehaviour
    {
        [SerializeField] protected Transform eyeTransform;
        [SerializeField] private StateSettingsObject settings;
        
        public StateSettingsObject Settings { get { return settings; } }
        public Transform EyeTransform { get { return eyeTransform; } }

        public Vector3 direction = new Vector3(-1, 0, 0);
        
    }
}