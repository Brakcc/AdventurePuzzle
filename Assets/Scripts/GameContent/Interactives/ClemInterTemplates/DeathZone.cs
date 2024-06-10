using GameContent.PlayerScripts;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class DeathZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            //other.gameObject.GetComponent<PlayerStateMachine>().IsDedge = true; // Change?
        }
    }
}