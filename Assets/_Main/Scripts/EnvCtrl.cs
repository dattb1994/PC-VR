using Ignis;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public class EnvCtrl : Singleton<EnvCtrl>
{
    public FlamePerMapStr flameConfig;
    public ExtinguisEquipStr equipStr;

    public Transform playerRoot;
    [HideInInspector] public List<FlammableObject> flames;
    [HideInInspector] public Transform door;
    [HideInInspector] public Transform startFire;
    public float maxTimePlay;
    IEnumerator Start()
    {
        Scene mainScene = SceneManager.GetSceneByName("Main");
        if (mainScene.isLoaded)
        {
            SceneManager.MoveGameObjectToScene(gameObject, mainScene);
            //Debug.Log($"Moved {go.name} to MainScene");
        }

        yield return new WaitForSeconds(.1f);

        var testMode = GameConfig.TestConfig;
        playerRoot = new GameObject("playerRoot").transform;
        playerRoot.parent = transform;
        playerRoot.gameObject.SetActive(false);
        door = transform.Find("door");
        startFire = transform.Find("start fire");

        float disDoorToFire = Vector3.Distance(door.position, startFire.position);
        Vector3 posPlayer = door.position;
        Vector3 direction = (door.position - startFire.position).normalized; // Tính hướng từ B đến A
        float disDefaut = ((int)GameSetting.typeEquipment == 3) ? 9 : 3;

        if (disDoorToFire > disDefaut)
        {
            posPlayer = startFire.position + direction * disDefaut;
        }

        playerRoot.position = posPlayer;
        playerRoot.LookAt(new Vector3(startFire.position.x, playerRoot.position.y, startFire.position.z));

        Instantiate(Resources.Load<GameObject>("door 2d"), door);

        foreach (Renderer ren in transform.GetComponentsInChildren<Renderer>())
        {
            if (ren.name == "Wall")
            {
                ren.enabled = testMode.showWall;
                ren.AddComponent<MeshCollider>();
            }
        }

        int level = GameSetting.level;

        flames = new List<FlammableObject>();

        int countFlame = 0;
        foreach (FlammableObject flame in transform.GetComponentsInChildren<FlammableObject>())
        {
            if (flame.gameObject.name != "Fire point")
            {
                if (flame.GetComponent<PointStartFire>() == null)
                {
                    flame.gameObject.SetActive(false);

                    flames.Add(flame);

                    //flame.overrideFireVFX = true;
                    //flame.customFireSFX = GameManager.Instance.fireVFX;
                    if (flame.gameObject.GetComponent<Collider>() == null)
                    {
                        if (flameConfig.colliderIsBox)
                        {
                            flame.gameObject.AddComponent<BoxCollider>();
                        }
                        else
                        {
                            var col = flame.gameObject.AddComponent<MeshCollider>();
                            col.convex = true;
                        }
                    }

                    ///Độ lớn ban đầu của ngọn lửa
                    flame.fireCrawlSpeed = flameConfig.firstCrawl + (level - 1) * (flameConfig.crawl) * flameConfig.crawlPerLv;

                    flame.isEffectOnForever = false;
                    flame.ignitionTime = 0;

                    Vector2 timeBurnLength = GameConfig.FlameConfig.clampTimeBurnLength;

                    maxTimePlay = timeBurnLength.x + (10 - (float)level) * (timeBurnLength.y - timeBurnLength.x) / 10;

                    flame.burnOutStart_s = 1000;
                    flame.burnOutLength_s = 5;

                    flame.useMeshFire = true;
                    flame.meshFireCount = GameConfig.FlameConfig.smokeConfig.meshFireCount;
                    flame.meshFireMeshFilters.Add(flame.GetComponent<MeshFilter>());

                    flame.flameLength = 1;
                    flame.flameParticleSize = 1;
                    flame.enableMaterialAnimation = false;

                    flame.enableMaterialAnimation = true;

                    if (countFlame == 0)
                    {
                        flame.setThisOnFireOnStart = true;
                        flame.customFireOriginOnStart = startFire;
                    }

                    flame.fullExtinguishToughness = flameConfig.fullExtinguishToughness;

                    flame.isReignitable = FlammableObject.ReIgnitable.No;

                    //flame.shaderToBurntInterpolateSpeed = 0;
                    //flame.shaderColorNoiseSpeed = 0;
                    //flame.shaderColorNoise = 0;

                    flame.enableMaterialAnimation = false;

                    FlameConfig flameSetting = GameConfig.FlameConfig;

                    flame.flameVFXMultiplier = flameSetting.smokeConfig.flameVFXMultiplier;
                    flame.flameParticleSize = flameSetting.smokeConfig.flameParticleSize;
                    flame.flameLength = flameSetting.smokeConfig.flameLength;

                    flame.smokeColorIntensity = flameSetting.smokeConfig.colorIntensity;
                    flame.smokeAlpha = flameSetting.smokeConfig.alpha;
                    flame.smokeVFXMultiplier = flameSetting.smokeConfig.vfxMultiplier;
                    flame.smokeParticleSize = flameSetting.smokeConfig.particleSize;
                    flame.smokeColor = flameSetting.smokeConfig.smokeColor;

                    flame.embersVFXMultiplier = flameSetting.smokeConfig.embersVFXMultiplier;
                    flame.embersBurstVFXMultiplier = flameSetting.smokeConfig.embersBurstVFXMultiplier;
                    flame.embersParticleSize = flameSetting.smokeConfig.embersParticleSize;

                    flame.customFireSFX = GameConfig.FlameConfig.customFireSFX;

                    flame.gameObject.SetActive(true);

                }
                else
                {
                    StartCoroutine(TryStopFire(flame));
                }
                countFlame++;
            }
            else
            {
                DestroyImmediate(flame);
            }
        }
        StartCoroutine(SetFireCraw());
        EventManager.StartListening(EventKey.OnFlashover.ToString(), ()=> StartCoroutine(DoOnFlashover()));
    }
    IEnumerator SetFireCraw()
    {
        yield return new WaitForSeconds(.5f);
        foreach (var item in flames)
        {
            item.fireCrawlSpeed = flameConfig.crawl;
        }
    }
    IEnumerator TryStopFire(FlammableObject flame)
    {
        flame.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        flame.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        flame.gameObject.SetActive(false);// = false;
    }
    IEnumerator DoOnFlashover()
    {
        foreach (var item in flames)
        {
            item.fireCrawlSpeed = flameConfig.crawl * flameConfig.flashover;// + 3 * GameConfig.FlameConfig.deltaWithLevel * GameConfig.FlameConfig.fireCrawlSpeedDelta;
        }
        yield return new WaitForSeconds(.5f);
        foreach (var item in flames)
        {
            item.fireCrawlSpeed = flameConfig.crawl;
        }
    }
    public bool IsExtinguished()
    {
        return FlameEngine.instance.transform.GetChild(0).childCount == 0;
    }
    public Transform Flame()
    {
        return flames[0].transform;
    }
    [System.Serializable]
    public struct FlamePerMapStr
    {
        [Tooltip("Hằng số cháy lan mặc định mét/giây")]
        public float crawl;

        [Tooltip("Độ lớn ban đầu của ngọn lửa mét/giây")]
        public float firstCrawl;

        [Tooltip("Hệ số tăng lên theo level")]
        public float crawlPerLv;

        [Tooltip("Tốc độ dập tắt đám cháy của hiệu ứng")]
        public float fullExtinguishToughness;

        [Tooltip("Độ phùng lên khi flashover")]
        public float flashover;

        public bool colliderIsBox;
    }

    [System.Serializable]
    public struct EquipFixStr
    {
        public float radius;
        public float power;
    }

    [System.Serializable]
    public struct ExtinguisEquipStr
    {
        [Tooltip("Bình c02")]
        public EquipFixStr co2;

        [Tooltip("Bình bột")]
        public EquipFixStr flour;

        [Tooltip("Bình nước")]
        public EquipFixStr water;

        [Tooltip("lăng chữa cháy")]
        public EquipFixStr steering;
    }
}
