using System;
using UnityEngine;

public class PotionStock : MonoBehaviour
{
    public static PotionStock Instance { get; private set; }

    public double Potion { get; private set; }
    public event Action<double> OnPotionChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Add(double amount)
    {
        if (amount <= 0)
        {
            return;
        }
        if (double.IsNaN(amount) || double.IsInfinity(amount))
        {
            return;
        }
        Potion += amount;
        OnPotionChanged?.Invoke(Potion);
    }

    public bool TrySpend(double amount)
    {
        if (amount <= 0) 
        {
            return true;
        }
        if (double.IsNaN(amount) || double.IsInfinity(amount) || Potion < amount) 
        {
            return false;
        }
        Potion -= amount;
        OnPotionChanged?.Invoke(Potion);
        return true;
    }

    // 현재 값을 강제로 갱신할 때 사용한다.
    public void NotifyChanged()
    {
        OnPotionChanged?.Invoke(Potion);
    }
}
