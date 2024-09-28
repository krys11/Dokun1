using UnityEngine;

public class TouchRotateAndSmoothZoomObject : MonoBehaviour
{
    public float rotationSpeed = 0.2f;  // Vitesse de rotation
    public float zoomSpeed = 0.05f;     // Vitesse de zoom
    public float minZoom = 0.5f;        // Zoom minimal (proximité maximale)
    public float maxZoom = 5f;          // Zoom maximal (distance maximale)
    public float smoothTime = 0.2f;     // Temps pour que l'animation de zoom soit lissée

    private Vector3 initialScale;        // Échelle initiale de l'objet pour zoom
    private Vector3 targetScale;         // Échelle cible pour un zoom lissé
    private Vector3 velocity = Vector3.zero; // Vitesse utilisée par SmoothDamp

    public ObjectViewerWithDescriptionsMobile viewerScript;

    private bool isRotating = false; // Flag pour déterminer si l'objet est en rotation

    void Start()
    {
        // Sauvegarder l'échelle initiale de l'objet
        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    void Update()
    {
        // Gestion de la rotation avec un seul doigt
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                // Si on déplace le doigt, on active la rotation
                // viewerScript.SetRotating(true);
                isRotating = true;

                // Calculer la différence de position par rapport à la dernière frame
                Vector2 deltaTouch = touch.deltaPosition;

                // Appliquer une rotation horizontale et verticale en fonction du mouvement du doigt
                float horizontalRotation = deltaTouch.x * rotationSpeed;
                float verticalRotation = deltaTouch.y * rotationSpeed;

                // Appliquer la rotation à l'objet
                transform.Rotate(Vector3.up, -horizontalRotation, Space.World); // Rotation horizontale (axe Y global)
                transform.Rotate(Vector3.right, verticalRotation, Space.Self);  // Rotation verticale (axe X local)
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Si le toucher se termine, désactiver la rotation
                // viewerScript.SetRotating(false);
                isRotating = false;
            }
        }
        // Gestion du zoom avec deux doigts
        else if (Input.touchCount == 2)
        {
            // viewerScript.SetRotating(false); // Lorsque la rotation se termine
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Calculer la position précédente des touches pour chaque doigt
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Calculer la distance entre les deux touches cette frame et la précédente
            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            // Calculer la différence de distance entre les deux frames
            float difference = currentMagnitude - prevMagnitude;

            // Appliquer le zoom en modifiant l'échelle de l'objet
            ZoomObject(difference * zoomSpeed);
        }
        else
        {
            if (isRotating)
            {
                // viewerScript.SetRotating(false); // Lorsque la rotation se termine
                isRotating = false;
            }
        }

        // Lissage du zoom vers la nouvelle échelle
        transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref velocity, smoothTime);
    }

    // Fonction pour zoomer ou dézoomer l'objet de manière fluide
    void ZoomObject(float increment)
    {
        // Calculer la nouvelle échelle cible
        targetScale = transform.localScale + Vector3.one * increment;

        // Limiter l'échelle cible entre minZoom et maxZoom
        targetScale = Vector3.ClampMagnitude(targetScale, maxZoom);
        targetScale = Vector3.Max(targetScale, initialScale * minZoom);
    }

    // Méthode pour désactiver la rotation de l'objet
    public void DisableRotation()
    {
        isRotating = false; // Désactive la rotation
        // viewerScript.SetRotating(false); // Met à jour l'état de rotation dans le script principal
    }

    // Méthode pour activer la rotation de l'objet
    public void EnableRotation()
    {
        isRotating = true; // Réactive la rotation
    }
}
