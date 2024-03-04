using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSeeds : MonoBehaviour
{
    public static PlayerSeeds instance;

    public Dictionary<PlantColor, int> m_seedDict = new Dictionary<PlantColor, int>();
    public UnityEvent<int[]> onSeedCountUpdate = new UnityEvent<int[]>();

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        m_seedDict.Add(PlantColor.Yellow, 1);
        m_seedDict.Add(PlantColor.Red, 1);
        m_seedDict.Add(PlantColor.Blue, 1);
        if (onSeedCountUpdate != null)
        {
            onSeedCountUpdate.Invoke(new int[] { GetPlantColorCount(PlantColor.Red), GetPlantColorCount(PlantColor.Blue), GetPlantColorCount(PlantColor.Yellow) });
        }
    }

    public void AddPlant(PlantColor seedColor)
    {
        m_seedDict[seedColor] = m_seedDict[seedColor] + 1;
        if (onSeedCountUpdate != null)
        {
            onSeedCountUpdate.Invoke(new int[] { GetPlantColorCount(PlantColor.Red), GetPlantColorCount(PlantColor.Blue), GetPlantColorCount(PlantColor.Yellow) });
        }
    }

    public int GetPlantColorCount(PlantColor seedColor)
    {
        return m_seedDict[seedColor];
    }

    public int GetTotalSeedCount()
    {
        return GetPlantColorCount(PlantColor.Yellow) + GetPlantColorCount(PlantColor.Red) + GetPlantColorCount(PlantColor.Blue);
    }



    public List<PlantColor> GetAvailableSeedColors()
    {
        List<PlantColor> available = new List<PlantColor>();

        if(m_seedDict[PlantColor.Yellow] > 0) available.Add(PlantColor.Yellow);
        if(m_seedDict[PlantColor.Blue] > 0) available.Add(PlantColor.Blue);
        if(m_seedDict[PlantColor.Red] > 0) available.Add(PlantColor.Red);

        return available;
    }

    public void DeductPlant(PlantColor seedColor)
    {
        m_seedDict[seedColor] = m_seedDict[seedColor] - 1;
        if(onSeedCountUpdate != null)
        {
            onSeedCountUpdate.Invoke(new int[] { GetPlantColorCount(PlantColor.Red), GetPlantColorCount(PlantColor.Blue), GetPlantColorCount(PlantColor.Yellow) });
        }
    }

}
