using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Tutor utility for creating the worked reference scenes. The student guide
// builds these objects by hand and never asks Copilot to make the example game.
public static class ReferenceSceneBuilder
{
    private const string SceneFolder = "Assets/Scenes";
    private static Font font;

    [MenuItem("Tools/Microgame Course/Build Reference Scenes")]
    public static void Build()
    {
        if (!AssetDatabase.IsValidFolder(SceneFolder))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        BuildMenu();
        BuildPractice();

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(SceneFolder + "/MainMenu.unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/PracticeTap.unity", true)
        };
        AssetDatabase.SaveAssets();
        EditorSceneManager.OpenScene(SceneFolder + "/MainMenu.unity");
        Debug.Log("Reference scenes built. Open MainMenu and press Play.");
    }

    private static void BuildMenu()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        MakeCamera();
        Canvas canvas = MakeCanvas();
        MakeEventSystem();

        Text title = MakeText(canvas.transform, "Title", "MICROGAME LAB", new Vector2(0, 500), new Vector2(900, 150), 64);
        title.color = new Color(0.94f, 0.97f, 1f);
        Text instruction = MakeText(canvas.transform, "Instruction", "One game per scene. Start with the practice game.", new Vector2(0, 340), new Vector2(900, 180), 36);
        instruction.color = new Color(0.65f, 0.78f, 0.9f);

        SceneNavigator navigator = new GameObject("Navigation").AddComponent<SceneNavigator>();
        Button button = MakeButton(canvas.transform, "PracticeButton", "PRACTICE TAP", new Vector2(0, 0), new Vector2(700, 170));
        UnityEventTools.AddPersistentListener(button.onClick, navigator.OpenPractice);

        Text note = MakeText(canvas.transform, "Note", "Future student games get their own scenes and menu buttons.", new Vector2(0, -360), new Vector2(900, 180), 30);
        note.color = new Color(0.65f, 0.78f, 0.9f);
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), SceneFolder + "/MainMenu.unity");
    }

    private static void BuildPractice()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        MakeCamera();
        Canvas canvas = MakeCanvas();
        MakeEventSystem();

        GameObject sessionObject = new GameObject("MicrogameSession");
        MicrogameSession session = sessionObject.AddComponent<MicrogameSession>();
        SceneNavigator navigator = new GameObject("Navigation").AddComponent<SceneNavigator>();

        Text heading = MakeText(canvas.transform, "Heading", "PRACTICE TAP", new Vector2(0, 720), new Vector2(850, 120), 54);
        heading.color = new Color(0.94f, 0.97f, 1f);
        Text timer = MakeText(canvas.transform, "TimerText", "", new Vector2(0, 610), new Vector2(700, 90), 44);

        RectTransform ready = MakePanel(canvas.transform, "ReadyPanel", new Vector2(0, 0), new Vector2(930, 1100));
        MakeText(ready, "ReadyText", "Tap the moving target five times before time runs out.", new Vector2(0, 200), new Vector2(760, 240), 42);
        Button start = MakeButton(ready, "StartButton", "START", new Vector2(0, -170), new Vector2(620, 160));
        UnityEventTools.AddPersistentListener(start.onClick, session.StartGame);

        RectTransform play = MakePanel(canvas.transform, "PlayArea", new Vector2(0, -80), new Vector2(900, 1000));
        play.GetComponent<Image>().color = new Color(0.09f, 0.20f, 0.31f);
        Text progress = MakeText(play, "ProgressText", "Taps left: 5", new Vector2(0, 390), new Vector2(750, 100), 42);
        Button targetButton = MakeButton(play, "TargetButton", "TAP", Vector2.zero, new Vector2(200, 200));
        targetButton.GetComponent<Image>().color = new Color(1f, 0.58f, 0.23f);

        TargetTapGame game = new GameObject("TargetTapGame").AddComponent<TargetTapGame>();
        UnityEventTools.AddPersistentListener(targetButton.onClick, game.TapTarget);

        RectTransform result = MakePanel(canvas.transform, "ResultPanel", new Vector2(0, 0), new Vector2(930, 1100));
        Text resultText = MakeText(result, "ResultText", "", new Vector2(0, 240), new Vector2(760, 180), 60);
        Button replay = MakeButton(result, "ReplayButton", "REPLAY", new Vector2(0, -80), new Vector2(620, 150));
        Button menu = MakeButton(result, "MenuButton", "MENU", new Vector2(0, -290), new Vector2(620, 150));
        UnityEventTools.AddPersistentListener(replay.onClick, navigator.Replay);
        UnityEventTools.AddPersistentListener(menu.onClick, navigator.OpenMenu);

        SetField(session, "game", game);
        SetField(session, "readyPanel", ready.gameObject);
        SetField(session, "playArea", play.gameObject);
        SetField(session, "resultPanel", result.gameObject);
        SetField(session, "timerText", timer);
        SetField(session, "resultText", resultText);
        SetField(game, "playArea", play);
        SetField(game, "target", targetButton.GetComponent<RectTransform>());
        SetField(game, "progressText", progress);

        play.gameObject.SetActive(false);
        result.gameObject.SetActive(false);

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), SceneFolder + "/PracticeTap.unity");
    }

    private static void MakeCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.035f, 0.075f, 0.13f);
        camera.orthographic = true;
        cameraObject.tag = "MainCamera";
    }

    private static Canvas MakeCanvas()
    {
        GameObject root = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        root.layer = 5;
        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    private static void MakeEventSystem()
    {
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private static RectTransform MakePanel(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.layer = 5;
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
        go.GetComponent<Image>().color = new Color(0.07f, 0.14f, 0.23f);
        return rect;
    }

    private static Text MakeText(Transform parent, string name, string value, Vector2 position, Vector2 size, int fontSize)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.layer = 5;
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
        Text text = go.GetComponent<Text>();
        text.font = font;
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.text = value;
        text.raycastTarget = false;
        return text;
    }

    private static Button MakeButton(Transform parent, string name, string label, Vector2 position, Vector2 size)
    {
        RectTransform rect = MakePanel(parent, name, position, size);
        rect.GetComponent<Image>().color = new Color(0.13f, 0.58f, 0.86f);
        Button button = rect.gameObject.AddComponent<Button>();
        Text text = MakeText(rect, "Label", label, Vector2.zero, size, 42);
        text.fontStyle = FontStyle.Bold;
        return button;
    }

    private static void SetField(Object owner, string fieldName, Object value)
    {
        SerializedObject serialized = new SerializedObject(owner);
        serialized.FindProperty(fieldName).objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }
}
