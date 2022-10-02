using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] GameObject healthIconPrefab;
    List<HealthIcon> hearts = new List<HealthIcon>();
    PlayerHealth playerHealth;
    GameObject[] healthIcons;

    private void Start()
    {
        playerHealth = GameManager.Instance.playerTransform.gameObject.GetComponent<PlayerHealth>();
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        float maxHremainder = playerHealth.playerMaxHealth % 2;
        int hToMake = (int)((playerHealth.playerMaxHealth / 2) + maxHremainder);
        for (int i = 0; i < hToMake; i++)
        {
            CreatEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int hStatusRemainder = Mathf.Clamp((int)playerHealth.playerHealth - (i * 2), 0, 2);
            hearts[i].SetHeart((HeartState)hStatusRemainder);
        }
    }

    public void CreatEmptyHeart()
    {
        GameObject newHeart = Instantiate(healthIconPrefab, transform);

        HealthIcon hComp = newHeart.GetComponent<HealthIcon>();
        hComp.SetHeart(HeartState.Empty);
        hearts.Add(hComp);
    }

    private void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthIcon>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerHealthUpdated += DrawHearts;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerHealthUpdated -= DrawHearts;
    }
}
