using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using UWE;

namespace MonkeySayMonkeyGet;

public static class Utils
{
    public static bool StringContainsAllWords(string original, string comparison)
    {
        return StringContainsAllWords(original, comparison.Split(' '));
    }

    public static bool StringContainsAllWords(string original, string[] comparison) // original is the whole sentence "I went to the Safe Shallows" and comparison is the words you're looking for (comparison MUST be in the SINGULAR form)
    {
        var originalWords = original.Split(' ');
        foreach (var comparing in comparison)
        {
            bool contains = false;
            foreach (var word in originalWords)
            {
                if (ComparePluralOrSingular(word, comparing))
                {
                    contains = true;
                }
            }
            if (!contains)
            {
                return false;
            }
        }
        return true;
    }

    public static bool ComparePluralOrSingular(string text, string singular) // 'text' will be either plural ("Peepers") or singular ("Peeper). 'singular' will always be singular ("Peeper")
    {
        if (text == singular)
        {
            return true;
        }
        if (text == singular + "s" && singular.Length > 2)
        {
            return true;
        }
        return false;
    }

    public static AudioSource PlaySoundEffect(string clipName, float duration)
    {
        var source = new GameObject("MonkeyAudioSource").AddComponent<AudioSource>();
        source.clip = Plugin.AssetBundle.LoadAsset<AudioClip>(clipName);
        source.Play();
        source.volume = Plugin.ModConfig.SoundVolume / 100f;
        Object.Destroy(source.gameObject, duration);
        return source;
    }

    public static void ZaWarudoVFX(bool timeFrozen, bool isJotaro)
    {
        if (timeFrozen)
        {
            if (!isJotaro)
            {
                Time.timeScale = 0f;
            }
        }
        else
        {
            Time.timeScale = 1f;
        }
        if (isJotaro)
        {
            if (timeFrozen)
            {
                var newSphere = Object.Instantiate(stasisSpherePrefab);
                newSphere.SetActive(true);
                currentSphere = newSphere.GetComponent<StasisSphere>();
                currentSphere.transform.position = Player.main.transform.position + new Vector3(20f, 20f, 20f);
                currentSphere.radius = 1000f;
                currentSphere.time = 500f;
                currentSphere.fieldEnergy = 1;
                currentSphere.EnableField();
            }
            else if (currentSphere)
            {
                currentSphere.CancelAll();
                Object.Destroy(currentSphere.gameObject);
            }
        }
        var grayscale = MainCamera.camera.GetComponent<Grayscale>();
        grayscale.effectAmount = timeFrozen ? 1.3f : 0f;
        if (timeFrozen) grayscale.enabled = true;
        timeStopActive = timeFrozen;
    }

    private const int pluralAmount = 10;
    private const int allAmount = 300;

    public static List<GameObject> FindReferencedObjects(SpeechInput input, float maxDistance = float.MaxValue)
    {
        var mentionedTechType = PhraseManager.GetReferencedTechType(input, out Amount amount);
        int limit = GetObjectLimitForAmount(amount);
        if (amount == Amount.LiterallyEverything)
        {
            mentionedTechType = TechType.None;
            limit = 2000;
        }
        return FindObjectsByTechType(mentionedTechType, limit, maxDistance); 
    }

    public static int GetObjectLimitForAmount(Amount amount)
    {
        int limit = 1;
        if (amount == Amount.Plural)
        {
            limit = pluralAmount;
        }
        if (amount == Amount.All)
        {
            limit = allAmount;
        }
        if (amount == Amount.LiterallyEverything)
        {
            limit = allAmount;
        }
        return limit;
    }

    private const float minDotForSingularReference = 0.5f;
    private const float maxDistanceForLimitedTechTypeReference = 200f;

