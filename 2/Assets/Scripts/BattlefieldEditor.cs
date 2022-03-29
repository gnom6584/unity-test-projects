using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldEditor : MonoBehaviour
{
    readonly DragTracker DragTracker = new DragTracker(0f);

    [SerializeField] Game game;

    [SerializeField] CameraPlaneRaycastReceiver leftArea;

    [SerializeField] CameraPlaneRaycastReceiver rightArea;

    [SerializeField] Soldier[] spawnSoldierPrefabs;

    [SerializeField] GameObject pointer;

    public float DragThereshold;

    Soldier _selectedSoldierPrefab;

    Soldier _target;

    Camera _camera;

    CameraPlaneRaycastReceiver _targetArea;

    bool _roundMode = false;
    
    public void SelectSoldierPrefab(int index)
    {
        if (index is 0)
        {
            _selectedSoldierPrefab = null;
            return;
        }

        _selectedSoldierPrefab = spawnSoldierPrefabs[index - 1];
    }

    public void DestroyTarget()
    {
        if (_target)
            Destroy(_target.gameObject);
    }

    void FocusArea(CameraPlaneRaycastReceiver area)
    {
        _targetArea = area;
        if(area == leftArea)
        {
            leftArea.LimitMode = CameraPlaneRaycastReceiver.Limit.None;
            rightArea.enabled = false;
        }
        else if(area == rightArea)
        {
            rightArea.LimitMode = CameraPlaneRaycastReceiver.Limit.None;
            leftArea.enabled = false;
        }
    }

    void UnfocusArea()
    {
        leftArea.enabled = true;
        rightArea.enabled = true;
        leftArea.LimitMode = CameraPlaneRaycastReceiver.Limit.Cancel;
        rightArea.LimitMode = CameraPlaneRaycastReceiver.Limit.Cancel;
    }
    
    Soldier TrySelectSoldier(Vector3 hitPosition)
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            var hitGo = hit.collider.gameObject;
            var soldier = hitGo.GetComponent<Soldier>();
            return soldier;
        }
        return null;
    }

    Soldier CreateNewSoldier(Vector3 hitPosition)
    {
        var newSoldier = Instantiate(_selectedSoldierPrefab, ProcessPosition(hitPosition), Quaternion.identity);

        if (_targetArea == leftArea)
            newSoldier.Team = game.Red;
        else if (_targetArea == rightArea)
            newSoldier.Team = game.Blue;

        return newSoldier;
    }

    void HandleClick(Vector3 hitPosition)
    {
        _target = TrySelectSoldier(hitPosition);
        if (!_target && _selectedSoldierPrefab)
            _target = CreateNewSoldier(hitPosition);
    }
    void ProccessInput(Vector3 hitPosition, CameraPlaneRaycastReceiver area)
    {
        if (!enabled)
            return;

        _camera = _camera == null? Camera.main : _camera;

        _roundMode = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetMouseButtonDown(0))
        {
            _target = null;
            FocusArea(area);

            HandleClick(hitPosition);

            if (_target)
            {
                DragTracker.Threshold = DragThereshold;
                DragTracker.Start(hitPosition, _target.transform.position);
            }
        }
        if (Input.GetMouseButton(0)) 
            DragTracker.Move(hitPosition);

        if (Input.GetMouseButtonUp(0))
        {
            UnfocusArea();
            DragTracker.Release();
        }
    }

    Vector3 ProcessPosition(Vector3 position)
    {
        if (_roundMode)
            position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));

        position = _targetArea.ClampHitPosition(position, .5f, .5f, .5f, .5f);

        return position;
    }

    void ProccessDrag(Vector3 position)
    {
        if (_target) 
            _target.transform.position = ProcessPosition(position);
    }

    void Start()
    {
        leftArea.OnHit.AddListener(position => ProccessInput(position, leftArea));
        rightArea.OnHit.AddListener(position => ProccessInput(position, rightArea));
        DragTracker.OnDrag += ProccessDrag;
    }

    private void OnDisable() 
    {
        if(pointer) 
            pointer.SetActive(false);
    }

    private void Update()
    {
        if (pointer) {
            if (_target)
                pointer.transform.position = _target.transform.position;

             pointer.SetActive(_target);
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
            DestroyTarget();
    }
}
