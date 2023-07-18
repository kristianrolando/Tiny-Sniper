using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aldo.PubSub;

public class EnemyStatus
{

    private EnemyController _controller;
    internal float _health;

    internal void Initial(EnemyController controller, float health)
    {
        _controller = controller;
        _health = health;
    }

    internal void GotDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    private void Die()
    {
        PublishSubscribe.Instance.Publish<MessageEnemyDie>(new MessageEnemyDie(_controller));
    }
}
