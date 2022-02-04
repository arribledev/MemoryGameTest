using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame.Views
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private float rotationTime;
        [SerializeField] private float showTime;

        private bool isOpen;
        private Sprite frontSprite;
        private float rotationPerSecond;

        public int Id { get; private set; }
        public event Action<CardView> OnOpened;

        public void Initialize(Sprite cardFrontSprite, int cardId)
        {
            isOpen = false;
            frontSprite = cardFrontSprite;
            Id = cardId;
            rotationPerSecond = 180 / rotationTime;
            image.sprite = backSprite;

            button.onClick.AddListener(Open);
        }

        public void Open()
        {
            StartCoroutine(OpenCoroutine());
        }

        public void Close()
        {
            StartCoroutine(CloseCoroutine());
        }

        private IEnumerator OpenCoroutine()
        {
            button.interactable = false;
            yield return StartCoroutine(RotationCoroutine(rotationPerSecond));
            yield return new WaitForSeconds(showTime);
            OnOpened?.Invoke(this);
        }

        private IEnumerator CloseCoroutine()
        {
            yield return StartCoroutine(RotationCoroutine(-rotationPerSecond));
            button.interactable = true;
        }

        private IEnumerator RotationCoroutine(float rotationAngle)
        {
            float time = 0;

            while (time < rotationTime)
            {
                this.transform.Rotate(Vector3.up, rotationAngle * Time.fixedDeltaTime);
                UpdateSprite();

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        private void UpdateSprite()
        {
            if (this.transform.forward.z < 0 != isOpen)
            {
                isOpen = !isOpen;
                image.sprite = isOpen ? frontSprite : backSprite;
            }
        }
    }
}