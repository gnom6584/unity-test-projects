using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public GameObject[] ObstaclesVariants;

    public Vector2Int Size;

    readonly List<GameObject> _obstacles = new List<GameObject>();

    public void CreateObstacles()
    {
        _obstacles.ForEach(Destroy);
        _obstacles.Clear();

        for(int i = 0; i < Size.y / 2; i++)
        {
            var go = Instantiate(ObstaclesVariants[Random.Range(0, ObstaclesVariants.Length)]);
            _obstacles.Add(go);
            go.transform.parent = transform;
            go.transform.localPosition = new Vector2(Random.Range((int) (-Size.x / 2f), (int) (Size.x / 2f) + 1), i * 2);
        }
    }
}
