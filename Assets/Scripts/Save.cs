using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



//Class handles gathering saves
public class Save : MonoBehaviour
{
   [SerializeField] public SaveManager saveManager;
   [SerializeField] private Config config;

   [SerializeField] private TextMeshProUGUI saveDescription;

   private SaveDataSerializable saveSlotData;

   private string currentPath;

   private void Start()
    {
        //Find a save data, else Generate
        SetUpSaveSlotData();
        saveDescription.text = saveSlotData.currentSlotInfo;
    }

    private void SetUpSaveSlotData()
    {
        saveSlotData = CheckSave();
        if (saveSlotData == null)
        {
            GenerateBlankSaveSlot();
        }
    }

    public void OnClick()
   {
      saveManager.SetCurrentSaveData(saveSlotData);
      Debug.Log(saveSlotData.currentLevel);
      SceneManager.LoadScene(saveSlotData.currentLevel);

   }

   private SaveDataSerializable CheckSave()
   {
      currentPath = $"{Application.persistentDataPath}/{this.gameObject.name}.json";
      Debug.Log(currentPath);
      Debug.Log(File.Exists(currentPath) ? "true" : "false");
      if (File.Exists(currentPath))
      {
         Debug.Log("File Exist for: " + this.gameObject.name);
         string json = File.ReadAllText(currentPath);
         SaveDataSerializable currentSaveData = JsonUtility.FromJson<SaveDataSerializable>(json);
         return currentSaveData;
      }
      
      return null;
   }

   private void GenerateBlankSaveSlot()
   {
      saveSlotData = new SaveDataSerializable
      {
         saveDataName = this.gameObject.name,
         currentLevel = "Level1",
         currentSlotInfo = "Empty",
         GameBeat = false,

      };
   }

   public void ResetSave()
   {
      currentPath = $"{Application.persistentDataPath}/{this.gameObject.name}.json";
      File.Delete(currentPath);
      GenerateBlankSaveSlot();
      saveDescription.text = saveSlotData.currentSlotInfo;
   }





}
