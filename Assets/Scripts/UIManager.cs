using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Référence au script GenerateObject (ou directement à la liste des objets si elle est dans ce script)
    private GenerateObject generateObjectScript;

    public GameObject UIHome;
    public GameObject UIMuseeNumerique;
    public GameObject UIRegleDuJeu;
    public GameObject UIAbout;
    public GameObject UIMuseeVirtuelle;
    public GameObject UIDecouvrir;
    public GameObject UIPanel;


    // Buttons
    public Button btnUIMuseeNumerique;
    public Button btnUIAbout;
    public Button btnUIMuseeVirtuelle;
    public Button btnUIRegleDuJeu;
    public Button btnUIInfo;

    //Buttons Quit
    public Button btnQuitUIMuseeNumerique;
    public Button btnQuitUIAbout;
    public Button btnQuitUIMuseeVirtuelle;
    public Button btnQuitUIRegleDuJeu;
    public Button btnQuitUIPanel;
    public Button btnQuitUIDecouvrir;

    // Start is called before the first frame update

    private void OpenMainMenu()
    {
        UIHome.SetActive(true);
        UIMuseeNumerique.SetActive(false);
        UIRegleDuJeu.SetActive(false);
        UIAbout.SetActive(false);
        UIMuseeVirtuelle.SetActive(false);
        UIDecouvrir.SetActive(false);
        UIPanel.SetActive(false);
    }

    private void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    private void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void OpenUIMuseeNumerique()
    {
        CloseMenu(UIHome);
        OpenMenu(UIMuseeNumerique);
    }

    public void OpenUIRegleDuJeu()
    {
        CloseMenu(UIHome);
        OpenMenu(UIRegleDuJeu);
    }

    public void OpenUIAbout()
    {
        CloseMenu(UIHome);
        OpenMenu(UIAbout);
    }

    public void OpenUIMuseeVirtuelle()
    {
        CloseMenu(UIHome);
        OpenMenu(UIMuseeVirtuelle);
    }

    public void OpenUIPanel()
    {
        OpenMenu(UIPanel);
    }


    //BTN Return
    public void QuitUIMuseeNumerique()
    {
        CloseMenu(UIMuseeNumerique);
        OpenMenu(UIHome);
    }

    public void QuitUIRegleDuJeu()
    {
        CloseMenu(UIRegleDuJeu);
        OpenMenu(UIHome);
    }

    public void QuitUIAbout()
    {
        CloseMenu(UIAbout);
        OpenMenu(UIHome);
    }

    public void QuitUIMuseeVirtuelle()
    {
        CloseMenu(UIMuseeVirtuelle);
        OpenMenu(UIHome);
    }

    public void QuitUIDecouvrir()
    {
        DeactivateAndResetAllObject3D();
        CloseMenu(UIDecouvrir);
        OpenMenu(UIMuseeNumerique);
    }

    public void QuitUIPanel()
    {
        CloseMenu(UIPanel);
    }

    // Méthode pour désactiver tous les objets 3D et réinitialiser leur position et rotation
    void DeactivateAndResetAllObject3D()
    {
        foreach (var obj in generateObjectScript.objects)
        {
            // Réinitialiser la position et la rotation à leur valeur initiale
            obj.object3D.position = obj.initialPosition;
            obj.object3D.rotation = obj.initialRotation;

            // Désactiver l'objet
            obj.object3D.gameObject.SetActive(false);
        }

        Debug.Log("Tous les objets 3D ont été désactivés et réinitialisés.");
    }

    void Start()
    {
        // Obtenir la référence au script GenerateObject
        generateObjectScript = FindObjectOfType<GenerateObject>();

        OpenMainMenu();

        // Ajouter les listeners pour les boutons open
        btnUIAbout.onClick.AddListener(() => OpenUIAbout());
        btnUIMuseeNumerique.onClick.AddListener(() => OpenUIMuseeNumerique());
        btnUIMuseeVirtuelle.onClick.AddListener(() => OpenUIMuseeVirtuelle());
        btnUIRegleDuJeu.onClick.AddListener(() => OpenUIRegleDuJeu());
        btnUIInfo.onClick.AddListener(() => OpenUIPanel());

        // Ajouter les listeners pour les boutons quit
        btnQuitUIAbout.onClick.AddListener(() => QuitUIAbout());
        btnQuitUIMuseeNumerique.onClick.AddListener(() => QuitUIMuseeNumerique());
        btnQuitUIMuseeVirtuelle.onClick.AddListener(() => QuitUIMuseeVirtuelle());
        btnQuitUIRegleDuJeu.onClick.AddListener(() => QuitUIRegleDuJeu());
        btnQuitUIPanel.onClick.AddListener(() => QuitUIPanel());
        btnQuitUIDecouvrir.onClick.AddListener(() => QuitUIDecouvrir());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
