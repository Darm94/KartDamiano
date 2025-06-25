using UnityEngine;
using TMPro;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;

    [SerializeField] private int maxGates = 5;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject standardCanvas;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text finalTimeText;
    [SerializeField] private TMP_Text gateText;
    
    private int gatesPassed = 0;
    private float elapsedTime = 0f;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;
            timeText.text = $"Tempo: {elapsedTime:F2} s";
        }
    }

    public void RegisterGatePassed()
    {
        if (isGameOver) return;

        gatesPassed++;
        gateText.text = $"GATES: {gatesPassed}/5";

        if (gatesPassed >= maxGates)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        Time.timeScale = 0f; // ferma il gioco

        gameOverCanvas.SetActive(true);
        standardCanvas.SetActive(false);
        AudioListener.pause = true;
        finalTimeText.text = $"Tempo Finale:\n {elapsedTime:F2} s";
    }
}