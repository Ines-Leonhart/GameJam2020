using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public int xDir;
    public int zDir; //Direction variables

    public GameObject enemy1; //Enemy1 prefab

    private float nextActionTime = 1.0f;
    public float period = 3f; //Timer variables

    // Update is called once per frame
    void Update()
    {
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
