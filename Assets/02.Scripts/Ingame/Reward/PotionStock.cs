using System;
using UnityEngine;

public class PotionStock : MonoBehaviour
{
    public static PotionStock Instance { get; private set; }

    public int Potion { get; private set; }
    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Add(int amount)
    {
        if (amount <= 0)
        {
            return;
        }
        Potion += amount;
        OnGoldChanged?.Invoke(Potion);
    }

    public bool TrySpend(int amount)
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
        OnGoldChanged?.Invoke(Potion);
        return true;
    }
}
