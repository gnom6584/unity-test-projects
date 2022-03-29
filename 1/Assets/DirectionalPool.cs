using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirectionalPool : MonoBehaviour
{
    public abstract class Adapter : ScriptableObject
    {
        public abstract GameObject Create(int index);
        
        public abstract void Rebind(GameObject gameObject, int index);

        public abstract float Size(int index);
    }

    public float Scroll;

    public float VisibleAreaSize;

    public Adapter FieldAdapter;

    Adapter _lastAdapter;

    float _lastVisibleAreaSize;

    readonly List<(GameObject GameObject, float Size, float Position)> _gameObjects = new List<(GameObject GameObject, float Size, float Position)>();

    void OnDrawGizmos()
    {
        static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180+arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180-arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        Gizmos.color = Color.green;
        DrawArrow(transform.position, transform.up * VisibleAreaSize);
        Gizmos.color = Color.red;
        DrawArrow(transform.position, transform.right);
        Gizmos.color = Color.blue;
        DrawArrow(transform.position, transform.forward);
    }

    void Update()
    {
        if(FieldAdapter != _lastAdapter || _lastVisibleAreaSize != VisibleAreaSize)
        {
            _lastAdapter = FieldAdapter;
            _lastVisibleAreaSize = VisibleAreaSize;

            Scroll = 0f;
            _gameObjects.ForEach(it => Destroy(it.GameObject));
            _gameObjects.Clear();

            float target = 0;
            int i = 0;

            while (target < VisibleAreaSize)
            {
                var go = FieldAdapter.Create(i);
                var size = FieldAdapter.Size(i);

                go.transform.parent = transform;
                go.transform.localPosition = new Vector2(0f, target);
                _gameObjects.Add((go, size, target));

                target += size;
                ++i;
            }
        }

        foreach (var (go, _, pos) in _gameObjects)
            go.transform.localPosition = new Vector3(0, Scroll + pos, 0);

        for (var i = 0; i < _gameObjects.Count; ++i)
        {
            var (go, _, pos) = _gameObjects[i];
            if (go.transform.localPosition.y < 0)
            {
                _gameObjects[i] = (_gameObjects[i].GameObject, _gameObjects[i].Size, VisibleAreaSize + _gameObjects[i].Position);
                FieldAdapter.Rebind(go, i);
            }
            else if (go.transform.localPosition.y > VisibleAreaSize)
            {
                _gameObjects[i] = (_gameObjects[i].GameObject, _gameObjects[i].Size, _gameObjects[i].Position - VisibleAreaSize);
                FieldAdapter.Rebind(go ,i);
            }
        }
    }

    public void InvalidateAdapter() => _lastAdapter = null;
}
