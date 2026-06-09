using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeSpeed = 1f;

    [SerializeField] private string gameSceneName = "VisualNovel";

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad(gameSceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("SceneFader has no scene configured.");
            yield break;
        }

        if (fadeGroup == null)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        // fade in (затемнення)
        while (fadeGroup.alpha < 1)
        {
            fadeGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // чек трохи щоб не різало
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(sceneName);
    }
}