using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
 

public class CharacterCreator : MonoBehaviour
{
    public GameObject character;
    public BodyPart[] bodyParts;
    public FlexibleColorPicker colorPicker;
    private Material selectedSkin;
    public InputField characterName;
    private GameObject selectedObj;
    private Color lastSelectedColor;

    void Awake()
    { 
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].parts = GameObject.FindGameObjectsWithTag(bodyParts[i].name);
            bodyParts[i].toggle = GameObject.Find(bodyParts[i].name+"Toggle").GetComponent<Toggle>();
            SkinInitialize(bodyParts[i]);
        } 
    }

    private void Update()
    {
        if (selectedSkin != null) {
            selectedSkin.color = colorPicker.color;
        }
    }

    private void SkinInitialize(BodyPart bodyPart) {
        for (int i = 0; i < bodyPart.parts.Length; i++)
        {
            if (bodyPart.parts[i].activeSelf)
                bodyPart.parts[i].SetActive(false);
        }

        if(bodyPart.defaultToggleVal)
            bodyPart.parts[bodyPart.currIndex].SetActive(true);
        bodyPart.toggle.isOn = bodyPart.defaultToggleVal;
        if (colorPicker.gameObject.activeSelf)
            colorPicker.gameObject.SetActive(false);
    }

    public void NextBodyPartSkin(BodyPart bodyPart)
    {
        for (int i = 0; i < bodyPart.parts.Length; i++)
        {
            if (bodyPart.parts[i].activeSelf)
                bodyPart.parts[i].SetActive(false);
        }
        bodyPart.currIndex++;
        bodyPart.currIndex = bodyPart.currIndex >= bodyPart.parts.Length ? 0 : bodyPart.currIndex;
        bodyPart.parts[bodyPart.currIndex].SetActive(true);
        bodyPart.toggle.isOn = true;
        bodyPart.defaultToggleVal = true;
        if (colorPicker.gameObject.activeSelf)
            colorPicker.gameObject.SetActive(false);
    }

    public void PrevBodyPartSkin(BodyPart bodyPart)
    {
        for (int i = 0; i < bodyPart.parts.Length; i++)
        {
            if(bodyPart.parts[i].activeSelf)
                bodyPart.parts[i].SetActive(false);
        }
        bodyPart.currIndex--;
        bodyPart.currIndex = bodyPart.currIndex < 0 ? bodyPart.parts.Length - 1: bodyPart.currIndex;
        bodyPart.parts[bodyPart.currIndex].SetActive(true);
        bodyPart.toggle.isOn = true;
        bodyPart.defaultToggleVal = true;
        if (colorPicker.gameObject.activeSelf)
            colorPicker.gameObject.SetActive(false);
    }

    public void ToggleValueChanged(BodyPart bodyPart)
    { 
        if (bodyPart.toggle.isOn == false)
        {
            for (int i = 0; i < bodyPart.parts.Length; i++)
            {
                if (bodyPart.parts[i].activeSelf)
                    bodyPart.parts[i].SetActive(false);
            }
        }
        else {
            bodyPart.parts[bodyPart.currIndex].SetActive(true); 
        }
        bodyPart.defaultToggleVal = bodyPart.toggle.isOn;
        if (colorPicker.gameObject.activeSelf)
            colorPicker.gameObject.SetActive(false);
    }

    public void DisplayColorPick(BodyPart bodyPart)
    {
        if(!colorPicker.gameObject.activeSelf)
            colorPicker.gameObject.SetActive(true);
        selectedObj = bodyPart.parts[bodyPart.currIndex];
        selectedObjects.Add(bodyPart.parts[bodyPart.currIndex]);
        selectedSkin = bodyPart.parts[bodyPart.currIndex].GetComponent<Renderer>().material;
    }
    public List<GameObject> selectedObjects = new List<GameObject>();

    public void SaveCharacter()
    {
        if (characterName.text != "")
        {
            Debug.Log(characterName.text + " SAVED");
            for (int i = 0; i < selectedObjects.Count; i++)
            {
                Material selectedMat = selectedObjects[i].GetComponent<Renderer>().material;
                AssetDatabase.CreateAsset(selectedMat, "Assets/OttoCharacterPack/CreatedCharacters/Materials/" + characterName.text + i + ".mat");
                Material SRm = (Material)AssetDatabase.LoadAssetAtPath("Assets/OttoCharacterPack/CreatedCharacters/Materials/" + characterName.text + i + ".mat", typeof(Material));
                selectedObjects[i].GetComponent<Renderer>().material = SRm;

            }
            string localPath = "Assets/OttoCharacterPack/CreatedCharacters/Prefabs/" + characterName.text + ".prefab";

            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(character, localPath, InteractionMode.UserAction);
        }
        else {
            Debug.Log("Please enter a character name");
        }

    }

}
