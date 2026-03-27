using System;
using TMPro;
using UnityEngine;

public class DeathCountUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathCounterText;
    private int deathCounter = 0;


    public void IncreaseDeathCounter()
    {
        this.deathCounter++;
        this.UpdateDeathCounterText();
    }

    private void UpdateDeathCounterText()
    {
        deathCounterText.text = "Deaths: " + this.deathCounter;
    }
}
