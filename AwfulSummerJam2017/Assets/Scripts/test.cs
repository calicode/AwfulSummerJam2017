using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class test : MonoBehaviour
{
    Text titleText;
    int fontSizeMax = 34;
    int fontSizeMin = 5;
    int textSizeIncrement = 1;
    bool textSizeIncreasing = true;


    // Use this for initialization
    void Start()
    {
        titleText = GetComponent<Text>();
        InvokeRepeating("ChangeFontSizeAndColor", 1, 1);
    }
    void ChangeFontSizeAndColor()
    {
        //titleText.color = newColor;


        if (textSizeIncreasing) { titleText.fontSize += textSizeIncrement; } else { titleText.fontSize -= textSizeIncrement; }
        if (titleText.fontSize >= fontSizeMax) { textSizeIncreasing = false; }
        if (titleText.fontSize <= fontSizeMin) { textSizeIncreasing = true; }

        Color newColor = new Color(Random.Range(1f, 255f) / 255f, Random.Range(1f, 255f) / 255f, Random.Range(1f, 255f) / 255f);
        titleText.color = Color.Lerp(titleText.color, newColor, .5f);

    }
    // Update is called once per frame
    void Update()
    {
        titleText.transform.Rotate(Vector3.forward * Time.deltaTime);
    }
}
