using UnityEngine;
using UnityEngine.UIElements;

public class MonsterGameUIBehavior : MonoBehaviour {
  [SerializeField] private GameObject _WorldRoot;

  private bool isDraggingObject;

  private void OnEnable() {
    VisualElement root = GetComponent<UIDocument>().rootVisualElement;

    VisualElement cubeItem = root.Q<VisualElement>("Item");


    // Hover style
    cubeItem.RegisterCallback<MouseEnterEvent>(e => {
      cubeItem.style.backgroundColor = new StyleColor(new Color(0.8f, 0.8f, 0.8f));
    });
    cubeItem.RegisterCallback<MouseLeaveEvent>(e => { cubeItem.style.backgroundColor = new StyleColor(Color.white); });


    cubeItem.RegisterCallback<ClickEvent>(e => {
      var prefab = Resources.Load<GameObject>("CubePrefab");
      var screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, -3);
      // Project screen center to world
      var worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

      Instantiate(prefab, worldCenter, Quaternion.identity);
    });

    // TODO: Below's code is for item dragging
    cubeItem.RegisterCallback<PointerDownEvent>(e => { Debug.Log("Pointer Down"); });
    cubeItem.RegisterCallback<PointerUpEvent>(e => { Debug.Log("Pointer Up"); });
    // Handle whether should render item
    cubeItem.RegisterCallback<MouseOutEvent>(e => {
      // IF mouse is clicking
      // Then call function, move the object until mouse is released 
    });
  }
}