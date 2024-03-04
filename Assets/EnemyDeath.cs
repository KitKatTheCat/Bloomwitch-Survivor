using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public Collectible Collectible;
    [SerializeField]private float m_dropChance;
    [SerializeField] private CollectibleType m_collectibleOverride;


    public void EnemyDrop()
    {

        float rand = Random.Range(0f,1f);
        float extraDropChance = Mathf.Max(10 - PlayerSeeds.instance.GetTotalSeedCount(), 0) * 0.08f;
        if (PerksManager.instance.IsPerkActive(Perk.MoreRedSeedDrop))
        {
            extraDropChance += 0.05f;
        }
        if (PerksManager.instance.IsPerkActive(Perk.MoreBlueSeedDrop))
        {
            extraDropChance += 0.05f;
        }
        if (PerksManager.instance.IsPerkActive(Perk.MoreYellowSeedDrop))
        {
            extraDropChance += 0.05f;
        }
        if (PerksManager.instance.IsPerkActive(Perk.MoreHealthDrop))
        {
            extraDropChance += 0.05f;
        }

        if (rand <= m_dropChance + extraDropChance)
        {
            Vector3 dir = (transform.position);
            Collectible collectible = Instantiate<Collectible>(Collectible, dir, Quaternion.identity);
            collectible.Init(m_collectibleOverride);
        }
    }
    
}
