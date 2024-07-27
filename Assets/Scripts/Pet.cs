using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Pet")]
public class Pet : ScriptableObject
{
    public BeastAnimatedInstance animatedInstance;
    public Sprite uiPicture;
    public PetEffect petEffect;
    public string petName;
    public Stats stats;
    public SoundData dying,summoned;
    public Vector3 playerPos,playerScale,enemyPos,enemyScale;

}