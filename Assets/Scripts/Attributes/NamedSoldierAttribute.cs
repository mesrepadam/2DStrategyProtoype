using UnityEngine;

public class NamedSoldierAttribute : PropertyAttribute
{
    public readonly string[] names = System.Enum.GetNames(typeof(SoldierType));
}
