using UnityEngine;

public class ClickSlime : MonoBehaviour
{
    [SerializeField] private string _name;

    public bool OnClick()
    {
#if UNITY_EDITOR
        Debug.Log($"{_name}에게 먹이를 줬다!");
#endif
        return true;
    }
}
