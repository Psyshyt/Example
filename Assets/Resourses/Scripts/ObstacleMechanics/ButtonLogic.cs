using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonLogic : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player;

    public GameObject TimePanel;
    public GameObject LeaderPanel;

    [SerializeField] private TMP_Text stopwatchText;
    [SerializeField] private TMP_Text _firstRun;

    private float elapsedTime;
    private bool isRunning;

    private List<float> LeaderTime; // Объявляем список для рекордов

    private void Awake()
    {
        LeaderTime = new List<float>(); // Инициализируем список в Awake
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime; 
            UpdateStopwatchText(); 
        }
    }

    public void StartObstacle()
    {
        isRunning = true;
        TimePanel.SetActive(true);
        LeaderPanel.SetActive(false);
    }

    public void EndObstacle()
    {
        isRunning = false;
        TimePanel.SetActive(false);
        LeaderPanel.SetActive(true);

        LeaderTime.Add(elapsedTime); 
        LeaderTime.Sort((x, y) => -y.CompareTo(x)); 

        _firstRun.text = ""; 
        foreach (float leaderTime in LeaderTime)
        {
            _firstRun.text += string.Format("{0:00}:{1:00}.{2:000}\n", 
                (int)(leaderTime / 60), 
                (int)leaderTime % 60, 
                (int)(leaderTime * 1000) % 1000); 
        }  
        TeleportPlayer();

        elapsedTime = 0; 
    }
    
    private void UpdateStopwatchText()
    {
        int milliseconds = (int)(elapsedTime * 1000) % 1000; 
        int seconds = (int)elapsedTime % 60; 
        int minutes = (int)(elapsedTime / 60) % 60;

        stopwatchText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    private void TeleportPlayer()
    {
        
        if (teleportTarget != null && player != null)
        {
            player.transform.position = teleportTarget.position; 
        }
    }
}
