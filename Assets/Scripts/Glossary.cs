using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using DG.Tweening;

public class Glossary:Singleton<Glossary>                   
{

    public List<GlossaryDefinition> definitions = new List<GlossaryDefinition>();
    public GlossaryEntry entryPrefab;
    public Transform holder;
    void Start(){
        foreach (var item in definitions)
        {
            GlossaryEntry e = Instantiate(entryPrefab,holder);
            e.Apply(item);
        }
        gameObject.SetActive(false);
    }

   public void Close(){
        gameObject.SetActive(false);
    }

}