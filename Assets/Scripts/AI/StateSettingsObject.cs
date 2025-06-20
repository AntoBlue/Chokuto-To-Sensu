using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "StateSettings", menuName = "ScriptableObjects/StateSettings", order = 0)]
    public class StateSettingsObject : ScriptableObject
    {
        [Header("State Settings")]
        [SerializeField] private float rotationTime = 0.5f;
        [SerializeField] private float sightDegrees = 90;
        [SerializeField] private float sightRadius = 3f;
        [SerializeField] private float damageRadius = 1f;

        public float RotationTime { get { return rotationTime; } }
        public float SightDegrees { get { return sightDegrees; } }
        public float SightRadius { get { return sightRadius; } }
        public float DamageRadius { get { return damageRadius; } }
        
        [Header("Depth Check")]
        [SerializeField] private float depthTestWall = 0.4f;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float depthTestFloor = 1.6f;
        [SerializeField] private float depthTestGround = 2;
        [SerializeField] private LayerMask groundFloorLayer;
        [SerializeField] private float floorCheckRotation = -55;
        
        public float DepthTestWall { get { return depthTestWall; } }
        public LayerMask WallLayer { get { return wallLayer; } }
        public float DepthTestFloor { get { return depthTestFloor; } }
        public float DepthTestGround { get { return depthTestGround; } }
        public LayerMask GroundFloorLayer { get { return groundFloorLayer; } }
        public float FloorCheckRotation { get { return floorCheckRotation; } }
        
    }
}