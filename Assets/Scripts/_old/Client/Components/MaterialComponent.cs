using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;


[GenerateAuthoringComponent]
public class MaterialComponent : IComponentData
{
    public UnityEngine.Material material;
}
