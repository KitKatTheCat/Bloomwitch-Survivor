using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    None = -1,
    RedSeed = 0,
    BlueSeed,
    YellowSeed,
    Health
}

public class Collectible : MonoBehaviour
{
    public CollectibleType m_collectibleType;

    int[] dropPool = new int[] {3, 3, 3, 1};
    public Sprite[] spriteArray;
    private SpriteRenderer sp;
    [SerializeField] HitFlash _flash;

    public void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    public void Init(CollectibleType collectible = CollectibleType.None)
    {
        if(PerksManager.instance.IsPerkActive(Perk.MoreRedSeedDrop))
        {
            dropPool[0] = 5;
        }
        if (PerksManager.instance.IsPerkActive(Perk.MoreBlueSeedDrop))
        {
            dropPool[1] = 5;
        }
        if (PerksManager.instance.IsPerkActive(Perk.MoreYellowSeedDrop))
        {
            dropPool[2] = 5;
        }
        if (PerksManager.instance.IsPerkActive(Perk.MoreHealthDrop))
        {
            dropPool[2] = 3;
        }
        int totalPool = 0;
        for(int i = 0; i < dropPool.Length; i++)
        {
            totalPool += dropPool[i];
        }

        int rand = Random.Range(0, totalPool);
        int selected = 0;
        int total = 0;

        for(int i = 0; i < dropPool.Length; i++)
        {
            total += dropPool[i];
            if(rand < total)
            {
                selected = i;
                break;
            }
        }

        m_collectibleType = (CollectibleType)selected;
        if(collectible != CollectibleType.None)
        {
            m_collectibleType = collectible;
        }
        sp.sprite = spriteArray[(int)m_collectibleType];

        StartCoroutine(FlashLoop());
    }

    public void Collect()
    {
        bool is_health = false;
        switch(m_collectibleType)
        {
            case CollectibleType.RedSeed:
                PlayerSeeds.instance.AddPlant(PlantColor.Red);
            break;
            case CollectibleType.YellowSeed:
                PlayerSeeds.instance.AddPlant(PlantColor.Yellow);
            break;
            case CollectibleType.BlueSeed:
                PlayerSeeds.instance.AddPlant(PlantColor.Blue);
            break;
            case CollectibleType.Health:
                is_health = true;
                Player.instance.Heal(3);
            break;
        }
        if (is_health) {
            Stem.SoundManager.Play("CombatSeedGrab");
        } else {
            Stem.SoundManager.Play("CombatSeedGrab");
        }
    }

    IEnumerator FlashLoop() {
        while (true) {
            _flash.GoFlash();
            yield return new WaitForSeconds(1);
        }
    }
}
