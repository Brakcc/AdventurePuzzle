using System.IO;
using UnityEngine; 

namespace UIScripts
{
    public class CheckpointManager : FileWriter
    {
        public Transform player;
        public static CheckpointManager CheckMInstance;
        
        private void Start()
        {
            CheckMInstance = this;
            
            if (File.Exists(Application.persistentDataPath + "/saveData.data"))
            {
                LoadData(3);
            }
            if (!(newPosCheckpoint is { x: 0, y: 0, z: 0 }))
            {
                SendToCheckpoint();
            }
            
            if (int.Parse(LoadValues(3)[0]) == 0)
            {
                WriteData(4, null, null, 1, newPosCheckpoint.x, newPosCheckpoint.y, newPosCheckpoint.z);
            }
        }

        public void SendToCheckpoint()
        {
            player.position = newPosCheckpoint;
        }
    }
}
