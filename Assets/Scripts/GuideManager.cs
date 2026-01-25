using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour
{
    public GameObject MenuObject;
    public GameObject GuideObject;
    public GameObject CarteObject;
    public GameObject Tuto1;
    public GameObject Tuto2;
    public GameObject Tuto3;

    public List<GameObject> objects;  // Liste des objets à gérer
    private int currentIndex = 0;  // Index de l'objet actuellement actif

    public GameObject UIHomeViewGuide;
    public GameObject UIGuide;
    public GameObject UICarte;

    void Start()
    {
        MenuObject.SetActive(true);
        GuideObject.SetActive(false);

        // Désactive tous les objets sauf le premier (index 0)
        for (int i = 0; i < objects.Count; i++)
        {
            if (i == currentIndex)
            {
                objects[i].SetActive(true);  // Active l'objet à l'index 0
            }
            else
            {
                objects[i].SetActive(false);  // Désactive les autres
            }
        }
    }

    // Méthode appelée lors du clic sur "Suivant"
    public void OnNextClicked()
    {
        // Désactive l'objet actuel
        objects[currentIndex].SetActive(false);

        // Incrémente l'index pour passer à l'objet suivant
        currentIndex++;
        if (currentIndex >= objects.Count)  // Si l'index dépasse le dernier objet, revenir au début
        {
            currentIndex = 0;
        }

        // Active le nouvel objet
        objects[currentIndex].SetActive(true);
    }

    // Méthode appelée lors du clic sur "Précédent"
    public void OnPreviousClicked()
    {
        // Désactive l'objet actuel
        objects[currentIndex].SetActive(false);

        // Décrémente l'index pour passer à l'objet précédent
        currentIndex--;
        if (currentIndex < 0)  // Si l'index est inférieur à 0, revenir au dernier objet
        {
            currentIndex = objects.Count - 1;
        }

        // Active le nouvel objet
        objects[currentIndex].SetActive(true);
    }

    // Allez sur l'objet Guide
    public void GoGuide()
    {
        MenuObject.SetActive(false);
        GuideObject.SetActive(true);
        CarteObject.SetActive(false);
    }

    public void GoCarte()
    {
        MenuObject.SetActive(false);
        GuideObject.SetActive(false);
        CarteObject.SetActive(true);
    }

    public void GoMenu()
    {
        MenuObject.SetActive(true);
        GuideObject.SetActive(false);
        CarteObject.SetActive(false);
    }

    public void SkipTuto()
    {
        Tuto1.SetActive(false);
        Tuto2.SetActive(false);
        Tuto3.SetActive(false);
        currentIndex = 3;  // Passer à l'objet 4 (index 3) pour commencer la démonstration du guide
        objects[3].SetActive(true);
    }

    public void OnButtonReturnClick()
    {
        // Vérifie si l'UIObjectViewer3D est actif
        if (UIHomeViewGuide.activeSelf)
        {
            UIGuide.SetActive(false);
            UICarte.SetActive(false);
            SceneManager.LoadScene("HomeScreen");
        }
        else if (UICarte.activeSelf)
        {
            UICarte.SetActive(false);
            UIGuide.SetActive(false);
            UIHomeViewGuide.SetActive(true);

        }
        else if (UIGuide.activeSelf)
        {
            UICarte.SetActive(false);
            UIGuide.SetActive(false);
            UIHomeViewGuide.SetActive(true);
        }
    }


}
