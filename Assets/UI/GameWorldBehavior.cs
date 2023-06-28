using System.Collections.Generic;
using DataStore;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Playtest2Behavior : MonoBehaviour {
  [SerializeField] private GameObject worldRoot;
  private static readonly Vector3 SpawnPoint = new(0, 0.8F, 8);

  private void InitializeHotbarItems(IEnumerable<ItemData.ItemEntry> items) {
    var root = GetComponent<UIDocument>().rootVisualElement;
    var itemListContainer = root.Q<VisualElement>("ItemListContainer");
    
    itemListContainer.Clear();
    
    foreach (var item in items) {
      var image = Resources.Load<Texture2D>($"Images/Icons/{item.previewImagePath ?? item.prefabPath}");
      var entry = new CustomizationBox(image, _ => {
        AudioManager.Instance.PlayAudio($"spawn/{item.spawnSfxPath}", oneShot: true);
        // TODO: Override this with a custom spawn function later on
        SpawnPrefab(item.prefabPath);
      }); 
      
      itemListContainer.Add(entry);
    }
  }

  private void InitializeTextBubbleHotbarItems(IEnumerable<ItemData.ItemEntry> items) {
    var root = GetComponent<UIDocument>().rootVisualElement;
    var itemListContainer = root.Q<VisualElement>("ItemListContainer");
    
    itemListContainer.Clear();

    foreach (var item in items) {
      var image = Resources.Load<Texture2D>($"Images/textbox");
      var entry = new CustomizationBox(image, _ => {
        // TODO: Move this to a separate file
        var visualTreeAsset =
          Resources.Load<VisualTreeAsset>("UI/Components/TextBubbleModal");
        visualTreeAsset.CloneTree(root);
        
        var cancelButton = root.Q<TemplateContainer>("CancelButtonContainer").Q<Button>("Button");
        var doneButton = root.Q<TemplateContainer>("DoneButtonContainer").Q<Button>("Button");

        cancelButton.RegisterCallback<ClickEvent>(_ => {
          // Remove the whole element
          root.Remove(root.Q<VisualElement>("TextBubbleModalBackground"));
        });
        
        doneButton.RegisterCallback<ClickEvent>(_ => {
          var content = root.Q<TextField>().text;
          
          var textBubblePlane = Resources.Load<GameObject>("TextBubblePlane");
          var projectionPanelSetting = Resources.Load<PanelSettings>("ProjectionPanelSettings");
          
          var spawnedTextObject = Instantiate(textBubblePlane);
          var rendererUiDoc = spawnedTextObject.GetComponent<UIDocument>();
          var textBubble = new TextBubble(content);
          var renderTexture = new RenderTexture(900, 400, 24);
          
          // Setup everything to be rendered to a texture
          
          rendererUiDoc.panelSettings = projectionPanelSetting;
          rendererUiDoc.panelSettings.targetTexture = renderTexture;
          Debug.Log(rendererUiDoc.rootVisualElement);
          rendererUiDoc.rootVisualElement.Clear();
          rendererUiDoc.rootVisualElement.Add(textBubble.parent);

          Debug.Log(spawnedTextObject.transform.GetChild(0).gameObject);
          spawnedTextObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.mainTexture =
            renderTexture;
          spawnedTextObject.gameObject.tag = "Draggable";
          spawnedTextObject.transform.position = new Vector3(0, 4, 8);
          
          // Remove the whole element
          root.Remove(root.Q<VisualElement>("TextBubbleModalBackground"));
        });

        // Instantly focus on the TextField
        root.Q<TextField>().Focus();
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "");
      });
      
      itemListContainer.Add(entry);
    }
  }
  
  private static void SpawnPrefab(string prefabPath) {
    var prefab = Resources.Load<GameObject>(prefabPath);
    // Get the center of the screen
    Debug.Assert(Camera.main != null, "Camera.main != null");
    
    // Spawn prefab
    var spawnedObject = Instantiate(prefab, SpawnPoint, Quaternion.identity);
    spawnedObject.gameObject.tag = "Draggable";
  }

  private void InitializeUI() {
    var root = GetComponent<UIDocument>().rootVisualElement;
    var itemListContainer = root.Q<VisualElement>("ItemListContainer");
    var resetButton = root.Q<Button>("resetButton");
    var screenshotButton = root.Q<Button>("screenshotButton");

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
    
    screenshotButton.RegisterCallback<ClickEvent>(_ => {
      AudioManager.Instance.PlayCameraClick();
    });
    screenshotButton.RegisterCallback<ClickEvent>(_ => {
      Screenshot.ScreenshotCameraPoint();
    });

    // Category buttons
    // Adding ItemBoxes dynamically to the Item List
    var uiItemMap = ItemData.UIItemMap;
    InitializeHotbarItems(uiItemMap.buildings);
    
    var buildingCategoryButton = root.Q<Button>("Building");
    buildingCategoryButton.RegisterCallback<ClickEvent>(_ => {
      AudioManager.Instance.PlayUiClick();
      InitializeHotbarItems(ItemData.UIItemMap.buildings);
    });
    var treesCategoryButton = root.Q<Button>("Trees");
    treesCategoryButton.RegisterCallback<ClickEvent>(_ => {
      AudioManager.Instance.PlayUiClick();
      InitializeHotbarItems(ItemData.UIItemMap.trees);
    });
    var decorCategoryButton = root.Q<Button>("Decor");
    decorCategoryButton.RegisterCallback<ClickEvent>(_ => {
      AudioManager.Instance.PlayUiClick();
      InitializeHotbarItems(ItemData.UIItemMap.decorations);
    });
    var textCategoryButton = root.Q<Button>("Text");
    textCategoryButton.RegisterCallback<ClickEvent>(_ => {
      AudioManager.Instance.PlayUiClick();
      InitializeTextBubbleHotbarItems(ItemData.UIItemMap.textBoxes);
    });
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