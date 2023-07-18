using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAble
{
    Vector3 Position { get; }
    void GotDamage(float damage);
}
