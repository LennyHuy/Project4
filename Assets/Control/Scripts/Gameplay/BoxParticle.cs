using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxParticle : MonoBehaviour
{
    private Color particleColor;

    // private void OnParticleSystemStopped()
    // {
    //     ObjectPoolManager.ReturnObjectToPool(gameObject);
    // }

    public void CheckBox(string name){
        //Remove the "(Clone)" word behind each string
        string goName = name.Substring(0, name.Length -7);
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        switch(goName){
            case "Box" :        
            particleColor = Color.white;
            main.startColor = particleColor;
            break;
            case "QuestionBox" :        
            particleColor = Color.yellow;
            main.startColor = particleColor;
            break;
            case "BadBox" :        
            particleColor = Color.red;
            main.startColor = particleColor;
            break;
            case "BonusBox" :
            particleColor = Color.green;
            main.startColor = particleColor;
            break;
        }

    }
}
