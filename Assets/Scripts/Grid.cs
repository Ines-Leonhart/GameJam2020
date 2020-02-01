using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] int rows;
    [SerializeField] int columns;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] float spawnerOffset;
    [SerializeField] GameObject plantPrefab;
    [SerializeField] GameObject cellsContainer;
    [SerializeField] GameObject plantsContainer;

    List<List<GameObject>> cells = new List<List<GameObject>>();

    void Start()
    {
        var size = GetCellSize();

        var x = 0;
        for (int i = -columns / 2; i <= columns / 2; ++i)
        {
            cells.Add(new List<GameObject>());
            for (int j = -rows / 2; j <= rows / 2; ++j)
            {
                var go = Instantiate(cellPrefab, cellsContainer.transform);
                cells[x].Add(go);

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
            ++x;
        }

        var collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        var colliderSize = new Vector3(size.x * columns, size.y, size.z * rows);
        collider.size = colliderSize;

        InstantiatePlants();
    }

    Vector3 GetCellSize()
    {
        return cellPrefab.GetComponent<Renderer>().bounds.size;
    }

    void InstantiateSpawner(Vector3 initialPosition, Vector3 offset, Vector3 direction)
    {
        var spawner = Instantiate(enemySpawner);
        spawner.transform.position = initialPosition + offset;

        var spawnerCmp = spawner.GetComponent<enemySpawner>();
        spawnerCmp.xDir = (int)direction.x;
        spawnerCmp.zDir = (int)direction.z;
    }

    void InstantiatePlants()
    {
        // TODO: use number based on difficulty
        var numPlantsToInstantiate = Random.Range(2, (rows * columns) / 2);

        List<(int, int)> randomPositions = new List<(int, int)>();

        while (randomPositions.Count < numPlantsToInstantiate)
        {
            var randomR = Random.Range(0, rows);
            var randomC = Random.Range(0, columns);

            if (!randomPositions.Contains((randomR, randomC)))
            {
                randomPositions.Add((randomR, randomC));
            }
        }

        var cellSize = GetCellSize();

        foreach (var t in randomPositions)
        {
            var cellGO = cells[t.Item1][t.Item2];
            var plant = Instantiate(plantPrefab, plantsContainer.transform);
            cellGO.GetComponent<cellScript>().Plant = plant.GetComponent<Plant>();

            // var plantSize = plant.GetComponent<MeshRenderer>().bounds.size;

            var position = cellGO.transform.position;
            position.y += cellSize.y / 2; // (cellSize.y / 2 + plantSize.y / 2);

            plant.transform.position = position;
        }
    }

    public float getSizeZ()
    {
        var size = cellPrefab.GetComponent<Renderer>().bounds.size;
        return size.z * columns;
    }

    public float getSizeX()
    {
        var size = cellPrefab.GetComponent<Renderer>().bounds.size;
        return size.x * rows;
    }

    public int GetNumberPlantsOnLine(Vector3 position)
    {
        var num = 0;

        // We need to find the right column
        if (position.z < cells[0][0].transform.position.z)
        {
            var column = 0;

            // We only need to check row 0
            for (var i = 0; i < cells.Count; ++i)
            {
                if (position.x == cells[i][0].transform.position.x)
                {
                    column = i;
                    break;
                }
            }

            for (var i = 0; i < cells[column].Count; ++i)
            {
                if (cells[column][i].GetComponent<cellScript>().Plant != null)
                {
                    ++num;
                }
            }
        }
        // We need to find the right row
        else
        {
            var row = 0;

            // We only need to check column 0
            for (var i = 0; i < cells[0].Count; ++i)
            {
                if (position.z == cells[0][i].transform.position.z)
                {
                    row = i;
                    break;
                }
            }

            for (var i = 0; i < cells.Count; ++i)
            {
                if (cells[i][row].GetComponent<cellScript>().Plant != null)
                {
                    ++num;
                }
            }
        }

        return num;
    }
}
