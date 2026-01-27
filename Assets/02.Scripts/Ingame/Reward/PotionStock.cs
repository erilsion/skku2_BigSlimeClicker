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
    }

    public void Add(double amount)
    {
        if (amount <= 0)
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
        if (Potion < amount) 
        {
            return false;
        }
        Potion -= amount;
        OnPotionChanged?.Invoke(Potion);
        return true;
    }
}
