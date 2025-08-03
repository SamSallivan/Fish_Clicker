using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Main")]
    public float fishCount;
    public TMP_Text fishCountText;
    
    public TMP_Text perClickText;
    public TMP_Text perSecondText;
    public TMP_Text lastSecondText;

    [Header("Rod")] 
    public GameObject rodParent;
    public float rodCount = 1;
    public TMP_Text rodCountText;
    public float rodCostBase = 10;
    public float rodCost = 10;
    public TMP_Text rodCostText;
    
    [Header("Rod Factory")]
    public GameObject rodFactoryParent;
    public int rodFactoryCount;
    public TMP_Text rodFactoryCountText;
    public float rodFactoryCostBase = 1000;
    public float rodFactoryCost = 1000;
    public TMP_Text rodFactoryCostText;
    
    [Header("Tank")]
    public GameObject tankParent;
    public float tankCount;
    public TMP_Text tankCountText;
    public float tankCostBase = 100;
    public float tankCost = 100;
    public TMP_Text tankCostText;
    
    [Header("Tank Factory")]
    public GameObject tankFactoryParent;
    public int tankFactoryCount;
    public TMP_Text tankFactoryCountText;
    public float tankFactoryCostBase = 10000;
    public float tankFactoryCost = 10000;
    public TMP_Text tankFactoryCostText;
    
    [Header("Vessel")]
    public GameObject vesselParent;
    public int vesselCount;
    public TMP_Text vesselCountText;
    public float vesselCostBase = 100;
    public float vesselCost = 100;
    public TMP_Text vesselCostText;
    
    public float vesselCost2 = 150;
    public TMP_Text vesselCostText2;
    
    [Header("Aquarium")]
    public GameObject aquariumParent;
    public int aquariumCount;
    public TMP_Text aquariumCountText;
    public float aquariumCostBase = 100;
    public float aquariumCost = 100;
    public TMP_Text aquariumCostText;
    
    public float aquariumCost2 = 75;
    public TMP_Text aquariumCostText2;
    
    [Header("Other")]
    public float tankTime;
    public float aquariumTime;
    public float rodFactoryTime;
    public float tankFactoryTime;
    public GameObject fishParent;
    public GameObject fishPrefab;
    
    private List<float> log = new List<float>();

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        //Text
        fishCountText.text = "" + Mathf.RoundToInt(fishCount);
        
        perClickText.text = Mathf.RoundToInt(rodCount + vesselCount * vesselCost2 * 1.5f).ToString();
        perSecondText.text = Mathf.RoundToInt(tankCount * 10f + aquariumCount * aquariumCost2 * 15f).ToString();
        
        rodCountText.text = Mathf.RoundToInt(rodCount).ToString();
        tankCountText.text = Mathf.RoundToInt(tankCount).ToString();
        rodFactoryCountText.text = rodFactoryCount.ToString();
        tankFactoryCountText.text = tankFactoryCount.ToString();
        vesselCountText.text = vesselCount.ToString();
        aquariumCountText.text = aquariumCount.ToString();

        //Per second
        if (tankCount > 0)
        {
            tankTime += Time.deltaTime;
            float minInterval = 1f / (tankCount * 10f);
            if (tankTime >= minInterval)
            {
                fishCount += tankTime / minInterval;
                StartCoroutine(SpawnFish(tankParent.transform.position, Mathf.RoundToInt(tankTime / minInterval)));
                tankTime = 0;
            }
        }
        
        if (aquariumCount > 0)
        {
            aquariumTime += Time.deltaTime;
            float minInterval = 1f / (aquariumCount * aquariumCost2 * 15f);
            if (aquariumTime >= minInterval)
            {
                fishCount += aquariumTime / minInterval;
                StartCoroutine(SpawnFish(aquariumParent.transform.position, Mathf.RoundToInt(aquariumTime / minInterval)));
                aquariumTime = 0;
            }
        }

        if (rodFactoryCount > 0)
        {
            rodFactoryTime += Time.deltaTime;
            float minInterval = 1f / rodFactoryCount;
            if (rodFactoryTime >= minInterval)
            {
                rodCount += rodFactoryTime / minInterval;
                rodFactoryTime = 0;
            }
        }

        if (tankFactoryCount > 0)
        {
            tankFactoryTime += Time.deltaTime;
            float minInterval = 1f / tankFactoryCount;
            if (tankFactoryTime >= minInterval)
            {
                tankCount += tankFactoryTime / minInterval;
                tankFactoryTime = 0;
            }
        }
        
        //Unlock
        if (!rodParent.activeInHierarchy && fishCount >= rodCostBase)
        {
            rodParent.SetActive(true);
            rodCostText.text = "Cost " + Mathf.Round(rodCost) + " Fishes";
        }
        
        if (!rodFactoryParent.activeInHierarchy && fishCount >= rodFactoryCostBase)
        {
            rodFactoryParent.SetActive(true);
            rodFactoryCostText.text = "Cost " + Mathf.Round(rodFactoryCostBase) + " Fishes";
        }
        
        if (!tankParent.activeInHierarchy && fishCount >= tankCost)
        {
            tankParent.SetActive(true);
            tankCostText.text = "Cost " + Mathf.Round(tankCostBase) + " Fishes";
        }
        
        if (!tankFactoryParent.activeInHierarchy && fishCount >= tankFactoryCostBase)
        {
            tankFactoryParent.SetActive(true);
            tankFactoryCostText.text = "Cost " + Mathf.Round(tankFactoryCostBase) + " Fishes";
        }
        
        if (!vesselParent.activeInHierarchy && fishCount >= vesselCost)
        {
            vesselParent.SetActive(true);
            vesselCostText.text = "Cost " + Mathf.Round(vesselCost) + " Fishes";
            
            vesselCostText2.text = "Cost " + Mathf.Round(vesselCost2) + " Fishing Rods";
        }
        
        if (!aquariumParent.activeInHierarchy && fishCount >= aquariumCost)
        {
            aquariumParent.SetActive(true);
            aquariumCostText.text = "Cost " + Mathf.Round(aquariumCost) + " Fishes";
            
            aquariumCostText2.text = "Cost " + Mathf.Round(aquariumCost2) + " Breeding Tanks";
        }
        
    }

    public void FixedUpdate()
    {
        log.Add(fishCount);
        if (log.Count >= 1/Time.fixedDeltaTime)
        {
            log.RemoveAt(0);
        }

        int fps = Mathf.RoundToInt(fishCount - log[0]);
        lastSecondText.text = fps.ToString();
    }

    public void Fish()
    {
        fishCount += rodCount;
        StartCoroutine(SpawnFish(rodParent.transform.position, Mathf.RoundToInt(rodCount)));
        
        fishCount += vesselCount * vesselCost2 * 1.5f;
        StartCoroutine(SpawnFish(vesselParent.transform.position, Mathf.RoundToInt(vesselCount * 200f)));
    }

    public void BuyFishingRod()
    {
        if (fishCount >= rodCost)
        {
            rodCount += 1;
            fishCount -= rodCost;
            rodCost += rodCostBase;
            rodCostText.text = "Cost " + Mathf.Round(rodCost) + " Fishes";
        }
    }
    

    public void BuyFishBreedingTank()
    {
        if (fishCount >= tankCost)
        {
            tankCount += 1;
            fishCount -= tankCost;
            tankCost += tankCostBase / 2;
            tankCostText.text = "Cost " + Mathf.Round(tankCost) + " Fishes";
        }
    }

    public void BuyFishingRodFactory()
    {
        if (fishCount >= rodFactoryCost)
        {
            rodFactoryCount += 1;
            fishCount -= rodFactoryCost;
            rodFactoryCost += rodFactoryCostBase / 2;
            rodFactoryCostText.text = "Cost " + Mathf.Round(rodFactoryCost) + " Fishes";
        }
    }

    public void BuyTankFactory()
    {
        if (fishCount >= tankFactoryCost)
        {
            tankFactoryCount += 1;
            fishCount -= tankFactoryCost;
            tankFactoryCost += tankFactoryCostBase / 2;
            tankFactoryCostText.text = "Cost " + Mathf.Round(tankFactoryCost) + " Fishes";
        }
    }

    public void BuyVessel()
    {
        if (rodCount >= vesselCost2 && fishCount >= vesselCost)
        {
            vesselCount += 1;
            fishCount -= vesselCost;
            vesselCost += vesselCostBase / 2;
            vesselCostText.text = "Cost " + Mathf.Round(vesselCost) + " Fishes";
            
            rodCount -= vesselCost2;
            vesselCostText2.text = "Cost " + Mathf.Round(vesselCost2) + " Fishing Rods";
        }
    }

    public void BuyAquarium()
    {
        if (tankCount >= aquariumCost2 && fishCount >= aquariumCost)
        {
            aquariumCount += 1;
            fishCount -= aquariumCost;
            aquariumCost += aquariumCostBase / 2;
            aquariumCostText.text = "Cost " + Mathf.Round(aquariumCost) + " Fishes";
            
            tankCount -= aquariumCost2;
            aquariumCostText2.text = "Cost " + Mathf.Round(aquariumCost2) + " Breeding Tanks";
        }
    }

    public IEnumerator SpawnFish(Vector3 position, int count = 1)
    {
        if (count >= 10)
        {
            count = 10;
        }
        
        for (int i = 0; i < count; i++)
        {
            Instantiate(fishPrefab, position + new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-100f, 100f), 0), quaternion.identity, fishParent.transform);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
