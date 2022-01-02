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

    private List<Material> materials;

    public static event Action<int, GameObject, bool> OnTouchedObstacle;

    private void Start()
    {
        // default value, can be changed in the inspector individually for each obstacle
        hitPoints = 10;

        //AddMaterialsToList();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == puppetLayer)
        {
            ControlEmissionOnHover();
        }
    }*/

    /// <summary>
    /// Everytime the puppet with layer 10 touches an obstacle, the OnTouchedObstacle Event gets invoked
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == puppetArmsLayer && this.gameObject.layer == animalLayer)
        {
            OnTouchedObstacle?.Invoke(hitPoints, gameObject, IsAdded);
        } else if (other.gameObject.layer == puppetLegsLayer && this.gameObject.layer == plantsLayer)
        {
            OnTouchedObstacle?.Invoke(hitPoints, gameObject, IsAdded);
        } else if (gameObject.layer == farmerLayer)
        {
            OnTouchedObstacle?.Invoke(hitPoints, gameObject, IsAdded);
        }
    }

    // Control the emission of the object 
    private void ControlEmissionOnHover()
    {
        foreach (Material m in materials)
        {
           // m.EnableKeyword("Emission");

            if (IsAdded)
            {
                m.SetColor("_EMISSION_COLOR", new Color(0.0f, 0.8f, 0.0f));
            } else
            {
                m.SetColor("_EMISSION_COLOR", new Color(0.8f, 0.0f, 0.0f));
            }
        }

        StartCoroutine("ResetMaterials");
    }


    private IEnumerator ResetMaterials()
    {
        yield return new WaitForSeconds(2f);

        foreach (Material m in materials)
        {
            //m.DisableKeyword("Emission");
            //m.SetColor("_EmissionColor", new Color(0.15f, 0.15f, 0.15f));
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
            }

        }

        Renderer[] rendererChildren = this.gameObject.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rendererChildren.Length; i++)
        {
            foreach (Material mChild in rendererChildren[i].materials)
            {
                materials.Add(mChild);
            }
        }
    }
}
