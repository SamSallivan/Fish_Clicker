using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public TMP_Text fishPerClickText;
    public TMP_Text fishPerSecondText;
    
    public int fishCount;
    public TMP_Text fishCountText;
    
    public int fishingRodCount = 1;
    public TMP_Text fishingRodCountText;
    
    public int fishbreedingTankCount;
    public TMP_Text fishbreedingTankCountText;
    
    public float fishbreedingTankTime;
    
    public int saltFishCount;
    public TMP_Text saltFishCountText;

    public void Update()
    {
        fishPerClickText.text = fishingRodCount.ToString();
        fishPerSecondText.text = fishbreedingTankCount.ToString();
        
        fishCountText.text = fishCount.ToString();
        fishingRodCountText.text = fishingRodCount.ToString();
        fishbreedingTankCountText.text = fishbreedingTankCount.ToString();
        //saltFishCountText.text = saltFishCount.ToString();
        
        fishbreedingTankTime += Time.deltaTime;
        if (fishbreedingTankTime >= 1)
        {
            fishbreedingTankTime = 0;
            fishCount += fishbreedingTankCount;
        }
    }

    public void Fish()
    {
        fishCount += fishingRodCount;
    }

    public void BuyFishingRod()
    {
        if (fishCount >= 10)
        {
            fishCount -= 10;
            fishingRodCount += 1;
        }
    }
    

    public void BuyFishBreedingTank()
    {
        if (fishCount >= 100)
        {
            fishCount -= 100;
            fishbreedingTankCount += 1;
        }
    }
}
