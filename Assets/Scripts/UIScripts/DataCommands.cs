using UnityEngine;

namespace UIScripts
{
    public class DataCommands : MonoBehaviour
    {
        public string actionCommandName;
        public string pauseCommandName;

        public DataCommands(string actionNewName, string pauseNewName)
        {
            actionCommandName = actionNewName;
            pauseCommandName = pauseNewName;
        }
    }
}
