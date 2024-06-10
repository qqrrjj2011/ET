using System.Collections.Generic;
using Unity.Mathematics;
using Random = System.Random;

namespace ET.Server
{
    [FriendOfAttribute(typeof(ET.OwnerComponent))]
    public static partial class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask FindPathMoveToAsync(this Unit unit, float3 target, bool needBroadCast = true)
        {
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
            if (speed < 0.01)
            {
                if (needBroadCast)
                {
                    unit.SendStopBroadCast(2);
                }
                else
                {
                    unit.SendStop(2);
                }
                return;
            }

            M2C_PathfindingResult m2CPathfindingResult = M2C_PathfindingResult.Create();
            unit.GetComponent<PathfindingComponent>().Find(unit.Position, target, m2CPathfindingResult.Points);

            if (m2CPathfindingResult.Points.Count < 2)
            {
                if (needBroadCast)
                {
                    unit.SendStopBroadCast(3);
                }
                else
                {
                    unit.SendStop(3);
                }

                return;
            }

            // 广播寻路路径
            m2CPathfindingResult.Id = unit.Id;
            if (needBroadCast)
            {
                MapMessageHelper.Broadcast(unit, m2CPathfindingResult);
            }
            else
            {
                if (unit.Type() == UnitType.Genarals)
                {
                    long ownerId = unit.GetComponent<OwnerComponent>().ownerId;
                    MapMessageHelper.SendToClient(unit.Root().GetComponent<UnitComponent>().GetChild<Unit>(ownerId), m2CPathfindingResult);
                }
                else
                {
                    MapMessageHelper.SendToClient(unit, m2CPathfindingResult);
                }
                //MapMessageHelper.SendToClient(unit, m2CPathfindingResult);
            }

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();

            bool ret = await moveComponent.MoveToAsync(m2CPathfindingResult.Points, speed);
            if (ret) // 如果返回false，说明被其它移动取消了，这时候不需要通知客户端stop
            {
                if (needBroadCast)
                {
                    unit.SendStopBroadCast(0);
                }
                else
                {
                    unit.SendStop(0);
                }
            }
        }

        /// <summary>
        /// 跟随
        /// </summary>
        public static async ETTask MoveFollw(this Unit unit, float3 target, bool needBroadCast)
        {
            await unit.Root().GetComponent<TimerComponent>().WaitAsync(new Random().Next(500, 1500));
            unit.FindPathMoveToAsync(target, needBroadCast).Coroutine();
        }

        public static void StopBroadCast(this Unit unit, int error)
        {
            unit.GetComponent<MoveComponent>().Stop(error == 0);
            unit.SendStopBroadCast(error);
        }

        public static void Stop(this Unit unit, int error)
        {
            unit.GetComponent<MoveComponent>().Stop(error == 0);
            unit.SendStop(error);
        }

        // error: 0表示协程走完正常停止
        public static void SendStopBroadCast(this Unit unit, int error)
        {
            M2C_Stop m2CStop = M2C_Stop.Create();
            m2CStop.Error = error;
            m2CStop.Id = unit.Id;
            m2CStop.Position = unit.Position;
            m2CStop.Rotation = unit.Rotation;

            MapMessageHelper.Broadcast(unit, m2CStop);
        }

        // error: 0表示协程走完正常停止
        public static void SendStop(this Unit unit, int error)
        {
            M2C_Stop m2CStop = M2C_Stop.Create();
            m2CStop.Error = error;
            m2CStop.Id = unit.Id;
            m2CStop.Position = unit.Position;
            m2CStop.Rotation = unit.Rotation;
            if (unit.Type() == UnitType.Genarals)
            {
                long ownerId = unit.GetComponent<OwnerComponent>().ownerId;
                MapMessageHelper.SendToClient(unit.Root().GetComponent<UnitComponent>().GetChild<Unit>(ownerId), m2CStop);
            }
            else
            {
                MapMessageHelper.SendToClient(unit, m2CStop);
            }

        }
    }
}