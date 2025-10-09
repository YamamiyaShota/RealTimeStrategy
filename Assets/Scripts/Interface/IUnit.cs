public interface IUnit
{
    public int HP { get; }
    public int ATK { get; }
    public int DF { get; }
    public float AttackInterval { get; }
    public float MoveSpeed { get; }
    public float AttackRange { get; }
    public void AddDamage(int atk);
}