using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pausePanel;

    [Header("Main Menu")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        LockGameCursor();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.pKey.wasPressedThisFrame ||
            (isPaused && Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        // Giữ chuột hiện và có thể click khi đang pause,
        // kể cả khi một script khác cố khóa chuột.
        if (isPaused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PauseGame()
    {
        if (isPaused)
            return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game paused.");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        LockGameCursor();

        Debug.Log("Game resumed.");
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;

        isPaused = false;

        Scene activeScene = SceneManager.GetActiveScene();

        Debug.Log("Reset game: " + activeScene.name);

        SceneManager.LoadScene(activeScene.buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (string.IsNullOrWhiteSpace(mainMenuSceneName))
        {
            Debug.LogError("Chưa nhập tên scene Main Menu.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
        {
            Debug.LogError(
                "Không tìm thấy scene '" + mainMenuSceneName +
                "' trong Build Profiles. Hãy thêm scene vào Scene List."
            );
            return;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void LockGameCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}