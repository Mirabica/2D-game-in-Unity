using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhindaManager : MonoBehaviour
{
 public int ghindaCount;
 public Text ghindaText;


    void Start()
    {
        
    }

    void Update()
    {
      ghindaText.text = ghindaCount.ToString();   
    }



}
