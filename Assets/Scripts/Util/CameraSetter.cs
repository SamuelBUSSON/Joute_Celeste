using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
   private void Awake()
   {
      var cam = GetComponent<CinemachineVirtualCamera>();
     // cam.GetCinemachineComponent<CinemachineGroupComposer>().m_MaximumOrthoSize = Screen.width * 0.5f;
   }
}
