using System;
using System.Collections.Generic;
using System.Linq;
using Crafting.Recipe;
using Ui.Journal;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class JournalEntryEditorWindow : EditorWindow
    {
        private TabCollection _collection;
        private Tab _tab;
        private Page _page;
        private bool IsCollectionNull => _collection == null;

        private SerializedObject _selectedTab;
        private SerializedObject _selectedPage;

        private Vector2 _tabSelectionScrollView;
        private bool _subTabsFoldout;
        private bool _pageFoldout = true;
        private Vector2 _tabScrollView;

        public static void Open(TabCollection collection)
        {
            JournalEntryEditorWindow window = GetWindow<JournalEntryEditorWindow>("Journal Entry Editor");
            window._collection = collection;
        }

        public static void Open(Tab[] collection)
        {
            JournalEntryEditorWindow window = GetWindow<JournalEntryEditorWindow>("Journal Entry Editor");
            TabCollection tabCollection = CreateInstance<TabCollection>();
            tabCollection.tabs = collection;
            window._collection = tabCollection;
        }
        
        private void OnGUI()
        {
            if (IsCollectionNull) return;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("box", GUILayout.Width(200), GUILayout.ExpandHeight(true));
                {
                    _tabSelectionScrollView = EditorGUILayout.BeginScrollView(_tabSelectionScrollView);
                    {
                        foreach (Tab tab in _collection.tabs)
                            if (GUILayout.Button(tab.name)) SelectTab(tab);
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    _tabScrollView = EditorGUILayout.BeginScrollView(_tabScrollView);
                    {
                        if (_selectedTab != null)
                            DrawTab();
                        else
                            EditorGUILayout.LabelField("Please select a Tab from the left.");
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTab()
        {
            if (_selectedTab == null) return;

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(_selectedTab.FindProperty("page"));

                _subTabsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_subTabsFoldout, "SubTabs:");
                if(_subTabsFoldout) {
                    EditorGUILayout.BeginVertical();
                    {
                        foreach (SerializedProperty property in _selectedTab.FindProperty("subTabs"))
                        {
                            EditorGUILayout.BeginHorizontal("box");
                            {
                                EditorGUILayout.PropertyField(property, GUIContent.none);
                                EditorGUI.BeginDisabledGroup(property.objectReferenceValue == null);
                                if(GUILayout.Button("Go To."))
                                    SelectTab(property.objectReferenceValue as Tab);
                                EditorGUI.EndDisabledGroup();
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Add")) _selectedTab.FindProperty("subTabs").arraySize++;
                        if (GUILayout.Button("Remove")) _selectedTab.FindProperty("subTabs").arraySize--;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            EditorGUILayout.EndVertical();

            _page = _tab.page;
            _selectedPage = _page == null ? null : new SerializedObject(_page);
            
            EditorGUILayout.BeginVertical("box");
            {
                if (_selectedPage != null)
                {
                    _pageFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_pageFoldout, "Page:");
                    if(_pageFoldout) DrawPage();
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
                else EditorGUILayout.LabelField("No page selected");
            }
            EditorGUILayout.EndVertical();

            _selectedTab.ApplyModifiedProperties();
        }
        
        private void DrawPage()
        {
            if (_selectedPage == null) return;

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PropertyField(_selectedPage.FindProperty("icon"));
                EditorGUILayout.PropertyField(_selectedPage.FindProperty("description"));
            }
            EditorGUILayout.EndVertical();

            switch (_page)
            {
                case WeaponClassPage _:
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(_selectedPage.FindProperty("weaponClass"));
                    EditorGUILayout.EndVertical();
                    break;
            }

            _selectedPage.ApplyModifiedProperties();
        }

        private void SelectTab(Tab tab)
        {
            _tab = tab;
            _page = null;
            _selectedTab = new SerializedObject(tab);
            _selectedPage = null;
            _tabScrollView = Vector2.zero;
            _subTabsFoldout = false;
            _pageFoldout = true;
        }
    }

    [CustomEditor(typeof(TabCollection))]
    public class TabCollectionClassInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open Editor Window")) JournalEntryEditorWindow.Open(target as TabCollection);
        }
    }
}