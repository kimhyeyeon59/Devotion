using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void OnClickStartGame()
    {
        // TutorialScene 로드
        SceneManager.LoadScene("TutorialScene");
    }
}
