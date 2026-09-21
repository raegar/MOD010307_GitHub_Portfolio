using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MobileGameTemplate.Editor
{
    /// <summary>
    /// One-click scene builder retained so tutors can rebuild the demonstration scene.
    /// </summary>
    public static class StudentTemplateBuilder
    {
        private const string SceneFolder = "Assets/Scenes";
        private const string ScenePath = SceneFolder + "/MobileGameTemplate.unity";
        private static TMP_FontAsset templateFont;

        [MenuItem("Tools/Mobile Game Template/Rebuild Demo Scene")]
        public static void Build()
        {
            EnsureFolder("Assets", "Scenes");
            EnsureFolder("Assets", "Fonts");
            templateFont = GetOrCreateFont();

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera();
            CreateEventSystem();

            GameObject canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.matchWidthOrHeight = 0.5f;

            Image background = CreateImage("Background", canvasObject.transform, new Color(0.045f, 0.075f, 0.13f));
            Stretch(background.rectTransform);

            TMP_Text header = CreateText("Header", canvasObject.transform, "MICROGAME TEMPLATE", 54, TextAlignmentOptions.Center, Color.white);
            SetRect(header.rectTransform, new Vector2(0.08f, 0.86f), new Vector2(0.92f, 0.96f));

            Image menuPanel = CreatePanel("MenuPanel", canvasObject.transform);
            Image gamePanel = CreatePanel("GamePanel", canvasObject.transform);
            Image resultPanel = CreatePanel("ResultPanel", canvasObject.transform);

            TMP_Text menuTitle = CreateText("TitleText", menuPanel.transform, "WIN BUTTON", 84, TextAlignmentOptions.Center, new Color(0.3f, 0.85f, 1f));
            SetRect(menuTitle.rectTransform, new Vector2(0.08f, 0.67f), new Vector2(0.92f, 0.88f));

            TMP_Text menuInstructions = CreateText(
                "InstructionsText",
                menuPanel.transform,
                "Press START, wait for the countdown, then press WIN before the timer reaches zero.",
                38,
                TextAlignmentOptions.Center,
                new Color(0.88f, 0.92f, 1f));
            SetRect(menuInstructions.rectTransform, new Vector2(0.1f, 0.42f), new Vector2(0.9f, 0.66f));

            Button startButton = CreateButton("StartButton", menuPanel.transform, "START", new Color(0.1f, 0.6f, 0.9f));
            SetRect(startButton.GetComponent<RectTransform>(), new Vector2(0.22f, 0.15f), new Vector2(0.78f, 0.32f));

            TMP_Text countdownText = CreateText("CountdownText", gamePanel.transform, "3", 180, TextAlignmentOptions.Center, new Color(1f, 0.8f, 0.15f));
            SetRect(countdownText.rectTransform, new Vector2(0.1f, 0.42f), new Vector2(0.9f, 0.7f));

            TMP_Text timerText = CreateText("TimerText", gamePanel.transform, "Time: 5.0", 60, TextAlignmentOptions.Center, new Color(0.8f, 0.9f, 1f));
            SetRect(timerText.rectTransform, new Vector2(0.15f, 0.61f), new Vector2(0.85f, 0.74f));

            Image placeholderGamePanel = CreateImage("PlaceholderGamePanel", gamePanel.transform, Color.clear);
            placeholderGamePanel.raycastTarget = false;
            Stretch(placeholderGamePanel.rectTransform);

            TMP_Text gameTitle = CreateText("GameTitleText", placeholderGamePanel.transform, "PRESS WIN!", 70, TextAlignmentOptions.Center, Color.white);
            SetRect(gameTitle.rectTransform, new Vector2(0.08f, 0.75f), new Vector2(0.92f, 0.9f));

            Button winButton = CreateButton("WinButton", placeholderGamePanel.transform, "WIN", new Color(0.15f, 0.72f, 0.35f));
            SetRect(winButton.GetComponent<RectTransform>(), new Vector2(0.16f, 0.15f), new Vector2(0.84f, 0.42f));

            TMP_Text resultText = CreateText("ResultText", resultPanel.transform, "SUCCESS!", 92, TextAlignmentOptions.Center, Color.white);
            SetRect(resultText.rectTransform, new Vector2(0.08f, 0.58f), new Vector2(0.92f, 0.82f));

            Button replayButton = CreateButton("ReplayButton", resultPanel.transform, "REPLAY", new Color(0.1f, 0.6f, 0.9f));
            SetRect(replayButton.GetComponent<RectTransform>(), new Vector2(0.18f, 0.32f), new Vector2(0.82f, 0.47f));

            Button menuButton = CreateButton("MenuButton", resultPanel.transform, "MENU", new Color(0.32f, 0.38f, 0.5f));
            SetRect(menuButton.GetComponent<RectTransform>(), new Vector2(0.18f, 0.12f), new Vector2(0.82f, 0.27f));

            GameObject frameworkObject = new GameObject("MicrogameFramework");
            MicrogameManager manager = frameworkObject.AddComponent<MicrogameManager>();
            PlaceholderMicrogame placeholder = placeholderGamePanel.gameObject.AddComponent<PlaceholderMicrogame>();
            MicrogameUI ui = canvasObject.AddComponent<MicrogameUI>();

            ui.Configure(menuPanel.gameObject, gamePanel.gameObject, resultPanel.gameObject, countdownText, timerText, resultText);
            placeholder.Configure(winButton);
            manager.Configure(ui, placeholder);

            UnityEventTools.AddPersistentListener(startButton.onClick, manager.StartGame);
            UnityEventTools.AddPersistentListener(winButton.onClick, placeholder.Win);
            UnityEventTools.AddPersistentListener(replayButton.onClick, manager.Replay);
            UnityEventTools.AddPersistentListener(menuButton.onClick, manager.ReturnToMenu);

            gamePanel.gameObject.SetActive(false);
            resultPanel.gameObject.SetActive(false);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("MobileGameProject template scene created successfully at " + ScenePath);
        }

        private static void CreateCamera()
        {
            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.045f, 0.075f, 0.13f);
            camera.orthographic = true;
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);
        }

        private static void CreateEventSystem()
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        private static Image CreatePanel(string name, Transform parent)
        {
            Image panel = CreateImage(name, parent, new Color(0.08f, 0.12f, 0.2f, 0.96f));
            SetRect(panel.rectTransform, new Vector2(0.06f, 0.08f), new Vector2(0.94f, 0.84f));
            return panel;
        }

        private static Image CreateImage(string name, Transform parent, Color color)
        {
            GameObject gameObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            gameObject.transform.SetParent(parent, false);
            Image image = gameObject.GetComponent<Image>();
            image.color = color;
            return image;
        }

        private static TMP_Text CreateText(string name, Transform parent, string value, float size, TextAlignmentOptions alignment, Color color)
        {
            GameObject gameObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            gameObject.transform.SetParent(parent, false);
            TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
            text.text = value;
            text.font = templateFont;
            text.fontSize = size;
            text.alignment = alignment;
            text.color = color;
            text.textWrappingMode = TextWrappingModes.Normal;
            return text;
        }

        private static Button CreateButton(string name, Transform parent, string label, Color color)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            Image image = buttonObject.GetComponent<Image>();
            image.color = color;

            Button button = buttonObject.GetComponent<Button>();
            ColorBlock colors = button.colors;
            colors.highlightedColor = Color.Lerp(color, Color.white, 0.2f);
            colors.pressedColor = Color.Lerp(color, Color.black, 0.2f);
            button.colors = colors;

            TMP_Text text = CreateText("Label", buttonObject.transform, label, 58, TextAlignmentOptions.Center, Color.white);
            Stretch(text.rectTransform);
            return button;
        }

        private static TMP_FontAsset GetOrCreateFont()
        {
            const string fontPath = "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset";
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontPath);
            if (fontAsset != null)
            {
                return fontAsset;
            }

            UnityEditor.PackageManager.PackageInfo package =
                UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(TMP_Text).Assembly);
            string resourcePackage = System.IO.Path.Combine(
                package.resolvedPath,
                "Package Resources",
                "TMP Essential Resources.unitypackage");

            AssetDatabase.ImportPackage(resourcePackage, false);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontPath);

            if (fontAsset == null)
            {
                throw new System.InvalidOperationException("TMP Essential Resources could not be imported.");
            }

            return fontAsset;
        }

        private static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        private static void SetRect(RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }
    }
}



