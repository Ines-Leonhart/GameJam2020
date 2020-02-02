using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] GameObject vfxPrefab;
    [SerializeField] float speed;

    public int xDir;
    public int zDir; //Movement direction variables

    private BoxCollider boxCollider; //BoxCollider of this object
    private Rigidbody rb; //Rigidbody of this object

    Game Game
    {
        get
        {
            return Singleton.Get<Game>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider>();

        //Get a component reference to this object's Rigidbody2D
        rb = GetComponent<Rigidbody>();

        transform.LookAt(transform.position + new Vector3(xDir, 0, zDir), Vector3.up);
    }

    protected void Move(int xDir, int zDir)
    {
        rb.MovePosition(transform.position + new Vector3(xDir, 0, zDir) * Time.fixedDeltaTime * speed);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "gamePlane")
        {
            GetComponentInChildren<Animator>().SetBool("makeJump", true);
            Destroy(this.gameObject, 2f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GetComponentInChildren<Animator>().GetBool("makeJump"))
        {
            Move(xDir, zDir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var plant = other.GetComponent<Plant>();
        if (plant != null)
        {
            plant.Attack();

            GetComponentInChildren<Animator>().SetBool("makeJump", true);
            Destroy(this.gameObject, 2f);
        }
    }

    public void Kill()
    {
        Game.enemyExplosionSound.Play();
        // TODO: vfx
        if (vfxPrefab != null)
        {
            var vfx = Instantiate(vfxPrefab);
            vfx.transform.position = transform.position;
        }
        Destroy(this.gameObject);
    }
}
