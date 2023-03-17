using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectSelection : MonoBehaviour
{
    public SaveObjectsScript saveToFile = new();
    public GameObject selectedObject;
    public GameObject editObjectCanvas;
    public GameObject leftControllerCanvas;
    public GameObject editMovementCanvas;
    public TMP_Text timeText;
    public TMP_Text previewText;
    public TMP_Text editMovementText;
    public Button removeMovementButton;
    public Button saveMovementButton;
    public Button loopMovementButton;


    //testing loading
    public GameObject prefab;


    public void showEditCanvas()
    {
        leftControllerCanvas.SetActive(false);
        editMovementCanvas.SetActive(false);
        editObjectCanvas.SetActive(true);

        if (selectedObject.GetComponent<SelectableObject>().hasMovement) editMovementText.GetComponent<TextMeshProUGUI>().text = "Edit movement";
        else editMovementText.GetComponent<TextMeshProUGUI>().text = "Add movement";
    }

    public void showEditMovementCanvas()
    {
        editObjectCanvas.SetActive(false);
        editMovementCanvas.SetActive(true);
        UpdateTimeDisplay();
    }

    public void showLeftControllerCanvas()
    {
        editObjectCanvas.SetActive(false);
        leftControllerCanvas.SetActive(true);
    }

    public void ChangeSelectedObject(GameObject newSelectedObject)
    {
        if (selectedObject != null) selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");

        selectedObject = newSelectedObject;

        showEditCanvas();
    }

    public void RemoveSelection()
    {
        selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        selectedObject = null;

        showLeftControllerCanvas();
    }

    public void DeleteSelectedObject()
    {
        if (selectedObject != null) selectedObject.GetComponent<SelectableObject>().DeleteObject();

        showLeftControllerCanvas();

        selectedObject = null;
    }

    public void EditMovement()
    {
        selectedObject.GetComponent<SelectableObject>().EditMovement();
        showEditMovementCanvas();

        if (selectedObject.GetComponent<SelectableObject>().loopMovement) loopMovementButton.GetComponent<Image>().color = new Color(0f/255, 160f/255, 255f/255);
        else loopMovementButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }
    
    public void Preview()
    {
        selectedObject.GetComponent<SelectableObject>().Preview();
        removeMovementButton.interactable = !removeMovementButton.interactable;
        saveMovementButton.interactable = !saveMovementButton.interactable;
        loopMovementButton.interactable = !loopMovementButton.interactable;

        if (selectedObject.GetComponent<SelectableObject>().move)
        {
            previewText.GetComponent<TextMeshProUGUI>().text = "Stop preview";
        }
        else
        {
            previewText.GetComponent<TextMeshProUGUI>().text = "Start preview";
        }
    }
    /*
    public void EditSpeed(float value)
    {
        selectedObject.GetComponent<SelectableObject>().SetSpeed(value);
        Debug.Log(value);
    }
    */

    public void IncreaseTime()
    {
        selectedObject.GetComponent<SelectableObject>().speed += 0.5f;
        UpdateTimeDisplay();
    }

    public void DecreaseTime()
    {
        if (selectedObject.GetComponent<SelectableObject>().speed > 0.5f) selectedObject.GetComponent<SelectableObject>().speed -= 0.5f;
        UpdateTimeDisplay();
    }

    public void UpdateTimeDisplay()
    {
        timeText.GetComponent<TextMeshProUGUI>().text = selectedObject.GetComponent<SelectableObject>().speed.ToString()+"s";
    }

    public void RemoveMovement()
    {
        selectedObject.GetComponent<SelectableObject>().RemoveMovement();
        showEditCanvas();
    }

    public void SaveMovement()
    {
        selectedObject.GetComponent<SelectableObject>().SaveMovement();
        showEditCanvas();
    }

    public void ToogleLoopMovement()
    {
        selectedObject.GetComponent<SelectableObject>().ToggleLoopMovement();
        if (selectedObject.GetComponent<SelectableObject>().loopMovement) loopMovementButton.GetComponent<Image>().color = new Color(0f / 255, 160f / 255, 255f / 255);
        else loopMovementButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void SaveTest()
    {
        foreach (Transform child in transform )
        {
            SelectableObjectDataClass dataClass = new();
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            dataClass.startPosistion = so.startPos;
            dataClass.endPosistion = so.endPos;
            dataClass.time = so.speed;
            dataClass.hasMovement = so.hasMovement;
            saveToFile.AddObjectDataToList(dataClass);
        }
        saveToFile.SaveToJSON();
    }

    //test method, move to correct place later
    public void LoadTest()
    {
        SaveObjectsScript loadedData = new();
        loadedData.LoadFromJSON();
        foreach (SelectableObjectDataClass dataClass in loadedData.selectableObjectData)
        {
            GameObject o = Instantiate(prefab, transform);
            SelectableObject so = o.transform.Find("Sphere").GetComponent<SelectableObject>();

            // Sets the position of the prefab
            so.gameObject.transform.position = dataClass.startPosistion;
            o.transform.Find("Ghost Sphere").transform.position = dataClass.endPosistion;

            // Sets public variables in script to correct values
            so.startPos = dataClass.startPosistion;
            so.endPos = dataClass.endPosistion;
            so.speed = dataClass.time;
            so.hasMovement = dataClass.hasMovement;

            so.ShowGhostSphere(so.hasMovement);
        }
    }

}