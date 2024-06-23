using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleMenuController : MonoBehaviour {
    public void OnSimulate() {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void OnBack() {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void OnExit() {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}