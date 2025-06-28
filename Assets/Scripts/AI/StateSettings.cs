using UnityEngine;

namespace AI
{
    public class StateSettings : MonoBehaviour
    {
        [SerializeField] protected Transform eyeTransform;
        [SerializeField] private StateSettingsObject settings;
        [SerializeField] private GameObject target;
        
        public StateSettingsObject Settings { get { return settings; } }
        public Transform EyeTransform { get { return eyeTransform; } }
        public GameObject Target { get { return target; } set { target = value; } }

        public Vector3 direction = new Vector3(-1, 0, 0);
        
        
        
    }
}