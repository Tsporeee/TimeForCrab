using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneScript : MonoBehaviour
{
    public float screenChangeInterval;

    public List<GameObject> cutsceneImages;

    public List<GameObject> buttons;
    // Start is called before the first frame update
    public void Start()
    {
        foreach (var cutsceneImage in cutsceneImages)
        {
            cutsceneImage.gameObject.SetActive(false);
        }

        StartCutscenes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCutscenes()
    {
        StartCoroutine(CutsceneStart());
    }

    public void EndCutscene()
    {
        StartCoroutine(CutsceneEnd());
    }

    IEnumerator CutsceneStart()
    {
        cutsceneImages[0].SetActive(true);
        yield return new WaitForSeconds(screenChangeInterval);
        cutsceneImages[0].SetActive(false);
        cutsceneImages[1].SetActive(true);
        yield return new WaitForSeconds(screenChangeInterval);

        SceneManager.LoadScene(1);
    }

    IEnumerator CutsceneEnd()
    {


        cutsceneImages[0].SetActive(true);
        yield return new WaitForSeconds(screenChangeInterval);
        cutsceneImages[0].SetActive(false);
        cutsceneImages[1].SetActive(true);
        yield return new WaitForSeconds(screenChangeInterval);

        SceneManager.LoadScene(1);
    }
}
