using System;
using Unity.AI.Navigation;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Narration.Creature
{
    [Obsolete]
    public class NavMeshGenerator : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            _reMapCounter = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
                mod.AffectsAgentType(0);
            
            if (_reMapCounter <= Constants.ReMapNavMeshDelay)
            {
                //_reMapCounter += Time.deltaTime;
                return;
            }
            
            surface.BuildNavMesh();
            _reMapCounter = 0;
        }

        #endregion

        #region fields

        [SerializeField] private NavMeshSurface surface;
        [SerializeField] private NavMeshModifierVolume mod;
        
        [ReadOnly]
        private float _reMapCounter;

        #endregion
    }
}