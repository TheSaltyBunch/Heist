using System;
using Character;
using Game;
using UnityEngine;

namespace Camera
{
    public class CameraLogic : MonoBehaviour
    {
        [SerializeField] [Range(0, 30)] private float _bounds = 3;
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private GameObject _mask;

        [SerializeField] private float _offset = 10;
        [SerializeField] private PlayerControl _playerControl;

        [SerializeField] private bool _seesPlayer;
        [SerializeField] [Range(0, 1)] private float _smoothing = 0.2f;

        [SerializeField] public UnityEngine.Camera MainFloorCamera;
        [SerializeField] public UnityEngine.Camera BasementCamera;
        [SerializeField] public UnityEngine.Camera UICamera;

        private void Start()
        {
            MainFloorCamera.transform.localPosition = Vector3.back * _offset + Vector3.up * _offset;

            MainFloorCamera.cullingMask = MainFloorCamera.cullingMask |
                                          GameManager.GetPlayerMask(_playerControl.PlayerNumber, true);
            BasementCamera.cullingMask = BasementCamera.cullingMask |
                                         GameManager.GetPlayerMask(_playerControl.PlayerNumber, true);
        }

        private void Update()
        {
            RaycastHit hit;

            var _seesPlayer = Physics.Raycast(MainFloorCamera.transform.position,
                (_playerControl.transform.position - MainFloorCamera.transform.position).normalized,
                out hit, 100, _layerMask);
            if (_seesPlayer)
            {
                if (hit.collider.CompareTag("Player"))
                    _mask.GetComponent<MaskActivate>().StopShow();
                else _mask.GetComponent<MaskActivate>().StartShow();
            }

            //Debug.DrawRay(Camera.transform.position, (_playerControl.transform.position - Camera.transform.position).normalized, Color.red);
        }

        private void FixedUpdate()
        {
            var trackedPosition = _playerControl.transform.position +
                                  _playerControl.Control.MoveVector * _bounds +
                                  _playerControl.Control.FaceVector * _bounds / 2;

            transform.position = Vector3.Lerp(transform.position, trackedPosition, _smoothing);
        }

        public void SwitchFloors(Floor floor)
        {
            switch (floor)
            {
                case Floor.MainFloor:
                    MainFloorCamera.enabled = true;
                    BasementCamera.enabled = false;
                    break;
                case Floor.Basement:
                    BasementCamera.enabled = true;
                    MainFloorCamera.enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floor), floor, null);
            }
        }

        public void ShowPlayers(int[] showPlayer, int[] hidePlayer)
        {
            switch (_playerControl.Floor)
            {
                case Floor.MainFloor:
                    foreach (var player in showPlayer)
                    {
                        MainFloorCamera.cullingMask =
                            MainFloorCamera.cullingMask | GameManager.GetPlayerMask(player, true);
                    }

                    foreach (var player in hidePlayer)
                    {
                        BasementCamera.cullingMask =
                            BasementCamera.cullingMask & ~GameManager.GetPlayerMask(player, true);
                    }

                    break;
                case Floor.Basement:
                    foreach (var player in showPlayer)
                    {
                        BasementCamera.cullingMask =
                            BasementCamera.cullingMask | GameManager.GetPlayerMask(player, true);
                    }

                    foreach (var player in hidePlayer)
                    {
                        MainFloorCamera.cullingMask =
                            MainFloorCamera.cullingMask & ~GameManager.GetPlayerMask(player, true);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}