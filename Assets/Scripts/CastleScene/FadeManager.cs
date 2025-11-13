using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [Header("페이드 설정")]
    public Image fadeImage;              // 검은 이미지
    public float fadeDuration = 0.5f;    // 페이드 시간

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 씬 전환 시에도 유지하려면 주석 해제
        // DontDestroyOnLoad(gameObject);

        // 처음엔 투명하게
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false);
        }
    }

    // 페이드 아웃 (화면이 어두워짐)
    public IEnumerator FadeOut(float duration = -1f)
    {
        if (duration < 0) duration = fadeDuration;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            yield return StartCoroutine(Fade(0f, 1f, duration));
        }
    }

    // 페이드 인 (화면이 밝아짐)
    public IEnumerator FadeIn(float duration = -1f)
    {
        if (duration < 0) duration = fadeDuration;

        if (fadeImage != null)
        {
            yield return StartCoroutine(Fade(1f, 0f, duration));
            fadeImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }
}