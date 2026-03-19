using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

// Manages dynamic loading and unloading of UI assets using Unity's Addressable Asset System.
// This script loads both VisualTreeAsset and StyleSheet assets at runtime and applies them to a PanelRenderer component,
// providing efficient memory management and asynchronous loading capabilities.
public class AddressableExample : MonoBehaviour
{
    [SerializeField] private string uxmlAssetKey; // Addressable key for the UXML asset to load.
    [SerializeField] private AssetReference ussAsset; // AssetReference for the USS asset to load.
    private PanelRenderer panelRenderer;
    private AsyncOperationHandle<VisualTreeAsset> uxmlLoadHandle;
    private AsyncOperationHandle<StyleSheet> ussLoadHandle;
    private VisualTreeAsset loadedAsset;
    private StyleSheet loadedStyleSheet;

    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        if (panelRenderer != null)
        {
            panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
    }
    
    private void OnDisable()
    {
        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }
        UnloadUIDocument();
    }
    
    // Callback method invoked when the PanelRenderer reloads the UI.
    private void OnUIReload(PanelRenderer renderer, VisualElement rootElement)
    {
        // Load assets when UI is ready to receive them.
        if (!uxmlLoadHandle.IsValid())
        {
            LoadUIDocument();
        }
        
        if (!ussLoadHandle.IsValid())
        {
            LoadStyleSheet();
        }
        
        // Apply stylesheet if already loaded; guard against adding the same sheet on every reload.
        if (loadedStyleSheet != null && !rootElement.styleSheets.Contains(loadedStyleSheet))
        {
            rootElement.styleSheets.Add(loadedStyleSheet);
        }
    }
    
    private void LoadUIDocument()
    {
        Debug.Log($"Loading {uxmlAssetKey}");
        uxmlLoadHandle = Addressables.LoadAssetAsync<VisualTreeAsset>(uxmlAssetKey);
        uxmlLoadHandle.Completed += UxmlHandle_Completed;
    }
    
    private void LoadStyleSheet()
    {
        if (ussAsset != null)
        {
            Debug.Log($"Loading {ussAsset.RuntimeKey}");
            ussLoadHandle = Addressables.LoadAssetAsync<StyleSheet>(ussAsset);
            ussLoadHandle.Completed += UssHandle_Completed;
        }
        else
        {
            Debug.LogWarning("USS AssetReference is null, skipping style sheet loading.");
        }
    }
    
    private void UxmlHandle_Completed(AsyncOperationHandle<VisualTreeAsset> obj)
    {
        if (!isActiveAndEnabled || panelRenderer == null)
            return;
            
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            loadedAsset = obj.Result;
            panelRenderer.visualTreeAsset = loadedAsset;
        }
        else
        {
            Debug.LogError($"AssetKey {uxmlAssetKey} failed to load.");
        }
    }
    
    private void UssHandle_Completed(AsyncOperationHandle<StyleSheet> obj)
    {
        if (!isActiveAndEnabled || panelRenderer == null)
            return;
            
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            loadedStyleSheet = obj.Result;
        }
        else
        {
            Debug.LogError($"AssetReference {(ussAsset != null ? ussAsset.RuntimeKey : "(null)")} failed to load.");
        }
    }
    
    private void UnloadUIDocument()
    {
        if (uxmlLoadHandle.IsValid())
        {
            Debug.Log($"Unloading {uxmlAssetKey}");
            uxmlLoadHandle.Completed -= UxmlHandle_Completed;
            loadedAsset = null;
            Addressables.Release(uxmlLoadHandle);
            uxmlLoadHandle = default;
        }
        
        if (ussLoadHandle.IsValid())
        {
            Debug.Log($"Unloading {(ussAsset != null ? ussAsset.RuntimeKey : "(null)")}");
            ussLoadHandle.Completed -= UssHandle_Completed;
            loadedStyleSheet = null;
            Addressables.Release(ussLoadHandle);
            ussLoadHandle = default;
        }
    }
    
    private void OnDestroy()
    {
        // Clean up any remaining handles that weren't released in UnloadUIDocument.
        if (uxmlLoadHandle.IsValid())
        {
            uxmlLoadHandle.Completed -= UxmlHandle_Completed;
            Addressables.Release(uxmlLoadHandle);
            uxmlLoadHandle = default;
        }
        
        if (ussLoadHandle.IsValid())
        {
            ussLoadHandle.Completed -= UssHandle_Completed;
            Addressables.Release(ussLoadHandle);
            ussLoadHandle = default;
        }
    }
}