using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Header("Health Settings")]
    public int playerHealth = 3;
    [SerializeField] private Image[] hearts;

    [Header("Acorn UI")]
    public Image   ghindaIcon;   // <–– referință la UI Image-ul ghindei
    public Text    ghindaText;   // textul cu counter-ul de ghinde
    private GhindaManager ghindaManager;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button     restartButton;
    public Button     menuButton;

    private void Start()
    {
        // preia ghindaText dacă nu e setat în Inspector
        ghindaManager = FindObjectOfType<GhindaManager>();
        if (ghindaText == null && ghindaManager != null)
            ghindaText = ghindaManager.ghindaText;

        // ascunde GameOver Panel și leagă butoanele
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(false);

        if (restartButton != null) 
            restartButton.onClick.AddListener(OnRestartPressed);

        if (menuButton != null) 
            menuButton.onClick.AddListener(OnMenuPressed);

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        // colorează inimioarele
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].color = (i < playerHealth) ? Color.red : Color.black;

        // dacă 0 vieți → Game Over
        if (playerHealth <= 0)
            ShowGameOver();
    }

    private void ShowGameOver()
    {
        // oprește timpul
        Time.timeScale = 0f;

        // afișează panel-ul de Game Over
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(true);

        // ascunde inimioarele
        foreach (var heart in hearts)
            heart.gameObject.SetActive(false);

        // ascunde textul de ghinde
        if (ghindaText != null)
            ghindaText.gameObject.SetActive(false);

        // *ASCUNDE ȘI ICONIȚA GHINDEI*
        if (ghindaIcon != null)
            ghindaIcon.gameObject.SetActive(false);
    }

    private void OnRestartPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // schimbă cu numele scenei tale
    }
}
