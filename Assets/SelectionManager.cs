using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] 
    private LayerMask m_droidMask;

    private DeliveryBot m_selectedDroid = null;
    private Building m_prevDroidBuilding = null;

    private void Update()
    {
        Ray rayToMouseButton = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayToMouseButton, out RaycastHit hitInfo, Mathf.Infinity, m_droidMask))
        {
            DeliveryBot hitDroid = hitInfo.collider.GetComponentInParent<DeliveryBot>();

            if (Equals(hitDroid.StateMachine.CurrentStateType, DroidStates.Deliver) ||
                Equals(hitDroid.StateMachine.CurrentStateType, DroidStates.Slapped))
            {
                m_selectedDroid = hitDroid;

                if (m_selectedDroid.CurrentTarget != m_prevDroidBuilding && m_prevDroidBuilding != null)
                {
                    m_prevDroidBuilding.SetHighlighted(false);
                }
                
                m_selectedDroid.SetHighlighted(true);
                m_selectedDroid.CurrentTarget.SetHighlighted(true);
                
                m_prevDroidBuilding = m_selectedDroid.CurrentTarget;
            }
        }
        else
        {
            if (m_selectedDroid != null)
            {
                m_selectedDroid.SetHighlighted(false);
                m_selectedDroid.CurrentTarget.SetHighlighted(false);
            }
            m_selectedDroid = null;
            m_prevDroidBuilding = null;
        }
    }
}
