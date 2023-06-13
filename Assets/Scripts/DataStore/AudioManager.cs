using UnityEngine;
using UnityEngine.Timeline;

public class AudioManager : MonoBehaviour {
  public static AudioManager Instance { get; private set; }
  [SerializeField] private AudioSource activeBGM;

  // Start is called before the first frame update
  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      
      // Play BGM
      PlayBgm();
    }
    else {
      Destroy(gameObject);
    }
  }

  public static void PlayBgm() {
    var bgmNames = new[] {"nature1", "nature2"};
    
    var random = new System.Random();
    var index = random.Next(bgmNames.Length);
    var bgmName = bgmNames[index];
    
    PlayAudio($"bgm/{bgmName}");
  }
    
  public static void PlayUiClick() {
    PlayAudio("ui");
  }
  
  private static void PlayAudio(string audioPath) {
    var resourcePath = $"Audio/{audioPath}";

    var sound = Resources.Load<AudioClip>(resourcePath);
    if (sound == null) {
      Debug.LogWarning($"Sound: {resourcePath} not found!");
      return;
    }

    var audioSrc = new AudioSource();
    audioSrc.clip = sound;
    audioSrc.Play();
  }
}