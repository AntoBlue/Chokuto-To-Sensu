using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField] private float life = 3;
    [SerializeField] public float _speed;
    private Transform Player;
    [SerializeField] Rigidbody rb;

    public void Configure(float speed)
    {
        _speed = speed;

        Destroy(gameObject, 3);
    }

    void Awake()
    {
        Destroy(gameObject, life);
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 direction = Player.transform.localScale;
        rb.AddForce(new Vector3(direction.x, 0, 0) * (_speed * 10));

    }

    void OnCollisionEnter(Collision other)
    {
        //Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float moveDirection = Input.GetAxis("Horizontal");
        
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 direction = Player.transform.localScale;

        
        //transform.Translate(new Vector3(direction.x, 0, 0) * (Time.deltaTime * _speed), Space.Self);

        //if (moveDirection >= 0)
        //{
        //    direction = 1;
        //}
        //else if (moveDirection < 0)
        //{
        //    direction = -1;
        //}
        //transform.Translate(new Vector3 (1, 0, 0) * (Time.deltaTime * _speed), Space.Self);
        //if (PlayerDirection.transform.localScale.x > 0)
        //{
        //transform.Translate(new Vector3 (direction.x, 0, 0) * (Time.deltaTime * _speed), Space.Self);
        //}

        //else
        //{
        //    transform.Translate(new Vector3(1, 0, 0) * (Time.deltaTime * _speed), Space.Self);
        //}

        //if (Player.transform.localScale.x < 0)
        //{
        //    transform.Translate(new Vector3(-1, 0, 0) * (Time.deltaTime * _speed), Space.Self);
        //}
    }


}
