using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public int xDir;
    public int zDir; //Direction variables

    public GameObject enemy1; //Enemy1 prefab

    float period; //Timer variables
    float spawnTimestamp;

    Grid grid;

    Game Game
    {
        get
        {
            return Singleton.Get<Game>();
        }
    }

    void Start()
    {
        // TODO: determine period and action time based on level difficulty
        // TODO: also start this up once the player waters the first plant
        period = Random.Range(6f, 10f);
        spawnTimestamp = Time.realtimeSinceStartup;

        grid = FindObjectOfType<Grid>();
    }

    void Update()
    {
        if (Game.CurrentState != Game.State.Play)
        {
            return;
        }
        if (!Game.gameStarted)
        {
            return;
        }
        if (Time.realtimeSinceStartup - spawnTimestamp >= period
            && grid.GetNumberPlantsOnLine(transform.position) > 0)
        {
            GameObject my_enemy1 = Instantiate(enemy1, transform.position, transform.rotation);
            enemyMovement my_enemy1_mov = my_enemy1.GetComponent<enemyMovement>();

            my_enemy1_mov.xDir = xDir;
            my_enemy1_mov.zDir = zDir;

            spawnTimestamp = Time.realtimeSinceStartup;
        }
    }
}
