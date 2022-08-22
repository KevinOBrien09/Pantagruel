using UnityEngine;
using UnityEngine.SceneManagement;
using EasySpringBone;


public class Wind : MonoBehaviour
{
    public SpringBoneManager[] springBoneManagers;
    public GameObject[] winds;

    public enum WIND { LEFT, RIGHT, OFF };
    public WIND currentWind;
    private float windForce  = 0;
    [SerializeField] float windPressure = 0.08f;
    private float timePassed = 0;

    void OnEnable()
    {
        ActivateWind();
    }

    public void ActivateWind()
    {
        if(currentWind == WIND.RIGHT)
        {
            addExtraForce(180);
           
        }
        else if(currentWind == WIND.RIGHT)
        {
            addExtraForce(0);
          
        }
        else{
            removeExtraForce();
           
        }

    }


 


    #if UNITY_EDITOR
    void OnGUI()
    {
        var currentScene = SceneManager.GetActiveScene();
        var currentSceneName = currentScene.name;

        if(currentSceneName == "Anim")
        {
            if(Application.isEditor)
            {
                GUI.Box(new Rect(10, 10, 160, 150), "Wind Control");

                if(GUI.Button(new Rect(20, 40, 140, 30), "Turn On Left Wind"))
                {
                    addExtraForce(0);
                 //   switchWind(WIND.LEFT);
                }

                if(GUI.Button(new Rect(20, 80, 140, 30), "Turn On Right Wind"))
                {
                    addExtraForce(180);
                   // switchWind(WIND.RIGHT);
                }

                if(GUI.Button(new Rect(20, 120, 140, 30), "Turn Off Wind"))
                {
                    removeExtraForce();
                    //switchWind(WIND.OFF);
                }
            }

        }

      
       
    }
    #endif

    private void Update()
    {
        updateWindForce();
    }
    
    private void addExtraForce(float angle)
    {
        for(int i = 0; i < springBoneManagers.Length; i++)
        {
            springBoneManagers[i].extraForceDir = angle;
            springBoneManagers[i].forceLength = windForce;
            springBoneManagers[i].withExtraForce = true;
            springBoneManagers[i].alwaysUpdate = true;
        }
    }
    
    public void removeExtraForce()
    {
        for(int i = 0; i < springBoneManagers.Length; i++)
        {
            springBoneManagers[i].withExtraForce = false;
        }
    }
    
    private void updateWindForce()
    {
        timePassed += Time.deltaTime;
        if(timePassed > 0.1f)
        {
            timePassed = 0;
            windForce = Mathf.PerlinNoise(Time.time, 0) * windPressure;

            changeExtraForce();
        }
    }

    private void changeExtraForce()
    {
        for(int i = 0; i < springBoneManagers.Length; i++)
        {
            springBoneManagers[i].forceLength = windForce;
        }
    }
}
