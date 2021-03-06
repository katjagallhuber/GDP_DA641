using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerHitPoints : MonoBehaviour
{
    [SerializeField] private int hitPoints;
    [SerializeField] private bool IsAdded = true;

    private int plantsLayer = 9;
    private int animalLayer = 8;
    private int puppetArmsLayer = 6;
    private int puppetLegsLayer = 7;
    private int farmerLayer = 11;
    private int puppetLayer = 10;

    private List<Material> materials;
    private AudioSource source;

    public static event Action<int, GameObject, bool> OnTouchedObstacle;

    [SerializeField] private Renderer renderer;
    [SerializeField] private Material material; 


    private void Start()
    {
        // default value, can be changed in the inspector individually for each obstacle
        hitPoints = 10;

        if (TryGetComponent(out AudioSource audio))
        {
            source = audio;
            source.playOnAwake = false;
        }

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.mass = 0.1f;
        }

        renderer = gameObject.GetComponentInChildren<Renderer>(); ;
        material = renderer.material;

        AddMaterialsToList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == puppetLayer || other.gameObject.layer == puppetArmsLayer || other.gameObject.layer == puppetLegsLayer)
        {
            ControlEmissionOnHover();
        }

        if (other.gameObject.layer == puppetArmsLayer && this.gameObject.layer == animalLayer)
        {
            OnTouchedObstacle?.Invoke(hitPoints, gameObject, IsAdded);
            if (source)
                source.Play();
        }
        else if (other.gameObject.layer == puppetLegsLayer && this.gameObject.layer == plantsLayer)
        {
            OnTouchedObstacle?.Invoke(hitPoints, gameObject, IsAdded);
            if (source)
                source.Play();
        }
        else if (gameObject.layer == farmerLayer && other.gameObject.layer == puppetLayer || gameObject.layer == farmerLayer && other.gameObject.layer == puppetArmsLayer || gameObject.layer == farmerLayer && other.gameObject.layer == puppetLegsLayer)
        {
            OnTouchedObstacle?.Invoke(hitPoints, gameObject, IsAdded);
            if (source)
                source.Play();
        }
    }

    // Control the emission of the object 
    private void ControlEmissionOnHover()
    {
        foreach (Material m in materials)
        {
            m.EnableKeyword("_EMISSION");
            m.EnableKeyword("Emission");

            if (IsAdded)
            {
                m.SetColor("_EmissionColor", new Color(0.0f, 0.5f, 0.0f));
                m.SetColor("_EMISSION_COLOR", new Color(0.0f, 0.5f, 0.0f));
            } else
            {
                m.SetColor("_EmissionColor", new Color(0.5f, 0.0f, 0.0f));
                m.SetColor("_EMISSION_COLOR", new Color(0.5f, 0.0f, 0.0f));
            }

            Debug.Log(gameObject.name + " " + m.name);
        }

        StartCoroutine("ResetMaterials");
    }


    private IEnumerator ResetMaterials()
    {
        yield return new WaitForSeconds(2f);

        foreach (Material m in materials)
        {
            m.DisableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", new Color(0.15f, 0.15f, 0.15f));

            m.DisableKeyword("Emission");
            m.SetColor("_EMISSION_COLOR", new Color(0.0f, 0.0f, 0.0f));
        }
    }

    // Get all materials on the object to control their emission later on
    // Sometimes there are more than one material on an object
    private void AddMaterialsToList()
    {
        materials = new List<Material>();

        if (TryGetComponent(out MeshRenderer renderer))
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                materials.Add(renderer.materials[i]);
                materials[i].EnableKeyword("_EMISSION");
            }

        }

        Renderer[] rendererChildren = this.gameObject.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rendererChildren.Length; i++)
        {
            foreach (Material mChild in rendererChildren[i].materials)
            {
                materials.Add(mChild);
                mChild.EnableKeyword("_EMISSION");
            }
        }
    }

    private void ControlMaterial()
    {
        
        if(IsAdded)
        {
            material.EnableKeyword("Emission");
            material.SetColor("_EmissionColor", new Color(0.0f, 0.5f, 0.0f));
        }
        else
        {
            material.EnableKeyword("Emission");
            material.SetColor("_EmissionColor", new Color(0.5f, 0.0f, 0.0f));
        }
        
    }
}
