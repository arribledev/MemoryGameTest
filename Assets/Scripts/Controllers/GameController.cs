﻿using MemoryGame.Views;
using System;
using System.Collections;
using UnityEngine;

namespace MemoryGame.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameFieldGenerator gameFieldGenerator;
        [SerializeField] private int secondsPerPair;

        private CardView[] cards;
        private CardView openCard;
        private int totalPairsCount;
        private int secondsLeft;
        private int movesCount;
        private int guessedPairsCount;
        private bool gameIsActive = false;
        private Coroutine timerCoroutine;

        public event Action<bool> OnGameFinished;
        public event Action<int> OnMovesCountChanged;
        public event Action<int> OnTimerValueChanged;

        public void StartGame(int difficulty)
        {
            totalPairsCount = difficulty + 1;
            secondsLeft = totalPairsCount * secondsPerPair;
            movesCount = 0;
            guessedPairsCount = 0;
            openCard = null;

            OnTimerValueChanged?.Invoke(secondsLeft);
            OnMovesCountChanged?.Invoke(movesCount);

            cards = gameFieldGenerator.GenerateGameField(totalPairsCount);

            foreach (CardView card in cards)
            {
                card.OnOpened += RegisterOpenCard;
            }

            gameIsActive = true;
            timerCoroutine = StartCoroutine(CountdownCoroutine());
        }

        public void StopGame()
        {
            StopTimer();
            CleanBeforeGameEnd();
        }

        private void RegisterOpenCard(CardView card)
        {
            movesCount++;
            OnMovesCountChanged?.Invoke(movesCount);

            if (openCard == null)
            {
                openCard = card;
                return;
            }

            if (openCard.Id != card.Id)
            {
                openCard.Close();
                card.Close();

                openCard = null;
                return;
            }

            openCard = null;
            guessedPairsCount++;

            if (IsGameWon())
            {
                FinishGame(true);
            }
        }

        private void CleanBeforeGameEnd()
        {
            foreach (CardView card in cards)
            {
                card.OnOpened -= RegisterOpenCard;
                Destroy(card.gameObject);
            }
        }

        private IEnumerator CountdownCoroutine()
        {
            while (secondsLeft > 0)
            {
                yield return new WaitForSeconds(1);

                secondsLeft--;
                OnTimerValueChanged?.Invoke(secondsLeft);
            }

            RegisterTimerStop();
        }

        private void RegisterTimerStop()
        {
            FinishGame(IsGameWon());
        }

        private bool IsGameWon()
        {
            return guessedPairsCount == totalPairsCount;
        }

        private void StopTimer()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
        }

        private void FinishGame(bool win)
        {
            gameIsActive = false;
            StopTimer();
            OnGameFinished?.Invoke(win);
        }
    }
}
