using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSequenceManager : MonoBehaviour
{
    public float TimePerLogo = 1f;
    public Sprite[] Logos;
    public string NextScene = "Main";
    private Image logoImage;
    // Start is called before the first frame update
    void Start()
    {
        logoImage = GetComponentInChildren<Image>(true);
        logoImage.enabled = false;
        StartCoroutine(SplashLogos());
    }

    IEnumerator SplashLogos()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < Logos.Length; i++){
            Sprite logo = Logos[i];
            logoImage.sprite = logo;
            logoImage.enabled = true;
            yield return new WaitForSeconds(TimePerLogo);
        }

        SceneManager.LoadScene(NextScene);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene(NextScene);
        }
    }
}
