using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Utilisé pour TextMeshPro
using UnityEngine.UI;  // Utilisé pour Image et Button

public class ElementManager : MonoBehaviour
{
    [System.Serializable]
    public class ElementData
    {
        public string name;
        [TextArea] public string description;
        public Sprite image;  // Pour stocker l'image
        public Transform object3D;  // Non utilisé dans ce cas, mais peut être utilisé pour autre chose

        [HideInInspector] public Vector3 initialPosition;
        [HideInInspector] public Quaternion initialRotation;
    }

    public ElementData[] elementList;  // Liste des éléments
    public GameObject prefab;  // Le prefab à utiliser
    public Transform parent;  // L'objet parent où les prefabs seront instanciés

    public int maxLines = 3; // Nombre maximal de lignes visibles pour TextDes

    public GameObject UIObjectList;
    public GameObject UIObjectViewer3D;

    static public int index;


    void Start()
    {
        // Parcourt chaque élément de la liste et l'instancie
        for (int i = 0; i < elementList.Length; i++)
        {
            ElementData element = elementList[i];

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

            // Définir le nombre maximal de lignes visibles
            contentTextDes.maxVisibleLines = maxLines;

            // Optionnel : masquer le texte dépassant les limites
            contentTextDes.overflowMode = TextOverflowModes.Masking;

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

            // Mettre la description dans "TextDes" enfant de "ImageOmbre"
            TextMeshProUGUI imageOmbreTextDes = imageOmbreTransform.Find("TextDes").GetComponent<TextMeshProUGUI>();
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
            elementList[index].object3D.gameObject.SetActive(true);

            // Ici, tu peux appeler une autre fonction ou passer l'index à un autre script
            // Exemple d'appel à une fonction dans un autre script
            // AnotherScript.Instance.HandleElementSelection(index);
            UIObjectViewer3D.SetActive(true);
            UIObjectList.SetActive(false);
        }
    }
}
