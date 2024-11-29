using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialManager : MonoBehaviour
{
    public Image[] materialedImages;
    public string[] imageTag;
    public bool[] isOn;
    Material[] materials;

    private void Awake()
    {
        materials = new Material[3];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = materialedImages[i].material;
        }
    }

    private void Start()
    {
        for (int i = 0; i < materialedImages.Length; i++)
        {
            if (isOn[i])
            {
                OnMeterialWithNum(i);
            }
            else
            {
                OffMeterialWithNum(i);
            }
        }
    }

    public void OnMaterialWithTag(string str)
    {
        int num = FindNumber(str);
        if (num > -1)
        {
            OnMeterialWithNum(num);
        }
    }

    public void OffMaterialWithTag(string str)
    {
        int num = FindNumber(str);
        if (num > -1)
        {
            OffMeterialWithNum(FindNumber(str));
        }
    }

    void OnMeterialWithNum(int id)
    {
        ChangeMeterial(id, materials[id]);
    }

    void OffMeterialWithNum(int id)
    {
        ChangeMeterial(id, null);
    }

    void ChangeMeterial(int id, Material material)
    {
        
        materialedImages[id].material = material;
    }

    int FindNumber(string str)
    {
        for (int i = 0; i < imageTag.Length; i++)
        {
            if (str == imageTag[i])
            {
                return i;
            }
        }
        return -1;
    }
}
