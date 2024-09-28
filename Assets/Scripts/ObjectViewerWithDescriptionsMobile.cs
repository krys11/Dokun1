using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectViewerWithDescriptionsMobile : MonoBehaviour
{
    public TMP_Text descriptionText;  // UI Text pour afficher la description

    private int currentIndex;  // Index de l'objet actuellement affiché
    // private Vector2 startTouchPosition;  // Position de départ du toucher pour le swipe
    // private Vector2 endTouchPosition;    // Position de fin du toucher pour le swipe
    // private bool isSwiping = false;
    // private bool rotatingObject = false; // Flag pour éviter les interférences avec la rotation

    // Distance minimale pour reconnaître un swipe
    public float swipeThreshold = 50f;

    // Référence au script GenerateObject
    private GenerateObject generateObjectScript;

    void Awake()
    {
        // Obtenir la référence au script GenerateObject
        generateObjectScript = FindObjectOfType<GenerateObject>();

        currentIndex = GenerateObject.index;
    }

    void Start()
    {
        if (generateObjectScript == null)
        {
            Debug.LogError("Le script GenerateObject n'a pas été trouvé dans la scène.");
            return;
        }

        // Sauvegarder les positions et rotations initiales de chaque objet
        foreach (var obj in generateObjectScript.objects)
        {
            obj.initialPosition = obj.object3D.position;
            obj.initialRotation = obj.object3D.rotation;
        }

        // Afficher le premier élément au démarrage
        if (generateObjectScript.objects.Length > 0)
        {
            ActivateObject(currentIndex);
        }
    }

    void Update()
    {
        // if (Input.touchCount == 1)
        // {
        //     Touch touch = Input.GetTouch(0);

        //     // Gérer le swipe gauche/droite uniquement si on ne tourne pas l'objet
        //     if (!rotatingObject)
        //     {
        //         if (touch.phase == TouchPhase.Began)
        //         {
        //             startTouchPosition = touch.position;
        //             isSwiping = true;
        //         }
        //         else if (touch.phase == TouchPhase.Moved && isSwiping)
        //         {
        //             endTouchPosition = touch.position;
        //             DetectSwipe();
        //         }
        //         else if (touch.phase == TouchPhase.Ended)
        //         {
        //             isSwiping = false;
        //         }
        //     }
        // }
    }

    // Fonction pour détecter les mouvements de swipe et changer d'objet
    // void DetectSwipe()
    // {
    //     float deltaX = endTouchPosition.x - startTouchPosition.x;

    //     // Si la distance du swipe dépasse le seuil défini
    //     if (Mathf.Abs(deltaX) > swipeThreshold)
    //     {
    //         if (deltaX > 0)
    //         {
    //             // Swipe à droite : passer à l'objet précédent
    //             PreviousObject();
    //         }
    //         else
    //         {
    //             // Swipe à gauche : passer à l'objet suivant
    //             NextObject();
    //         }

    //         // Réinitialiser la détection de swipe après un changement d'objet
    //         isSwiping = false;
    //     }
    // }

    // Fonction pour afficher l'objet suivant
    public void NextObject()
    {
        currentIndex = (currentIndex + 1) % generateObjectScript.objects.Length; // Passer à l'objet suivant
        ActivateObject(currentIndex);
    }

    // Fonction pour afficher l'objet précédent
    public void PreviousObject()
    {
        currentIndex = (currentIndex - 1 + generateObjectScript.objects.Length) % generateObjectScript.objects.Length; // Revenir à l'objet précédent
        ActivateObject(currentIndex);
    }

    // Activer l'objet sélectionné et désactiver les autres
    void ActivateObject(int index)
    {
        // Désactiver tous les objets
        for (int i = 0; i < generateObjectScript.objects.Length; i++)
        {
            generateObjectScript.objects[i].object3D.gameObject.SetActive(false);
        }

        // Activer l'objet actuel
        generateObjectScript.objects[index].object3D.gameObject.SetActive(true);

        // Mettre à jour la description de l'objet
        descriptionText.text = generateObjectScript.objects[index].description;

        // Réinitialiser la position et la rotation de l'objet à leur état initial
        generateObjectScript.objects[index].object3D.position = generateObjectScript.objects[index].initialPosition;
        generateObjectScript.objects[index].object3D.rotation = generateObjectScript.objects[index].initialRotation;

        // Désactiver la rotation temporairement
        generateObjectScript.objects[index].object3D.GetComponent<TouchRotateAndSmoothZoomObject>().DisableRotation();

        // Réactiver la rotation après un court délai
        StartCoroutine(ReenableRotationAfterDelay(generateObjectScript.objects[index].object3D.GetComponent<TouchRotateAndSmoothZoomObject>()));
    }

    private IEnumerator ReenableRotationAfterDelay(TouchRotateAndSmoothZoomObject rotateScript)
    {
        // Attendre un court instant pour permettre au nouvel objet de se stabiliser
        yield return new WaitForSeconds(0.1f);
        rotateScript.EnableRotation(); // Réactiver la rotation
    }

    // Fonction pour indiquer si un objet est en cours de rotation (appelée depuis le script de rotation)
    // public void SetRotating(bool isRotating)
    // {
    //     rotatingObject = isRotating; // Empêche le swipe quand l'objet est en rotation
    // }
}
