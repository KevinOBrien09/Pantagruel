using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TreeRotationRandomiser : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public  float randMin = .3f;
    public    float randMax = .6f;
   
    public void Randomize()
    { 
        float y =  Random.Range(0f,360);
        transform.rotation = Quaternion.Euler(    
        transform.rotation.eulerAngles.x,y,y);
         float s = 0;
        if(gameObject.transform.parent.name.Contains("Billboard")){
s =  Random.Range(3f,4f);
        transform.localPosition = new Vector3(Random.Range(-.25f,.25f),   transform.localPosition.y,Random.Range(-.25f,.25f));
        transform.localScale = new Vector3(1.5f,s,s);
        transform.parent. Find("MiniMapIcon ").gameObject .AddComponent<MinimapIcon>();
        }
        else{
s = Random.Range(.75f,1.2f);
   
        transform.localScale = new Vector3(s,s,s);
        }
     
    }
}