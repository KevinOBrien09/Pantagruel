using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;



 [System.Serializable]
public class DialogBlock
{
    public string customName;
    public Character speaker;
  [TextArea(10,10)]  public string dialog;
    public bool isThought,showLeft;
    public CameraState cameraState;

}

[System.Serializable]
public class DialogBlockLanguage{
    public Language language;
    public TextAsset json;
}

 [System.Serializable]
public class Conversation{

public List<DialogBlock> blocks = new List<DialogBlock>();
}
   

public enum Language{ENG,FR,SP,PR}
[System.Serializable]
[CreateAssetMenu(menuName = "Dialog/Dialog", fileName = "Dialog")]
public class Dialog:ScriptableObject
{
    public List<DialogBlock> blocks = new List<DialogBlock>();
    public List<DialogBlockLanguage> dialogBlockLanguages = new List<DialogBlockLanguage>();
    public TextAsset textAsset;
   
    #if UNITY_EDITOR

    [ContextMenu("GenerateJSON")]
    public void GenerateJSON()
    {
       // string path = "H:/ProjectFolders/Pantagruel/Beasts-of-Pantagruel/";
        string s =   AssetDatabase.GetAssetPath(this);
        string fileName = this.name + ".asset";
        string soPruned = s.Replace(fileName,string.Empty);
        string jsonFileName = this.name +"ENG" + ".json";
        string fullPath = soPruned + jsonFileName;
        if(!JSONAlreadyExists(fullPath))
        {
            File.Create(fullPath).Dispose();
            Conversation c = new Conversation();
            c.blocks = new List<DialogBlock>(blocks);
            string jsonConversion = JsonUtility.ToJson(c,true);
            File.WriteAllText(fullPath, jsonConversion);
        }
        else
        {
            Conversation c = new Conversation();
            c.blocks = new List<DialogBlock>(blocks);
            string jsonConversion = JsonUtility.ToJson(c,true);
            File.WriteAllText(fullPath, jsonConversion);
        }
       
        textAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(fullPath, typeof(TextAsset));
       
        EditorUtility. SetDirty(this);
       
        bool JSONAlreadyExists(string dir)
        { return Directory.Exists(dir); }
    }

    #endif
}