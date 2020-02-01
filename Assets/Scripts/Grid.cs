using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] int rows;
    [SerializeField] int columns;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] float spawnerOffset;

    // Start is called before the first frame update
    void Start()
    {
        var size = cellPrefab.GetComponent<Renderer>().bounds.size;
        for (int i = -columns / 2; i <= columns / 2; ++i)
        {
            for (int j = -rows / 2; j <= rows / 2; ++j)
            {
                var go = Instantiate(cellPrefab, transform);
                var position = go.transform.position;
                position.x = i * (size.x);
                position.z = j * (size.z);

                go.transform.position = position;

                if (j == -rows / 2)
                {
                    InstantiateSpawner(position, new Vector3(0, 0, -spawnerOffset), new Vector3(0, 0, 1));
                }

                if (i == -columns / 2 || i == columns / 2)
                {
                    var offset = new Vector3(i == -columns / 2 ? -spawnerOffset : spawnerOffset, 0, 0);
                    var direction = new Vector3(i == -columns / 2 ? 1 : -1, 0, 0);
                    InstantiateSpawner(position, offset, direction);
                }
            }
        }
    }

    void InstantiateSpawner(Vector3 initialPosition, Vector3 offset, Vector3 direction)
    {
        var spawner = Instantiate(enemySpawner);
        spawner.transform.position = initialPosition + offset;

        var spawnerCmp = spawner.GetComponent<enemySpawner>();
        spawnerCmp.xDir = (int)direction.x;
        spawnerCmp.zDir = (int)direction.z;
    }
}
