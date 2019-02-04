using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Game
{
    public enum Characters
    {
        King,
        Jailbird,
        Shadow,
        Raccoon,
        Drone
    }

    public class GameManager : MonoBehaviour
    {
        public static readonly Dictionary<Characters, Stats> CharacterStats = new Dictionary<Characters, Stats>
        {
            {
                Characters.King, new Stats
                {
                    Health = 5,
                    Speed = 2,
                    Dexterity = 2,
                }
            },
            {
                Characters.Jailbird, new Stats
                {
                    Health = 4,
                    Speed = 5,
                    Dexterity = 3,
                }
            },
            {
                Characters.Shadow, new Stats
                {
                    Health = 3,
                    Speed = 4,
                    Dexterity = 5,
                }
            },
            {
                Characters.Raccoon, new Stats
                {
                    Health = 4,
                    Speed = 4,
                    Dexterity = 4,
                }
            },
            {
                Characters.Drone, new Stats
                {
                    Health = 3,
                    Speed = 3,
                    Dexterity = -1,
                }
            },
        };

        public static GameManager GameManagerRef;


        public static bool UseMultiScreen = true;

        //store the players character choice here
        public static Characters[] PlayerChoice =
            {Characters.Raccoon, Characters.Jailbird, Characters.Shadow, Characters.King};

        private void Awake()
        {
            if (GameManagerRef == null || GameManagerRef == this) GameManagerRef = this;
            else Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);
        }

        public static int GetPlayerMask(int playerNumber, bool bitShift) =>
            bitShift ? 1 << (28 + playerNumber) : (28 + playerNumber);
    }
}