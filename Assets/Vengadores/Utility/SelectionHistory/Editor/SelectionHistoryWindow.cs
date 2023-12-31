﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Vengadores.Utility.SelectionHistory.Editor
{
    public class SelectionHistoryWindow : EditorWindow
    {
        [MenuItem("Smashlab/Utility/Selection History")]
        public static void Init()
        {
            var window = GetWindow<SelectionHistoryWindow>();
        
            //Options
            window.autoRepaintOnSceneChange = true;
            window.titleContent.image = EditorGUIUtility.IconContent(EditorGUIUtility.isProSkin ? "d_UnityEditor.SceneHierarchyWindow" : "UnityEditor.SceneHierarchyWindow").image;
            window.titleContent.text = " Selection History";
            window.wantsMouseMove = true;

            //Show
            window.Show();
        }

        private string IconPrefix => EditorGUIUtility.isProSkin ? "d_" : "";

        private static bool RecordHierarchy
        {
            get => EditorPrefs.GetBool(PlayerSettings.productName + "_SH_RecordHierachy", true);
            set => EditorPrefs.SetBool(PlayerSettings.productName + "_SH_RecordHierachy", value);
        }

        private static bool RecordProject
        {
            get => EditorPrefs.GetBool(PlayerSettings.productName + "_SH_RecordProject", true);
            set => EditorPrefs.SetBool(PlayerSettings.productName + "_SH_RecordProject", value);
        }

        private static int MaxHistorySize
        {
            get => EditorPrefs.GetInt(PlayerSettings.productName + "_SH_MaxHistorySize", 50);
            set => EditorPrefs.SetInt(PlayerSettings.productName + "_SH_MaxHistorySize", value);
        }

        private AnimBool _settingAnimation;
        private bool _settingExpanded;
        private AnimBool _clearAnimation;
        private bool _historyVisible = true;

        private List<Object> _selectionHistory = new List<Object>();
        private static bool _muteRecording;
        private int _selectedIndex = -1;

        private bool _hasFocus;
    
        private void OnSelectionChange()
        {
            Repaint();
        
            if (_muteRecording || !Selection.activeObject) return;

            AddToHistory();
        }

        private void OnFocus()
        {
            //Items have have been deleted and should be removed from history
            _selectionHistory = _selectionHistory.Where(x => x != null).ToList();

            _hasFocus = true;
        }
    
        private void OnLostFocus()
        {
            _hasFocus = false;
        }

        private void OnInspectorUpdate() //10 fps
        {
            if (_hasFocus)  Repaint();
        }

        private void AddToHistory()
        {
            //Skip selected folders and such
            if (Selection.activeObject is DefaultAsset) return;

            if (EditorUtility.IsPersistent(Selection.activeObject) && !RecordProject) return;
            if (EditorUtility.IsPersistent(Selection.activeObject) == false && !RecordHierarchy) return;
        
            if(_selectionHistory.Contains(Selection.activeObject) == false) _selectionHistory.Insert(0, Selection.activeObject);
        
            //Trim end
            if(_selectionHistory.Count-1 == MaxHistorySize) _selectionHistory.RemoveAt(_selectionHistory.Count-1);
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += ListenForNavigationInput;
            
            _settingAnimation = new AnimBool(false);
            _settingAnimation.valueChanged.AddListener(Repaint);
            _settingAnimation.speed = 4f;
            _clearAnimation = new AnimBool(false);
            _clearAnimation.valueChanged.AddListener(Repaint);
            _clearAnimation.speed = _settingAnimation.speed;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= ListenForNavigationInput;
        }
    
        private void ListenForNavigationInput(SceneView sceneView)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.isKey && Event.current.keyCode == KeyCode.LeftBracket)
            {
                SelectPrevious();
            }
            if (Event.current.type == EventType.KeyDown &&  Event.current.isKey && Event.current.keyCode == KeyCode.RightBracket)
            {
                SelectNext();
            }
        }
    
        private void SetSelection(Object target, int index)
        {
            _muteRecording = true;
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
            _muteRecording = false;
        }

        private void SelectPrevious()
        {
            _selectedIndex--;
            _selectedIndex = Mathf.Clamp(_selectedIndex, 0, _selectionHistory.Count - 1);
            
            SetSelection(_selectionHistory[_selectedIndex], _selectedIndex);
        }

        private void SelectNext()
        {
            _selectedIndex++;
            _selectedIndex = Mathf.Clamp(_selectedIndex, 0, _selectionHistory.Count - 1);

            SetSelection(_selectionHistory[_selectedIndex], _selectedIndex);
        }

        private Vector2 _scrollPos;

        private void OnGUI()
        {
            _hasFocus = _hasFocus || (Event.current.type == EventType.MouseMove);

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(_selectionHistory.Count == 0))
                {
                    using (new EditorGUI.DisabledScope(_selectedIndex == _selectionHistory.Count-1))
                    {
                        if (GUILayout.Button(
                                new GUIContent(EditorGUIUtility.IconContent(IconPrefix + "back@2x").image,
                                    "Select previous (Left bracket key)"), EditorStyles.miniButtonLeft, GUILayout.Height(20f),
                                GUILayout.Width(30f)))
                        {
                            SelectNext();
                        }
                    }

                    using (new EditorGUI.DisabledScope(_selectedIndex == 0))
                    {
                        if (GUILayout.Button(
                                new GUIContent(EditorGUIUtility.IconContent(IconPrefix + "forward@2x").image,
                                    "Select next (Right bracket key)"), EditorStyles.miniButtonRight, GUILayout.Height(20),
                                GUILayout.Width(30f)))
                        {
                            SelectPrevious();
                        }
                    }

                    if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent(IconPrefix + "TreeEditor.Trash").image, "Clear history"), EditorStyles.miniButton))
                    {
                        _historyVisible = false;
                    }
                }
            
                GUILayout.FlexibleSpace();
            
                _settingExpanded = GUILayout.Toggle(_settingExpanded, new GUIContent(EditorGUIUtility.IconContent(IconPrefix + "Settings").image, "Edit settings"), EditorStyles.miniButtonMid);
                _settingAnimation.target = _settingExpanded;
            }
        
            if (EditorGUILayout.BeginFadeGroup(_settingAnimation.faded))
            {
                EditorGUILayout.Space();

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Record", EditorStyles.boldLabel, GUILayout.Width(100f));
                    RecordHierarchy = EditorGUILayout.ToggleLeft("Hierarchy", RecordHierarchy, GUILayout.MaxWidth(80f));
                    RecordProject = EditorGUILayout.ToggleLeft("Project window", RecordProject);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("History size", EditorStyles.boldLabel,GUILayout.Width(100f));
                    MaxHistorySize = EditorGUILayout.IntField(MaxHistorySize, GUILayout.MaxWidth(40f));
                }
            
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        
            _clearAnimation.target = !_historyVisible;
        
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, EditorStyles.helpBox, GUILayout.MaxHeight(maxSize.y-20f));
            {
                EditorGUILayout.BeginFadeGroup(1f-_clearAnimation.faded);
            
                var prevColor = GUI.color;
                var prevBgColor = GUI.backgroundColor;

                for (var i = 0; i < _selectionHistory.Count; i++)
                {
                    if(_selectionHistory[i] == null) continue;
                
                    var rect = EditorGUILayout.BeginHorizontal();
                
                    GUI.color = i % 2 == 0 ?  Color.grey * (EditorGUIUtility.isProSkin ? 1f : 1.7f) : Color.grey * (EditorGUIUtility.isProSkin ? 1.05f : 1.66f);
                
                    //Hover color
                    if (rect.Contains(Event.current.mousePosition) || Selection.activeObject == (_selectionHistory[i]))
                    {
                        GUI.color = EditorGUIUtility.isProSkin ? Color.grey * 1.1f : Color.grey * 1.5f;
                    }
                
                    //Selection outline
                    if (Selection.activeObject == (_selectionHistory[i]))
                    {
                        var outline = rect;
                        outline.x -= 1;
                        outline.y -= 1;
                        outline.width += 2;
                        outline.height += 2;
                        EditorGUI.DrawRect(outline, EditorGUIUtility.isProSkin ? Color.gray * 1.5f : Color.gray);
                    }

                    //Background
                    EditorGUI.DrawRect(rect, GUI.color);
                
                    GUI.color = prevColor;
                    GUI.backgroundColor = prevBgColor;

                    if (GUILayout.Button(new GUIContent(" " + _selectionHistory[i].name, EditorGUIUtility.ObjectContent(_selectionHistory[i], _selectionHistory[i].GetType()).image), EditorStyles.label, GUILayout.MaxHeight(17f)))
                    {
                        SetSelection(_selectionHistory[i], i);
                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndScrollView();

            //Once the list is collapse, clear the collection
            if(_clearAnimation.faded ==1f) _selectionHistory.Clear();
            //Reset
            if (_selectionHistory.Count == 0) _historyVisible = true;
        }
    }
}