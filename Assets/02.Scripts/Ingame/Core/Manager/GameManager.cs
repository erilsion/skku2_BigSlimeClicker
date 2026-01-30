using UnityEngine;

// 게임 매니저: 모든 데이터가 있는 갓 클래스
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private double _manualDamage = 1000;
    [SerializeField] private double _autoDamage = 100;

    public double ManualDamage => _manualDamage;
    public double AutoDamage => _autoDamage;

    public double Potion;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        DamageCalculation.Instance.Initialize(ManualDamage,AutoDamage);
    }
}
