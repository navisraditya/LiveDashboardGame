using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedSceneLoader : MonoBehaviour
{
[SerializeField]
private float delayBeforeLoading = 10f;
[SerializeField]
private string sceneNameToLoad;
private float timeElapsed;
private bool buttonPressed = false;

public void OnButtonPress()
{
    buttonPressed = true;
}

private void Update()
{
    if(buttonPressed)
    {        
        timeElapsed += Time.deltaTime;
        if (timeElapsed > delayBeforeLoading)

        {
        SceneManager.LoadScene(sceneNameToLoad);
        }
    }

}
}

