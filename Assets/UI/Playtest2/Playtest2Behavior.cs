using DataStore;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = System.Diagnostics.Debug;
using UnityEngine.SceneManagement;


public class Playtest2Behavior : MonoBehaviour {
  [SerializeField] private GameObject worldRoot;

  private static void SpawnPrefab(string prefabPath) {
    var prefab = Resources.Load<GameObject>(prefabPath);
    // Get the center of the screen
    Debug.Assert(Camera.main != null, "Camera.main != null");
    var screenCenterRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

    // Project ray to world
    Physics.Raycast(screenCenterRay, out var hit, 100, LayerMask.GetMask("World"));
    var spawnPoint = hit.point;

    // Spawn prefab
    var spawnedObject = Instantiate(prefab, spawnPoint, Quaternion.identity);
    spawnedObject.gameObject.tag = "Draggable";

    // // Adapt spawnPoint height to prefab's height
    // // This needs to be done after initial object spawn because Collider will be empty
    // // when the object is not active
    // Canvas.ForceUpdateCanvases();
    //
    // var prefabHeight = prefab.GetComponent<Collider>().bounds.size.y;
    // Debug.Log($"Prefab {prefabPath} height is {prefabHeight}");
    // spawnedObject.transform.position += new Vector3(0, prefabHeight / 2, 0);
  }

  private void InitializeUI() {
    var root = GetComponent<UIDocument>().rootVisualElement;
    var itemListContainer = root.Q<VisualElement>("ItemListContainer");
    var resetButton = root.Q<Button>("resetButton");

    // Handle reset button
    resetButton.RegisterCallback<ClickEvent>(_ => {
      AudioManager.Instance.PlayUiClick();
    });
    resetButton.clicked += () => {
      // Remove activeMonsterPrefab from DontDestroyOnLoad
      if (MonsterDataManager.Instance != null && MonsterDataManager.Instance.activeMonsterPrefab != null) {
        Destroy(MonsterDataManager.Instance.activeMonsterPrefab);
      }
      SceneManager.LoadScene("Scenes/SelectMonster");
    };

    // Add current monster to the UI
    var monsterEntry = new ItemBox("Monster", _ => {
      var activeMonster = MonsterDataManager.Instance.activeMonsterPrefab;
      Instantiate(activeMonster, Vector3.zero, Quaternion.identity);
    });
    itemListContainer.Add(monsterEntry);

    // Adding ItemBoxes dynamically to the Item List
    var uiItemMap = ItemData.UIItemMap;
    foreach (var item in uiItemMap.buildings) {
      var entry = new ItemBox(item.itemLabel, _ => SpawnPrefab(item.prefabPath)); 
      itemListContainer.Add(entry);
    }

    // SPECIAL CASE: Text Bubble
    var textBubbleItem = new ItemBox("Text Bubble", _ => {
      // Show PopUp
      // TODO: Move this to a separate file
      var visualTreeAsset =
        Resources.Load<VisualTreeAsset>("UI/Components/TextBubbleModal");
      visualTreeAsset.CloneTree(root);

      var doneButton = root.Q<TemplateContainer>("DoneButtonContainer").Q<Button>("Button");
      doneButton.RegisterCallback<ClickEvent>(e => {
        // Spawn Text Bubble
        Debug.Print("Done button clicked");
        var content = root.Q<TextField>().text;
        Debug.Print($"Text content: {content}");
        // var textBubblePrefab = Resources.Load<GameObject>("Objects/TextBubble");
        // var textBubble = Instantiate(textBubblePrefab, worldRoot.transform);
        // textBubble.GetComponent<TextBubbleBehavior>().SetText(content);

        // Reset Bubble
        root.Remove(root.Q<VisualElement>("TextBubbleModalBackground"));
      });

      // Instantly focus on the TextField
      root.Q<TextField>().Focus();
      TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "I was thinking of...");
    });
    itemListContainer.Add(textBubbleItem);
  }

  private void OnEnable() {
    this.InitializeUI();

    // TODO: Below's code is for item dragging
    // cubeItem.RegisterCallback<PointerDownEvent>(e => { Debug.Log("Pointer Down"); });
    // cubeItem.RegisterCallback<PointerUpEvent>(e => { Debug.Log("Pointer Up"); });
    // // Handle whether should render item
    // cubeItem.RegisterCallback<MouseOutEvent>(e => {
    //   // IF mouse is clicking
    //   // Then call function, move the object until mouse is released 
    // });
  }
}