using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Ця функція просто миттєво вантажить сцену
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Метод для кнопки "ВИХІД"
    public void ExitGame()
    {
        Debug.Log("Гра закривається...");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} // Оця дужка найважливіша, вона закриває весь клас!