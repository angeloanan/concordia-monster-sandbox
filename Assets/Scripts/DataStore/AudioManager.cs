using UnityEngine;

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

  public void PlayBgm() {
    var bgmNames = new[] {"nature1", "nature2"};
    
    var random = new System.Random();
    var index = random.Next(bgmNames.Length);
    var bgmName = bgmNames[index];
    
    PlayAudio($"bgm/{bgmName}");
  }
    
  public void PlayUiClick() {
    PlayAudio("ui");
  }
  
  private void PlayAudio(string audioPath, bool oneShot = false) {
    var resourcePath = $"Audio/{audioPath}";

    var sound = Resources.Load<AudioClip>(resourcePath);
    if (sound == null) {
      Debug.LogWarning($"Sound: {resourcePath} not found!");
      return;
    }

    // TODO: Memory leak here due to not destroying the audio source
    var audioSrc = gameObject.AddComponent<AudioSource>();
    audioSrc.clip = sound;
    if (oneShot) {
      audioSrc.PlayOneShot(sound);
    }
    else {
      audioSrc.Play();
    }
  }
}