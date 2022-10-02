using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("ManagerNull");
            }
            return _instance;
        }
    }

    [SerializeField] TextMeshProUGUI currentPointsText;    
    [SerializeField] TextMeshProUGUI highPointsText;    
    [SerializeField] TextMeshProUGUI lastPointsText;    
    public int currentPoints = 0;
    public int lastPoints = 0;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (this != _instance)
        {
            Destroy(this.gameObject);
        }
        highPointsText.text = PlayerPrefs.GetInt("hScore", lastPoints).ToString();
        lastPointsText.text = PlayerPrefs.GetInt("lScore", lastPoints).ToString();
    }

    public void AddPoints(int amt)
    {
        currentPoints += amt;
        currentPointsText.text = currentPoints.ToString();
    }

    public void SaveStats()
    {
        lastPoints = currentPoints;
        if (currentPoints > PlayerPrefs.GetInt("hScore", 0))
        {
            PlayerPrefs.SetInt("hScore", currentPoints);
        }
        PlayerPrefs.SetInt("lScore", lastPoints);
    }
}
