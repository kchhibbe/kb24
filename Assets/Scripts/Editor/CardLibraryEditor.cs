using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardLibraryData))]
public class CardLibraryEditor : Editor
{
    CardLibraryData cardLibraryData;
    bool displayCardSprites;
    bool displayCardLibrary;
    Vector2 cardSpritePosition;
    Vector2 cardLibraryPosition;

    private void OnEnable()
    {
        cardLibraryData = target as CardLibraryData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (cardLibraryData.cardSprites.Length == 0)
        {
            EditorGUILayout.HelpBox(string.Format("Unable to load card sprites from {0}", cardLibraryData.path), MessageType.Error);
        }
        else
        {
            DisplayCardSprites();
            if (cardLibraryData.cardLibray.Count == 0)
            {
                if (GUILayout.Button("Generate Random Card Library", EditorStyles.miniButton))
                    cardLibraryData.Randomize();
            }
            else
            {
                DisplayCardLibrary();
            }
        }

        EditorUtility.SetDirty(cardLibraryData);
    }

    private void DisplayCardSprites()
    {
        displayCardSprites = EditorGUILayout.Foldout(displayCardSprites, "Card Sprites");
        if (displayCardSprites)
        {
            cardSpritePosition = EditorGUILayout.BeginScrollView(cardSpritePosition);
            for (int i = 0; i < cardLibraryData.cardSprites.Length; i++)
            {
                cardLibraryData.cardSprites[i] = EditorGUILayout.ObjectField(string.Format("Sprite {0}", i), cardLibraryData.cardSprites[i], typeof(Sprite), false) as Sprite;
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void DisplayCardLibrary()
    {
        displayCardLibrary = EditorGUILayout.Foldout(displayCardLibrary, "Card Library");
        if (displayCardLibrary)
        {
            EditorGUILayout.LabelField("The index of the sprite for a card.");
            cardLibraryPosition = EditorGUILayout.BeginScrollView(cardLibraryPosition);
            for (int i = 0; i < cardLibraryData.cardLibray.Count; i++)
            {
                EditorGUILayout.LabelField(string.Format("Card {0}", i), cardLibraryData.cardLibray[i].ToString());
            }
            EditorGUILayout.EndScrollView();
        }
    }

}
