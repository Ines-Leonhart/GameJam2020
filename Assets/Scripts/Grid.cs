using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] int rows;
    [SerializeField] int columns;

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
            }
        }
    }
}
