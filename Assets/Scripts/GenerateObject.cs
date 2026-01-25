using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenerateObject : MonoBehaviour
{
    [System.Serializable]
    public class ObjectInfo
    {
        public string name;
        [TextArea] public string description;
        public Sprite image;  // Pour stocker l'image
        public Transform object3D;  // Non utilisé dans ce cas, mais peut être utilisé pour autre chose

        [HideInInspector] public Vector3 initialPosition;
        [HideInInspector] public Quaternion initialRotation;
    }

    public ObjectInfo[] objects;  // Liste d'objets avec image et description
    public int maxLines = 3; // Nombre maximal de lignes visibles pour TextDes

    public GameObject UIObjectList;
    public GameObject UIObjectViewer3D;

    public GameObject prefab;  // Le prefab à utiliser
    public Transform parent;

    static public int index;

    void Start()
    {
        // Parcourt chaque élément de la liste et l'instancie
        for (int i = 0; i < objects.Length; i++)
        {
            ObjectInfo element = objects[i];

            // Instancier le prefab
            GameObject instance = Instantiate(prefab, parent);

            // 1er enfant : Mettre le nom dans le composant TextMeshPro "TextName"
            TextMeshProUGUI textName = instance.transform.Find("TextName").GetComponent<TextMeshProUGUI>();
            textName.text = element.name;

            // 2ème enfant : Mettre l'image dans le composant Image "Image"
            Image imageComponent = instance.transform.Find("Image").GetComponent<Image>();
            imageComponent.sprite = element.image;

            // 3ème enfant : Mettre le nom et la description dans le ContentText, gérer le bouton TextLink
            Transform contentText = instance.transform.Find("ContentText");

            // Mettre le nom dans "TextName" enfant de "ContentText"
            TextMeshProUGUI contentTextName = contentText.Find("TextName").GetComponent<TextMeshProUGUI>();
            contentTextName.text = element.name;

            // Mettre la description dans "TextDes" enfant de "ContentText"
            TextMeshProUGUI contentTextDes = contentText.Find("TextDes").GetComponent<TextMeshProUGUI>();
            contentTextDes.text = element.description;

            // Récupérer le bouton "3D" de "ContentText"
            Button button3D = contentText.Find("3D").GetComponent<Button>();

            // Ajouter un listener pour appeler une fonction avec l'index de l'élément
            int currentIndex = i;  // Capture de la valeur actuelle de l'index
            button3D.onClick.AddListener(() => OnButtonClicked(currentIndex));

            // Gérer le bouton "TextLink" qui va afficher "ImageOmbre"
            Button textLinkButton = contentText.Find("TextLink").GetComponent<Button>();
            GameObject imageOmbre = instance.transform.Find("ImageOmbre").gameObject;

            textLinkButton.onClick.AddListener(() =>
            {
                imageOmbre.SetActive(true);  // Activer l'image ombre au clic sur le bouton
            });

            // 4ème enfant : "ImageOmbre" qui est désactivé de base
            // Remplir les informations dans les enfants de "ImageOmbre"
            Transform imageOmbreTransform = imageOmbre.transform;

            // Mettre le nom dans "TextName" enfant de "ImageOmbre"
            TextMeshProUGUI imageOmbreTextName = imageOmbreTransform.Find("TextName").GetComponent<TextMeshProUGUI>();
            imageOmbreTextName.text = element.name;

            // Récupérer le contenu du `ScrollView` dans "ImageOmbre" pour le texte de la description
            Transform scrollViewContent = imageOmbreTransform.Find("Scroll View/Viewport/Content");

            // Mettre la description dans "TextDes" enfant du contenu du `ScrollView`
            TextMeshProUGUI imageOmbreTextDes = scrollViewContent.Find("TextDes").GetComponent<TextMeshProUGUI>();
            imageOmbreTextDes.text = element.description;

            // Gérer le bouton "ImageButton" dans "ImageOmbre" pour le désactiver
            Button imageButton = imageOmbreTransform.Find("ImageButton").GetComponent<Button>();
            imageButton.onClick.AddListener(() =>
            {
                imageOmbre.SetActive(false);  // Désactiver "ImageOmbre" au clic sur le bouton
            });

            // Désactiver "ImageOmbre" au départ
            imageOmbre.SetActive(false);
        }


        // Fonction appelée lors du clic sur le bouton "3D", avec l'index de l'élément
        void OnButtonClicked(int indexx)
        {
            index = indexx;
            Debug.Log("Bouton 3D cliqué pour l'élément d'index : " + index);

            // Activer l'objet actuel
            objects[index].object3D.gameObject.SetActive(true);

            UIObjectViewer3D.SetActive(true);
            UIObjectList.SetActive(false);
        }
    }

    public void OnButtonReturnClick()
    {
        // Vérifie si l'UIObjectViewer3D est actif
        if (UIObjectViewer3D.activeSelf)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                // Désactiver l'objet 3D
                objects[i].object3D.gameObject.SetActive(false);
            }

            // Si oui, désactive UIObjectViewer3D et active UIObjectList
            UIObjectViewer3D.SetActive(false);
            UIObjectList.SetActive(true);
        }
        else if (UIObjectList.activeSelf)
        {
            // Si UIObjectList est déjà actif, charge la nouvelle scène
            SceneManager.LoadScene("HomeScreen");
        }
    }

    public void GoToAR()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            // Désactiver l'objet actuel
            objects[i].object3D.gameObject.SetActive(false);
        }
        SceneManager.LoadScene("ARScreen");
    }
}
