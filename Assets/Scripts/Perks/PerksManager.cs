using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksManager : MonoBehaviour
{
    public static PerksManager instance;

    private HashSet<Perk> m_activePerks = new HashSet<Perk>();
    [SerializeField] private List<Perk> m_debugPerks;
    private List<Perk> m_availablePerks = new List<Perk>();
    private int PERK_SELECT_COUNT = 3;


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        ResetPool();
        #if UNITY_EDITOR
        foreach(Perk perk in m_debugPerks)
        {
            AddToActivePerks(perk);
        }
        #endif
    }

    public void ResetPool()
    {
        foreach(Perk perk in System.Enum.GetValues(typeof(Perk)))
        {
            m_availablePerks.Add(perk);
        }
    }
    
    public Perk[] GetRandomFromAvailablePool()
    {

        Perk[] perks = new Perk[Mathf.Min(PERK_SELECT_COUNT, m_availablePerks.Count)];
        List<Perk> availablePerks = new List<Perk>(m_availablePerks);
        for(int i = 0; i < m_availablePerks.Count && i < PERK_SELECT_COUNT; i++)
        {
            Perk randPerk = availablePerks[Random.Range(0, availablePerks.Count)];
            availablePerks.Remove(randPerk);
            perks[i] = randPerk;
        }
        return perks;
    }

    public void AddToActivePerks(Perk perk)
    {
        m_availablePerks.Remove(perk);
        m_activePerks.Add(perk);
    }

    public bool IsPerkActive(Perk perk)
    {
        return m_activePerks.Contains(perk);
    }

    public int GetActivePerkCount()
    {
        return m_activePerks.Count;
    }

    public int GetTotalPerkCount()
    {
        return System.Enum.GetValues(typeof(Perk)).Length;
    }
}
