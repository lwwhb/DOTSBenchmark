using Unity.Scenes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ObjectGeneratorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("DOTSBenchmark/Tools/CubeGeneratorWindow")]
    public static void ShowExample()
    {
        ObjectGeneratorWindow wnd = GetWindow<ObjectGeneratorWindow>();
        wnd.titleContent = new GUIContent("对象辅助生成工具");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        var protoPrefabField = root.Q<ObjectField>("protoPrefabField");
        var xHalfCountField = root.Q<IntegerField>("xHalfCountField");
        var zHalfCountField = root.Q<IntegerField>("zHalfCountField");
        var generateButton = root.Q<Button>("generateButton");
        var cleanupButton = root.Q<Button>("cleanupButton");
        var resultInfoLabel = root.Q<Label>("resultInfoLabel");
        generateButton.RegisterCallback<ClickEvent>((evt) =>
        {
            var protoPrefab = protoPrefabField.value as GameObject;
            if (protoPrefab == null)
            {
                resultInfoLabel.text = "错误：请选择原型预制体";
                return;
            }
            int xHalfCount = xHalfCountField.value;
            int zHalfCount = zHalfCountField.value;
            if (xHalfCount <= 0 || zHalfCount <= 0)
            {
                resultInfoLabel.text = "错误：xHalfCount和zHalfCount值必须大于0";
                return;
            }
            Scene scene = SceneManager.GetActiveScene();
            if (scene != null)
            {
                var subScene = Object.FindFirstObjectByType<SubScene>();
                if (subScene == null)
                {
                    resultInfoLabel.text = "错误：该场景下没有EntitiesScene子场景对象";
                    return;
                }

                for (int z = 0; z < zHalfCount * 2; z++)
                {
                    for (int x = 0; x < xHalfCount * 2; x++)
                    {
                        Vector3 position = new Vector3((x-xHalfCount)*1.1f, 0, (z-zHalfCount)*1.1f);
                        Instantiate(protoPrefab, position, new Quaternion(), subScene.transform);
                    }
                }
            }
            resultInfoLabel.text = "成功：生成对象成功！";
        });
        cleanupButton.RegisterCallback<ClickEvent>((evt) =>
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene != null)
            {
                var subScene = Object.FindFirstObjectByType<SubScene>();
                if (subScene == null)
                {
                    resultInfoLabel.text = "错误：该场景下没有EntitiesScene子场景对象";
                    return;
                }
                
                for (int i = subScene.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(subScene.transform.GetChild(i).gameObject);
                }
                
                resultInfoLabel.text = "成功：清理对象成功！";
            }
        });

    }
}
