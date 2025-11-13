using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BlackLoadingSceneManager : MonoBehaviour
{
    public static string nextScene;  // 이동할 씬 이름
    public Image fadeImage;          // 전체 화면을 덮는 검은색 이미지
    public float fadeDuration = 1f;  // 페이드 시간(초)
    public float minDisplayTime = 2f; // 최소 유지 시간(초)

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        // 1. 페이드 인 (화면 완전 까맣게)
        yield return StartCoroutine(Fade(0f, 1f));

        // 2. 최소 유지 시간
        yield return new WaitForSeconds(minDisplayTime);

        // 3. 다음 씬 비동기 로드 시작
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // 4. 씬 활성화 직전 페이드 아웃
        yield return StartCoroutine(Fade(1f, 0f));

        // 5. 씬 전환
        op.allowSceneActivation = true;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;
            yield return null;
        }
    }
}
