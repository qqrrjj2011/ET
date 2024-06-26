﻿using ET.Server.EventType;

namespace ET.Server
{
    [FriendOf(typeof(AttributeEntry))]
    [FriendOf(typeof(EquipInfoComponent))]
    [Event(SceneType.Map)]
    public class ChangeEquipItemEvent_ChangeNumeric : AEvent<Scene,ChangeEquipItem>
    {
        protected override async ETTask Run(Scene scene, ChangeEquipItem args)
        {
            EquipInfoComponent equipInfoComponent = args.Item.GetComponent<EquipInfoComponent>();

            if (equipInfoComponent == null)
            {
                return;
            }

            NumericComponent numericComponent = args.Unit.GetComponent<NumericComponent>();
            foreach (var entry in equipInfoComponent.EntryList)
            {
                AttributeEntry ent = entry;
                int numericTypeKey =  ent.Key * 10 + 2;
                if (args.EquipOp == EquipOp.Load)
                {
                    numericComponent[numericTypeKey] += ent.Value;
                }
                else if (args.EquipOp == EquipOp.Unload)
                {
                    numericComponent[numericTypeKey] -= ent.Value;
                }
            }

            await ETTask.CompletedTask;
        }

         
    }
}