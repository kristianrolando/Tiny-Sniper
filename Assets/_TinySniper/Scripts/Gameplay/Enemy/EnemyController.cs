using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageAble
{
    [SerializeField] float health;

    [Header("Movement")]
    [SerializeField] Transform[] wayPoints;
    [SerializeField] GameObject enemyObj;
    [SerializeField] bool isPatrol;

    private NavMeshAgent _agent;

    [Header("Ragdoll")]
    [SerializeField] GameObject mainBodyObj;
    [SerializeField] GameObject rigBodyParent;

    [Header("Shoot")]
    [SerializeField] GameObject enemyBulletPref;


    EnemyStatus _status = new EnemyStatus();
    EnemyMovement _movement = new EnemyMovement();
    RagdollController _ragdoll = new RagdollController();
    EnemyShoot _shoot = new EnemyShoot();

    public bool isAlert;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        Initial();
    }
    private void Start()
    {
        _ragdoll.DisableRagdollMode();
    }
    private void Initial()
    {
        _status.Initial(this, health);
        _movement.Initial(_agent, wayPoints, enemyObj);
        _ragdoll.Initial(mainBodyObj, rigBodyParent);
        _shoot.Initial(enemyObj.transform, enemyBulletPref);
    }

    private void Update()
    {
        if(isPatrol)
            _movement.PatrolMovement();
    }
    private void ShootPlayer()
    {
        GameObject _target = GameObject.Find("Player");
        _shoot.EnemyShootTarget(_target.transform);
    }

    public void GotDamage(float damage)
    {
        _status.GotDamage(damage);
        if(_status._health <= 0)
        {
            _ragdoll.EnableRagdollMode();
            isPatrol = false;
        }
    }
}
