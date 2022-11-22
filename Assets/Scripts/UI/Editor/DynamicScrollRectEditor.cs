using UnityEditor;
using UnityEditor.UI;

namespace GameCore.UI.Editor {
  //ָ������Ҫ�Զ���༭���Ľű�
  [CustomEditor(typeof(DynamicScrollRect), true)]
  //ʹ���� SerializedObject �� SerializedProperty ϵͳ����ˣ������Զ����������༭����������undo�� �� ��Ԥ�Ƹ���prefab override����
  [CanEditMultipleObjects]
  public class DynamicScrollRectEditor : ScrollRectEditor {
    //��Ӧ������MyButton�д������ֶ�
    //PS:��Ҫע��һ�㣬ʹ��SerializedProperty ����������ֶ�ǰ��[SerializeField]
    private SerializedProperty GridTemplate;
    private SerializedProperty GridSize;
    private SerializedProperty Spacing;
    private SerializedProperty LayoutDirection;

    protected override void OnEnable() {
      base.OnEnable();
      GridTemplate = serializedObject.FindProperty("GridTemplate");
      GridSize = serializedObject.FindProperty("GridSize");
      Spacing = serializedObject.FindProperty("Spacing");
      LayoutDirection = serializedObject.FindProperty("LayoutDirection");
    }
    //�����ر�ע�⣬������������л���ʽ����Ҫ�� OnInspectorGUI ��ͷ�ͽ�β����һ�� serializedObject.Update();  serializedObject.ApplyModifiedProperties();
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      serializedObject.Update();
      //��ʾ���Ǵ���������
      EditorGUILayout.PropertyField(GridTemplate);
      EditorGUILayout.PropertyField(GridSize);
      EditorGUILayout.PropertyField(Spacing);
      EditorGUILayout.PropertyField(LayoutDirection);
      serializedObject.ApplyModifiedProperties();
    }
  }
}