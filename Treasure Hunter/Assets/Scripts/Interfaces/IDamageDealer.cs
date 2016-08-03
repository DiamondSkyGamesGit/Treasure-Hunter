using UnityEngine;
using System.Collections;

public interface IDamageDealer  {

    void DealDamage(IDamageable target, float damage);
}
