using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class QRCodeManager : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;  // Référence au ARTrackedImageManager
    public List<PrefabForQR> prefabsForQR;  // Liste associant QR code et prefabs à afficher

    private Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();

    [System.Serializable]
    public class PrefabForQR
    {
        public string qrCodeName;  // Le nom/identifiant du QR code
        public GameObject prefab;  // Le prefab à afficher
    }

    void OnEnable()
    {
        // S'abonner à l'événement quand une image est détectée
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        // Se désabonner pour éviter les erreurs
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Parcourir toutes les images détectées
        foreach (var trackedImage in eventArgs.added)
        {
            // Identifier le QR code détecté par son nom
            var referenceImageName = trackedImage.referenceImage.name;
            Debug.Log("QR Code détecté: " + referenceImageName);

            // Chercher le prefab correspondant au QR code
            var prefabForQR = prefabsForQR.Find(p => p.qrCodeName == referenceImageName);

            if (prefabForQR != null && !instantiatedPrefabs.ContainsKey(referenceImageName))
            {
                // Instancier le prefab correspondant au QR code
                GameObject newPrefab = Instantiate(prefabForQR.prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                instantiatedPrefabs.Add(referenceImageName, newPrefab);
            }
        }

        // Gérer les images mises à jour (si la position ou la rotation change)
        foreach (var trackedImage in eventArgs.updated)
        {
            var referenceImageName = trackedImage.referenceImage.name;

            if (instantiatedPrefabs.ContainsKey(referenceImageName))
            {
                // Mettre à jour la position du prefab en fonction du QR code
                instantiatedPrefabs[referenceImageName].transform.position = trackedImage.transform.position;
                instantiatedPrefabs[referenceImageName].transform.rotation = trackedImage.transform.rotation;
            }
        }

        // Gérer les images supprimées
        foreach (var trackedImage in eventArgs.removed)
        {
            var referenceImageName = trackedImage.referenceImage.name;

            if (instantiatedPrefabs.ContainsKey(referenceImageName))
            {
                // Détruire le prefab correspondant
                Destroy(instantiatedPrefabs[referenceImageName]);
                instantiatedPrefabs.Remove(referenceImageName);
            }
        }
    }
}
