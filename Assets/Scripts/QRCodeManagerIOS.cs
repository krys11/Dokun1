using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS
using UnityEngine.XR.ARKit;
#endif

public class QRCodeManagerIOS : MonoBehaviour
{
    [Header("AR Configuration")]
    public ARTrackedImageManager trackedImageManager;
    public List<PrefabForQR> prefabsForQR;

    [Header("iOS Specific Settings")]
    [SerializeField]
    private bool autoFocus = true;
    [SerializeField, Range(0.5f, 4f), Tooltip("Distance maximale de détection en mètres (recommandé: 0.5-2m)")]
    private float maxTrackingDistance = 2.0f;
    [SerializeField, Range(0.1f, 0.5f), Tooltip("Taille minimale du QR code en mètres (recommandé: 0.2m)")]
    private float minQRCodeSize = 0.2f;

    private Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();
    private bool isARKitSupported = false;

    [System.Serializable]
    public class PrefabForQR
    {
        public string qrCodeName;
        public GameObject prefab;
        [Tooltip("Distance maximale de détection pour ce QR code spécifique")]
        public float maxDetectionDistance = 3.0f;
    }

    void Start()
    {
        // Vérifier le support ARKit
        CheckARKitSupport();
        
        if (!isARKitSupported)
        {
            Debug.LogWarning("ARKit n'est pas supporté sur cet appareil");
            enabled = false;
            return;
        }

        ConfigureARKit();
    }

    private void CheckARKitSupport()
    {
#if UNITY_IOS
        var subsystems = new List<XRImageTrackingSubsystem>();
        SubsystemManager.GetInstances(subsystems);
        isARKitSupported = subsystems.Count > 0;
#else
        isARKitSupported = false;
        Debug.LogWarning("Cette version est spécifique à iOS");
#endif
    }

    private void ConfigureARKit()
    {
#if UNITY_IOS
        if (trackedImageManager != null)
        {
            var arCameraManager = FindObjectOfType<ARCameraManager>();
            if (arCameraManager != null)
            {
                // Configuration de la caméra AR
                arCameraManager.autoFocusRequested = autoFocus;
            }
            
            // Configuration du tracking d'images
            trackedImageManager.requestedMaxNumberOfMovingImages = 1;
            
            // Activer le tracking
            trackedImageManager.enabled = true;
            Debug.Log("Configuration iOS : Tracking d'images configuré");
        }
#endif
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Gérer les nouvelles images détectées
        foreach (var trackedImage in eventArgs.added)
        {
            HandleNewTrackedImage(trackedImage);
        }

        // Gérer les images mises à jour
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        // Gérer les images supprimées
        foreach (var trackedImage in eventArgs.removed)
        {
            RemoveTrackedImage(trackedImage);
        }
    }

    private void HandleNewTrackedImage(ARTrackedImage trackedImage)
    {
        var referenceImageName = trackedImage.referenceImage.name;
        Debug.Log($"QR Code détecté (iOS): {referenceImageName}");

        // Vérifier la taille du QR code
        float qrCodeSize = trackedImage.size.magnitude;
        if (qrCodeSize < minQRCodeSize)
        {
            Debug.LogWarning($"QR Code trop petit ({qrCodeSize}m). Minimum requis: {minQRCodeSize}m");
            return;
        }

        var prefabForQR = prefabsForQR.Find(p => p.qrCodeName == referenceImageName);

        if (prefabForQR != null && !instantiatedPrefabs.ContainsKey(referenceImageName))
        {
            if (IsWithinTrackingDistance(trackedImage, prefabForQR.maxDetectionDistance))
            {
                GameObject newPrefab = Instantiate(prefabForQR.prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                instantiatedPrefabs.Add(referenceImageName, newPrefab);
            }
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        var referenceImageName = trackedImage.referenceImage.name;

        if (instantiatedPrefabs.TryGetValue(referenceImageName, out GameObject prefabObject))
        {
            // Vérifier la qualité du tracking et la distance
            if (trackedImage.trackingState == TrackingState.Tracking && 
                IsWithinTrackingDistance(trackedImage, maxTrackingDistance))
            {
                prefabObject.SetActive(true);
                prefabObject.transform.position = trackedImage.transform.position;
                prefabObject.transform.rotation = trackedImage.transform.rotation;

                // Mise à jour fluide de la position (spécifique iOS)
                StartCoroutine(SmoothUpdatePosition(prefabObject.transform, trackedImage.transform));
            }
            else
            {
                prefabObject.SetActive(false);
            }
        }
    }

    private void RemoveTrackedImage(ARTrackedImage trackedImage)
    {
        var referenceImageName = trackedImage.referenceImage.name;

        if (instantiatedPrefabs.TryGetValue(referenceImageName, out GameObject prefabObject))
        {
            Destroy(prefabObject);
            instantiatedPrefabs.Remove(referenceImageName);
        }
    }

    private bool IsWithinTrackingDistance(ARTrackedImage trackedImage, float maxDistance)
    {
        float distance = Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position);
        return distance <= maxDistance;
    }

    private System.Collections.IEnumerator SmoothUpdatePosition(Transform objectTransform, Transform targetTransform)
    {
        float elapsedTime = 0;
        float updateDuration = 0.1f; // Durée de l'interpolation en secondes
        Vector3 startPosition = objectTransform.position;
        Quaternion startRotation = objectTransform.rotation;

        while (elapsedTime < updateDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / updateDuration;

            objectTransform.position = Vector3.Lerp(startPosition, targetTransform.position, t);
            objectTransform.rotation = Quaternion.Lerp(startRotation, targetTransform.rotation, t);

            yield return null;
        }
    }

    private void OnDestroy()
    {
        // Nettoyage des ressources
        foreach (var prefab in instantiatedPrefabs.Values)
        {
            if (prefab != null)
                Destroy(prefab);
        }
        instantiatedPrefabs.Clear();
    }

    // Méthode de debug pour iOS
    private void LogTrackingQuality(ARTrackedImage trackedImage)
    {
#if UNITY_IOS && !UNITY_EDITOR
        Debug.Log($"Tracking State: {trackedImage.trackingState}");
        Debug.Log($"Distance to camera: {Vector3.Distance(Camera.main.transform.position, trackedImage.transform.position)}m");
#endif
    }
}
