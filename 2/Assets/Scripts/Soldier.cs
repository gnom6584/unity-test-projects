using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Soldier : MonoBehaviour, Team.IMember
{
    public SoldierView View;

    public Rigidbody Rigidbody;

    [SerializeField] float Speed;

    [SerializeField] float AttackRadius;

    [SerializeField] float Dps;

    [SerializeField] float Health;

    public Team Team
    {
        set
        {
            if (_team == value)
                return;
            
            _team?.Leave(this);
            _team = value;
            _team.Join(this);
            View?.OnTeamChanged(value);
        }
        get => _team;
    }

    Team _team;

    Team.ICommand _currentCommand;

    Vector3 _velocity;

    public void TakeDamage(float damage) => Health -= damage;

    public void ReceiveCommand(Team.ICommand command) => _currentCommand = command;

    void ProccessCommand(Team.ICommand command)
    {
        if (_currentCommand is CommandAttackNearby)
        {
            var allSoldiers = FindObjectsOfType(typeof(Soldier)) as Soldier[];

            var nearbyEnemy = allSoldiers
                .Where(it => it.Team != Team)
                .OrderBy(it => Vector3.Distance(it.transform.position, transform.position))
                .FirstOrDefault();

            if (nearbyEnemy)
            {
                if (Vector3.Distance(nearbyEnemy.transform.position, transform.position) > AttackRadius)
                    _velocity = (nearbyEnemy.transform.position - transform.position).normalized * Speed;
                else
                    nearbyEnemy.TakeDamage(Dps * Time.deltaTime);
            }
        }
    } 

    void Update()
    {
        _velocity = Vector3.zero;

        ProccessCommand(_currentCommand);

        Rigidbody.velocity = _velocity;

        if (Health <= 0f)
            Destroy(gameObject);
    }

    void OnDestroy() => Team?.Leave(this);

#if UNITY_EDITOR 
    static Mesh GizmoCylinderMesh;

    void OnDrawGizmos()
    {
        GizmoCylinderMesh ??= Resources.GetBuiltinResource<Mesh>("Cylinder.fbx");
        var color = Color.red;
        color.a = 0.15f;
        Gizmos.color = color;
        Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.Scale(new Vector3(0.5f * AttackRadius, 0.5f, 0.5f * AttackRadius));
        Gizmos.DrawMesh(GizmoCylinderMesh);
    }
#endif
}
