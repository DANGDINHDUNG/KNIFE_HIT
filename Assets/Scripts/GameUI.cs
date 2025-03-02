using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject restartButton;

    [Header("Knife Count Display")]
    [SerializeField] private GameObject knifeCountPanel;
    [SerializeField] private GameObject knifeIconPrefab;
    [SerializeField] private Color usedKifeColor;
    private int knifeCountIndexToChange = 0;

    public void ShowRestartButton()
    {
        restartButton.SetActive(true);
    }

    public void ShowKnifeCount(int knifeCount)
    {
        knifeCountPanel.SetActive(true);
        for (int i = 0; i < knifeCount; i++)
        {
            GameObject knifeIcon = Instantiate(knifeIconPrefab, knifeCountPanel.transform);
        }
    }

    public void IncreasingKnifeCount()
    {
        GameObject knifeIcon = Instantiate(knifeIconPrefab, knifeCountPanel.transform);
    }

    public void ResetKnifeCount(int knifeCount)
    {
        for (int i = 0; i < knifeCount; i++)
        {
            ResetDisplayKnifeCount(i);
        }
        knifeCountIndexToChange = 0;
    }

    public void DecrementDisplayKnifeCount() 
    { 
        knifeCountPanel.transform.GetChild(knifeCountIndexToChange).GetComponent<Image>().color = usedKifeColor;
        knifeCountIndexToChange++;
    }

    public void ResetDisplayKnifeCount(int index)
    {
        knifeCountPanel.transform.GetChild(index).GetComponent<Image>().color = Color.white;
    }
}
