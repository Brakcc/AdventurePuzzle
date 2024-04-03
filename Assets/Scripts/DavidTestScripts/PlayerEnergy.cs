using UnityEngine;
using static SourceTypes;

public class PlayerEnergy : MonoBehaviour
{
    public SourceType playerSourceType
    {
        get { return _playerSourceType; }
        set
        {
            _playerSourceType = value;
            GetComponent<Renderer>().material = SetUpEnergyMaterial(_playerSourceType);
        }
    }
    private SourceType _playerSourceType;

}
