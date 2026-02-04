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

    public ComponentType GetComponentOnObject<ComponentType>(string onObject) where ComponentType : Component
    {
        var go = GameObject.Find(onObject);
        if (go)
        {
            var c = go.GetComponent<ComponentType>();
            if (c)
            {
                return c;
            }
        }
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
        return GameObject.Find("Water");
    }
    public bool A2WaterCollider()
    {
        return GetComponentOnObject<BoxCollider2D>("Water");
    }
    public bool A2WaterColor()
    {
        var sr = GetComponentOnObject<SpriteRenderer>("Water");
        if (sr)
        {
            return sr.color.r.Equals(0) && sr.color.g.Equals(0) && sr.color.b.Equals(1) && sr.color.a <= 0.6f;
        }
        return false;
    }

    public bool A2WaterPosition()
    {
        var water = GameObject.Find("Water");
        if (water)
        {
            return Vector3.Distance(water.transform.position, new Vector2(92, -36)) < 15f;
        }
        return false;
    }


    public bool A2WaterScale()
    {
        var water = GameObject.Find("Water");
        if (water)
        {
            var tallEnough = water.transform.localScale.y > 5f;
            var wideEnough = water.transform.localScale.x > 20f;
            return tallEnough && wideEnough;
        }
        return false;
    }

    public bool A4Effector()
    {
        return GetWaterEffector();
    }
    public bool A4ColliderUsedByEffector()
    {
        var collider = GetComponentOnObject<Collider2D>("Water");
        if (collider)
        {
            return collider.usedByEffector;
        }
        return false;
    }
    public bool A4ColliderIsTrigger()
    {
        var collider = GetComponentOnObject<Collider2D>("Water");
        if (collider)
        {
            return collider.isTrigger;
        }
        return false;
    }

    public bool A6WaterLevel()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            return Mathf.Approximately(effector.surfaceLevel, 2.56f);
        }
        return false;
    }
    public bool A8Density()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            return Mathf.Approximately(effector.density, 0.5f);
        }
        return false;
    }
    public bool A9Drag()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            return Mathf.Approximately(effector.linearDamping, 0.1f) && Mathf.Approximately(effector.angularDamping, 1f);
        }
        return false;
    }
    public bool A10Flow()
    {
        var effector = GetWaterEffector();
        if (effector)
        {
            return effector.flowMagnitude > 0;
        }
        return false;
    }
    public bool A10CarPosition()
    {
        var car = GameObject.Find("CarBody");
        if (car)
        {
            return car.transform.position.x > 150;
        }
        return false;
    }


    public bool B1AntiGrav()
    {
        return GameObject.Find("Antigravity");
    }
    public bool B1AntiGravCollider()
    {
        return GetComponentOnObject<BoxCollider2D>("Antigravity");
    }
    public bool B1AntiGravColor()
    {
        var sr = GetComponentOnObject<SpriteRenderer>("Antigravity");
        if (sr)
        {
            return sr.color.r.Equals(1) && sr.color.g.Equals(0) && sr.color.b.Equals(0) && sr.color.a <= 0.6f;
        }
        return false;
    }

    public bool B1AntiGravPosition()
    {
        var antigrav = GameObject.Find("Antigravity");
        if (antigrav)
        {
            return Vector3.Distance(antigrav.transform.position, new Vector2(265, 10)) < 15f;
        }
        return false;
    }


    public bool B1AntiGravScale()
    {
        var antigrav = GameObject.Find("Antigravity");
        if (antigrav)
        {
            var tallEnough = antigrav.transform.localScale.y > 5f;
            var wideEnough = antigrav.transform.localScale.x > 20f;
            return tallEnough && wideEnough;
        }
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
            return effector.forceAngle.Equals(90) && effector.forceMagnitude.Equals(15) && effector.forceVariation.Equals(0) && effector.forceTarget.Equals(EffectorSelection2D.Rigidbody);
        }
        return false;
    }
    public bool B2ColliderUsedByEffectorAndIsTrigger()
    {
        var collider = GetComponentOnObject<Collider2D>("Antigravity");
        if (collider)
        {
            return collider.usedByEffector && collider.isTrigger;
        }
        return false;
    }

    public bool C1CurvesSelected()
    {
        if (Selection.gameObjects.Length == 10)
        {
            for (var i = 0; i < Selection.gameObjects.Length; i++)
            {
                if (Selection.gameObjects[i].name.StartsWith("Curve") == false) return false;
            }
            return true;
        }
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
                if (effector.speed.Equals(50) == false/* || effector.speedVariation.Equals(0) == false || Mathf.Approximately(effector.forceScale, 0.1f) == false*/) return false;
            }
            return true;
        }
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
                if (collider.usedByEffector == false) return false;
                if (collider.isTrigger == true) return false;
            }
            return true;
        }
        return false;
    }
}
