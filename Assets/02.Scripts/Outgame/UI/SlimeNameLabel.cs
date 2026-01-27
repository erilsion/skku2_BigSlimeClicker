using UnityEngine;
using System.Collections;
using TMPro;

public class SlimeNameLabel : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;

    private Coroutine _bindCoroutine;

    private void OnEnable()
    {
        _bindCoroutine = StartCoroutine(Bind_Coroutine());
    }

    private IEnumerator Bind_Coroutine()
    {
        while (SlimeNameData.Instance == null) 
        {
            yield return null;
        }
        SlimeNameData.Instance.OnNameChanged += Handle;
        Handle(SlimeNameData.Instance.Name);
    }

    private void OnDisable()
    {
        if (SlimeNameData.Instance != null)
        {
            SlimeNameData.Instance.OnNameChanged -= Handle;
        }
    }

    private void Handle(string name)
    {
        if (_nameText == null) 
        {
            return;
        }
        _nameText.text = string.IsNullOrWhiteSpace(name) ? "이름 없음" : name;
    }
}
