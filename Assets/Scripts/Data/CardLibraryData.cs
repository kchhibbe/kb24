using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.U2D;
using UnityEditor;

[CreateAssetMenu(menuName = "DeckBuildingChallenge/NewCardData", fileName = "NewCardData.asset")]
public class CardLibraryData : ScriptableObject
{
    [Tooltip("Name of the folder under Resources that contain all the card sprites.")]
    public readonly string path = "CardArt";
    public int numberOfCardsInLibrary = 150;
    public SpriteAtlas atlas;
    [HideInInspector]
    public Sprite[] cardSprites;
    [HideInInspector]
    [Tooltip("A list of random integers which is the index of the image in the card sprites list.")]
    public List<int> cardLibray;

    private void Awake()
    {
        atlas = Resources.Load(path, typeof(SpriteAtlas)) as SpriteAtlas;
        Debug.Log(atlas.name);
        cardSprites = new Sprite[atlas.spriteCount];
        atlas.GetSprites(cardSprites);
        // cardSprites = Resources.LoadAll(path, typeof(Sprite)).Cast<Sprite>().ToList();
        cardLibray = new List<int>();
    }

    public void Randomize()
    {
        for (int i = 0; i < numberOfCardsInLibrary; i++)
        {
            cardLibray.Add(Random.Range(0, cardSprites.Length));
        }
    }
}
