using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Managers
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager _instance;
        public static SceneManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<SceneManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value);
                    return;
                }
                _instance = value;
                DontDestroyOnLoad(value);
            }
        }
        public event Action<SceneIndexes> OnBeginSceneChange;
        public event Action<SceneIndexes> OnEndSceneChange;
        
        private void Awake() => Instance = this;

        public SceneIndexes CurrentScene { get; private set; }
        private readonly List<AsyncOperation> loadingProgressList = new List<AsyncOperation>();
        
        public void ChangeScene(SceneIndexes index, Action onCompleteCallBack = null)
        {
            if (index == CurrentScene) return;
         
            OnBeginSceneChange?.Invoke(CurrentScene);
            
            if(CurrentScene != SceneIndexes.Persistent)
                loadingProgressList.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int) CurrentScene));
            loadingProgressList.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int) index, LoadSceneMode.Additive));

            CurrentScene = index;
            StartCoroutine(GetLoadingProgress(onCompleteCallBack));
        }

        IEnumerator GetLoadingProgress(Action onCompleteCallBack = null)
        {
            LoadingScreenManager.HideScreen();
            yield return new WaitForEndOfFrame();
            
            foreach(var i in loadingProgressList)
                while (!i.isDone)
                    yield return null;
            
            LoadingScreenManager.ShowScreen();
            OnEndSceneChange?.Invoke(CurrentScene);
            onCompleteCallBack?.Invoke();
        }
    }
}