using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

// '업그레이드'라는 게임 콘텐츠의 도메인 클래스이다.
// 도메인이란 핵심 데이터와 규칙을 말한다.
// 게임의 본질이기 때문에 가장 먼저 만들고, 가장 나중에 바뀐다.
// 핵심 데이터와 규칙을 가지고 있다 -> 응집도가 높다 -> 표현력이 높아야 한다.
public class Upgrade
{
    // 기획 데이터 (기획자가 정한 값) => UpgradeSpecData로 뺐다.
    // 1. 기획 테이블의 데이터를 가져온다.
    public readonly UpgradeDefinitionSO UpgradeDefinition;

    // 런타임 데이터 (게임 중간에 바뀌는 데이터) (플레이어가 만들어가는 값)
    public int Level {  get; private set; }

    // 업그레이드 비용
    public Currency Cost => UpgradeDefinition.BaseCost + Math.Pow(UpgradeDefinition.CostGrowth, Level);  // 지수 공식: 기본 비용 + 증가량 ^ 레벨
    public double Bonus => Level * UpgradeDefinition.PlusPerLevel;     // 선형 공식: 기본 비용 + 레벨 * 증가량
    public bool IsMaxLevel => Level >= UpgradeDefinition.MaxLevel;

    // 2. 핵심 규칙(유효성)을 작성한다.
    public Upgrade(UpgradeDefinitionSO upgradeDefinition)
    {
        UpgradeDefinition = upgradeDefinition;

        if (UpgradeDefinition.BaseCost < 0) throw new ArgumentException($"기본 비용은 0보다 커야 합니다: {UpgradeDefinition.BaseCost}");
        if (UpgradeDefinition.CostGrowth <= 0) throw new ArgumentException($"비용 증가량은 0보다 커야 합니다: {UpgradeDefinition.CostGrowth}");
        if (UpgradeDefinition.PlusPerLevel < 0) throw new ArgumentException($"레벨 당 증가량은 0보다 커야 합니다: {UpgradeDefinition.PlusPerLevel}");
    }

    public bool TryLevelUp()
    {
        if (IsMaxLevel)
        {
            return false;
        }
        Level++;
        return true;
    }

    public void RestoreLevel(int savedLevel)
    {
        Level = Math.Clamp(
            savedLevel,
            0,
            UpgradeDefinition.MaxLevel
        );
    }
}
