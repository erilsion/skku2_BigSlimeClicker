using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UpgradeDefinition")]
public class UpgradeDefinition : ScriptableObject
{
    [Header("ID")]
    public EUpgradeType UpgradeType;

    [Header("대상 클릭 타입")]
    public EClickType TargetClickType;

    [Header("표시용(UI)")]
    public string DisplayName;

    [Header("가격")]
    public double BaseCost = 10;
    public double CostGrowth = 1.15;

    [Header("효과")]
    public double PlusPerLevel = 1; // 단순 + 누적
}
