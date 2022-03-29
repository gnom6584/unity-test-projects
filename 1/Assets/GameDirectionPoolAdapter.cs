using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Adapter", menuName = "GameFieldAdapters/Adapter", order = 1)]
public class GameDirectionPoolAdapter : DirectionalPool.Adapter
{
    public GameObject SegmentPrefab;

    public override GameObject Create(int index) => Instantiate(SegmentPrefab);

    public override void Rebind(GameObject gameObject, int index) => gameObject.GetComponent<Segment>().CreateObstacles();    

    public override float Size(int index) => 5;
}
