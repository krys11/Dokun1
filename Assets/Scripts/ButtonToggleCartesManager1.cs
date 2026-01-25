using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggleCartesManager1 : MonoBehaviour
{
    public GameObject ButtonMateriauxActive;
    public GameObject MateriauxContentActive;
    public GameObject TextMateriauxInactive;
    public GameObject ButtonArtisanInactive;
    public GameObject ArtisanContentActive;
    public GameObject TextArtisanInactive;
    public GameObject ButtonActionInactive;
    public GameObject ActionContentActive;
    public GameObject TextActionInactive;
    public GameObject ButtonEuvresInactive;
    public GameObject EuvresContentActive;
    public GameObject TextEuvresInactive;


    void Start()
    {

        // Initialisation: Active "quide actif" et désactive les autres
        SetActiveGroup(ButtonMateriauxActive, TextMateriauxInactive, TextArtisanInactive, TextActionInactive, TextEuvresInactive);
        MateriauxContentActive.SetActive(true);
        ArtisanContentActive.SetActive(false);
        ActionContentActive.SetActive(false);
        EuvresContentActive.SetActive(false);
    }

    // Fonction pour activer un groupe et désactiver les autres
    public void OnMateriauxInactifClicked()
    {
        SetActiveGroup(ButtonMateriauxActive, TextMateriauxInactive, TextArtisanInactive, TextActionInactive, TextEuvresInactive);
        MateriauxContentActive.SetActive(true);
        ArtisanContentActive.SetActive(false);
        ActionContentActive.SetActive(false);
        EuvresContentActive.SetActive(false);
    }

    // Fonction pour activer le groupe "Artisan" et désactiver les autres
    public void OnArtisanInactifClicked()
    {
        SetActiveGroup(ButtonArtisanInactive, TextArtisanInactive, TextMateriauxInactive, TextActionInactive, TextEuvresInactive);
        ArtisanContentActive.SetActive(true);
        MateriauxContentActive.SetActive(false);
        ActionContentActive.SetActive(false);
        EuvresContentActive.SetActive(false);
    }

    // Fonction pour activer le groupe "Action" et désactiver les autres
    public void OnActionInactifClicked()
    {
        SetActiveGroup(ButtonActionInactive, TextActionInactive, TextMateriauxInactive, TextArtisanInactive, TextEuvresInactive);
        ActionContentActive.SetActive(true);
        MateriauxContentActive.SetActive(false);
        ArtisanContentActive.SetActive(false);
        EuvresContentActive.SetActive(false);
    }

    // Fonction pour activer le groupe "Euvres" et désactiver les autres
    public void OnEuvresInactifClicked()
    {
        SetActiveGroup(ButtonEuvresInactive, TextEuvresInactive, TextMateriauxInactive, TextArtisanInactive, TextActionInactive);
        EuvresContentActive.SetActive(true);
        MateriauxContentActive.SetActive(false);
        ArtisanContentActive.SetActive(false);
        ActionContentActive.SetActive(false);
    }

    // Active le groupe spécifié et désactive les autres
    void SetActiveGroup(GameObject activeObject, GameObject inactiveToActivate, GameObject otherInactive1, GameObject otherInactive2, GameObject otherInactive3)
    {
        // Activer l'objet actif
        activeObject.SetActive(true);

        // Désactiver les autres actifs (ceux qui doivent être remplacés)
        ButtonMateriauxActive.SetActive(activeObject == ButtonMateriauxActive);
        ButtonArtisanInactive.SetActive(activeObject == ButtonArtisanInactive);
        ButtonActionInactive.SetActive(activeObject == ButtonActionInactive);
        ButtonEuvresInactive.SetActive(activeObject == ButtonEuvresInactive);

        // Activer les objets inactifs correspondants
        inactiveToActivate.SetActive(false);  // Désactiver l'inactif lié à l'actif actuel
        otherInactive1.SetActive(true);  // Activer les inactifs des autres groupes
        otherInactive2.SetActive(true);
        otherInactive3.SetActive(true);
    }
}

