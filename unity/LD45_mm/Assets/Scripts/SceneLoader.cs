using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string[] scenes;

    [SerializeField]
    private string[] _levelScenes;

    [SerializeField]
    private int _environmentIndex;

    private void Awake()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene != gameObject.scene)
        {
            Destroy(gameObject);
        }
        else
        {
            string activeScenePath = GetActiveScenePath(activeScene);
            LoadScenesAdditive(scenes, activeScenePath);
            LoadEnvironmentScene(activeScenePath);
        }
    }

    private string GetActiveScenePath(Scene activeScene)
    {
        string activeScenePath = activeScene.path;
        activeScenePath = activeScenePath.Replace(".unity", "");
        activeScenePath = activeScenePath.Replace("Assets/", "");
        return activeScenePath;
    }

    private void LoadScenesAdditive(string[] sceneList, string activeScenePath)
    {
        for (int i = 0; i < sceneList.Length; i++)
        {
            string scene = sceneList[i];
            if (!scene.Contains(activeScenePath))
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }
    }

    private void LoadEnvironmentScene(string currentScene)
    {
        if (_levelScenes.Length > 0)
        {
            _environmentIndex = Mathf.Clamp(_environmentIndex, 0, _levelScenes.Length - 1);
            SceneManager.LoadSceneAsync(_levelScenes[_environmentIndex], LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogError("No environment scenes set on SceneLoader!");
        }
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
