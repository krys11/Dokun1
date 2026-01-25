using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    // Références aux objets actifs et inactifs pour chaque groupe
    public GameObject quideActif;
    public GameObject quideInactif;
    public GameObject museeActif;
    public GameObject museeInactif;
    public GameObject quizActif;
    public GameObject quizInactif;
    public GameObject privacyPolicy;

    void Start()
    {
        // Initialisation: Active "quide actif" et désactive les autres
        SetActiveGroup(quideActif, quideInactif, museeInactif, quizInactif);
    }

    // Fonction pour activer un groupe et désactiver les autres
    public void OnQuideInactifClicked()
    {
        SetActiveGroup(quideActif, quideInactif, museeInactif, quizInactif);
    }

    public void OnMuseeInactifClicked()
    {
        SetActiveGroup(museeActif, museeInactif, quideInactif, quizInactif);
    }

    public void OnQuizInactifClicked()
    {
        SetActiveGroup(quizActif, quizInactif, quideInactif, museeInactif);
    }

    // Active le groupe spécifié et désactive les autres
    void SetActiveGroup(GameObject activeObject, GameObject inactiveToActivate, GameObject otherInactive1, GameObject otherInactive2)
    {
        // Activer l'objet actif
        activeObject.SetActive(true);

        // Désactiver les autres actifs (ceux qui doivent être remplacés)
        quideActif.SetActive(activeObject == quideActif);
        museeActif.SetActive(activeObject == museeActif);
        quizActif.SetActive(activeObject == quizActif);

        // Activer les objets inactifs correspondants
        inactiveToActivate.SetActive(false);  // Désactiver l'inactif lié à l'actif actuel
        otherInactive1.SetActive(true);  // Activer les inactifs des autres groupes
        otherInactive2.SetActive(true);
    }

    // Go to Screen
    public void GoToGuideScreen()
    {
        SceneManager.LoadScene("GuideScreen");
    }

    public void GoToMuseVirtuelScreen()
    {
        SceneManager.LoadScene("MuseVirtuelScreen");
    }

    public void GoToARScreen()
    {
        SceneManager.LoadScene("ARScreen");
    }

    public void ActivePrivacyPolicy()
    {
        privacyPolicy.SetActive(true);
    }

    public void DestactivePrivacyPolicy()
    {
        privacyPolicy.SetActive(false);
    }

    public void YesQuitGame()
    {
#if UNITY_EDITOR
        // Quitter l'éditeur
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
