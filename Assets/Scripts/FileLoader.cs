using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Windows.Forms;
using UnityEditor;
using Button = UnityEngine.UI.Button;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#elif UNITY_STANDALONE_WIN
using System.Windows.Forms;
#elif UNITY_STANDALONE_OSX
using System.Diagnostics;
using System.IO;
#endif

public class FileLoader : MonoBehaviour
{

    public static FileLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Button loadButton;

    // public InputField searchInput;
    // public Transform content;
    private string filePath = "";

    [SerializeField] private Transform uiProfileListObj;

    [SerializeField] private GameObject uiProfilePrefab;

    // [SerializeField] private VirtualizedList virtualizedList;
    [SerializeField] private GameObject uiCountryPrefab;

    public Dictionary<Country, List<Profile>> countryProfiles = new Dictionary<Country, List<Profile>>();
    public List<Country> countries = new List<Country>();

    void Start()
    {
        loadButton.onClick.AddListener(() =>
        {
            filePath = SelectFile();
            if (!string.IsNullOrEmpty(filePath))
            {
                LoadData(filePath);
            }
        });
        // searchInput.onValueChanged.AddListener(FilterProfiles);
    }


    
    
    public string SelectFile()
    {
#if UNITY_EDITOR
        return EditorUtility.OpenFilePanel("Select a File", "", "");
#elif UNITY_STANDALONE_WIN
        return OpenFileWindows();
#elif UNITY_STANDALONE_OSX
        return OpenFileMac();
#else
        return string.Empty;
#endif
    }
    #if UNITY_STANDALONE_WIN
    private string OpenFileWindows()
    {
        string path = string.Empty;
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Title = "Select a File",
            Filter = "All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            path = openFileDialog.FileName;
        }
        return path;
    }
    #endif
    private string OpenFileMac()
    {
        string path = string.Empty;
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "osascript",
                Arguments = "-e 'POSIX path of (choose file with prompt \"Select a file\")'",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        path = process.StandardOutput.ReadLine();
        process.WaitForExit();
        return path;
    }
   

    void LoadData(string filePath)
    {
        Debug.Log("Load Data");
        int index = 0;
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // Debug.Log(line.Substring(2, 2));
                if (line.Substring(2, 2) == "12")
                {
                    string countryName = line.Substring(14, 30).Trim();
                    var newCountry = new Country
                    {
                        Name = countryName,
                        Index = index
                    };
                    Debug.Log(countryName);
                    if (!countryProfiles.ContainsKey(newCountry))
                    {
                        countryProfiles[newCountry] = new List<Profile>();
                        countries.Add(newCountry);
                        index++;
                    }
                }
                else if (line.Substring(2, 2) == "01")
                {

                    Profile profile = new Profile
                    {
                        Name = line.Substring(9, 10).Trim(),
                        Surname = line.Substring(39, 15).Trim(),
                        Company = line.Substring(58, 25).Trim(),
                        Email = line.Substring(92, 25).Trim(),
                        CountryIndex = line.Substring(120, 3).Trim(),
                        Flag = line.Substring(123, 1) == "1"
                    };

                    var parsedCountryIndex = int.Parse(profile.CountryIndex);
                    var countryName = countries.Find(country => country.Index == parsedCountryIndex).Name;
                    profile.CountryName = countryName;
                  countryProfiles[countries.Find(country => country.Index == parsedCountryIndex)].Add(profile);
                }

            }

            // LogProfiles();
            // DisplayProfiles();
            DisplayCountries();
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    private void LogProfiles()
    {
        // var c1 = countryProfiles.First();
        // Debug.Log("<color=red>Country: </color> " + c1.Key.Name);
        // c1.Value.ForEach(profile =>
        // {
        //     Debug.Log("<color=blue>Profile: </color> " + profile.Name + " " + profile.Surname + " " +
        //               profile.Company + " " + profile.Email + " " + profile.CountryIndex + " " + profile.Flag);
        // });
    }

    private void DisplayCountries()
    {
        countries.ForEach(country =>
        {
            var countryObj = Instantiate(uiCountryPrefab, uiProfileListObj);
            countryObj.GetComponent<SetCountry>().SetCountryData(country, uiProfileListObj);
            // Search.instance.countryList.Add(countryObj.GetComponent<SetCountry>());
        });

    }
}


[System.Serializable]
public class Profile
{
    public string Name;
    public string Surname;
    public string Company;
    public string Email;
    public string CountryIndex;
    public bool Flag;
    public string CountryName;
}

[System.Serializable]
public class Country
{
    public string Name;
    public int Index;
}
