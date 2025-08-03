using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour
{
    public List<Sprite> fishSprites = new List<Sprite>();
    public float speed = 1f;
    public RectTransform rectTransform;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
        
        float size = UnityEngine.Random.Range(0.5f, 2f);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * size, rectTransform.sizeDelta.y * size);
        
        int index = UnityEngine.Random.Range(0, fishSprites.Count - 1);
        GetComponent<Image>().sprite = fishSprites[index];
        
        speed *= UnityEngine.Random.Range(0.5f, 2f);
    }

    void FixedUpdate()
    {
        rectTransform.position = Vector3.Lerp(rectTransform.position, new Vector3(Screen.width/2f, Screen.height/2f, 0), speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, new (Screen.width/2f, Screen.height/2f, 0)) < 50.0f)
        {
            Destroy(gameObject);
        }
    }
}
