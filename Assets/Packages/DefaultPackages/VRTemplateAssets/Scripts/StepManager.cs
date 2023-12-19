using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Controls the steps in the in coaching card.
    /// </summary>
    public class StepManager : MonoBehaviour
    {
        [Serializable]
        class Step
        {
            [SerializeField]
            public GameObject stepObject;

            [SerializeField]
            public string buttonText;
        }

        [SerializeField]
        public TextMeshProUGUI m_StepButtonTextField;
        public GameObject continueButton;
        public GameObject backButton;
        

        [SerializeField]
        List<Step> m_StepList = new List<Step>();

        int m_CurrentStepIndex = 0;

        public void Start()
        {
            if (m_CurrentStepIndex == 0)
            {
                backButton.SetActive(false);
            }

        }

        public void Next()
        {
     
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = (m_CurrentStepIndex + 1) % m_StepList.Count;

            if (m_CurrentStepIndex >= 1)
            {
                backButton.SetActive(true);
            }
            if (m_StepList[m_CurrentStepIndex].stepObject == null) return;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
            m_StepButtonTextField.text = m_StepList[m_CurrentStepIndex].buttonText;
 
        }

        public void Previous()
        {
            if (m_CurrentStepIndex == 1)
            {
                backButton.SetActive(false);
            } 
            
                m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
                m_CurrentStepIndex = m_CurrentStepIndex - 1 % m_StepList.Count;
                m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
                m_StepButtonTextField.text = m_StepList[m_CurrentStepIndex].buttonText;
            
            
        }
    }
}
