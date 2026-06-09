using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueSwitcher : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textDisplay;
    public RawImage backgroundDisplay;
    public CanvasGroup fadeScreenGroup;

    [Header("Ending UI")]
    public GameObject dialoguePanel;
    public GameObject sceneButton; 
    public GameObject endingButton;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("0 - Intro")]
    public AudioClip bellSound;
    [TextArea(3, 10)] public string[] introSentences;

    [Header("1 - Church")]
    public Texture churchTexture;
    [TextArea(3, 10)] public string[] churchSentences;

    [Header("2 - Darkness")]
    public AudioClip doorSound;
    [TextArea(3, 10)] public string[] darkSentences;

    [Header("3 - Room")]
    public Texture roomTexture;
    [TextArea(3, 10)] public string[] roomSentences;

    [Header("4 - Darkness 2")]
    public AudioClip forestSound;
    [TextArea(3, 10)] public string[] darkSentences2;

    [Header("5 - Chapel")]
    public Texture chapelTexture;
    [TextArea(3, 10)] public string[] chapelSentences;

    [Header("6 - Final")]
    [TextArea(3, 10)] public string[] finalSentences;

    [Header("Next Scene")]
    public string nextSceneName = "VisualNovel2";

    [Header("Ending Scene")]
    public string endingSceneName = "EndingScene";

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float fadeSpeed = 1.5f;

    private int state = 0;
    private int index = 0;

    private bool isTyping = false;
    private bool isTransitioning = false;

    private Coroutine typingCoroutine;

    void Start()
    {
        fadeScreenGroup.alpha = 1f;

        backgroundDisplay.texture = churchTexture;
        backgroundDisplay.color = Color.white;

        if (audioSource != null && bellSound != null)
        {
            audioSource.PlayOneShot(bellSound);
        }

        StartCurrentDialogue();
    }

    void Update()
    {
        if (isTransitioning)
            return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                NextSentence();
            }
        }
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textDisplay.text = GetCurrentSentence();
        ApplyColorLogic();
        isTyping = false;
    }

    void NextSentence()
    {
        string[] current = GetCurrentArray();

        if (current == null || current.Length == 0)
            return;

        if (index < current.Length - 1)
        {
            index++;
            StartDialogue(current[index]);
        }
        else
        {
            if (state == 6)
            {
                // ТОТАЛЬНИЙ ПЕРЕХІД: якщо прочитали фразу "Досить...", наступний клік одразу вантажить нову сцену
                GoToNextScene();
                return;
            }
            else
            {
                SwitchState();
            }
        }
    }

    void SwitchState()
    {
        switch (state)
        {
            case 0: StartCoroutine(GoToChurch()); break;
            case 1: StartCoroutine(GoToDarkness()); break;
            case 2: StartCoroutine(GoToRoom()); break;
            case 3: StartCoroutine(GoToDarknessAfterRoom()); break;
            case 4: StartCoroutine(GoToChapel()); break;
            case 5: StartCoroutine(GoToFinalDarkness()); break;
        }
    }

    void StartCurrentDialogue()
    {
        index = 0;
        string[] current = GetCurrentArray();

        if (current != null && current.Length > 0)
        {
            StartDialogue(current[0]);
        }
    }

    void StartDialogue(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textDisplay.text = "";
        ApplyColorLogic();

        foreach (char letter in sentence)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void ApplyColorLogic()
    {
        if (state == 6)
        {
            textDisplay.color = Color.red;
        }
        else if (state == 4 && index == darkSentences2.Length - 1)
        {
            textDisplay.color = Color.red;
        }
        else
        {
            textDisplay.color = Color.white; 
        }
    }

    string GetCurrentSentence()
    {
        string[] current = GetCurrentArray();
        if (current == null || current.Length == 0) return "";
        return current[index];
    }

    string[] GetCurrentArray()
    {
        switch (state)
        {
            case 0: return introSentences;
            case 1: return churchSentences;
            case 2: return darkSentences;
            case 3: return roomSentences;
            case 4: return darkSentences2;
            case 5: return chapelSentences;
            case 6: return finalSentences;
        }
        return null;
    }

    IEnumerator GoToChurch()
    {
        isTransitioning = true;
        textDisplay.text = "";
        backgroundDisplay.texture = churchTexture;
        backgroundDisplay.color = Color.white;

        while (fadeScreenGroup.alpha > 0f)
        {
            fadeScreenGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        fadeScreenGroup.alpha = 0f;
        yield return new WaitForSeconds(0.2f);

        state = 1;
        isTransitioning = false;
        StartCurrentDialogue();
    }

    IEnumerator GoToDarkness()
    {
        isTransitioning = true;
        textDisplay.text = "";

        while (fadeScreenGroup.alpha < 1f)
        {
            fadeScreenGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        fadeScreenGroup.alpha = 1f;

        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }
        yield return new WaitForSeconds(1f);

        state = 2;
        isTransitioning = false;
        StartCurrentDialogue();
    }

    IEnumerator GoToRoom()
    {
        isTransitioning = true;
        textDisplay.text = "";
        backgroundDisplay.texture = roomTexture;
        backgroundDisplay.color = Color.white;

        while (fadeScreenGroup.alpha > 0f)
        {
            fadeScreenGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        fadeScreenGroup.alpha = 0f;
        yield return new WaitForSeconds(0.2f);

        state = 3;
        isTransitioning = false;
        StartCurrentDialogue();
    }

    IEnumerator GoToDarknessAfterRoom()
    {
        isTransitioning = true;
        textDisplay.text = "";

        while (fadeScreenGroup.alpha < 1f)
        {
            fadeScreenGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        fadeScreenGroup.alpha = 1f;

        if (audioSource != null && forestSound != null)
        {
            audioSource.PlayOneShot(forestSound);
        }
        yield return new WaitForSeconds(1f);

        state = 4;
        isTransitioning = false;
        StartCurrentDialogue();
    }

    IEnumerator GoToChapel()
    {
        isTransitioning = true;
        textDisplay.text = "";
        backgroundDisplay.texture = chapelTexture;
        backgroundDisplay.color = Color.white;

        while (fadeScreenGroup.alpha > 0f)
        {
            fadeScreenGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        fadeScreenGroup.alpha = 0f;
        yield return new WaitForSeconds(0.2f);

        state = 5;
        isTransitioning = false;
        StartCurrentDialogue();
    }

    IEnumerator GoToFinalDarkness()
    {
        isTransitioning = true;
        textDisplay.text = "";
        yield return new WaitForSeconds(0.5f);

        state = 6;
        isTransitioning = false;
        StartCurrentDialogue();
    }

    public void GoToNextScene()
    {
        LoadSceneSafely(nextSceneName);
    }

    public void ShowEnding()
    {
        LoadSceneSafely(endingSceneName);
    }

    void LoadSceneSafely(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("Scene name is not configured.");
            return;
        }

        isTransitioning = true;
        SceneManager.LoadScene(sceneName);
    }
}