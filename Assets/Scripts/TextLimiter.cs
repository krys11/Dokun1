using TMPro;
using UnityEngine;

public class TextLimiter : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Le composant TextMeshPro
    public int maxLines = 3; // Nombre maximal de lignes visibles

    void Start()
    {
        // Définir le nombre maximal de lignes visibles
        textComponent.maxVisibleLines = maxLines;

        // Optionnel : masquer le texte dépassant les limites
        textComponent.overflowMode = TextOverflowModes.Masking;
    }
}
