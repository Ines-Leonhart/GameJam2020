using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public int xDir;
    public int zDir; //Direction variables

    public GameObject enemy1; //Enemy1 prefab

    float period; //Timer variables
    float spawnTimestamp;
    float startTimestamp;
    float awaitStart;
    int maxEnemies;

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
        period = Random.Range(6f, 10f);
        awaitStart = Random.Range(2f, 10f);
        spawnTimestamp = 0;
        startTimestamp = 0;
        maxEnemies = Game.maxEnemies+Game.PlayerLevel*Game.increase;
        Debug.Log("This level max enemies: " + maxEnemies);
        grid = FindObjectOfType<Grid>();
    }

    private void Spawn()
    {
        GameObject my_enemy1 = Instantiate(enemy1, transform.position, transform.rotation);
        enemyMovement my_enemy1_mov = my_enemy1.GetComponent<enemyMovement>();

        my_enemy1_mov.xDir = xDir;
        my_enemy1_mov.zDir = zDir;

        spawnTimestamp = Time.realtimeSinceStartup;
        period = Random.Range(6f, 10f);
    }

    private int getTotalNumberOfEnemies()
    {
        return GameObject.FindObjectsOfType<enemyMovement>().Length;
    }

    private int getEnemiesInLine()
    {
        var counter = 0;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z), new Vector3(xDir, 0, zDir)*100, Color.red, 10);
        var ray = Physics.RaycastAll(transform.position, new Vector3(xDir, 0, zDir)*100);
        if(ray.Length > 0)
        {
            foreach (RaycastHit hit in ray)
            {
                if (hit.transform.gameObject.GetComponent<enemyMovement>())
                {
                    counter++;
                }
            }
        }
        return counter;
    }

    private void Update()
    {
        if (!Game.gameStarted || Game.CurrentState != Game.State.Play)
        {
            return;
        }
        if(startTimestamp == 0)
        {
            startTimestamp = Time.realtimeSinceStartup;
        }
        var nPlants = grid.GetNumberPlantsOnLine(transform.position);
        var nEnemiesInLine = getEnemiesInLine();
        var nTotalEnemies = getTotalNumberOfEnemies();

        if (spawnTimestamp == 0 && Time.realtimeSinceStartup - startTimestamp >= awaitStart 
            && nPlants > 0 && nPlants > nEnemiesInLine && nTotalEnemies < maxEnemies)
        {
            Spawn();
        }
        else if (Time.realtimeSinceStartup - spawnTimestamp >= period
           && nPlants > 0 && nPlants > nEnemiesInLine && nTotalEnemies < maxEnemies)
        {
            Spawn();
        }
    }
}
