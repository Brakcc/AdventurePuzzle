using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
   [Header("Parameter Change")] 
   
   [SerializeField] private string parameterName;
   [SerializeField] private float parameterValue;

   
   private void OnTriggerEnter(Collider collider)
   {
      if (collider.tag.Equals("Player"))
      {
         AudioManager.instance.SetAmbienceParameter(parameterName, parameterValue);
      }
   }
}