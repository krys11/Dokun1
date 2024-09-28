using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectViewerWithDescriptionsMobile : MonoBehaviour
{
    [System.Serializable]
    public class ObjectInfo
    {
        public Transform object3D;  // Référence à l'objet 3D
        [TextArea] public string description;  // Description de l'objet

        // Stocker la position et la rotation initiales
        [HideInInspector] public Vector3 initialPosition;
        [HideInInspector] public Quaternion initialRotation;
    }

    public ObjectInfo[] objects;  // Liste d'objets 3D avec descriptions
    public TMP_Text descriptionText;  // UI Text pour afficher la description

    private int currentIndex = 0;  // Index de l'objet actuellement affiché
    private Vector2 startTouchPosition;  // Position de départ du toucher pour le swipe
    private Vector2 endTouchPosition;    // Position de fin du toucher pour le swipe
    private bool isSwiping = false;
    private bool rotatingObject = false; // Flag pour éviter les interférences avec la rotation

    // Distance minimale pour reconnaître un swipe
    public float swipeThreshold = 50f;

    void Start()
    {
        // Sauvegarder les positions et rotations initiales de chaque objet
        foreach (var obj in objects)
        {
            obj.initialPosition = obj.object3D.position;
            obj.initialRotation = obj.object3D.rotation;
        }

        // Afficher le premier élément au démarrage
        if (objects.Length > 0)
        {
            ActivateObject(currentIndex);
        }
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Gérer le swipe gauche/droite uniquement si on ne tourne pas l'objet
            if (!rotatingObject)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    startTouchPosition = touch.position;
                    isSwiping = true;
                }
                else if (touch.phase == TouchPhase.Moved && isSwiping)
                {
                    endTouchPosition = touch.position;
                    DetectSwipe();
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isSwiping = false;
                }
            }
        }
    }

    // Fonction pour détecter les mouvements de swipe et changer d'objet
    void DetectSwipe()
    {
        float deltaX = endTouchPosition.x - startTouchPosition.x;

        // Si la distance du swipe dépasse le seuil défini
        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            if (deltaX > 0)
            {
                // Swipe à droite : passer à l'objet précédent
                PreviousObject();
            }
            else
            {
                // Swipe à gauche : passer à l'objet suivant
                NextObject();
            }

            // Réinitialiser la détection de swipe après un changement d'objet
            isSwiping = false;
        }
    }

    // Fonction pour afficher l'objet suivant
    void NextObject()
    {
        currentIndex = (currentIndex + 1) % objects.Length; // Passer à l'objet suivant
        ActivateObject(currentIndex);
    }

    // Fonction pour afficher l'objet précédent
    void PreviousObject()
    {
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length; // Revenir à l'objet précédent
        ActivateObject(currentIndex);
    }

    // Activer l'objet sélectionné et désactiver les autres
    void ActivateObject(int index)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].object3D.gameObject.SetActive(i == index);
        }

        // Mettre à jour la description de l'objet
        descriptionText.text = objects[index].description;

        // Réinitialiser la position et la rotation de l'objet à leur état initial
        objects[index].object3D.position = objects[index].initialPosition;
        objects[index].object3D.rotation = objects[index].initialRotation;

        // Désactiver la rotation temporairement
        objects[index].object3D.GetComponent<TouchRotateAndSmoothZoomObject>().DisableRotation();

        // Réactiver la rotation après un court délai
        StartCoroutine(ReenableRotationAfterDelay(objects[index].object3D.GetComponent<TouchRotateAndSmoothZoomObject>()));
    }

    private IEnumerator ReenableRotationAfterDelay(TouchRotateAndSmoothZoomObject rotateScript)
    {
        // Attendre un court instant pour permettre au nouvel objet de se stabiliser
        yield return new WaitForSeconds(0.1f);
        rotateScript.EnableRotation(); // Réactiver la rotation
    }

    // Fonction pour indiquer si un objet est en cours de rotation (appelée depuis le script de rotation)
    public void SetRotating(bool isRotating)
    {
        rotatingObject = isRotating; // Empêche le swipe quand l'objet est en rotation
    }
}
