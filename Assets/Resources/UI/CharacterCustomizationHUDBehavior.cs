using UnityEngine;
using UnityEngine.UIElements;

public class CustomizationMap {
  public string Label;
  public EventCallback<ClickEvent> OnClick;
}

public class CharacterCustomizationHUDBehavior : MonoBehaviour {
  [SerializeField] private CharacterCustomization _CharacterCustomization;
  private int w = 3840;
  private int h = 2160;
  private string screenshotPathLocation = "Screenshot.png";

  private void OnEnable() {
    var characterCustomizationMap = new CustomizationMap[] {
      new() { Label = "Body Type", OnClick = _ => { _CharacterCustomization.SwitchBody(); } },
      new() { Label = "Eyes", OnClick = _ => { _CharacterCustomization.SwitchEyes(); } },
      new() { Label = "Mouth", OnClick = _ => { _CharacterCustomization.SwitchMouth(); } },
      new() { Label = "Ears", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Horns", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Wings", OnClick = _ => { _CharacterCustomization.SwitchWing(); } },
      new() { Label = "Tail", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Fur", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Color", OnClick = _ => { Debug.Log("Unimplemented"); } },
    };

    var root = GetComponent<UIDocument>().rootVisualElement;
    var customizationWindow = root.Q<VisualElement>("CustomizationWindow");
    var customizationWindowContent = customizationWindow.Q<VisualElement>("ContentContainer");

    var screenContainer = root.Q<VisualElement>("ScreenContainer");
    var screenshotButtonContainer = screenContainer.Q("ScreenshotButton");
    var screenshotButton = screenshotButtonContainer.Q<Button>("Button");

    foreach (CustomizationMap type in characterCustomizationMap) {
      var entry = new CharacterCustomizationListEntry(type.Label, type.OnClick);
      customizationWindowContent.Add(entry);
    }

    screenshotButton.RegisterCallback<ClickEvent>(_ => {
      // TODO: Remove duplicated code; Scripts/Screenshot.cs
      System.Diagnostics.Debug.Assert(Camera.main != null, "Camera.main != null");

      var targetTexture = new RenderTexture(w, h, 32);
      var screenshotTexture = new Texture2D(w, h, TextureFormat.RGBA32, false);

      // Temporarily taking over main camera
      // Switching camera's render target to created texture
      Camera.main.targetTexture = targetTexture;
      Camera.main.Render();

      // Give back control to main camera, render back to screen
      Camera.main.targetTexture = null;

      RenderTexture.active = targetTexture;
      screenshotTexture.ReadPixels(new Rect(0, 0, w, h), 0, 0);

      RenderTexture.active = null; // JC: added to avoid errors
      Destroy(targetTexture);

      // Encode texture into PNG
      var bytes = screenshotTexture.EncodeToPNG();
      System.IO.File.WriteAllBytes(screenshotPathLocation, bytes);
      Debug.Log($"Took screenshot to: {screenshotPathLocation}");
    });
    
    var spawnButtonContainer = screenContainer.Q("SpawnButton");
    var spawnButton = spawnButtonContainer.Q<Button>("Button");
    
    spawnButton.RegisterCallback<ClickEvent>(_ => {
      Spawn();
    });
  }

  private static void Spawn() {
      var prefab = Resources.Load<GameObject>("CubePrefab");
      var screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, -3);
      // Project screen center to world
      var worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

      Instantiate(prefab, worldCenter, Quaternion.identity);
  }
}