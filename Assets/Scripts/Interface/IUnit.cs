public interface IUnit
{
    public int HP { get; }
    public int ATK { get; }
    public int DF { get; }
    public float MoveSpeed { get; }
    public float AttackInterval { get; }
    public float AttackRange { get; }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="atk">攻撃力</param>
    /// <returns>trueなら死亡,falseなら生存</returns>
    public bool AddDamage(int atk);
}