using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IMF
{
    public class Dice : MonoBehaviour
    {

        private const float ANIME_CHANGE_TIME = 0.04f;

        [SerializeField]
        private Sprite[] _diceSpriteArray = null;

        [SerializeField]
        private Image _diceImage = null;

        private int _diceMax = 6;
        private bool _isDiceAnimation = false;
        private float _diceAnimetime = 0.0f;

        private int _diceIndex = 0;
        public int diceIndex { get { return _diceIndex; } }


        public void StartAnimation()
        {
            SetDiceActive(true);
            _diceIndex = UnityEngine.Random.Range(0, 5);
            _isDiceAnimation = true;
            SetImage(_diceIndex);
        }

        void Update()
        {
            if (!_isDiceAnimation)
            {
                return;
            }
            _diceAnimetime += Time.deltaTime;
            if (_diceAnimetime >= ANIME_CHANGE_TIME)
            {
                _diceAnimetime = 0.0f;
                _diceIndex++;
                if (_diceIndex >= _diceMax)
                {
                    _diceIndex = 0;
                }
                SetImage(_diceIndex);
            }
        }

        public void DecideDice()
        {
            _isDiceAnimation = false;
        }

        public void SetImage(int index)
        {
            if (index < 0)
            {
                SetDiceActive(false);
            }
            else
            {
                _diceImage.sprite = _diceSpriteArray[index];
            }
        }

        public void SetDiceActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }

    }
}
