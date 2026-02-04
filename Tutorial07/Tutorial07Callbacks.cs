using UnityEngine;
using Unity.Tutorials.Core.Editor;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Implement your Tutorial callbacks here.
/// </summary>
[CreateAssetMenu(fileName = DefaultFileName, menuName = "Tutorials/" + DefaultFileName + " Instance")]
public class Tutorial07Callbacks : ScriptableObject
{
    public const string DefaultFileName = "Tutorial07Callbacks";

    public static ScriptableObject CreateInstance()
    {
        return ScriptableObjectUtils.CreateAsset<Tutorial07Callbacks>(DefaultFileName);
    }

    public static bool playPressedThisTutorialPage = false;

    public void ResetPlayDetection()
    {
        //Debug.Log("Resetting Play Detector");
        playPressedThisTutorialPage = false;
    }

    public void DetectPlayBeingPressed()
    {
        if (Application.isPlaying)
        {
            //Debug.Log("Detected Play Mode");
            playPressedThisTutorialPage = true;
        }
    }

    public bool HasPlayBeenPressed()
    {
        return playPressedThisTutorialPage;
    }

    // Helper for case-insensitive GameObject finding
    GameObject FindObject(string name)
    {
        var all = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var go in all)
        {
            if (go.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                return go;
        }
        return null;
    }

    public ComponentType GetComponentOnObject<ComponentType>(string onObject) where ComponentType : Component
    {
        var go = FindObject(onObject);
        if (go)
        {
            var c = go.GetComponent<ComponentType>();
            if (c)
            {
                return c;
            }
            Criterion.globalLastKnownError = $"The object <go>{onObject}</go> is missing a {typeof(ComponentType).Name} component.";
            return null;
        }
        Criterion.globalLastKnownError = $"Scene is missing a GameObject named <go>{onObject}</go>.";
        return null;
    }

    public BuoyancyEffector2D GetWaterEffector()
    {
        return GetComponentOnObject<BuoyancyEffector2D>("Water");
    }
    public AreaEffector2D GetAntiGravEffector()
    {
        return GetComponentOnObject<AreaEffector2D>("Antigravity");
    }

    public bool A2Water()
    {
        if (FindObject("Water") != null) return true;
        Criterion.globalLastKnownError = "Scene is missing a GameObject named <go>Water</go>.";
        return false;
    }
    public bool A2WaterCollider()
    {
        return GetComponentOnObject<BoxCollider2D>("Water") != null;
    }
    public bool A2WaterColor()
    {
        var sr = GetComponentOnObject<SpriteRenderer>("Water");
        if (sr)
        {
            if (sr.color.r.Equals(0) && sr.color.g.Equals(0) && sr.color.b.Equals(1) && sr.color.a <= 0.6f)
                return true;
            
            Criterion.globalLastKnownError = "The <go>Water</go> sprite needs to be blue (R=0, G=0, B=1) and partially transparent (Alpha <= 0.6 or 150).";
            return false;
        }
        return false;
    }

    public bool A2WaterPosition()
    {
        var water = FindObject("Water");
        if (water)
        {
            if (Vector3.Distance(water.transform.position, new Vector2(92, -36)) < 15f)
                return true;

            Criterion.globalLastKnownError = $"The <go>Water</go> is currently at {water.transform.position}, but should be near (92, -36).";
            return false;
        }
        Criterion.globalLastKnownError = "Scene is missing a GameObject named <go>Water</go>.";
        return false;
    }


    public bool A2WaterScale()
    {
        var water = FindObject("Water");
        if (water)
        {
            var tallEnough = water.transform.localScale.y > 5f;
            var wideEnough = water.transform.localScale.x > 20f;
            if (tallEnough && wideEnough)
                return true;

            Criterion.globalLastKnownError = $"The <go>Water</go> should be larger. Current scale is {water.transform.localScale}, try making X > 20 and Y > 5.";
            return false;
        }
        Criterion.globalLastKnownError = "Scene is missing a GameObject named <go>Water</go>.";
        return false;
    }

