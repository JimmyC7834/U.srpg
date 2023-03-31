// using System;
// using Game.Unit;
// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor(typeof(UnitSO))]
// public class UnitSOInspectorDrawer : UnityEditor.Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         if (GUILayout.Button("Open Parts Editor"))
//             UnitSOEditorWindow.Open(target as UnitSO);
//     }
// }
//
// [CustomEditor(typeof(GridPartSO))]
// public class PartInspectorDrawer : UnityEditor.Editor
// {
//     private bool _showNative = false;
//     private bool[] _editorCompatibility = new bool[4];
//     
//     private SerializedProperty _icon; 
//     private SerializedProperty _displayName;
//     private SerializedProperty _description;
//     private SerializedProperty _skill;
//     private SerializedProperty _abilities;
//     private SerializedProperty _compatibility;
//     private SerializedProperty _compatUp;
//     private SerializedProperty _compatDown;
//     private SerializedProperty _compatLeft;
//     private SerializedProperty _compatRight;
//     private SerializedProperty _abilityActivation;
//
//     private void OnEnable()
//     {
//         _icon = serializedObject.FindProperty("_icon");
//         _displayName = serializedObject.FindProperty("_displayName");
//         _description = serializedObject.FindProperty("_description");
//         _skill = serializedObject.FindProperty("_skill");
//         _abilities = serializedObject.FindProperty("_abilities");
//         _compatibility = serializedObject.FindProperty("_compatibility");
//         _compatUp = serializedObject.FindProperty("_compatUp");
//         _compatDown = serializedObject.FindProperty("_compatDown");
//         _compatLeft = serializedObject.FindProperty("_compatLeft");
//         _compatRight = serializedObject.FindProperty("_compatRight");
//         _abilityActivation = serializedObject.FindProperty("_abilityActivation");
//     }
//
//     public override void OnInspectorGUI()
//     {
//         GridPartSO part = target as GridPartSO;
//         serializedObject.UpdateIfRequiredOrScript();
//         _showNative = EditorGUILayout.Foldout(_showNative, "Native Inspector");
//         if (_showNative)
//             base.OnInspectorGUI();
//         serializedObject.Update();
//         EditorGUILayout.Space();
//         
//         // First panel
//         EditorGUILayout.BeginHorizontal();
//         
//         // Icon selection
//         _icon.objectReferenceValue = EditorGUILayout.ObjectField(
//             "Part Icon", 
//             _icon.objectReferenceValue, 
//             typeof(Sprite), false);
//         
//         // compatibility settings
//         EditorGUILayout.BeginHorizontal();
//         _compatLeft.boolValue = EditorGUILayout.Toggle(_compatLeft.boolValue, GUILayout.MinHeight(53));
//         EditorGUILayout.BeginVertical();
//         _compatUp.boolValue = EditorGUILayout.Toggle(_compatUp.boolValue);
//         _compatDown.boolValue = EditorGUILayout.Toggle(_compatDown.boolValue, GUILayout.MinHeight(50));
//         EditorGUILayout.EndVertical();
//         _compatRight.boolValue = EditorGUILayout.Toggle(_compatRight.boolValue, GUILayout.MinHeight(53));
//         EditorGUILayout.Space();
//         EditorGUILayout.Space();
//         EditorGUILayout.Space();
//         EditorGUILayout.EndHorizontal();
//         EditorGUILayout.EndHorizontal();
//  
//         // Name and Description
//         EditorGUILayout.PropertyField(_displayName);
//         EditorGUILayout.PropertyField(_description);
//         // _description.stringValue = EditorGUILayout.TextArea(_description.stringValue, GUILayout.MinHeight(50));
//         EditorGUILayout.PropertyField(_skill);
//         EditorGUILayout.IntSlider(_abilityActivation, 0, 8);
//         EditorGUILayout.PropertyField(_abilities, true);
//         
//         serializedObject.ApplyModifiedProperties();
//     }
// }
//
// public class UnitSOEditorWindow : ExtendedEditorWindow
// {
//     [SerializeField] private static int _partPanelWidth = 9;
//
//     private GridPartSO[] _partArr;
//     
//     public static void Open(UnitSO unitSO)
//     {
//         UnitSOEditorWindow window = GetWindow<UnitSOEditorWindow>("UnitSO editor");
//         if (unitSO._parts == null || unitSO._parts.Length == 0)
//             unitSO._parts = new GridPartSO[_partPanelWidth * _partPanelWidth];
//         window._partArr = new GridPartSO[_partPanelWidth * _partPanelWidth];
//         window.serializedObject = new SerializedObject(unitSO);
//     }
//
//     private void OnGUI()
//     {
//         // DrawProperties(serializedObject.FindProperty("_displayName"), true);
//         // DrawProperties(serializedObject.FindProperty("_id"), true);
//
//
//         int startX = 0;
//         int startY = 0;
//         int dX = 64;
//         int dY = 64;
//
//         UnitSO unitSO = serializedObject.targetObject as UnitSO;
//         for (int i = 0; i < _partPanelWidth; i++)
//         {
//             for (int j = 0; j < _partPanelWidth; j++)
//             {
//                 Rect spriteRect = new Rect(startX + j * dX, startY + i * dY, 64, 64);
//                 _partArr[i * _partPanelWidth + j] = EditorGUI.ObjectField(
//                     spriteRect, _partArr[i * _partPanelWidth + j], typeof(GridPartSO)) as GridPartSO;
//                 if (_partArr[i * _partPanelWidth + j] != null)
//                     DrawTexturePreview(spriteRect, _partArr[i * _partPanelWidth + j].icon);
//                 unitSO._parts[i * _partPanelWidth + j] = _partArr[i * _partPanelWidth + j];
//             }
//         }
//     }
//     
//     private void DrawTexturePreview(Rect position, Sprite sprite)
//     {
//         Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
//         Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);
//  
//         Rect coords = sprite.textureRect;
//         coords.x /= fullSize.x;
//         coords.width /= fullSize.x;
//         coords.y /= fullSize.y;
//         coords.height /= fullSize.y;
//  
//         Vector2 ratio;
//         ratio.x = position.width / size.x;
//         ratio.y = position.height / size.y;
//         float minRatio = Mathf.Min(ratio.x, ratio.y);
//  
//         Vector2 center = position.center;
//         position.width = size.x * minRatio;
//         position.height = size.y * minRatio;
//         position.center = center;
//  
//         GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
//     }
// }
//
// public class ExtendedEditorWindow : EditorWindow
// {
//     protected SerializedObject serializedObject;
//     protected SerializedProperty currentProperty;
//
//     protected void DrawProperties(SerializedProperty prop, bool drawChildren)
//     {
//         string lastPropPath = string.Empty;
//         foreach (SerializedProperty p in prop)
//         {
//             if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
//             {
//                 EditorGUILayout.BeginHorizontal();
//                 p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
//                 EditorGUILayout.EndHorizontal();
//
//                 if (p.isExpanded)
//                 {
//                     EditorGUI.indentLevel++;
//                     DrawProperties(p, drawChildren);
//                     EditorGUI.indentLevel--;
//                 }
//             }
//             else
//             {
//                 if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }
//                 lastPropPath = p.propertyPath;
//                 EditorGUILayout.PropertyField(p, drawChildren);
//             }
//         }
//     }
//
//     protected void DrawProperties(SerializedProperty prop, bool drawChildren, Action onValueChanged)
//     {
//         EditorGUI.BeginChangeCheck();
//         DrawProperties(prop, drawChildren);
//         if (EditorGUI.EndChangeCheck()) onValueChanged();
//     }
//     
//     protected void DrawField(string propName, bool relative)
//     {
//         if (relative && currentProperty != null)
//         {
//             EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propName), relative);
//         }
//         else if (serializedObject != null)
//         {
//             EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), relative);
//         }
//     }
//     
//     protected void DrawField(string propName, bool relative, Action onValueChanged)
//     {
//         EditorGUI.BeginChangeCheck();
//         DrawField(propName, relative);
//         if (EditorGUI.EndChangeCheck()) onValueChanged();
//     }
//
//     protected void VerticalBoxRegion(int width, int minHeight, bool expandable, Action contentGUI)
//     {
//         EditorGUILayout.BeginVertical(
//             "box", 
//             GUILayout.MaxWidth(width), 
//             GUILayout.MinWidth(width), 
//             GUILayout.MinHeight(minHeight),
//             GUILayout.ExpandHeight(expandable)
//             );
//         contentGUI();
//         EditorGUILayout.EndVertical();
//     }
//     
//     protected void HorinzontalBoxRegion(int minWidth, int height, bool expandable, Action contentGUI)
//     {
//         EditorGUILayout.BeginHorizontal(
//             "box", 
//             GUILayout.MinWidth(minWidth), 
//             GUILayout.MaxHeight(height), 
//             GUILayout.MinHeight(height),
//             GUILayout.ExpandHeight(expandable)
//         );
//         contentGUI();
//         EditorGUILayout.EndHorizontal();
//     }
// }
