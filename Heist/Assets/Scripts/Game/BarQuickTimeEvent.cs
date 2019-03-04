using System;
using System.Collections;
using System.Linq;
using Character;
using Rewired;
using UnityEngine;
using UnityEngine.UI;
using Player = Rewired.Player;
using Random = UnityEngine.Random;

namespace Game
{
    public class BarQuickTimeEvent : QuickTimeEvent
    {
        [SerializeField] private Image _greenArea;

        [SerializeField] private Image _pointer;


        private Input _controlInput;
        private int _dir;
        private float _index;
        private Player _player;

        private float _range;

        public Type QuickTimeType;

        private Input ControlInput
        {
            get => _controlInput;
            set
            {
                if (!Equals(value, _controlInput))
                    if (value.A && !_controlInput.A)
                        PressButton(Button.A);

                _controlInput = value;
            }
        }

        private void PressButton(Button button)
        {
            if (button == Button.A && CheckIndex())
            {
                //success
                Events?.Invoke(this, new QuickTimeEventArgs
                {
                    Result = true, State = (int) (_index * 100), Type = QuickTimeType
                });
                StartCoroutine(FlashColour(Color.green));
            }
            else
            {
                //failure
                Events?.Invoke(this, new QuickTimeEventArgs
                {
                    Result = false, State = (int) (_index * 100), Type = QuickTimeType
                });
                StartCoroutine(FlashColour(Color.red));
            }
        }

        private IEnumerator FlashColour(Color color)
        {
            _pointer.color = color;
            yield return new WaitForSeconds(0.1f);
            _pointer.color = Color.white;
            Generate();
        }

        public void Generate(PlayerControl player)
        {
            _player = player.Player;
            Generate();
        }

        public void Generate()
        {
            if (_player == null) _player = ReInput.players.Players.First();


            _greenArea.fillOrigin = (Random.Range(1, 4) + _greenArea.fillOrigin) % 4;
            _greenArea.fillAmount = _range;
            _greenArea.fillClockwise = false;

            _dir = 1;
        }

        private bool CheckIndex()
        {
            var point = ((int) _greenArea.fillOrigin) - 2; //top == 0, bottom == -2, right == -1, left == 1

            float startAngle = 0;
            switch (point)
            {
                case -2:
                    startAngle = 0.5f;
                    break;
                case -1:
                    startAngle = 0.75f;
                    break;
                case 0:
                    startAngle = 0;
                    break;
                case 1:
                    startAngle = 0.25f;
                    break;
            }

            float origin;

            if (_greenArea.fillClockwise)
            {
                origin = (startAngle - _range / 2) % 1;
            }
            else
            {
                origin = (startAngle + _range / 2) % 1;
            }
            if (Math.Abs(_index - origin) < _range || Math.Abs((1 - _index) - origin) < _range)
            {
                return true;
            }

            return false;
        }

        public void SetDexterity(int dexterity)
        {
            switch (dexterity)
            {
                case 2:
                    _range = 0.05f;
                    break;
                case 3:
                    _range = 0.1f;
                    break;
                case 4:
                    _range = 0.17f;
                    break;
                case 5:
                    _range = 0.25f;
                    break;
            }
        }

        public void Update()
        {
            if (_player != null)
            {
                ControlInput = new Input
                {
                    A = _player.GetButton("QuickTimeA")
                };
            }

            _index += Time.deltaTime * _dir;

            if (_index > 1) _index %= 1;
            _pointer.transform.rotation = Quaternion.Euler(0, 0, _index * 360);
        }

        public event QuickTimeEventHandler Events;

        private struct Input
        {
            public bool A;
        }
    }
}