using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot
{
    private Transform _enemyObj;
    private GameObject _bulletEnemyPref;

    private float time;

    internal void Initial(Transform enemyObj, GameObject bulletEnemyPref)
    {
        _enemyObj = enemyObj;
        _bulletEnemyPref = bulletEnemyPref;
    }

    internal void EnemyShootTarget(Transform target)
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            _enemyObj.LookAt(target);
            GameObject _bullet = MonoBehaviour.Instantiate(_bulletEnemyPref, _enemyObj.position, Quaternion.identity);
            _bullet.GetComponent<EnemyBullet>().Initial(target);

            time = 0.8f;
        }
    }

}
