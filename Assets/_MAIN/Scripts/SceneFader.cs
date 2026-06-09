using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeSpeed = 1f;

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad("GameScene"));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        // fade in (затемнення)
        while (fadeGroup.alpha < 1)
        {
            fadeGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // чек трохи щоб не різало
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("VisualNovel");
    }
}