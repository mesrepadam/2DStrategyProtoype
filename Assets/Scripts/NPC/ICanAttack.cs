
using UnityEngine;

public interface ICanAttack 
{
    void Attack(ICanTakeDamage target, Vector2 targetPos);
}
