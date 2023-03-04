
public interface ICanTakeDamage
{
    int HP { get; }
    void TakeDamage(int damage);
    void Die();
}
