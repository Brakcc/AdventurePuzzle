using UnityEngine;

namespace UIScripts
{
    public class CheckPointScript : FileWriter
    {
        private void OnTriggerEnter(Collider player)
        {
            if (player.CompareTag("Player"))
            {
                //Player Detected
                var position = transform.position;
                WriteData(3, 
                    null, null,
                    saveChosen, position.x, position.y, position.z);
                CheckpointManager.CheckMInstance.newPosCheckpoint = position;
            }
        }
    }
}
