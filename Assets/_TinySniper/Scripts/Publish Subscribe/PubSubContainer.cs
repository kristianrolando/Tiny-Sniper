using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Aldo.PubSub
{
    public class PubSubContainer { }

    public struct MessageEnemyDie
    {
        public EnemyController _enemy;
        public MessageEnemyDie(EnemyController _enemy)
        {
            this._enemy = _enemy;
        }
    }

    public struct MessageStartButtonPressed { }
    public struct MessageShootMiss{ }
    public struct MessageGameplayStart { }
    public struct MessageHoldBreath
    {
        public bool holdBreath;

        public MessageHoldBreath(bool _holdBreath)
        {
            holdBreath = _holdBreath;
        }
    }

}
