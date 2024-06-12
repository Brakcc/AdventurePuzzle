using UnityEngine;

namespace DebuggingClem
{
    public class Chargment : MonoBehaviour
    {
        [SerializeField] private float dist;
        [SerializeField] private Transform pivot;
        private Transform self;
        private GameObject player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            self = transform.GetChild(0);
        }

        private void Update()
        {
            if (Mathf.Abs(Vector3.Distance(player.transform.position, pivot.transform.position)) >= dist)
            {
                self.gameObject.SetActive(false);
            }
            else
            {
                self.gameObject.SetActive(true);
            }
        }
    }
}