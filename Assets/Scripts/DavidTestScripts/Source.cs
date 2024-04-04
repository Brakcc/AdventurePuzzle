using UnityEngine;
using static SourceTypes;

public class Source : MonoBehaviour
{
    public SourceType mySourceType;
    private SourceType _myOriginalSourceType;
    private bool _energyTook;
    
    private void Start()
    {
        _myOriginalSourceType = mySourceType;
        GetComponent<Renderer>().material = SetUpEnergyMaterial(mySourceType);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && other.CompareTag("Player") && !_energyTook)
        {
            _energyTook = true;
            other.GetComponent<PlayerEnergy>().playerSourceType = mySourceType;
            mySourceType = SourceType.NoEnergy;
            GetComponent<Renderer>().material = SetUpEnergyMaterial(mySourceType);
        }
    }
    
}
