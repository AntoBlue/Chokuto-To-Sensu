using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace AI
{
    public class StateController : MonoBehaviour
    {
        [SerializeField] protected Transform eyeTransform;
        [SerializeField] private StateSettingsObject settings;
        [SerializeField] private GameObject target;

        private float _targetSpeed;
        private bool _isRotating;
        private Animator _animator;
        private Rigidbody _rb;

        public Rigidbody Rb => _rb;
        public Animator Animator  => _animator;

        public StateSettingsObject Settings
        {
            get { return settings; }
        }

        public Transform EyeTransform
        {
            get { return eyeTransform; }
        }

        public GameObject Target
        {
            get { return target; }
            set { target = value; }
        }

        public Vector3 direction = new Vector3(-1, 0, 0);

        public bool IsGrounded
        {
            get
            {
                Debug.DrawLine(_rb.position,
                    _rb.position + Vector3.down * (Settings.DepthTestGround * 0.6f),
                    Color.green);
                return Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit,
                    Settings.DepthTestGround * 0.6f,
                    (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Enemy")));
            }
        }

        public bool CanSeePlayer
        {
            get { return SphereCast(Settings.SightRadius, Color.red); }
        }

        public bool CanAttackPlayer
        {
            get { return SphereCast(Settings.DamageRadius, Color.yellow); }
        }

        public bool CanWalkForward
        {
            get
            {
                Vector3 wallRotation =
                    Quaternion.Euler(0, 0, Settings.WallCheckRotation * transform.forward.x) *
                    transform.forward;
                Debug.DrawLine(_rb.position, _rb.position + wallRotation * Settings.DepthTestWall,
                    Color.green);
                if (!Physics.Raycast(_rb.position, wallRotation, out RaycastHit wall,
                        Settings.DepthTestWall,
                        (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Enemy"))))
                {
                    Vector3 floorRotation =
                        Quaternion.Euler(0, 0, Settings.FloorCheckRotation * transform.forward.x) *
                        transform.forward;
                    Debug.DrawLine(_rb.position, _rb.position +
                                                 (floorRotation) * Settings.DepthTestFloor,
                        Color.green);
                    return Physics.Raycast(_rb.position, (floorRotation), out RaycastHit floor,
                        Settings.DepthTestFloor,
                        (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Enemy")));
                }

                return false;
            }
        }

        public bool SphereCast(float traceRadius, Color color)
        {
            Debug.DrawLine(transform.position - transform.forward * traceRadius,
                transform.position + transform.forward * traceRadius,
                color);
            Collider[] hits = Physics.OverlapSphere(transform.position, traceRadius);
            Collider hit = hits.FirstOrDefault(hit => hit.gameObject.CompareTag("Player"));

            if (hit)
            {
                GameObject target = hit.gameObject.GetComponent<Targettable>().Target;
                Vector3 rayDirection = (target.transform.position - EyeTransform.position).normalized;

                float degresAngle = Vector3.Angle(transform.forward, rayDirection);

                if (degresAngle <= Settings.SightDegrees)
                {
                    Debug.DrawLine(EyeTransform.position, transform.position + rayDirection * traceRadius,
                        Color.magenta);
                    if (Physics.Raycast(EyeTransform.position, rayDirection, out RaycastHit playerTest,
                            traceRadius) && playerTest.collider.CompareTag("Player"))
                    {
                        Target = target;
                        return true;
                    }
                }
            }

            return false;
        }

        public void StartRotation(Quaternion desiredRotation)
        {
            if (!_isRotating)
            {
                StartCoroutine(Rotate(Quaternion.Euler(0, -90 * transform.forward.x, 0)));
            }
        }

        private IEnumerator Rotate(Quaternion desiredRotation)
        {
            _isRotating = true;
            Quaternion orgRotation = transform.rotation;
            float time = 0;

            while (time < Settings.RotationTime)
            {
                time += Time.deltaTime;

                transform.rotation =
                    Quaternion.Slerp(orgRotation, desiredRotation,
                        time / Settings.RotationTime);
                yield return null;
            }

            direction.x *= -1;

            transform.rotation = desiredRotation;
            _isRotating = false;
        }

        public void InterpSpeedTo(float speed)
        {
            if (_targetSpeed != speed)
            {
                _targetSpeed = speed;
                StopCoroutine(nameof(InterpSpeedTo));
                StartCoroutine(InterpSpeedToInternal(_targetSpeed));
            }
        }

        private IEnumerator InterpSpeedToInternal(float targetSpeed)
        {
            float startSpeed = _animator.GetFloat("Blend");
            float time = 0;
            float maxTime = 0.3f;
            while (time < maxTime)
            {
                time += Time.deltaTime;

                _animator.SetFloat("Blend", Mathf.Lerp(startSpeed, targetSpeed, time / maxTime));
                yield return null;
            }
        }

        private void Awake()
        {
            _animator = gameObject.GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}