using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartSceneLoad : MonoBehaviour
{
    private StartCutscene cutscene;
    // Start is called before the first frame update
    public void Start()
    {
        cutscene = FindFirstObjectByType<StartCutscene>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Startgame()
    {
        cutscene.StartCutscenes();

    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
