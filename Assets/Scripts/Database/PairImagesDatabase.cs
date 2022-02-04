using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PairImagesDatabase", menuName = "Database/PairImagesDatabase", order = 1)]
public class PairImagesDatabase : ScriptableObject
{
    [SerializeField] private List<PairImagesList> pairImagesTable;
    
    public Sprite[] GetImagesOfType(PairImagesType type)
    {
        PairImagesList pairImagesList = pairImagesTable.Find(imagesList => imagesList.ImagesType == type);

        if (pairImagesList == null)
        {
            Debug.LogError("No images of type {type} found in database.");
            return new Sprite[0];
        }

        Sprite[] images = Resources.LoadAll<Sprite>(pairImagesList.ImagesFolderPath);

        return images;
    }

    public enum PairImagesType
    {
        Dogs
    }

    [Serializable]
    public class PairImagesList
    {
        [SerializeField] private PairImagesType imagesType;
        [SerializeField] private string imagesFolderPath;

        public PairImagesType ImagesType => imagesType;
        public string ImagesFolderPath => imagesFolderPath;
    }
}
