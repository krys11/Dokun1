using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggleGuideManager : MonoBehaviour
{
    public GameObject ButtonEtapesActive;
    public GameObject EtapesContentActive;
    public GameObject TextEtapesInactive;
    public GameObject ButtonRegleActive;
    public GameObject RegleContentActive;
    public GameObject TextRegleInactive;
    // public GameObject ButtonQuizActive;
    // public GameObject TextQuizInactive;

    void Start()
    {

        // Initialisation: Active "quide actif" et désactive les autres
        SetActiveGroup(ButtonEtapesActive, TextEtapesInactive, TextRegleInactive); //TextQuizInactive);
        EtapesContentActive.SetActive(true);
        RegleContentActive.SetActive(false);
    }

    // Fonction pour activer un groupe et désactiver les autres
    public void OnEtapesInactifClicked()
    {
        SetActiveGroup(ButtonEtapesActive, TextEtapesInactive, TextRegleInactive); //TextQuizInactive);
        EtapesContentActive.SetActive(true);
        RegleContentActive.SetActive(false);

    }

    public void OnRegleInactifClicked()
    {
        SetActiveGroup(ButtonRegleActive, TextRegleInactive, TextEtapesInactive); //TextQuizInactive);
        RegleContentActive.SetActive(true);
        EtapesContentActive.SetActive(false);
    }

    // public void OnQuizInactifClicked()
    // {
    //     SetActiveGroup(ButtonQuizActive, TextQuizInactive, TextEtapesInactive, TextRegleInactive);
    //     EtapesContentActive.SetActive(false);
    //     RegleContentActive.SetActive(false);
    // }

    // Active le groupe spécifié et désactive les autres
    void SetActiveGroup(GameObject activeObject, GameObject inactiveToActivate, GameObject otherInactive1) //GameObject otherInactive2)
    {
        // Activer l'objet actif
        activeObject.SetActive(true);

        // Désactiver les autres actifs (ceux qui doivent être remplacés)
        ButtonEtapesActive.SetActive(activeObject == ButtonEtapesActive);
        ButtonRegleActive.SetActive(activeObject == ButtonRegleActive);
        // ButtonQuizActive.SetActive(activeObject == ButtonQuizActive);

        // Activer les objets inactifs correspondants
        inactiveToActivate.SetActive(false);  // Désactiver l'inactif lié à l'actif actuel
        otherInactive1.SetActive(true);  // Activer les inactifs des autres groupes
        // otherInactive2.SetActive(true);
    }
}
