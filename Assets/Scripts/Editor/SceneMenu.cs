using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneMenu
{
    [MenuItem("Scenes/Persistent")]
    public static void OpenPersistent()
    {
        OpenBuildZero();
    }

    [MenuItem("Scenes/LevelSelector")]
    public static void OpenLevelSelector()
    {
        OpenScene("LevelSelector");
    }

    [MenuItem("Scenes/Game1")]
    public static void OpenGame1()
    {
        OpenGameScene();
    }

    private static void OpenBuildZero()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/System/Persistent.unity", OpenSceneMode.Single);
    }

    private static void OpenScene(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/System/Persistent.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene("Assets/Scenes/GameSystems/MusicSelector/" + sceneName + ".unity", OpenSceneMode.Additive);
    }

    private static void OpenGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/System/Persistent.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene("Assets/Scenes/Games/Game#1/Game1.unity", OpenSceneMode.Additive);
    }
}