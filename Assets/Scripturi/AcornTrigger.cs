using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;  // pentru TextMeshProUGUI

public class GoldenAcorn : MonoBehaviour
{
    [Header("Next Level")]
    public string nextLevelScene;

    [Header("Requirements")]
    public int requiredGhinde = 20;
    private GhindaManager ghindaManager;

    [Header("Completion Popup UI (mai mic decât ecranul)")]
    public GameObject completionUI;  // Panel-ul mic cu imagine + text + butoane
    
    public Image ghindaIcon;     // iconița ghindei
    public Text  ghindaText;     // textul cu numărul de ghinde

    [SerializeField] private Image[] hearts;

    public Button yesButton;
    public Button noButton;

    [Header("Warning UI")]
    public TextMeshProUGUI warningText;  // TextMeshPro în loc de legacy Text
    public float warningDuration = 2f;

    private CircleCollider2D triggerCollider;

    void Awake()
    {
        // găsește managerul de ghinde
        ghindaManager = FindObjectOfType<GhindaManager>();
        if (ghindaManager == null)
            Debug.LogWarning("Nu există GhindaManager în scenă!");

        // referință la collider
        triggerCollider = GetComponent<CircleCollider2D>();
        if (triggerCollider == null)
            //Debug.LogError("GoldenAcorn necesită CircleCollider2D (IsTrigger = true)!");

        // popup-urile ascunse la start
        if (completionUI != null) completionUI.SetActive(false);
        if (warningText   != null) warningText.gameObject.SetActive(false);

        // legare butoane
        if (yesButton != null) yesButton.onClick.AddListener(OnYesPressed);
        if (noButton  != null) noButton .onClick.AddListener(OnNoPressed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
 
        if (!other.CompareTag("Player") || ghindaManager == null)
            return;

        if (ghindaManager.ghindaCount < requiredGhinde)
        {
            int left = requiredGhinde - ghindaManager.ghindaCount;
            if (warningText != null)
                StartCoroutine(ShowWarning($"Mai trebuie să aduni încă {left} ghinde!"));
            return;
        }

        triggerCollider.enabled = false;
        Time.timeScale = 0f;

        if (completionUI != null)
            completionUI.SetActive(true);

        if (ghindaIcon != null)
            ghindaIcon.gameObject.SetActive(false);
        if (ghindaText != null)
            ghindaText.gameObject.SetActive(false);
        foreach (var heart in hearts)
            heart.gameObject.SetActive(false);
    }

    private IEnumerator ShowWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(warningDuration);
        warningText.gameObject.SetActive(false);
    }

    private void OnYesPressed()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nextLevelScene))
            SceneManager.LoadScene(nextLevelScene);
    }

    private void OnNoPressed()
    {
        if (completionUI != null)
            completionUI.SetActive(false);


        Time.timeScale = 1f;
        triggerCollider.enabled = true;
    }
}
