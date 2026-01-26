using UnityEngine;

public class ClickSlime : MonoBehaviour, Clickable
{
    [SerializeField] private string _name;

    public bool OnClick(ClickInfo clickInfo)
    {
#if UNITY_EDITOR
        Debug.Log($"{_name}에게 먹이를 줬다!");
#endif
        return true;
    }
}
