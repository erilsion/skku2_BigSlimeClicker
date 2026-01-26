using UnityEngine;

// 게임 매니저: 모든 데이터가 있는 갓 클래스
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int ManualDamage = 10;
    public int AutoDamage = 3;
    public int Potion;

    private void Awake()
    {
        Instance = this;
    }
}
