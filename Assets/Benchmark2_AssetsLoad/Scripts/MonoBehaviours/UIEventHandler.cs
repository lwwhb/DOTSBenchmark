using Benchmark2_AssetsLoad.Scripts.Systems;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Benchmark2_AssetsLoad.Scripts.MonoBehaviours
{
    enum ButtonState
    {
        BS_UnLoaded = 0,
        BS_Loading,
        BS_Loaded,
        BS_Unloading
    }
    public class UIEventHandler : MonoBehaviour
    {
        public Button loadEntityPrefabButton;
        public Button loadGameObjectPrefabButton;
        ButtonState _entityPrefabButtonState = ButtonState.BS_UnLoaded;
        ButtonState _gameObjectPrefabButtonState = ButtonState.BS_UnLoaded;
#if !USE_UNMANAGEDSYSTEM
        private UIInteropManagedSystem _uiInteropManagedSystem;
#else
        public Action OnLoadEntityPrefab;
        public Action OnLoadGameObjectPrefab;
        public Action OnUnloadEntityPrefab;
        public Action OnUnloadGameObjectPrefab;
#endif
        public void Start()
        {
#if !USE_UNMANAGEDSYSTEM
            _uiInteropManagedSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged(typeof(UIInteropManagedSystem)) as
                UIInteropManagedSystem;
            if (_uiInteropManagedSystem != null)
            {
                _uiInteropManagedSystem.SetUIEventHandler(this);
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(_uiInteropManagedSystem);
            }
#else
            var uiInteropSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<UIInteropSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                .AddSystemToUpdateList(uiInteropSystem);
#endif
        }

        public void HandleLoadEntityPrefab()
        {
                if (_entityPrefabButtonState == ButtonState.BS_UnLoaded)
                {
                    SetEntityPrefabButtonState(ButtonState.BS_Loading);
#if !USE_UNMANAGEDSYSTEM
                    if(_uiInteropManagedSystem != null)
                        _uiInteropManagedSystem.LoadEntityPrefab();
#else
                    OnLoadEntityPrefab?.Invoke();
#endif
                    
                }
                else if(_entityPrefabButtonState == ButtonState.BS_Loaded)
                {
                    SetEntityPrefabButtonState(ButtonState.BS_Unloading);
#if !USE_UNMANAGEDSYSTEM
                    if(_uiInteropManagedSystem != null)
                        _uiInteropManagedSystem.UnloadEntityPrefab();
#else
                    OnUnloadEntityPrefab?.Invoke();
#endif
                }
        }

        public void HandleLoadGameObjectPrefab()
        {
                if (_gameObjectPrefabButtonState == ButtonState.BS_UnLoaded)
                {
                    SetGameObjectPrefabButtonState(ButtonState.BS_Loading);
#if !USE_UNMANAGEDSYSTEM
                    if(_uiInteropManagedSystem != null)
                        _uiInteropManagedSystem.LoadGoPrefab();
#else
                    OnLoadGameObjectPrefab?.Invoke();
#endif
                }
                else if(_gameObjectPrefabButtonState == ButtonState.BS_Loaded)
                {
                    SetGameObjectPrefabButtonState(ButtonState.BS_Unloading);
#if !USE_UNMANAGEDSYSTEM
                    if(_uiInteropManagedSystem != null)
                        _uiInteropManagedSystem.UnloadGoPrefab();
#else
                    OnUnloadGameObjectPrefab?.Invoke();
#endif
                }
        }

        public void OnEntityPrefabLoaded()
        {
            Debug.Log("OnEntityPrefabLoaded");
            SetEntityPrefabButtonState(ButtonState.BS_Loaded);
        }
        
        public void OnEntityPrefabUnloaded()
        {
            Debug.Log("OnEntityPrefabUnloaded");
            SetEntityPrefabButtonState(ButtonState.BS_UnLoaded);
        }
        
        public void OnGameObjectPrefabLoaded()
        {
            Debug.Log("OnGameObjectPrefabLoaded");
            SetGameObjectPrefabButtonState(ButtonState.BS_Loaded);
        }

        public void OnGameObjectPrefabUnloaded()
        {
            Debug.Log("OnEntityGameObjectUnloaded");
            SetGameObjectPrefabButtonState(ButtonState.BS_UnLoaded);
        }
        
        private void SetEntityPrefabButtonState(ButtonState state)
        {
            if (!_entityPrefabButtonState.Equals(state))
            {
                if (state == ButtonState.BS_UnLoaded)
                {
                    if (loadEntityPrefabButton != null)
                    {
                        loadEntityPrefabButton.enabled = true;
                        loadEntityPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "LoadEntityPrefab";
                    }
                }
                else if (state == ButtonState.BS_Loading)
                {
                    if (loadEntityPrefabButton != null)
                    {
                        loadEntityPrefabButton.enabled = false;
                        loadEntityPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "Loading...";
                    }
                }
                else if(state == ButtonState.BS_Loaded)
                {
                    if (loadEntityPrefabButton != null)
                    {
                        loadEntityPrefabButton.enabled = true;
                        loadEntityPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "UnLoadEntityPrefab";
                    }
                }
                else if(state == ButtonState.BS_Unloading)
                {
                    if (loadEntityPrefabButton != null)
                    {
                        loadEntityPrefabButton.enabled = false;
                        loadEntityPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "UnLoading...";
                    }
                }
                _entityPrefabButtonState = state;
            }
        }
        
        private void SetGameObjectPrefabButtonState(ButtonState state)
        {
            if (!_gameObjectPrefabButtonState.Equals(state))
            {
                if (state == ButtonState.BS_UnLoaded)
                {
                    if (loadGameObjectPrefabButton != null)
                    {
                        loadGameObjectPrefabButton.enabled = true;
                        loadGameObjectPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "LoadGoPrefab";
                    }
                }
                else if (state == ButtonState.BS_Loading)
                {
                    if (loadGameObjectPrefabButton != null)
                    {
                        loadGameObjectPrefabButton.enabled = false;
                        loadGameObjectPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "Loading...";
                    }
                }
                else if(state == ButtonState.BS_Loaded)
                {
                    if (loadGameObjectPrefabButton != null)
                    {
                        loadGameObjectPrefabButton.enabled = true;
                        loadGameObjectPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "UnLoadGoPrefab";
                    }
                }
                else if(state == ButtonState.BS_Unloading)
                {
                    if (loadGameObjectPrefabButton != null)
                    {
                        loadGameObjectPrefabButton.enabled = false;
                        loadGameObjectPrefabButton.gameObject.GetComponentInChildren<TMP_Text>().text = "UnLoading...";
                    }
                }
                _gameObjectPrefabButtonState = state;
            }
        }
    }
}