    public bool A4Effector()
    {
        return GetWaterEffector() != null;
    }
    public bool A4ColliderUsedByEffector()
    {
        var collider = GetComponentOnObject<Collider2D>("Water");
        if (collider)
        {
            if (collider.usedByEffector)
                return true;
            
            Criterion.globalLastKnownError = "The <go>Water</go>'s BoxCollider2D does not have 'Used By Effector' checked.";
            return false;
        }
        return false;
    }
    public bool A4ColliderIsTrigger()
    {
        var collider = GetComponentOnObject<Collider2D>("Water");
        if (collider)
        {
            if (collider.isTrigger)
                return true;

            Criterion.globalLastKnownError = "The <go>Water</go>'s BoxCollider2D does not have 'Is Trigger' checked.";
            return false;
        }
        return false;
    }

    public bool A6WaterLevel()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            if (Mathf.Approximately(effector.surfaceLevel, 2.56f))
                return true;
            
            Criterion.globalLastKnownError = $"The <go>Water</go>'s BuoyancyEffector2D Surface Level is {effector.surfaceLevel}, but should be approximately 2.56.";
            return false;
        }
        return false;
    }
    public bool A8Density()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            if (Mathf.Approximately(effector.density, 0.5f))
                return true;

            Criterion.globalLastKnownError = $"The <go>Water</go>'s BuoyancyEffector2D Density is {effector.density}, but should be 0.5.";
            return false;
        }
        return false;
    }
    public bool A9Drag()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            if (Mathf.Approximately(effector.linearDamping, 0.1f) && Mathf.Approximately(effector.angularDamping, 1f))
                return true;
            
            Criterion.globalLastKnownError = $"The <go>Water</go>'s BuoyancyEffector2D Linear Drag should be 0.1 (is {effector.linearDamping}) and Angular Drag should be 1 (is {effector.angularDamping}).";
            return false;
        }
        return false;
    }
    public bool A10Flow()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            if (effector.flowMagnitude > 0)
                return true;

            Criterion.globalLastKnownError = "The <go>Water</go>'s BuoyancyEffector2D Flow Magnitude should be greater than 0.";
            return false;
        }
        return false;
    }
    public bool A10CarPosition()
    {
        var car = FindObject("CarBody");
        if (car)
        {
            if (car.transform.position.x > 150)
                return true;
            
            Criterion.globalLastKnownError = "The <go>CarBody</go> has not moved far enough to the right (X > 150). Ensure the Water Flow is pushing it.";
            return false;
        }
        Criterion.globalLastKnownError = "Scene is missing a GameObject named <go>CarBody</go>.";
        return false;
    }


    public bool B1AntiGrav()
    {
        if (FindObject("Antigravity") != null) return true;
        Criterion.globalLastKnownError = "Scene is missing a GameObject named <go>Antigravity</go>.";
        return false;
    }
    public bool B1AntiGravCollider()
    {
        return GetComponentOnObject<BoxCollider2D>("Antigravity") != null;
    }
    public bool B1AntiGravColor()
    {
        var sr = GetComponentOnObject<SpriteRenderer>("Antigravity");
        if (sr)
        {
            if (sr.color.r.Equals(1) && sr.color.g.Equals(0) && sr.color.b.Equals(0) && sr.color.a <= 0.6f)
                return true;
            
            Criterion.globalLastKnownError = "The <go>Antigravity</go> sprite needs to be red (R=1, G=0, B=0) and partially transparent (Alpha <= 0.6 or 150).";
            return false;
        }
        return false;
    }

    public bool B1AntiGravPosition()
    {
        var antigrav = FindObject("Antigravity");
        if (antigrav)
        {
            if (Vector3.Distance(antigrav.transform.position, new Vector2(265, 10)) < 15f)
                return true;

            Criterion.globalLastKnownError = $"The <go>Antigravity</go> object is at {antigrav.transform.position}, but should be near (265, 10).";
            return false;
        }
        Criterion.globalLastKnownError = "Scene is missing a GameObject named <go>Antigravity</go>.";
        return false;
    }


    public bool B1AntiGravScale()
    {
        var antigrav = FindObject("Antigravity");
        if (antigrav)
        {
            var tallEnough = antigrav.transform.localScale.y > 5f;
            var wideEnough = antigrav.transform.localScale.x > 20f;
            if (tallEnough && wideEnough)
                return true;
            
            Criterion.globalLastKnownError = $"The <go>Antigravity</go> should be larger. Current scale is {antigrav.transform.localScale}, try making X > 20 and Y > 5.";
            return false;
        }
        Criterion.globalLastKnownError = "Scene is missing a GameObject named <go>Antigravity</go>.";
        return false;
    }

    public bool B2Effector()
    {
        return GetAntiGravEffector() != null;
    }
    public bool B2EffectorSettings()
    {
        var effector = GetAntiGravEffector();
        if (effector)
        {
            if (effector.forceAngle.Equals(90) && effector.forceMagnitude.Equals(15) && effector.forceVariation.Equals(0) && effector.forceTarget.Equals(EffectorSelection2D.Rigidbody))
                return true;
            
            Criterion.globalLastKnownError = $"Check <go>Antigravity</go>'s AreaEffector2D settings: Force Angle=90 (is {effector.forceAngle}), Magnitude=15 (is {effector.forceMagnitude}), Variation=0 (is {effector.forceVariation}), Target=Rigidbody.";
            return false;
        }
        return false;
    }
    public bool B2ColliderUsedByEffectorAndIsTrigger()
    {
        var collider = GetComponentOnObject<Collider2D>("Antigravity");
        if (collider)
        {
            bool usedByEffector = collider.usedByEffector;
            bool isTrigger = collider.isTrigger;
            
            if (usedByEffector && isTrigger)
                return true;

            if (!usedByEffector) Criterion.globalLastKnownError = "The <go>Antigravity</go> Collider must have 'Used By Effector' checked.";
            else if (!isTrigger) Criterion.globalLastKnownError = "The <go>Antigravity</go> Collider must have 'Is Trigger' checked.";
            return false;
        }
        return false;
    }

    public bool C1CurvesSelected()
    {
        if (Selection.gameObjects.Length == 10)
        {
            for (var i = 0; i < Selection.gameObjects.Length; i++)
            {
                if (Selection.gameObjects[i].name.StartsWith("Curve", System.StringComparison.OrdinalIgnoreCase) == false)
                {
                    Criterion.globalLastKnownError = "Please select only the 10 'Curve' objects.";
                    return false;
                }
            }
            return true;
        }
        Criterion.globalLastKnownError = $"Please select exactly 10 objects (the Curves). You currently have {Selection.gameObjects.Length} selected.";
        return false;
    }
    public List<SurfaceEffector2D> GetCurveEffectors()
    {
        var lst = new List<SurfaceEffector2D>();
        for (var i = 1; i <= 10; i++)
        {
            var effector = GetComponentOnObject<SurfaceEffector2D>("Curve " + i);
            if (effector)
            {
                lst.Add(effector);
            }
        }
        return lst;
    }
    public bool C2Effectors()
    {
        var lst = GetCurveEffectors();
        if (lst.Count == 10)
        {
            for (var i = 0; i < lst.Count; i++)
            {
                var effector = lst[i];
                if (effector.speed.Equals(50) == false/* || effector.speedVariation.Equals(0) == false || Mathf.Approximately(effector.forceScale, 0.1f) == false*/)
                {
                     Criterion.globalLastKnownError = $"The object <go>{effector.name}</go> has SurfaceEffector2D speed {effector.speed}, but it should be 50.";
                     return false;
                }
            }
            return true;
        }
        if (lst.Count < 10)
             Criterion.globalLastKnownError = $"Found only {lst.Count} SurfaceEffector2D components on the Curves. There should be 10.";
        return false;
    }
    public bool C2ColliderUsedByEffector()
    {
        var lst = GetCurveEffectors();
        if (lst.Count == 10)
        {
            for (var i = 0; i < lst.Count; i++)
            {
                var collider = lst[i].gameObject.GetComponent<Collider2D>();
                if (collider.usedByEffector == false)
                {
                    Criterion.globalLastKnownError = $"The object <go>{lst[i].name}</go> Collider needs 'Used By Effector' checked.";
                    return false;
                } 
                if (collider.isTrigger == true)
                {
                    Criterion.globalLastKnownError = $"The object <go>{lst[i].name}</go> Collider should NOT be a trigger.";
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
