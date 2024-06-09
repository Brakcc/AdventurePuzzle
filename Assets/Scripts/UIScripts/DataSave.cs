using UnityEngine;

namespace UIScripts
{
    public class DataSave : MonoBehaviour
    {
        public int saveChosen;
        
        public float save1X;
        public float save1Y;
        public float save1Z;

        public float save2X;
        public float save2Y;
        public float save2Z;
        
        public float save3X;
        public float save3Y;
        public float save3Z;

        public DataSave(int saveChoose, Vector3 newPos = default)
        {
            if (newPos != default)
            {
                switch (saveChoose)
                {
                    case 1:
                        save1X = newPos.x;
                        save1Y = newPos.y;
                        save1Z = newPos.z;
                        Debug.Log(newPos.x);
                        Debug.Log(save1X);
                        break;

                    case 2:
                        save2X = newPos.x;
                        save2Y = newPos.y;
                        save2Z = newPos.z;
                        break;

                    case 3:
                        save3X = newPos.x;
                        save3Y = newPos.y;
                        save3Z = newPos.z;
                        break;
                }
            }
            else
            {
                saveChosen = saveChoose;
            }
            
        }
    }
}
