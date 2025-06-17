using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField] private float life = 3;
    [SerializeField] public float _speed;
    [SerializeField] private int damage;
    [SerializeField] private int rotationSpeed;
    private Transform Player;
    [SerializeField] Rigidbody rb;

    public void Configure(float speed)
    {
        _speed = speed;
    }

    void Deactivate()
    {    
        gameObject.SetActive(false);
    }

    public void Activate(GameObject Player)
    {
        Vector3 direction = Player.transform.localScale;
        rb.AddForce(new Vector3(direction.x, 0, 0) * (_speed * 10));
        Invoke("Deactivate", 3);
    }

    void OnCollisionEnter(Collision other)
    {
        //Destroy(gameObject);
        Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //damage interaction
        }
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

        transform.Rotate(rotationSpeed, 0, 0 * Time.deltaTime);


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
