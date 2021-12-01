using System;
using Managers;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Components.Player
{
    [ExecuteAlways]
    public class PlayerHudController : ImmediateModeShapeDrawer
    {
        #pragma warning disable 649
        [SerializeField] private Crosshair crosshair;
        [SerializeField] private Transform crosshairTransform;
        #pragma warning restore 649

        public Vector3 CrosshairPosition
        {
            get => crosshairTransform.position;
            set => crosshairTransform.position = value;
        }

        private void Awake()
        {
            SceneManager.PlayerHudController = this;
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);

            if (cam)
            {
                using (Draw.Command(cam))
                {
                    Draw.ZTest = CompareFunction.Always; // to make sure it draws on top of everything like a HUD
                    Draw.Matrix = crosshairTransform.localToWorldMatrix; // draw it in the space of crosshairTransform
                    Draw.BlendMode = ShapesBlendMode.Transparent;
                    Draw.LineGeometry = LineGeometry.Flat2D;
                    crosshair.DrawCrosshair();
                }
            }
        }
    }
}