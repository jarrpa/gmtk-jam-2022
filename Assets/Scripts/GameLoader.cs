using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    // List of FMOD Banks to load
    [FMODUnity.BankRef]
    public List<string> Banks = new List<string>();

    public string nextSceneName = "";

    private Coroutine loadBankOp;
    private AsyncOperation loadSceneOp;

    public void Awake()
    {
        StartCoroutine(LoadBanksAsync());
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadBanksAsync()
    {
        if (Banks.Count == 0) yield break;

        foreach (var bank in Banks)
            FMODUnity.RuntimeManager.LoadBank(bank, true);

        while (!FMODReady())
            yield return null;

        if (loadSceneOp != null) loadSceneOp.allowSceneActivation = true;
    }

    IEnumerator LoadSceneAsync()
    {
        if (nextSceneName == "") yield break;

        loadSceneOp = SceneManager.LoadSceneAsync(nextSceneName);
        loadSceneOp.allowSceneActivation = FMODReady();

        while (!loadSceneOp.isDone)
            yield return null;
    }

    public static bool FMODReady() {
        return FMODUnity.RuntimeManager.HaveAllBanksLoaded && !FMODUnity.RuntimeManager.AnySampleDataLoading();
    }
}