    public static List<GameObject> FindObjectsByTechType(TechType techType, int limit, float maxDistance = float.MaxValue)
    {
        var linkedTechTypes = PhraseManager.TechTypeData.GetLinkedTechTypes(techType);
        var list = new List<GameObject>();
        var camPosition = MainCameraControl.main.transform.position;
        var singular = limit == 1;
        var notReferringToAll = limit < allAmount;

        bool IsValid(Transform transform)
        {
            var dist = Vector3.Distance(camPosition, transform.transform.position);
            if (notReferringToAll)
            {
                if (dist > maxDistanceForLimitedTechTypeReference)
                {
                    return false;
                }
            }
            if (dist > maxDistance)
            {
                return false;
            }
            if (singular)
            {
                var dot = PointOnScreenDot(transform.position);
                if (dot < minDotForSingularReference)
                {
                    return false;
                }
            }
            var lm = transform.gameObject.GetComponent<LiveMixin>();
            if (lm && !lm.IsAlive())
            {
                return false;
            }
            return true;
        }

        bool MatchingTechType(GameObject searchedObject)
        {
            var searchedObjectTechType = CraftData.GetTechType(searchedObject);
            if (linkedTechTypes.ContainsTechType(searchedObjectTechType) || techType == TechType.None)
            {
                return true;
            }
            if (techType == TechType.Cyclops)
            {
                var subroot = searchedObject.GetComponent<SubRoot>();
                if (subroot != null && subroot.isCyclops)
                {
                    return true;    
                }
            }
            return false;
        }

        var cellManager = LargeWorld.main.streamer.cellManager;

        foreach (KeyValuePair<Int3, BatchCells> keyValuePair in cellManager.batch2cells)
        {
            foreach (EntityCell entityCell in keyValuePair.Value.All())
            {
                if (entityCell.liveRoot != null)
                {
                    foreach (Transform child in entityCell.liveRoot.transform)
                    {
                        if (MatchingTechType(child.gameObject))
                        {
                            if (!IsValid(child))
                            {
                                continue;
                            }
                            list.Add(child.gameObject);
                            if (list.Count >= limit)
                            {
                                return list;
                            }
                        }
                    }
                }
            }
        }

        foreach (Transform child in GetGlobalRoot())
        {
            if (MatchingTechType(child.gameObject))
            {
                if (!IsValid(child))
                {
                    continue;
                }
                list.Add(child.gameObject);
                if (list.Count >= limit)
                {
                    return list;
                }
            }
        }
        return list;
    }

    public static Transform GetLandscapeRoot()
    {
        return LargeWorld.main.transform.parent;
    }

    public static Transform GetGlobalRoot()
    {
        return GetLandscapeRoot().Find("Global Root");
    }

    public static float PointOnScreenDot(Vector3 point)
    {
        var player = Player.main.viewModelCamera.transform;
        Vector3 direction = (point - player.position).normalized;
        return Vector3.Dot(direction, player.forward);
    }

    public static bool CannotDropItems()
    {
        return PlayerInsideStructure();
    }

    public static bool PlayerInsideStructure()
    {
        return Player.main.GetCurrentSub() != null || Player.main.GetVehicle() != null || Player.main.currentEscapePod != null;
    }

    public static bool PlayerInLifepod()
    {
        return Player.main.currentEscapePod != null;
    }

    public static void PlayCredits()
    {
        SceneManager.LoadSceneAsync("EndCreditsSceneCleaner", LoadSceneMode.Single);
    }

