using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenManager : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();  // Liste des objets à afficher
    public float fadeTime = 1.0f;  // Durée du fondu

    public List<GameObject> screens = new List<GameObject>();  // Liste des objets/pages à afficher
    public Button nextButton1;  // Référence au bouton "Suivant"
    public Button nextButton2;  // Référence au bouton "Suivant"

    private int currentPageIndex = 0;  // Index de la page actuelle

    void Start()
    {
        // Démarre la séquence d'affichage des objets
        StartCoroutine(DisplayObjectsInSequence());
        nextButton1.onClick.AddListener(ShowNextPage);
        nextButton2.onClick.AddListener(ShowNextPage);
    }

    // Affiche la page en fonction de l'index
    void ShowPage(int index)
    {
        // Désactive toutes les pages
        foreach (GameObject page in screens)
        {
            page.SetActive(false);
        }

        // Active la page correspondante
        if (index >= 0 && index < screens.Count)
        {
            screens[index].SetActive(true);
        }
    }

    // Affiche la page suivante
    void ShowNextPage()
    {
        // Incrémente l'index de la page actuelle
        if (currentPageIndex < screens.Count - 1) // Vérifie si on n'est pas déjà à la dernière page
        {
            currentPageIndex++;
            ShowPage(currentPageIndex); // Affiche la nouvelle page
        }
    }

    IEnumerator DisplayObjectsInSequence()
    {
        // Parcourt tous les objets de la liste
        for (int i = 0; i < objects.Count; i++)
        {
            if (i > 0)
            {
                yield return StartCoroutine(FadeOut(objects[i - 1]));
            }

            // Active l'objet courant avec une animation de fondu en entrée
            yield return StartCoroutine(FadeIn(objects[i]));
            yield return new WaitForSeconds(2);

            // Désactive le dernier objet après l'affichage
            if (i < objects.Count - 1)
            {
                yield return StartCoroutine(FadeOut(objects[i]));
            }
        }

        // Affiche un message dans la console après avoir affiché tous les objets
        screens[0].SetActive(true);
    }

    // Fonction pour faire disparaître un objet en fondu
    IEnumerator FadeOut(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;

        for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 0;
        obj.SetActive(false);
    }

    // Fonction pour faire apparaître un objet en fondu
    IEnumerator FadeIn(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        obj.SetActive(true);
        float startAlpha = canvasGroup.alpha;

        for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    public void GoHomeScreen()
    {
        SceneManager.LoadScene("HomeScreen");
    }
}
