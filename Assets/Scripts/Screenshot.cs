using UnityEngine;

public class Screenshot : MonoBehaviour {
  public static void ScreenshotActiveScene(string screenshotPathLocation = "Screenshot.png") {
    ScreenCapture.CaptureScreenshot(screenshotPathLocation, 4);
  }

  /// <summary>
  /// Takes a screenshot from the camera's POV
  /// </summary>
  /// <param name="screenshotPathLocation">Where the screenshot will be saved relative to project's root</param>
  /// <param name="w">Width of the screenshot</param>
  /// <param name="h">Height of the screenshot</param>
  public static void ScreenshotCameraPoint(string screenshotPathLocation = "Screenshot.png", int w = 3840,
    int h = 2160) {
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
  }
}