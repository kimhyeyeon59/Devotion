using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public GameObject settingsPanel;

    // 환경설정 버튼에서 호출
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // 닫기 버튼에서 호출
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
