using System;
using UnityEngine;

public class DragTracker
{
    public event Action<Vector3> OnDrag;

    public float Threshold;

    public DragTracker() => Threshold = 0f;

    public DragTracker(float threshold) => Threshold = threshold;
    

    Vector3? _startPosition; 

    Vector3? _targetStartPosition; 

    Vector3? _thresholdPosition;

    public void Start(Vector3 position, Vector3 targetStartPosition)
    {
        _startPosition = position;
        _targetStartPosition = targetStartPosition;
    } 

    public void Start(Vector3 position) => Start(position, Vector3.zero);

    public void Move(Vector3 position) 
    { 
        if(!_startPosition.HasValue)
            return;

        var delta = position - _startPosition.Value;

        if (!_thresholdPosition.HasValue && Vector3.Distance(position, _startPosition.Value) >= Threshold)
            _thresholdPosition = position;

        if(_thresholdPosition.HasValue)
            OnDrag?.Invoke(position - _thresholdPosition.Value + _targetStartPosition.Value);

    }

    public void Release() 
    { 
        _startPosition = null;
        _targetStartPosition = null;
        _thresholdPosition = null;
    }
}

