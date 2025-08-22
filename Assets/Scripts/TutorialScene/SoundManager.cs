using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject muteButton;     // 음소거하기 버튼
    public GameObject unmuteButton;   // 음소거해제 버튼

    public void MuteAllSounds()
    {
        AudioListener.volume = 0f;
        muteButton.SetActive(false);
        unmuteButton.SetActive(true);
    }

    public void UnmuteAllSounds()
    {
        AudioListener.volume = 1f;
        muteButton.SetActive(true);
        unmuteButton.SetActive(false);
    }

    private void Start()
    {
        // 초기 상태: 음소거 해제 상태라고 가정
        muteButton.SetActive(true);
        unmuteButton.SetActive(false);
    }
}
