using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYPEPOLY_SwordGenerator : MonoBehaviour
{
    [Header("Sword position in world space")]
    public Vector3 positionForNewSword;
    [Header("Prefabs for generation")]
    public List<GameObject> blades;
    public List<GameObject> grips;
    public List<GameObject> pommels;
    public List<GameObject> crossguards;
    [Header("Materials")]
    public List<Material> materials;
    [Header("Gems")]
    [Range(0,100)]
    public int gemChance = 0;
    public List<GameObject> gems;

    [Header("Tap this toggle in play mode to generate")]
    public bool TapToGenerate = false;
    
    void FixedUpdate()
    {
        if(TapToGenerate)
        {
            TapToGenerate = false;
            GenerateSwordByPreset();
        }
    }
    public void GenerateSwordByPreset()
    {
            GenerateSword(positionForNewSword, false, gemChance, null);
    }
    public void GenerateSword(Vector3 _pos, bool _deleteOtherChilds, int _gemsChance, Transform _parent = null)
    {
        if (_parent != null)
        {
            if (_deleteOtherChilds)
            {
                for (int i = _parent.childCount - 1; i >= 0; i--)
                {
                    Destroy(_parent.GetChild(i).gameObject);
                }
            }
        }
        else
        {
            GameObject swordGO = new GameObject();
            swordGO.transform.position = _pos;
            swordGO.name = "HYPEPOLY - Sword";
            _parent = swordGO.transform;
        }
        List<GameObject> swordParts = new List<GameObject>();

        GameObject _blade = Instantiate(blades[Random.Range(0, blades.Count)]);
        _blade.transform.localScale = Vector3.one;
        _blade.transform.parent = _parent;
        _blade.transform.localPosition = Vector3.zero;
        _blade.transform.localRotation = Quaternion.identity;
        swordParts.Add(_blade);

        GameObject _crossguard = Instantiate(crossguards[Random.Range(0, crossguards.Count)]);
        _crossguard.transform.localScale = Vector3.one;
        _crossguard.transform.parent = _parent;
        _crossguard.transform.localPosition = Vector3.zero;
        _crossguard.transform.localRotation = Quaternion.identity;
        swordParts.Add(_crossguard);

        GameObject _grip = Instantiate(grips[Random.Range(0, grips.Count)]);
        _grip.transform.localScale = Vector3.one;
        _grip.transform.parent = _parent;
        _grip.transform.localPosition = new Vector3(0f, -0.1f, 0f);
        _grip.transform.localRotation = Quaternion.identity;
        swordParts.Add(_grip);

        GameObject _pommel = Instantiate(pommels[Random.Range(0, pommels.Count)]);
        _pommel.transform.localScale = Vector3.one;
        _pommel.transform.parent = _parent;
        _pommel.transform.localPosition = new Vector3(0f, -1f, 0f);
        _pommel.transform.localRotation = Quaternion.identity;
        swordParts.Add(_pommel);

        if (_gemsChance < Random.Range(0,100))
        {
            int gemNumber = Random.Range(0, gems.Count);
            GameObject _gem1 = Instantiate(gems[gemNumber]);
            _gem1.transform.localScale = Vector3.one;
            _gem1.transform.parent = _parent;
            _gem1.transform.localPosition = new Vector3(0f, 0.265f, -0.08f);
            _gem1.transform.localRotation = Quaternion.identity;
            swordParts.Add(_gem1);

            GameObject _gem2 = Instantiate(gems[gemNumber]);
            _gem2.transform.localScale = Vector3.one;
            _gem2.transform.parent = _parent;
            _gem2.transform.localPosition = new Vector3(0f, 0.265f, 0.08f);
            _gem2.transform.localRotation = Quaternion.identity;
            swordParts.Add(_gem2);
        }

        int materialNumber = Random.Range(0, materials.Count);
        foreach(GameObject go in swordParts)
        {
            go.GetComponent<MeshRenderer>().material = materials[materialNumber];
        }
    }
}
