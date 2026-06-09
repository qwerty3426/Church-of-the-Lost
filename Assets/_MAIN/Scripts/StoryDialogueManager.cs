using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryDialogueManager : MonoBehaviour
{
    [Header("UI Елементи Діалогів")]
    public TextMeshProUGUI textDisplay;
    public GameObject choicePanel;
    public RawImage backgroundDisplay;
    public GameObject dialoguePane; // Твій DialoguePane з ієрархії

    [Header("ЕФЕКТНІ КІНО-ТИТРИ (ПО ЦЕНТРУ)")]
    public TextMeshProUGUI creditsTextDisplay; // Сюди новий текст (CreditsText)
    public GameObject sponsorPhotoObject;     // Сюди об'єкт картинки з фоткою Олі
    public float fadeActionSpeed = 1.5f;       // Швидкість появи тексту

    [Header("Об'єкти для переходів")]
    public CanvasGroup characterCanvasGroup; // жінка
    public CanvasGroup manCanvasGroup; // чоловік
    public CanvasGroup priestCanvasGroup; // батюшка

    public CanvasGroup introScreenGroup;
    public CanvasGroup fadeScreenGroup;

    [Header("0 - Налаштування вступу (Intro)")]
    [TextArea(3, 10)]
    public List<string> introSentences;

    [Header("1 - Церква (Church)")]
    public Texture churchTexture;

    [Header("2 - Фінальна локація")]
    public Texture finalTexture;

    [Header("Кнопки вибору")]
    public Button posBtn;
    public Button neuBtn;
    public Button negBtn;

    public TextMeshProUGUI posText;
    public TextMeshProUGUI neuText;
    public TextMeshProUGUI negText;

    [Header("Фінальні репліки")]
    [TextArea(3, 10)]
    public List<string> finalSentences;
    
    [Header("Текст перед батюшкою")]
    [TextArea(3, 10)]
    public List<string> priestIntroSentences;
    
    [Header("Нейтральна кінцівка")]
    public Texture neutralEnding1;
    public Texture neutralEnding2;
    public Texture neutralEnding3;
    public Texture neutralEnding4;

    [TextArea(3, 10)]
    public List<string> neutralEndingText1;
    [TextArea(3, 10)]
    public List<string> neutralEndingText2;
    [TextArea(3, 10)]
    public List<string> neutralEndingText3;
    [TextArea(3, 10)]
    public List<string> neutralEndingText4;

    [Header("Погана кінцівка")]
    public Texture badEnding1;
    public Texture badEnding2;

    [TextArea(3, 10)]
    public List<string> badEndingText1;
    [TextArea(3, 10)]
    public List<string> badEndingText2;

    [Header("Хороша кінцівка")]
    public Texture goodEnding1;
    public Texture goodEnding2;

    [TextArea(3, 10)]
    public List<string> goodEndingText1;
    [TextArea(3, 10)]
    public List<string> goodEndingText2;

    [Header("Швидкість тексту гри")]
    public float typingSpeed = 0.08f; 
    public float fadeSpeed = 1.0f;

    private bool waitingForClick = false;
    private bool skipTyping = false;

    private int selectedChoice = 0;
    private int goodPoints = 0;
    private int neutralPoints = 0;
    private int badPoints = 0;

    void Start()
    {
        choicePanel.SetActive(false);

        if (introScreenGroup != null) introScreenGroup.alpha = 1f;
        if (fadeScreenGroup != null) fadeScreenGroup.alpha = 0f;
        if (characterCanvasGroup != null) characterCanvasGroup.alpha = 0f;
        if (manCanvasGroup != null) manCanvasGroup.alpha = 0f;
        if (priestCanvasGroup != null) priestCanvasGroup.alpha = 0f;
        
        if (sponsorPhotoObject != null) sponsorPhotoObject.SetActive(false);
        if (creditsTextDisplay != null) creditsTextDisplay.text = "";

        // ЗАПУСКАЄМО ІСТОРІЮ АВТОМАТИЧНО НА СТАРТІ
        StartCoroutine(FullStoryRoutine());
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !choicePanel.activeSelf)
        {
            if (!waitingForClick) skipTyping = true;
            else waitingForClick = false;
        }
    }

    IEnumerator FullStoryRoutine()
    {
        // УВІМКНЕМО ПЛАШКУ ДІАЛОГІВ, ЩОБ ПЕРШЕ СЛОВО БУЛО ВИДНО!
        if (dialoguePane != null) dialoguePane.SetActive(true);

        // ВСТУП (Тут почне писатися твій "лох")
        foreach (string line in introSentences) yield return Say(line);
        textDisplay.text = "";
        yield return new WaitForSeconds(1f);

        // ПОЯВА ЦЕРКВИ
        if (backgroundDisplay != null) backgroundDisplay.texture = churchTexture;
        yield return StartCoroutine(FadeEffect(introScreenGroup, 0f));
        yield return StartCoroutine(FadeEffect(characterCanvasGroup, 1f));

        // ДІАЛОГ ЖІНКИ
        yield return Say("Отче…");
        yield return Say("Я вже не знаю, чи можна це назвати каяттям.");
        yield return Say("Можливо, я просто втомилася мовчати.");
        yield return ShowChoices("", "Я вас слухаю.", "");
        yield return Say("Дивно знову бути тут.");
        yield return Say("Стільки років минуло…");
        yield return Say("Навіть тиша тут та сама.");
        yield return ShowChoices("Що вас привело сьогодні?", "Ви прийшли зізнатися?", "Говоріть прямо.");
        yield return Say("Була одна сестра…");
        yield return Say("Сестра Анна.");
        yield return Say("Вона дізналася правду.");
        yield return ShowChoices("Що саме вона дізналася?", "Про що ви говорите?", "Не говоріть загадками.");
        yield return Say("Анна хотіла все розповісти людям.");
        yield return ShowChoices("І ви хотіли її зупинити?", "Чому це вас так налякало?", "Що ви з нею зробили?");
        yield return Say("Бо вона не розуміла.");
        yield return Say("Люди не шукають правди.");
        yield return ShowChoices("", "І що сталося із сестрою Анною?", "");
        yield return Say("Тієї ночі… вона не вийшла за двері.");
        yield return ShowChoices("", "Ви її вбили?", "");
        yield return Say("Я молюся щодня.");
        yield return Say("Але щоночі я чую її кроки.");
        yield return Say("Воно знає, що я тут.");

        // ФІНАЛ ЖІНКИ
        textDisplay.text = ""; 
        yield return StartCoroutine(FadeEffect(characterCanvasGroup, 0f));
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));
        if (backgroundDisplay != null) backgroundDisplay.texture = finalTexture;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));

        foreach (string line in finalSentences) yield return Say(line);
        yield return Say("І що це було?");

        // ПЕРЕХІД ДО ЧОЛОВІКА
        textDisplay.text = ""; 
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));
        yield return StartCoroutine(FadeEffect(manCanvasGroup, 1f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));

        // ДІАЛОГ ЧОЛОВІКА
        yield return Say("...");
        yield return Say("Ви теж це чули?");
        yield return Say("Здається вона приходить сюди щоночі.");
        yield return new WaitForSeconds(1f);
        yield return Say("Пробачте...");
        yield return Say("Я навіть не представився.");
        yield return Say("Андрій.");
        yield return Say("Я давно сюди не приходив.");
        yield return ShowChoices("Щось вас привело сюди.", "Ви виглядаєте наляканим.", "Говоріть вже нормально.");
        yield return Say("Можливо.");
        yield return Say("Просто... останнім часом я погано сплю.");
        yield return Say("Іноді мені здається, ніби я досі чую той звук.");
        yield return ShowChoices("Який звук?", "Що саме вам сниться?", "Ви поводитесь дивно.");
        yield return Say("Не знаю.");
        yield return Say("Може, це просто стара церква.");
        yield return Say("Дерево тут завжди скрипіло.");
        yield return Say("Але тієї ночі...");
        yield return Say("...воно звучало інакше.");
        yield return ShowChoices("Що сталося тієї ночі?", "Ви були тут?", "Що ви приховуєте?");
        yield return Say("Я допомагав після служби.");
        yield return Say("Мав уже йти додому.");
        yield return Say("Та почув голоси.");
        yield return Say("Жіночі.");
        yield return Say("Одна з них плакала.");
        yield return ShowChoices("І що було далі?", "Ви бачили когось?", "Чому ви мовчите?" );
        yield return Say("Я не пам’ятаю всього.");
        yield return Say("Чесно.");
        yield return Say("Пам’ятаю тільки...");
        yield return Say("...як раптом усе стихло.");
        yield return Say("Ніби сама церква затамувала подих.");
        yield return Say("Я багато років переконував себе, що нічого не сталося.");
        yield return Say("Що я просто не так усе зрозумів.");
        yield return Say("Що якби це було важливо, хтось бы заговорив.");
        yield return Say("Але ніхто так і не заговорив.");
        yield return Say("І я теж мовчав.");
        yield return Say("Мабуть, саме тому я сьогодні тут.");
        yield return Say("Бо деякі речі не зникають, навіть через роки.");
        yield return Say("Воно просто чекають.");
        yield return Say("Чекають, поки хтось наважиться подивитися на них знову.");
        yield return Say("Скоро ви все дізнаєтеся.");
        
        // ПЕРЕХІД ДО БАТЮШКИ
        textDisplay.text = ""; 
        yield return StartCoroutine(FadeEffect(manCanvasGroup, 0f));
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));

        foreach (string line in priestIntroSentences) yield return Say(line);

        textDisplay.text = ""; 
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        yield return StartCoroutine(priestCanvasGroup != null ? FadeEffect(priestCanvasGroup, 1f) : null);

        // ДІАЛОГ БАТЮШКИ
        yield return Say("Мир вам, дитино.");
        yield return Say("Кажуть, церква пам'ятає кожну молитву.");
        yield return Say("Та є речі, які вона пам'ятає значно довше.");
        yield return Say("Ви прийшли шукати відповіді.");
        yield return Say("Але спершу мені потрібно поставити вам одне запитання.");
        yield return ShowChoices("Яке саме?", "Ви знаєте, що тут сталося?", "Мені набридли загадки.");
        yield return Say("Скажіть...");
        yield return Say("Якби людина зробила щось жахливе.");
        yield return Say("Але була переконана, що чинить правильно.");
        yield return Say("Чи зробило б це її винною?");
        yield return ShowChoices("Так.", "Не знаю.", "Ні.");
        yield return Say("Цікаво.");
        yield return Say("Колись я поставив собі те саме запитання.");
        yield return Say("І досі не впевнений у відповіді.");
        yield return ShowChoices("Про що ви говорите?", "Ви когось захищаєте?", "Ви щось приховуєте.");
        yield return Say("Усі ми щось приховуємо.");
        yield return Say("Одні приховують правду.");
        yield return Say("Інші — власний страх.");
        yield return Say("А дехто приховує мовчання.");
        yield return ShowChoices("Що сталося тієї ночі?", "Ви були тут?", "Знову загадки.");
        yield return Say("Я був тут багато ночей.");
        yield return Say("Церква ніколи не буває порожньою.");
        yield return Say("Навіть коли здається, що нікого немає.");
        yield return Say("Стіни слухають.");
        yield return Say("Підлога пам'ятає.");
        yield return Say("А люди... люди забувають.");
        yield return ShowChoices("Ви знали сестру Анну?", "Чому всі про неї говорять?", "Ви уникаєте відповіді.");
        yield return Say("Анна була доброю людиною.");
        yield return Say("Занадто доброю для цього місця.");
        yield return Say("Вона ставила запитання.");
        yield return Say("А декілька запитань небезпечні.");
        yield return ShowChoices("Небезпечні для кого?", "Що вона дізналася?", "Хтось змусив її мовчати?");
        yield return Say("Люди завжди шукають прості відповіді.");
        yield return Say("Одного винного.");
        yield return Say("Одного монстра.");
        yield return Say("Але справжній гріх рідко належить одній людині.");
        yield return Say("Іноді його несуть усі.");
        yield return ShowChoices("Що ви маєте на увазі?", "Ви говорите про себе?", "Я вам не вірю.");
        yield return Say("Можливо.");
        yield return Say("Можливо, кожен із нас винен трохи більше, ніж готовий визнати.");
        yield return Say("Хтось брехав.");
        yield return Say("Хтось мовчав.");
        yield return Say("Хтось просто відвернувся.");
        yield return Say("І цього виявилося достатньо.");
        yield return Say("Дивно.");
        yield return Say("Минуло стільки років.");
        yield return Say("А вони все ще повертаються сюди.");
        yield return Say("Спочатку одна.");
        yield return Say("Потім інший.");
        yield return Say("Наче шукають те, що давно поховане.");
        yield return Say("Але деякі речі не можна поховати.");
        yield return Say("Особливо правду.");
        yield return ShowChoices("Правду потрібно відкрити людям.", "Я не хочу мати з цим нічого спільного.", "Усього цього ніколи не було.");

        if (goodPoints > neutralPoints && goodPoints > badPoints)
        {
            textDisplay.text = ""; 
            yield return StartCoroutine(GoodEnding());
        }
        else if (neutralPoints > goodPoints && neutralPoints > badPoints)
        {
            textDisplay.text = ""; 
            yield return StartCoroutine(NeutralEnding());
        }
        else
        {
            textDisplay.text = ""; 
            yield return StartCoroutine(BadEnding());
        }
    }

    IEnumerator GoodEnding()
    {
        if (priestCanvasGroup != null) yield return StartCoroutine(FadeEffect(priestCanvasGroup, 0f));
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = goodEnding1;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in goodEndingText1) yield return Say(line);
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = goodEnding2;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in goodEndingText2) yield return Say(line);

        textDisplay.text = ""; 
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));
        
        ClearDialogueBeforeCredits();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowCredits());
    }

    IEnumerator NeutralEnding()
    {
        if (priestCanvasGroup != null) yield return StartCoroutine(FadeEffect(priestCanvasGroup, 0f));
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = neutralEnding1;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in neutralEndingText1) yield return Say(line);
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = neutralEnding2;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in neutralEndingText2) yield return Say(line);
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = neutralEnding3;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in neutralEndingText3) yield return Say(line);
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = neutralEnding4;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in neutralEndingText4) yield return Say(line);

        textDisplay.text = ""; 
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));
        
        ClearDialogueBeforeCredits();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowCredits());
    }

    IEnumerator BadEnding()
    {
        if (priestCanvasGroup != null) yield return StartCoroutine(FadeEffect(priestCanvasGroup, 0f));
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = badEnding1;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in badEndingText1) yield return Say(line);
        textDisplay.text = "";
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));

        backgroundDisplay.texture = badEnding2;
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        foreach (var line in badEndingText2) yield return Say(line);

        textDisplay.text = ""; 
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 1f));
        
        ClearDialogueBeforeCredits();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowCredits());
    }

    void ClearDialogueBeforeCredits()
    {
        if (backgroundDisplay != null)
        {
            backgroundDisplay.texture = null;
            backgroundDisplay.color = Color.black; 
        }
        if (dialoguePane != null) dialoguePane.SetActive(false); 
        textDisplay.text = "";
    }

    IEnumerator ShowCredits()
    {
        if (creditsTextDisplay == null) yield break;

        string[] credits =
        {
            "Працювали над проєктом",
            "Коцюба Артем",
            "Жабчик Дарина",
            "Рогалінська Олександра",
            "ДЯКУЄМО НАШОМУ СПОНСОРУ"
        };

        CanvasGroup creditsGroup = creditsTextDisplay.GetComponent<CanvasGroup>();
        if (creditsGroup == null) creditsGroup = creditsTextDisplay.gameObject.AddComponent<CanvasGroup>();

        RectTransform textRect = creditsTextDisplay.GetComponent<RectTransform>();
        Vector2 originalPosition = textRect.anchoredPosition;

        for (int i = 0; i < credits.Length; i++)
        {
            creditsGroup.alpha = 0f;
            creditsTextDisplay.text = credits[i];
            textRect.anchoredPosition = originalPosition + new Vector2(0, -15f);

            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * fadeActionSpeed;
                creditsGroup.alpha = Mathf.Lerp(0f, 1f, time);
                textRect.anchoredPosition = Vector2.Lerp(originalPosition + new Vector2(0, -15f), originalPosition, time);
                yield return null;
            }

            yield return new WaitForSeconds(2.2f);

            time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * fadeActionSpeed;
                creditsGroup.alpha = Mathf.Lerp(1f, 0f, time);
                yield return null;
            }
            yield return new WaitForSeconds(0.4f);
        }

        if (sponsorPhotoObject != null)
        {
            creditsTextDisplay.text = "OLYA RIP";
            textRect.anchoredPosition = originalPosition - new Vector2(0, 80f); 

            sponsorPhotoObject.SetActive(true); 

            float spawnTime = 0f;
            while (spawnTime < 1f)
            {
                spawnTime += Time.deltaTime * fadeActionSpeed;
                creditsGroup.alpha = Mathf.Lerp(0f, 1f, spawnTime);
                yield return null;
            }

            yield return new WaitForSeconds(4.5f); 

            spawnTime = 0f;
            while (spawnTime < 1f)
            {
                spawnTime += Time.deltaTime * fadeActionSpeed;
                creditsGroup.alpha = Mathf.Lerp(1f, 0f, spawnTime);
                yield return null;
            }
            sponsorPhotoObject.SetActive(false); 
        }

        creditsTextDisplay.text = "";
    }

    IEnumerator BadChoiceEffect()
    {
        textDisplay.color = Color.red;
        StartCoroutine(FadeEffect(fadeScreenGroup, 0.4f));
        yield return Say("...");
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(FadeEffect(fadeScreenGroup, 0f));
        textDisplay.color = Color.white;
    }

    IEnumerator FadeEffect(CanvasGroup cg, float targetAlpha)
    {
        if (cg == null) yield break;
        while (!Mathf.Approximately(cg.alpha, targetAlpha))
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
            yield return null;
        }
    }

    IEnumerator Say(string line)
    {
        waitingForClick = false;
        skipTyping = false;
        textDisplay.text = "";

        foreach (char letter in line)
        {
            if (skipTyping) break;
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        textDisplay.text = line;
        yield return new WaitForSeconds(0.05f);
        waitingForClick = true;

        while (waitingForClick) yield return null;
    }

    IEnumerator ShowChoices(string p, string n, string neg)
    {
        posText.text = p; neuText.text = n; negText.text = neg;
        posBtn.gameObject.SetActive(!string.IsNullOrEmpty(p));
        neuBtn.gameObject.SetActive(!string.IsNullOrEmpty(n));
        negBtn.gameObject.SetActive(!string.IsNullOrEmpty(neg));

        choicePanel.SetActive(true);
        bool clicked = false; selectedChoice = 0;

        posBtn.onClick.RemoveAllListeners();
        neuBtn.onClick.RemoveAllListeners();
        negBtn.onClick.RemoveAllListeners();

        posBtn.onClick.AddListener(() => { selectedChoice = 1; clicked = true; });
        neuBtn.onClick.AddListener(() => { selectedChoice = 2; clicked = true; });
        negBtn.onClick.AddListener(() => { selectedChoice = 3; clicked = true; });

        while (!clicked) yield return null;
        choicePanel.SetActive(false);

        if (selectedChoice == 1) goodPoints++;
        else if (selectedChoice == 2) neutralPoints++;
        else if (selectedChoice == 3) badPoints++;

        if (selectedChoice == 3) yield return StartCoroutine(BadChoiceEffect());
    }
}