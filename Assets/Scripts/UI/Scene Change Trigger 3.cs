using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger3 : MonoBehaviour
{
    public string targetSceneName = "CastleInsideScene"; // 최종 이동할 씬 이름

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BlackLoadingSceneManager.nextScene = targetSceneName; // 다음 씬 이름 저장
            SceneManager.LoadScene("BlackLoadingScene");          // 로딩씬으로 이동
        }
    }

}
