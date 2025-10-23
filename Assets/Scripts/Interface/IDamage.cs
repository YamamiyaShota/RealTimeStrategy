public interface IDamage
{
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="atk">攻撃力</param>
    /// <returns>trueなら死亡,falseなら生存</returns>
    public bool AddDamage(int atk);
}