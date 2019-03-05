using System.Collections;
using Character;
using Game;
using UnityEngine;

namespace Hazard
{
    public class LethalLaser : Hazard
    {
        [SerializeField] [Range(0.5f, 10)] private float _cooldown;


        public BoxCollider Collider;
        public GameObject Laser1;
        public GameObject Laser2;

        private void Awake()
        {
            Collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) StartCoroutine(Trigger(other.GetComponentInParent<PlayerControl>()));
        }

        private new IEnumerator Trigger(PlayerControl player)
        {
            if (PlacedByPlayer) Destroy(this.gameObject, 5);
            float elapsed = 0;
            do
            {
                player.BaseCharacter.Stacks += 1;
                yield return null;
                elapsed += Time.deltaTime;
            } while (elapsed < _cooldown);
        }

        public override bool Place(Vector3 position)
        {
            LayerMask layers = ~LayerMask.GetMask("Hazard", "Environment", "VFX");

            Vector3 fwd = Vector3.positiveInfinity,
                rt = Vector3.positiveInfinity,
                lt = Vector3.positiveInfinity,
                bk = Vector3.positiveInfinity;

            RaycastHit hit;
            if (Physics.Raycast(position, Vector3.forward, out hit, 20, layers)) fwd = hit.point;

            if (Physics.Raycast(position, Vector3.right, out hit, 20, layers)) rt = hit.point;

            if (Physics.Raycast(position, Vector3.left, out hit, 20, layers)) lt = hit.point;

            if (Physics.Raycast(position, Vector3.back, out hit, 20, layers)) bk = hit.point;

            if (Vector3.Distance(fwd, bk) > _maxGap && Vector3.Distance(rt, lt) > _maxGap) return false;
            
            if (Vector3.Distance(fwd, bk) > Vector3.Distance(rt, lt))
            {
                transform.position = (rt + lt) / 2;

                transform.LookAt(rt);

                Laser1.transform.position = rt + Vector3.up;
                Laser2.transform.position = lt + Vector3.up;


                Laser1.transform.LookAt(Laser2.transform);
                Laser2.transform.LookAt(Laser1.transform);


                Collider.size = new Vector3
                {
                    x = Laser1.transform.localPosition.x * 2 + 0.1f,
                    z = Laser1.transform.localPosition.z * 2, y = 1.5f
                };
            }
            else
            {
                //vertical is less than horizontal
                transform.position = (fwd + bk) / 2;

                transform.LookAt(fwd);

                Laser1.transform.position = fwd + Vector3.up;
                Laser2.transform.position = bk + Vector3.up;


                Laser1.transform.LookAt(Laser2.transform);
                Laser2.transform.LookAt(Laser1.transform);


                Collider.size = new Vector3
                {
                    x = Laser1.transform.localPosition.x * 2 + 0.1f,
                    z = Laser1.transform.localPosition.z * 2, y = 1.5f
                };
            }

            return true;
        }

        public void SetFloor(LayerMask layerMask)
        {
            gameObject.layer = layerMask;
            GameManager.SetLayerOnAll(Laser1, layerMask);
            Laser2.layer = layerMask;
            Laser2.transform.GetChild(0).gameObject.layer = layerMask;
        }
    }
}