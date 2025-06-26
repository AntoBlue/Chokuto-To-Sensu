using UnityEngine;

namespace DefaultNamespace
{
    public class HasOwner: MonoBehaviour
    {
        [SerializeField] private GameObject owner;

        public GameObject Owner
        {
            get => owner;
            set => owner = value;
        }
    }
}