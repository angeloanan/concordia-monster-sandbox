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
    const string bgmTheme = "bgm";

    PlayAudio($"bgm/{bgmTheme}", looping: true, volume: 0.5f);
    // PlayRandomNatureBgm();
  }

  private void PlayRandomNatureBgm() {
    var natureBgm = new[] { "nature1", "nature2" };

    var random = new System.Random();
    var index = random.Next(natureBgm.Length);
    var randomNatureBgm = natureBgm[index];
    PlayAudio($"bgm/{randomNatureBgm}");
    Invoke(nameof(PlayRandomNatureBgm), 4 * 60);
  }

  public void PlayUiClick() {
    PlayAudio("ui", oneShot: true);
  }

  public void PlayCameraClick() {
    PlayAudio("camera", oneShot: true);
  }

  public void PlayAudio(string audioPath, bool oneShot = false, bool looping = false, float volume = 1.0f) {
    var resourcePath = $"Audio/{audioPath}";

    var sound = Resources.Load<AudioClip>(resourcePath);
    if (sound == null) {
      Debug.LogWarning($"Sound: {resourcePath} not found!");
      return;
    }

    AudioSource audioSrc;
    if (activeBGM == null) {
      // Abuse the fact that BGM is always going to be the first audio source to be called
      activeBGM = gameObject.AddComponent<AudioSource>();
      audioSrc = activeBGM;
    }
    else {
      // TODO: Memory leak here due to not destroying the audio source
      audioSrc = gameObject.AddComponent<AudioSource>();
    }
    
    audioSrc.clip = sound;
    audioSrc.volume = volume;
    if (looping) audioSrc.loop = true;
    if (oneShot) {
      audioSrc.PlayOneShot(sound);
    }
    else {
      audioSrc.Play();
    }
  }
}