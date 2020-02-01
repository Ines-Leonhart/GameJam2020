using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public int xDir;
    public int zDir; //Direction variables

    public GameObject enemy1; //Enemy1 prefab

    private float nextActionTime;
    public float period; //Timer variables

    void Start()
    {
        // TODO: determine period and action time based on level difficulty
        period = Random.Range(3f, 10f);
        nextActionTime = Random.Range(1f, period);
    }

    void Update()
    {
        // TODO: check if there's a plant in this row/column
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;

            GameObject my_enemy1 = Instantiate(enemy1, transform.position, transform.rotation);
            enemyMovement my_enemy1_mov = my_enemy1.GetComponent<enemyMovement>();
            
            my_enemy1_mov.xDir = xDir;
            my_enemy1_mov.zDir = zDir;
        }
    }
}
