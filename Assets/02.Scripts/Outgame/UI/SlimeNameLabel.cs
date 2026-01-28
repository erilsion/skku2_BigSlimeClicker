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
        SlimeNameData.Instance.OnNameChanged += UpdateName;
        UpdateName(SlimeNameData.Instance.Name);
    }

    private void OnDisable()
    {
        if (SlimeNameData.Instance == null)
        {
            return;
        }
        SlimeNameData.Instance.OnNameChanged -= UpdateName;
    }

    private void UpdateName(string name)
    {
        if (_nameText == null) 
        {
            return;
        }
        _nameText.text = string.IsNullOrWhiteSpace(name) ? "" : name;
    }
}
