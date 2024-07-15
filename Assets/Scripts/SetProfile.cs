using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetProfile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI company;
    [SerializeField] private TextMeshProUGUI mail;
    [SerializeField] private Image flag;
    
    public void SetProfileData(Profile profile)
    {
        name.text = String.Format("{0} {1}", profile.Name, profile.Surname);
        company.text = profile.Company;
        mail.text = profile.Email;
        flag.gameObject.SetActive(profile.Flag); 
    }
}
