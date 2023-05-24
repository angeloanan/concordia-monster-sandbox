using UnityEngine;

public class Screenshot : MonoBehaviour {
  /// <summary>
  /// Where to save screenshot, relative to project file's location
  /// </summary>
  [SerializeField] string screenshotPathLocation = "Screenshot.png";
  // 4K res
  [SerializeField] private int w = 3840;
  [SerializeField] private int h = 2160;

  public void ScreenshotActiveScene() {
    ScreenCapture.CaptureScreenshot(this.screenshotPathLocation, 4);
  }

  /**
   * Takes a screenshot from the camera's point of view.
   * Screenshot by default is 4K at 32 bits per pixel.
   */
  public void ScreenshotCameraPoint() {
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