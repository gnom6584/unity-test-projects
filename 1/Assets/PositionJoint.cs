using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionJoint : MonoBehaviour
{
    public enum Type
    {
        Hard,
        Lerp
    }

    public Transform Target;

    public Type JoinType;

    public float LerpSpeed;

    public bool JoinX = true;

    public bool JoinY = true;

    public bool JoinZ = true;

    void LateUpdate()
    {
        if (Target == null)
            return;

        var position = transform.position;
        var targetPosition = Target.position;
        
        if(JoinX)
            position.x = JoinType is Type.Lerp ? Mathf.Lerp(position.x, targetPosition.x, Time.fixedDeltaTime * LerpSpeed) : targetPosition.x;
        
        if(JoinY)
            position.y = JoinType is Type.Lerp ? Mathf.Lerp(position.y, targetPosition.y, Time.fixedDeltaTime * LerpSpeed) : targetPosition.y;
        
        if(JoinZ)
            position.z = JoinType is Type.Lerp ? Mathf.Lerp(position.z, targetPosition.z, Time.fixedDeltaTime * LerpSpeed) : targetPosition.z;

        transform.position = position;
    }
}
