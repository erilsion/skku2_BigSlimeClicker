using UnityEngine;
using System.Collections;

public class SlimeNamePopup : MonoBehaviour
{
    [SerializeField] private SlimeNamePopupUI _popupUI;

    private IEnumerator Start()
    {
        // 저장소 준비를 대기한다.
        while (SlimeNameData.Instance == null)
        {
            yield return null;
        }

        // 팝업 참조가 늦게 들어오거나 씬 로딩 이슈 등 만약을 위해 한 프레임 더 여유를 준다.
        yield return null;

        if (!SlimeNameData.Instance.HasName)
        {
            // 이름이 없으면 팝업을 띄운다.
            if (_popupUI != null)
            {
                _popupUI.Open();
            }
        }
    }
}
