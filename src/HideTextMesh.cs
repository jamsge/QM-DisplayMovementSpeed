using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

namespace QM_DisplayMovementSpeedContinued
{
    public class HideTextMesh : MonoBehaviour
    {
        public TextMeshPro textMesh;
        public SpriteRenderer spriteRenderer;
        public void Start()
        {
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
        public void FixedUpdate ()
        {

            //Debug
            //Text mesh is coming back null, but seems to be still checking for game object.
            if (textMesh == null || textMesh.gameObject == null || textMesh.renderer == null || 
                spriteRenderer == null || spriteRenderer.gameObject == null)
            {
                return;
            }

            if (Plugin.show)
            {
                textMesh.renderer.enabled = spriteRenderer.enabled;
            }
            else
            {
                textMesh.renderer.enabled = false;
            }
        }
    }
}