    public static FMODAsset GetFMODAsset(string path)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = path;
        return asset;
    }

    private static GameObject stasisSpherePrefab;
    private static StasisSphere currentSphere;
    public static bool timeStopActive;

    public static IEnumerator ZaWarudo()
    {
        if (stasisSpherePrefab == null)
        {
            var stasisRifleTask = CraftData.GetPrefabForTechTypeAsync(TechType.StasisRifle);
            yield return stasisRifleTask;
            var stasisRifle = stasisRifleTask.GetResult();
            stasisSpherePrefab = stasisRifle.GetComponent<StasisRifle>().effectSpherePrefab;
        }
        yield return ZaWarudoCoroutine(VoiceCommands.StarPlatinum.StarPlatinumActivated);
    }

    private static IEnumerator ZaWarudoCoroutine(bool isJotaro)
    {
        Utils.ZaWarudoVFX(true, isJotaro);
        Utils.PlaySoundEffect("TimeStop", 10f);
        Mono.ZaWarudoBall.PlayVFX(Player.main.transform.position);
        var time = 5f;
        yield return new WaitForSecondsRealtime(time);
        Utils.ZaWarudoVFX(false, isJotaro);
    }

    public static void MoveSubjectToPosition(Subject subject, Vector3 position, float random = 0f)
    {
        // exit if invalid
        if (subject.gameObject == null && subject.gameObjectArray == null)
        {
            return;
        }

        var isPlayer = subject.type == Subject.Type.Self;

        if (subject.type == Subject.Type.OtherArray)
        {
            foreach (var go in subject.gameObjectArray)
            {
                if (go)
                {
                    go.transform.position = position + Random.insideUnitSphere * random;
                }
            }
        }
        else
        {
            subject.gameObject.transform.position = position + Random.insideUnitSphere * random;
        }

        // fixes for player
        if (isPlayer)
        {
            Player.main.SetPosition(position + Random.insideUnitSphere * random);
        }
        if (isPlayer && !Player.main.precursorOutOfWater && !PlayerInsideStructure())
        {
            Player.main.OnPlayerPositionCheat();
        }
        Player.main.SetPrecursorOutOfWater(false);
    }

    public static void MoveSubjectInDirection(Subject subject, Vector3 direction)
    {
        var playerPosBefore = Player.main.transform.position;

        // exit if invalid
        if (subject.gameObject == null && subject.gameObjectArray == null)
        {
            return;
        }

        var isPlayer = subject.type == Subject.Type.Self;

        if (subject.type == Subject.Type.OtherArray)
        {
            foreach (var go in subject.gameObjectArray)
            {
                if (go)
                {
                    go.transform.position += direction;
                }
            }
        }
        else
        {
            subject.gameObject.transform.position += direction;
        }

        // fixes for player
        if (isPlayer)
        {
            Player.main.SetPosition(playerPosBefore + direction);
        }
        if (isPlayer && !Player.main.precursorOutOfWater)
        {
            Player.main.OnPlayerPositionCheat();
        }
        Player.main.SetPrecursorOutOfWater(false);
    }

    public static void SetSubjectYValue(Subject subject, float yValue)
    {
        // exit if invalid
        if (subject.gameObject == null && subject.gameObjectArray == null)
        {
            return;
        }

        var isPlayer = subject.type == Subject.Type.Self;

        if (subject.type == Subject.Type.OtherArray)
        {
            foreach (var go in subject.gameObjectArray)
            {
                if (go)
                {
                    go.transform.position = new Vector3(go.transform.position.x, yValue, go.transform.position.z);
                }
            }
        }
        else
        {
            subject.gameObject.transform.position = new Vector3(subject.gameObject.transform.position.x, yValue, subject.gameObject.transform.position.z);
        }

        // fixes for player
        if (isPlayer)
        {
            Player.main.SetPosition(new Vector3(Player.main.transform.position.x, yValue, Player.main.transform.position.z));
        }
        if (isPlayer && !Player.main.precursorOutOfWater && !PlayerInsideStructure())
        {
            Player.main.OnPlayerPositionCheat();
        }
        Player.main.SetPrecursorOutOfWater(false);
    }

    public static void ApplySNShaders(GameObject prefab, float shininess, float specInt, float illumStrength)
    {
        var renderers = prefab.GetComponentsInChildren<Renderer>(true);
        var newShader = Shader.Find("MarmosetUBER");
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                Material material = renderers[i].materials[j];
                Texture specularTexture = material.GetTexture("_SpecGlossMap");
                Texture emissionTexture = material.GetTexture("_EmissionMap");
                material.shader = newShader;

                material.DisableKeyword("_SPECGLOSSMAP");
                material.DisableKeyword("_NORMALMAP");
                if (specularTexture != null)
                {
                    material.SetTexture("_SpecTex", specularTexture);
                    material.SetFloat("_SpecInt", specInt);
                    material.SetFloat("_Shininess", shininess);
                    material.EnableKeyword("MARMO_SPECMAP");
                    material.SetColor("_SpecColor", new Color(1f, 1f, 1f, 1f));
                    material.SetFloat("_Fresnel", 0.24f);
                    material.SetVector("_SpecTex_ST", new Vector4(1.0f, 1.0f, 0.0f, 0.0f));
                }
                if (material.IsKeywordEnabled("_EMISSION"))
                {
                    material.EnableKeyword("MARMO_EMISSION");
                    material.SetFloat("_EnableGlow", 1f);
                    material.SetTexture("_Illum", emissionTexture);
                    material.SetFloat("_GlowStrength", illumStrength);
                    material.SetFloat("_GlowStrengthNight", illumStrength);
                }

                if (material.GetTexture("_BumpMap"))
                {
                    material.EnableKeyword("MARMO_NORMALMAP");
                }

                /*if (CompareStrings(material.name, "Cutout", ECCStringComparison.Contains))
                {
                    material.EnableKeyword("MARMO_ALPHA_CLIP");
                }
                if (CompareStrings(material.name, "Transparent", ECCStringComparison.Contains))
                {
                    material.EnableKeyword("_ZWRITE_ON");
                    material.EnableKeyword("WBOIT");
                    material.SetInt("_ZWrite", 0);
                    material.SetInt("_Cutoff", 0);
                    material.SetFloat("_SrcBlend", 1f);
                    material.SetFloat("_DstBlend", 1f);
                    material.SetFloat("_SrcBlend2", 0f);
                    material.SetFloat("_DstBlend2", 10f);
                    material.SetFloat("_AddSrcBlend", 1f);
                    material.SetFloat("_AddDstBlend", 1f);
                    material.SetFloat("_AddSrcBlend2", 0f);
                    material.SetFloat("_AddDstBlend2", 10f);
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack | MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    material.renderQueue = 3101;
                    material.enableInstancing = true;
                }*/
            }
        }
    }

    public static void RemovePrefabComponents(GameObject go)
    {
        Object.Destroy(go.GetComponent<PrefabIdentifier>());
        Object.Destroy(go.GetComponent<LargeWorldEntity>());
    }
}
