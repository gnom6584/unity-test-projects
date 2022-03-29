using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CameraPlaneRaycastReceiver : MonoBehaviour
{
    public enum Limit
    {
        None,
        Clamp,
        Cancel
    }

    public UnityEvent<Vector3> OnHit;

    public Vector3 Hit { private set; get; }

    public Limit LimitMode;

    public Camera Camera;

    public Vector3 ClampHitPosition(Vector3 point, float paddingLeft = 0f, float paddingTop = 0f, float paddingRight = 0f, float paddingBottom = 0f)
    {
        var horizontalPaddings = new Vector2(paddingLeft, paddingRight) / transform.localScale.x;
        var verticalPaddings = new Vector2(paddingTop, paddingBottom) / transform.localScale.y;
        var localPoint = transform.InverseTransformPoint(point);
        localPoint.x = Mathf.Clamp(localPoint.x, -.5f + horizontalPaddings.x, .5f - horizontalPaddings.y);
        localPoint.y = Mathf.Clamp(localPoint.y, -.5f + verticalPaddings.x, .5f - verticalPaddings.y);
        point = transform.TransformPoint(localPoint);
        return point;
    }

    void Update()
    {
        Camera = Camera != null ? Camera : Camera.main;

        var plane = new Plane(transform.rotation * Vector3.forward, Vector3.zero);

        var ray = Camera.ScreenPointToRay(Input.mousePosition);

        if (!plane.Raycast(ray, out var distance))
            return;

        var point = ray.GetPoint(distance);

        switch (LimitMode)
        {
            case Limit.None:
                UpdateHitState(point);
                break;
            case Limit.Clamp:
                UpdateHitState(ClampHitPosition(point));
                break;
            case Limit.Cancel:
                {
                    var localPoint = transform.InverseTransformPoint(point);
                    bool cancel = Mathf.Clamp(localPoint.x, -.5f, .5f) != localPoint.x;
                    cancel |= Mathf.Clamp(localPoint.y, -.5f, .5f) != localPoint.y;
                    if (!cancel)
                        UpdateHitState(point);
                }
                break;
        }
    }

    void UpdateHitState(Vector3 point)
    {
        OnHit?.Invoke(point);
        Hit = point;
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        var green = Color.green;
        green.a = 0.25f;
        Gizmos.color = green;

        Gizmos.DrawCube(Vector3.zero, new Vector3(1f, 1f, 0f));
    }

#endif
}
