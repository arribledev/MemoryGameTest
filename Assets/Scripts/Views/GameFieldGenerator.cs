using MemoryGame.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MemoryGame.Views
{
    public class GameFieldGenerator : MonoBehaviour
    {
        [SerializeField] private PairImagesDatabase pairImagesDatabase;
        [SerializeField] private CardView cardViewPrefab;
        [SerializeField] private RectTransform gameField;
        [SerializeField] private float spacing;

        private System.Random rand;

        private void Awake()
        {
            rand = new System.Random();
        }

        public CardView[] GenerateGameField(int pairsCount)
        {
            CardView[] cards;

            cards = new CardView[pairsCount * 2];

            Sprite[] cardImagesArray = pairImagesDatabase.GetImagesOfType(PairImagesDatabase.PairImagesType.Dogs);
            List<int> cardImageIndexes = Enumerable.Range(0, cardImagesArray.Length - 1).ToList().RandomElements(rand, pairsCount);
            cardImageIndexes.AddRange(cardImageIndexes);
            cardImageIndexes = cardImageIndexes.RandomOrder(rand);

            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = Instantiate(cardViewPrefab, gameField);
                int cardId = cardImageIndexes[i];
                cards[i].Initialize(cardImagesArray[cardId], cardId);
            }

            SetupCardsGrid(cards);

            return cards;
        }

        //For nearly-square field
        private void SetupCardsGrid(CardView[] cards)
        {
            if (cards.Length > 42 && spacing > 10)
            {
                spacing = 10;
            }

            if (gameField.rect.width > gameField.rect.height)
            {
                gameField.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gameField.rect.height);
            }

            int cardsCountInRow = (int)Math.Ceiling(Math.Sqrt(cards.Length));
            int cardsCountInStraightColumn = cards.Length / cardsCountInRow;
            int cardsLeftCount = cards.Length % cardsCountInRow;
            int cardsCountInColumn = cardsCountInStraightColumn + (cardsLeftCount > 0 ? 1 : 0);
            float cardSize = (gameField.rect.width - spacing) / cardsCountInRow - spacing;

            Vector2 startPosition = new Vector2(
                -(cardsCountInRow / 2f - 0.5f) * (cardSize + spacing),
                (cardsCountInColumn / 2f - 0.5f) * (cardSize + spacing));

            Vector3 cardPosition = startPosition;
            Vector2 cardSizeVector = Vector2.one * cardSize;
            int cardIndex = 0;

            for (int i = 0; i < cardsCountInStraightColumn; i++)
            {
                for (int j = 0; j < cardsCountInRow; j++)
                {
                    RectTransform cardRectTransform = cards[cardIndex].GetComponent<RectTransform>();
                    cardRectTransform.sizeDelta = cardSizeVector;
                    cardRectTransform.anchoredPosition = cardPosition;

                    cardPosition.x += cardSize + spacing;
                    cardIndex++;
                }

                cardPosition.y -= cardSize + spacing;
                cardPosition.x = startPosition.x;
            }

            cardPosition.x = -(cardsLeftCount / 2f - 0.5f) * (cardSize + spacing);
            for (int i = 0; i < cardsLeftCount; i++)
            {
                RectTransform cardRectTransform = cards[cardIndex].GetComponent<RectTransform>();
                cardRectTransform.sizeDelta = cardSizeVector;
                cardRectTransform.anchoredPosition = cardPosition;

                cardPosition.x += cardSize + spacing;
                cardIndex++;
            }
        }
    }
}
