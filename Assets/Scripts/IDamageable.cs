using System;

public interface IDamageable
{
    public int Health { get; set; }

    public event Action<int, int> OnHealthChanged;

    public void ApplyDamage(int damage);
}
