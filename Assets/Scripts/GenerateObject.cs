using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateObject : MonoBehaviour
{
    [System.Serializable]
    public class ObjectInfo
    {
        public Transform object3D;  // Référence à l'objet 3D
        public Sprite imageObject3D;  // Référence à l'image de l'objet
        [TextArea] public string description;  // Description de l'objet

        // Stocker la position et la rotation initiales
        [HideInInspector] public Vector3 initialPosition;
        [HideInInspector] public Quaternion initialRotation;
    }

    public ObjectInfo[] objects;  // Liste d'objets avec image et description

    public GameObject InstantiateGameObject; // GameObject déjà initialisé dans la scène

    public GameObject parentContent;
    public GameObject UIMuseeNumerique;
    public GameObject UIDecouvrir;

    static public int index;

    // Start is called before the first frame update
    void Start()
    {
        if (InstantiateGameObject == null)
        {
            Debug.LogError("Le GameObject initialisé n'est pas assigné.");
            return;
        }

        Vector3 initialPosition = InstantiateGameObject.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 0; i < objects.Length; i++)
        {
            // Créer une copie du GameObject initialisé sous parentContent
            GameObject newObject = Instantiate(InstantiateGameObject, parentContent.transform);

            // Appliquer la nouvelle position (si nécessaire)
            Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
            newObject.GetComponent<RectTransform>().anchoredPosition = newPosition;

            // Mettre à jour l'image et la description
            UpdateChildImage(newObject.transform, "ImageObject", objects[i].imageObject3D);
            UpdateChildText(newObject.transform, "TextDescription", objects[i].description);

            // Récupérer et assigner l'index au bouton BtnDecouvrir
            AssignButtonListener(newObject.transform, "BtnDecouvrir", i);
        }
    }

    // Méthode pour mettre à jour le texte d'un enfant
    void UpdateChildText(Transform parent, string childName, string newText)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == childName)
            {
                TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = newText;
                }
                else
                {
                    Debug.LogError($"TextMeshProUGUI n'est pas trouvé sur le GameObject '{childName}'.");
                }
                break; // Sortir de la boucle après avoir trouvé et mis à jour l'élément
            }
        }
    }

    // Méthode pour mettre à jour l'image d'un enfant
    void UpdateChildImage(Transform parent, string childName, Sprite newImage)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == childName)
            {
                Image imageComponent = child.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = newImage;
                }
                else
                {
                    Debug.LogError($"Image n'est pas trouvé sur le GameObject '{childName}'.");
                }
                break; // Sortir de la boucle après avoir trouvé et mis à jour l'élément
            }
        }
    }

    // Méthode pour récupérer le bouton et lui ajouter un listener
    void AssignButtonListener(Transform parent, string buttonName, int index)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == buttonName)
            {
                Button buttonComponent = child.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    // Ajouter un listener au bouton qui accède à la page en fonction de l'index
                    buttonComponent.onClick.AddListener(() => OnButtonClick(index));
                }
                else
                {
                    Debug.LogError($"Button n'est pas trouvé sur le GameObject '{buttonName}'.");
                }
                break; // Sortir de la boucle après avoir trouvé et assigné le listener
            }
        }
    }

    // Cette méthode sera appelée lorsque le bouton est cliqué
    void OnButtonClick(int indexValue)
    {
        index = indexValue;
        Debug.Log("Bouton cliqué pour l'objet à l'index : " + indexValue);

        // Activer l'objet actuel
        objects[index].object3D.gameObject.SetActive(true);

        UIMuseeNumerique.gameObject.SetActive(false);
        UIDecouvrir.gameObject.SetActive(true);
        // Navigue vers une nouvelle page ou exécute une action basée sur l'index
        // Exemple : Charger une nouvelle scène ou afficher plus de détails
        // SceneManager.LoadScene("NomDeLaScene");  // Si tu veux charger une nouvelle scène
    }

    // Update is called once per frame
    void Update()
    {
    }
}
