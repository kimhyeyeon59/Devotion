using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    public string targetSceneName = "VillageScene"; // 최종 이동할 씬 이름

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadingSceneManager.nextScene = targetSceneName; // 다음 씬 이름 저장
            SceneManager.LoadScene("LoadingScene");          // 로딩씬으로 이동
        }
    }
}
