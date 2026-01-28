using UnityEngine;

// 게임 매니저: 모든 데이터가 있는 갓 클래스
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public double ManualDamage = 1000;
    public double AutoDamage = 100;
    public double Potion;

    private void Awake()
    {
        _instance = this;
    }
}
