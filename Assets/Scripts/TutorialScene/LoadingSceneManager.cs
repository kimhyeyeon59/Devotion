using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    public TMP_Text loadingText;
    public float dotInterval = 0.5f;
    public float minLoadingTime = 5f; // 최소 유지 시간(초)

    private void Start()
    {
        StartCoroutine(LoadingTextAnimation());
        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadingTextAnimation()
    {
        string baseText = "Loading";
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount % 3) + 1;
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(dotInterval);
        }
    }

    private IEnumerator LoadSceneWithDelay()
    {
        float startTime = Time.time;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // 바로 씬 전환 막기

        // 로딩 진행
        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // 최소 로딩 시간 보장
        float elapsed = Time.time - startTime;
        if (elapsed < minLoadingTime)
            yield return new WaitForSeconds(minLoadingTime - elapsed);

        // 이제 씬 전환
        op.allowSceneActivation = true;
    }
}
