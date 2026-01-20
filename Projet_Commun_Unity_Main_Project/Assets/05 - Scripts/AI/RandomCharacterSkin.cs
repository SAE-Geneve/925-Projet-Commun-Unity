using System.Collections.Generic;
using UnityEngine;

public class RandomCharacterSkin : MonoBehaviour
{
    [System.Serializable]
    public struct SkinOption
    {
        public Mesh mesh;
        public Material material;
    }

    [Header("Réglages")]
    public SkinnedMeshRenderer targetRenderer;

    [Header("Liste des Apparences Possibles")]
    public List<SkinOption> skins = new List<SkinOption>();

    public int randomIndex;

    void Awake()
    {
        RandomizeAppearance();
    }
    [ContextMenu("Randomize Now")]
    public void RandomizeAppearance()
    {
        if (skins == null || skins.Count == 0)
        {
            Debug.LogWarning("La liste des skins est vide !");
            return;
        }

        if (targetRenderer == null)
        {
            Debug.LogError("Il manque le SkinnedMeshRenderer !");
            return;
        }
        
        randomIndex = Random.Range(0, skins.Count);
        SkinOption selectedSkin = skins[randomIndex];
        
        targetRenderer.sharedMesh = selectedSkin.mesh;
        targetRenderer.material = selectedSkin.material;
    }
}