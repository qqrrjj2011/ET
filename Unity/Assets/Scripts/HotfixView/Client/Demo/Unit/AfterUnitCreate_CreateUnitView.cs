using UnityEngine;

namespace ET.Client
{
 
    [Event(SceneType.Current)]
    public class AfterUnitCreate_CreateUnitView: AEvent<Scene, EventType.AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, EventType.AfterUnitCreate args)
        {
            Unit unit = args.Unit;
            // Unit View层
            string assetsPath = PathHelper.PathUnit(unit.Config().PrefabName); 
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsPath);
            GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");

            GlobalComponent globalComponent = scene.Root().GetComponent<GlobalComponent>();
            GameObject go = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            go.transform.position = unit.Position;
            unit.AddComponent<GameObjectComponent>().GameObject = go;
            unit.AddComponent<AnimatorComponent>();

            if (unit.Type() == UnitType.Player)
            {
                unit.AddComponent<CameraComponent,Transform,Transform>(go.transform,Camera.main.transform);
                
            }

            await ETTask.CompletedTask;
        }
    }
}