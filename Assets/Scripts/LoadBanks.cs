using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBanks : MonoBehaviour
{
    // List of Banks to load
    [FMODUnity.BankRef]
    public List<string> Banks = new List<string>();

    public string nextSceneName = "Menu";

    public void Start()
    {
        StartCoroutine(LoadBanksAsync());
    }

    IEnumerator LoadBanksAsync()
    {
        var loadOp = SceneManager.LoadSceneAsync(nextSceneName);
        loadOp.allowSceneActivation = false;

        foreach (var bank in Banks)
            FMODUnity.RuntimeManager.LoadBank(bank, true);

        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded || FMODUnity.RuntimeManager.AnySampleDataLoading())
            yield return null;

        loadOp.allowSceneActivation = true;

        while (!loadOp.isDone)
            yield return null;
    }
}
