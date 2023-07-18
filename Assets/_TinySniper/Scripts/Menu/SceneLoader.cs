using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider _progressBar;

    private void Start()
    {
        this.gameObject.SetActive(true);
        _progressBar.value = 0;
        loadingPanel.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        loadingPanel.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            _progressBar.value = operation.progress;

            yield return null;
        }
    }
}