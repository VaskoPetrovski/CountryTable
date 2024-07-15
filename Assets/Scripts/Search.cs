using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class Search : MonoBehaviour {
        
   [SerializeField] public TMP_InputField searchInput;
    public static Search instance;
    private string lastSearch;
    
    [SerializeField] private GameObject normalView;
    [SerializeField] private GameObject searchView;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform searchViewParent;
    [SerializeField] private GameObject searchObjectPrefab;
    
    private List<GameObject> displayList = new List<GameObject>();
    private void Awake()
    {
     if (instance == null) {
         instance = this;
     }
     else
     {
         Destroy(this);
     }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
           Debug.Log("Search Input: " + searchInput.text);
           if (searchInput.text.Equals(lastSearch)|| searchInput.text.Length == 0)
           {
               return;
           }
           SearchProfiles();
           searchView.SetActive(true);
           normalView.SetActive(false);
           scrollRect.content = searchViewParent.GetComponent<RectTransform>();
           lastSearch = searchInput.text;
        }
        
        if (Input.GetKeyDown(KeyCode.Backspace) && searchInput.text.Length == 0)
        {
          normalView.SetActive(true);
          searchView.SetActive(false);
          scrollRect.content = normalView.GetComponent<RectTransform>();
          displayList.ForEach(obj => Destroy(obj));
        }
    }
    
    public void SearchProfiles()
    {
        if (displayList.Count > 0)
        {
            displayList.ForEach(obj => Destroy(obj));
            displayList.Clear();
        }
        Debug.Log("Search Input: " + searchInput.text);
       var  searchText = searchInput.text.ToLower();
       foreach (var profile in FileLoader.instance.countryProfiles.Values)
       {
           foreach (var profileData in profile)
           {
               if (profileData.Name.ToLower().Equals(searchText) ||
                   profileData.Surname.ToLower().Equals(searchText) ||
                   profileData.CountryName.ToLower().Equals(searchText)) {
                   
                   var newProfile = Instantiate(searchObjectPrefab, searchViewParent);
                   newProfile.GetComponent<SetProfile>().SetProfileData(profileData);
                   displayList.Add(newProfile);
               }
           }
       }
    }
}
    

