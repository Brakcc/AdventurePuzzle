using UnityEngine;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO
{
    [CreateAssetMenu(fileName = "InteractDatas", menuName = "PlayerDatasSO/IinteractDatas")]
    public class InteractDatasSO : AbstractPlayerDatasSO
    {
        public float absorbTime;

        public float applyTime;
        
        public GameObject EnergyPosCloseVFX;

        public GameObject GreenDepVFX;

        public GameObject BlueDepVFX;

        public GameObject GreenSourceVFX;
        
        public GameObject BlueSourceVFX;

        public void OnVFX(byte id, Vector3 pos, Vector3 dir)
        {
            var b = Vector3.Angle(Vector3.right, new Vector3(dir.x, 0, dir.z).normalized);
            var a = Quaternion.Euler(new Vector3(0, b, 0));
            
            switch (id)
            {
                case 0:
                    var tempV0 = Instantiate(EnergyPosCloseVFX, pos, a);
                    if (!tempV0.TryGetComponent<ParticleSystem>(out var p0))
                    {
                        Destroy(tempV0);
                        return;
                    }
                    p0.Play();
                    Destroy(tempV0, 3.5f);
                    break;
                case 1:
                    var tempV1 = Instantiate(GreenDepVFX, pos, a);
                    if (!tempV1.TryGetComponent<ParticleSystem>(out var p1))
                    {
                        Destroy(tempV1);
                        return;
                    }
                    p1.Play();
                    Destroy(tempV1, 8f);
                    break;
                case 2:
                    var tempV2 = Instantiate(BlueDepVFX, pos, a);
                    if (!tempV2.TryGetComponent<ParticleSystem>(out var p2))
                    {
                        Destroy(tempV2);
                        return;
                    }
                    p2.Play();
                    Destroy(tempV2, 8f);
                    break;
                case 3:
                    var tempV3 = Instantiate(GreenSourceVFX, pos, a);
                    if (!tempV3.TryGetComponent<ParticleSystem>(out var p3))
                    {
                        Destroy(tempV3);
                        return;
                    }
                    p3.Play();
                    Destroy(tempV3, 8f);
                    break;
                case 4:
                    var tempV4 = Instantiate(BlueSourceVFX, pos, a);
                    if (!tempV4.TryGetComponent<ParticleSystem>(out var p4))
                    {
                        Destroy(tempV4);
                        return;
                    }
                    p4.Play();
                    Destroy(tempV4, 8f);
                    break;
            }
        }

        public void OnVFX(byte id, Vector3 pos, Quaternion dir)
        {
            //var a = Vector3.Angle(Vector3.right, dir);
            //var a = Quaternion.Euler(dir);


            switch (id)
            {
                case 0:
                    var tempV0 = Instantiate(EnergyPosCloseVFX, pos, dir);
                    if (!tempV0.TryGetComponent<ParticleSystem>(out var p0))
                    {
                        Destroy(tempV0);
                        return;
                    }
                    tempV0.transform.Rotate(Vector3.up, -90);
                    p0.Play();
                    Destroy(tempV0, 3.5f);
                    break;
                case 1:
                    var tempV1 = Instantiate(GreenDepVFX, pos, dir);
                    if (!tempV1.TryGetComponent<ParticleSystem>(out var p1))
                    {
                        Destroy(tempV1);
                        return;
                    }

                    tempV1.transform.Rotate(Vector3.up, -90);
                    p1.Play();
                    Destroy(tempV1, 8f);
                    break;
                case 2:
                    var tempV2 = Instantiate(BlueDepVFX, pos, dir);
                    if (!tempV2.TryGetComponent<ParticleSystem>(out var p2))
                    {
                        Destroy(tempV2);
                        return;
                    }

                    tempV2.transform.Rotate(Vector3.up, -90);
                    p2.Play();
                    Destroy(tempV2, 8f);
                    break;
                case 3:
                    var tempV3 = Instantiate(GreenSourceVFX, pos, dir);
                    if (!tempV3.TryGetComponent<ParticleSystem>(out var p3))
                    {
                        Destroy(tempV3);
                        return;
                    }

                    tempV3.transform.Rotate(Vector3.up, -90);
                    p3.Play();
                    Destroy(tempV3, 8f);
                    break;
                case 4:
                    var tempV4 = Instantiate(BlueSourceVFX, pos, dir);
                    if (!tempV4.TryGetComponent<ParticleSystem>(out var p4))
                    {
                        Destroy(tempV4);
                        return;
                    }

                    tempV4.transform.Rotate(Vector3.up, -90);
                    p4.Play();
                    Destroy(tempV4, 8f);
                    break;
            }
        }
    }
}