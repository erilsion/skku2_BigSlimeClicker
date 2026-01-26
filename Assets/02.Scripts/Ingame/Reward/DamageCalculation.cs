using UnityEngine;

public class DamageCalculation : MonoBehaviour
{
    public static DamageCalculation Instance { get; private set; }

    [Header("기본 재화 획득량")]
    [SerializeField] private int _manualBase = 10;
    [SerializeField] private int _autoBase = 3;

    // 업그레이드 시 사용하는 정보이다.
    public int ManualBonus { get; private set; }
    public int AutoBonus { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int GetManualDamage() => _manualBase + ManualBonus;
    public int GetAutoDamage() => _autoBase + AutoBonus;

    public void UpgradeManual(int plus) => ManualBonus += plus;
    public void UpgradeAuto(int plus) => AutoBonus += plus;
}
