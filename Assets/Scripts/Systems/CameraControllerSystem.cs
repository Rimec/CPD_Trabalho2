using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace CPD.Gnoma
{
    public partial class CameraControllerSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<TreeTag>();
        }

        protected override void OnUpdate()
        {
            var treeEntity = SystemAPI.GetSingletonEntity<TreeTag>();
            var treeScale = SystemAPI.GetComponent<LocalTransform>(treeEntity).Scale;

            var cameraSingleton = CameraSingleton.Instance;
            if (cameraSingleton == null) return;
            var positionFactor = (float)SystemAPI.Time.ElapsedTime * cameraSingleton.Speed;
            var height = cameraSingleton.HeightAtScale(treeScale);
            var radius = cameraSingleton.RadiusAtScale(treeScale);

            cameraSingleton.transform.position = new Vector3
            {
                x = Mathf.Cos(positionFactor) * radius,
                y = height,
                z = Mathf.Sin(positionFactor) * radius
            };
            cameraSingleton.transform.LookAt(Vector3.zero, Vector3.up);
        }
    }
}