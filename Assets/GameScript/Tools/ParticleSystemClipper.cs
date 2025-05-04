using System;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class ParticleSystemClipper : MonoBehaviour
{
    const string ShaderName = "Bleach/Particles Additive Area Clip";
    const string SpineShaderName = "Spine/Skeleton";
    const float ClipInterval = 0.5f;

    UIPanel m_targetPanel;
    Shader m_shader;
    private UIRoot uiRoot;

    private Boolean needClip = true;
    void Start()
    {
        // find panel
        m_targetPanel = GetComponent<UIPanel>();
        uiRoot = GameObject.Find("UI Root").GetComponent<UIRoot>();
        if (uiRoot == null)
            needClip = false;
        if (m_targetPanel == null)
            needClip = false;
        if (m_targetPanel.clipping != UIDrawCall.Clipping.SoftClip)
            needClip = false;
        m_shader = Shader.Find(ShaderName);

        //if (!IsInvoking("Clip"))
            //InvokeRepeating("Clip", 0, ClipInterval);
    }

    Vector4 CalcClipArea()
    {
        Vector4 clipRegion = m_targetPanel.finalClipRegion;
        Vector4 nguiArea = new Vector4()
        {
            x = clipRegion.x - clipRegion.z / 2,
            y = clipRegion.y - clipRegion.w / 2,
            z = clipRegion.x + clipRegion.z / 2,
            w = clipRegion.y + clipRegion.w / 2
        };

        Vector3 pos = m_targetPanel.transform.position - uiRoot.transform.position;
        float h = 2;
        float temp = h / uiRoot.manualHeight;

        return new Vector4()
        {
            x = pos.x + nguiArea.x * temp,
            y = pos.y + nguiArea.y * temp,
            z = pos.x + nguiArea.z * temp,
            w = pos.y + nguiArea.w * temp
        };
    }

    void Clip()
    {
        if (!needClip)
            return;
        ParticleSystem[] particleSystems = this.GetComponentsInChildren<ParticleSystem>();
        SkeletonAnimation[] spines = this.GetComponentsInChildren<SkeletonAnimation>();
        if (particleSystems.Length <= 0 && spines.Length <= 0)
            return;
        Vector4 clipArea = CalcClipArea();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            ParticleSystem ps = particleSystems[i];
            Material mat = ps.transform.GetComponent<Renderer>().material;
            if (mat.shader.name != ShaderName)
                mat.shader = m_shader;
            mat.SetVector("_Area", clipArea);
        }
        for (int i = 0; i < spines.Length; i++)
        {
            SkeletonAnimation skeleton = spines[i];
            Material mat = skeleton.GetComponent<Renderer>().material;
            if (mat.shader.name == SpineShaderName)
            {
                //mat.shader = Shader.Find(SpineShaderName); 
                mat.SetInt("_NEEDCLIP", 1);
                mat.SetVector("_Area", clipArea);
            }
        }
    }

    private void Update()
    {
        Clip();
    }

    void OnDestroy()
    {
        //CancelInvoke("Clip");
    }
}