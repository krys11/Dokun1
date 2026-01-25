using UnityEngine;

public class RotateObject : MonoBehaviour
{
    void Start()
    {
        // Déclenche la rotation continue dès le début
        RotateY();
    }

    void RotateY()
    {
        // Applique une rotation en continu sur l'axe Y
        iTween.RotateBy(gameObject, iTween.Hash(
            "y", 1,                // Rotation complète sur l'axe Y (1 tour)
            "time", 15f,            // Durée de la rotation
            "easetype", iTween.EaseType.linear, // Mouvement linéaire pour un effet de rotation fluide
            "looptype", iTween.LoopType.loop    // Boucle infinie
        ));
    }
}
