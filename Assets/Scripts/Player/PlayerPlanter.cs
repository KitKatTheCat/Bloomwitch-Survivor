using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stem;

public class PlayerPlanter : MonoBehaviour
{
    public bool m_canPlant = true;
    public float m_plantRadius = 5;
    public int m_maxPlantInRadius = 3;
    public float m_plantingDuration = 5;

    public LayerMask m_plantLayer;
    public LayerMask m_rootLayer;
    public Collider2D m_rootCheck;

    public RedPlant m_redPlantPrefab;
    public BluePlant m_bluePlantPrefab;
    public YellowPlant m_yellowPlantPrefab;
    public RedPlant m_basePlantPrefab;
    public Root m_rootPrefab;
    
    public bool m_isPlanting;
    public float m_rootDistance = 2f;

    private RedPlant m_basePlant;
    private List<Vector2> m_plantLocations = new List<Vector2>();
    private List<Vector2> m_randomRootLocations = new List<Vector2>();
    private List<int> m_plantRootCount = new List<int>();
    private List<int> m_extraRootCount = new List<int>();
    private List<Root> m_currentRoots = new List<Root>();
    private List<PlantColor> m_seedsToPlant = new List<PlantColor>();
    public float m_durationLeft;
    private float m_maxDuration;

    [SerializeField] AudioSource _audioRootsLoop;
    [SerializeField] PlayerAnimation m_animation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("plant"))
        {
            GeneratePlantPositions();
            if(m_canPlant)
            {
                m_animation.Plant();
                SoundManager.Play("PlantModeBegin");

                m_basePlant = Instantiate<RedPlant>(m_basePlantPrefab, transform.position, Quaternion.identity);
                m_basePlant.PlayerRootAdjust();
                m_basePlant.Init();
            }
            else
            {
                Player.instance.PlantError();
                SoundManager.Play("PlantModeError");
            }

            if(m_plantLocations.Count > 0)
            {
                m_isPlanting = true;
                m_maxDuration = m_plantingDuration;
                if (PerksManager.instance.IsPerkActive(Perk.PlayerFasterPlant))
                {
                    m_maxDuration /= 2;
                }
                m_durationLeft = m_maxDuration;
                _audioRootsLoop.Play();
            }
            
        }
        if(Input.GetButton("plant"))
        {
            if(m_isPlanting)
            {
                float progress = 1 - Mathf.InverseLerp(0, m_maxDuration, m_durationLeft);

                for(int i = 0; i < m_plantLocations.Count; i++)
                {
                    
                    Vector2 targetPos = Vector2.Lerp(transform.position, m_plantLocations[i], progress);
                    Vector2 extraRootPos = Vector2.Lerp(transform.position, m_plantLocations[i], progress - 0.1f);
                    Vector2 dir = (targetPos - (Vector2)transform.position); 
                    float distance = dir.magnitude;
                    dir.Normalize();
                    int rootCount = (int)Mathf.Floor(distance / m_rootDistance);
                    if (m_plantRootCount[i] < rootCount)
                    {
                        Quaternion rotation = Quaternion.LookRotation(dir);
                        rotation.x = 0;
                        rotation.y = 0;
                        m_currentRoots.Add(Instantiate<Root>(m_rootPrefab, targetPos, rotation));
                        if(Random.Range(0f, 1f) <= 0.5f)
                        {
                            rotation = Quaternion.AngleAxis(Random.Range(-60f, 60f), Vector3.forward) * rotation;
                            m_currentRoots.Add(Instantiate<Root>(m_rootPrefab, extraRootPos, rotation));
                        }
                        m_plantRootCount[i] = rootCount;
                    }
                }

                for (int i = 0; i < m_randomRootLocations.Count; i++)
                {

                    Vector2 targetPos = Vector2.Lerp(transform.position, m_randomRootLocations[i], progress);
                    Vector2 dir = (targetPos - (Vector2)transform.position);
                    float distance = dir.magnitude;
                    dir.Normalize();
                    int rootCount = (int)Mathf.Floor(distance / m_rootDistance);
                    if (m_extraRootCount[i] < rootCount)
                    {
                        Quaternion rotation = Quaternion.LookRotation(dir);
                        rotation.x = 0;
                        rotation.y = 0;
                        m_currentRoots.Add(Instantiate<Root>(m_rootPrefab, targetPos, rotation));
                    }
                    m_extraRootCount[i] = rootCount;
                }

                m_durationLeft -= Time.deltaTime;
                if(m_durationLeft <= 0)
                {
                    for (int i = 0; i < m_plantLocations.Count; i++)
                    {
                        Plant(m_seedsToPlant[i], m_plantLocations[i]);
                    }
                    m_isPlanting = false;
                    m_currentRoots.Clear();
                    SoundManager.Play("PlantModeBloom");
                    _audioRootsLoop.Stop();
                }
            }
            if(PerksManager.instance.IsPerkActive(Perk.PlayerRegenWhilePlant))
            {
                Player.instance.Heal(0.5f * Time.deltaTime);
            }
        }
        else if(Input.GetButtonUp("plant"))
        {
            if(m_isPlanting)
            {
                for(int i = 0; i < m_currentRoots.Count; i++)
                {
                    // m_currentRoots[i].Destroy();
                }
                m_currentRoots.Clear();

                m_isPlanting = false;

                //SoundManager.Play("PlantModeEnd");
            }

            m_animation.ResumeAnimation();
            if (m_basePlant != null)
            {
                m_basePlant.Destroy();
                m_basePlant = null;
                SoundManager.Play("PlantModeEnd");
            }
            
            _audioRootsLoop.Stop();
        }
        
    }

    void Plant(PlantColor color, Vector2 position)
    {
        PlayerSeeds.instance.DeductPlant(color);
        switch(color)
        {
            case PlantColor.Yellow:
                YellowPlant yellowPlant = Instantiate<YellowPlant>(m_yellowPlantPrefab, position, Quaternion.identity);
                yellowPlant.Init();
                break;
            case PlantColor.Blue:
                BluePlant bluePlant = Instantiate<BluePlant>(m_bluePlantPrefab, position, Quaternion.identity);
                bluePlant.Init();
                break;
            case PlantColor.Red:
                RedPlant redPlant = Instantiate<RedPlant>(m_redPlantPrefab, position, Quaternion.identity);
                redPlant.Init();
                break;
        }

    }

    void GeneratePlantPositions()
    {
        m_plantLocations.Clear();
        m_plantRootCount.Clear();
        m_extraRootCount.Clear();
        m_randomRootLocations.Clear();

        var rootsInArea = m_rootCheck.IsTouchingLayers(m_rootLayer);

        var plantsInArea = Physics2D.CircleCastAll(transform.position, m_plantRadius, Vector2.zero, 0, m_plantLayer);

        // Check if can plant at position
        // Check if how many plants in radius
        // Collect Possible Plant Locations

        m_canPlant = true;
        if(rootsInArea)
        {
            m_canPlant = false;
            return;
        }

        var maxTries = 100;
        var tries = 0;

        float minRadius = 2;

        List<PlantColor> available = PlayerSeeds.instance.GetAvailableSeedColors();
        m_seedsToPlant = available;
        m_maxPlantInRadius = available.Count;
        int randomRootCount = Random.Range(1,4);
        while (tries < maxTries && m_plantLocations.Count < m_maxPlantInRadius)
        {
            float randomRadius = Random.Range(minRadius, m_plantRadius);
            Vector2 randPos = (Vector2)transform.position + (Random.insideUnitCircle * randomRadius);
            if (!Physics2D.BoxCast(randPos, Vector2.one * 1.5f, 0, Vector2.zero, 0, m_rootLayer))
            {
                var possible = true;
                for (int i = 0; i < m_plantLocations.Count; i++)
                {
                    if ((m_plantLocations[i] - randPos).magnitude < 3)
                    {
                        possible = false;
                        break;
                    }
                }
                if (possible)
                {
                    m_plantLocations.Add(randPos);
                    m_plantRootCount.Add(0);

                }
            }

            tries++;
        }

        for(int i = 0; i < randomRootCount; i++)
        {
            float randomRadius = Random.Range(minRadius - 1, m_plantRadius - 2);
            Vector2 randPos = (Vector2)transform.position + (Random.insideUnitCircle * randomRadius);
            m_randomRootLocations.Add(randPos);
            m_extraRootCount.Add(0);
        }
    }
}
