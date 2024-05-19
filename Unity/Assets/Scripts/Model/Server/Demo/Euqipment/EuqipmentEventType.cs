
namespace ET.Server
{
    namespace EventType
    {
        public struct ChangeEquipItem
        {
            public Unit Unit;
            public ServerItem Item;
            public EquipOp EquipOp;
        }
    }
}