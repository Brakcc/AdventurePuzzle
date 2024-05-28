using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class PistonInter : ReceptorInter
    {
        [SerializeField] private Transform positionBase;
        [SerializeField] private Transform positionPushed;
        [SerializeField] private Transform groupToPush;

        public bool testVariable;
        void Start()
        {
            SetUpCenter();
        }

        // Update is called once per frame
        void Update()
        {
            if (testVariable){SetUpCenter();}
        }

        void SetUpCenter()
        {
            GetComponent<BoxCollider>().size =
                new Vector3(Vector3.Distance(transform.GetChild(0).position, transform.GetChild(1).position)/2,
                    GetComponent<BoxCollider>().size.y, GetComponent<BoxCollider>().size.z);

            GetComponent<BoxCollider>().center = new Vector3(
                transform.GetChild(0).position.x  + Vector3.Distance(transform.GetChild(0).position, transform.GetChild(1).position)/2,
                GetComponent<BoxCollider>().center.y, GetComponent<BoxCollider>().center.z);
        }
    }
}
