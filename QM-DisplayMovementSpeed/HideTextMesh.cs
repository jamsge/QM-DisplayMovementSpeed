using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

namespace QM_DisplayMovementSpeed
{
    public class HideTextMesh : MonoBehaviour
    {
        public TextMeshPro textMesh;
        public SpriteRenderer spriteRenderer;
        public Monster monster;
        void Start()
        {
            Debug.Log("creating hidetextmesh component");
            SpriteRenderer[] spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer s in spriteRenderers)
            {
                if (s.name == "shadow")
                {
                    spriteRenderer = s;
                    break;
                }
            }
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.transform.GetChild(gameObject.transform.childCount - 1).GetComponent<SpriteRenderer>();
            }
            textMesh = gameObject.GetComponentInChildren<TextMeshPro>();


        }

        void FixedUpdate ()
        {
            if (Plugin.show)
                textMesh.renderer.enabled = spriteRenderer.enabled;
            else
                textMesh.renderer.enabled = false;
        }
    }
}
