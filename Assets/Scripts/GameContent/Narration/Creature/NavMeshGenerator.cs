using Unity.AI.Navigation;
using UnityEngine;

namespace GameContent.Narration.Creature
{
    public class NavMeshGenerator : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            _reMapCounter = 0;
        }

        private void Update()
        {
            if (_reMapCounter <= Constants.ReMapNavMeshDelay)
            {
                _reMapCounter += Time.deltaTime;
                return;
            }
            
            surface.BuildNavMesh();
            _reMapCounter = 0;
        }

        #endregion

        #region fields

        [SerializeField] private NavMeshSurface surface;
        private NavMeshModifier mod;
        
        private float _reMapCounter;

        #endregion
    }
}