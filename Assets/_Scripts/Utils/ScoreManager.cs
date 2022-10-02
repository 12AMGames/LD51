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
    float currentPoints = 0;

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
    }

    public void AddPoints(float amt)
    {
        currentPoints += amt;
        currentPointsText.text = currentPoints.ToString();
    }
}
