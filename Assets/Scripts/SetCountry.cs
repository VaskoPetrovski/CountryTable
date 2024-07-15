using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetCountry : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI countryName;
    [SerializeField] private Button displayProfilesButton;
    [SerializeField] private Transform profileListParent;
    [SerializeField] private bool isEnabled;
    [SerializeField] private GameObject uiProfilePrefab;
    
    private Country _country;

    private bool firstRunFlag = true;
    private List<GameObject> objectList = new List<GameObject>();
    
    private void Start()
    {
        displayProfilesButton.onClick.AddListener(ChangeState);
    }
   public void SetCountryData(Country country, Transform scrollParent)
        {
            countryName.text = country.Name;
            _country = country;
            profileListParent = scrollParent;
        }
   
   public void ChangeState()
   {
       isEnabled = !isEnabled;
       DisplayProfiles();
   }
   
    public void DisplayProfiles()
    {
        Debug.Log("Display Profiles" + "Country: " + _country.Name);
        
        // Default View
        if (isEnabled)
        {
            if (firstRunFlag)
            {
                foreach (var profile in FileLoader.instance.countryProfiles[_country])
                {
                    Debug.Log("Profile: " + profile.Name + "Country: " + _country.Name);
                   // instantaite below this object in the same hierarchy
                   var newProfile = Instantiate(uiProfilePrefab, profileListParent);
                   // var newProfile = Instantiate(uiProfilePrefab, transform.parent);
                    newProfile.GetComponent<SetProfile>().SetProfileData(profile);
                    objectList.Add(newProfile);
                    
                    // // Adjust the position in the hierarchy
                    int currentIndex = gameObject.transform.GetSiblingIndex();
                    newProfile.transform.SetSiblingIndex(currentIndex + 1);
                }

                firstRunFlag = false;
            }
            else
            {
                objectList.ForEach(obj => obj.SetActive(true));
            }
        }
        else
        {
            objectList.ForEach(obj => obj.SetActive(false));
        }
    }
   
   
}
