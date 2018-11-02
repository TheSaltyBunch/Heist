using System.Collections;
using UnityEngine;

namespace Character
{
    public class Character : MonoBehaviour
    {
        private bool _damageCooldown;
        private bool _firstDamage;
        private int _stacks;
        private bool _stun;
        private bool _stunCooldown;

        private float _timeSinceDamage;

        public int Stacks
        {
            get { return _stacks; }
            set
            {
                if (value > _stacks && !(_stun || _stunCooldown || _damageCooldown))
                {
                    _firstDamage = true;
                    _timeSinceDamage = 0;
                    _stacks = value;
                    StartCoroutine(DamageCooldown());
                    if (_stacks >= 4) StartCoroutine(Stun());
                }
                else if (value < _stacks)
                {
                    _stacks = value;
                    if (_stacks < 0)
                        _stacks = 0;
                }
            }
        }

        private void Update()
        {
            //Stack decreasing
            _timeSinceDamage += Time.deltaTime;
            if (_firstDamage && _stacks > 0)
            {
                //wait for 20 sec
                if (_timeSinceDamage >= 20)
                {
                    Stacks -= 1;
                    _timeSinceDamage = 0;
                    _firstDamage = false;
                }
            }
            else if (_stacks > 0)
            {
                //wait for 5 secs
                if (_timeSinceDamage >= 5)
                {
                    Stacks -= 1;
                    _timeSinceDamage = 0;
                }
            }
        }


        private IEnumerator DamageCooldown()
        {
            _damageCooldown = true;
            yield return new WaitForSeconds(2);
            _damageCooldown = false;
        }

        private IEnumerator Stun()
        {
            _stun = true;
            yield return new WaitForSeconds(5);
            _stun = false;
            _stunCooldown = true;
            yield return new WaitForSeconds(3);
            _stunCooldown = false;
        }
    }
}