namespace ET.Client
{
    public static class DamageCalcuateHelper
    {
        public static int CalcuateDamageValue(Unit attackUnit,Unit TargetUnit,ref SRandom random)
        {
            int damage = attackUnit.GetComponent<NumericComponent>().GetAsInt(NumericType.DamageValue);
            int dodge  = TargetUnit.GetComponent<NumericComponent>().GetAsInt(NumericType.Dodge);
            int aromr  = TargetUnit.GetComponent<NumericComponent>().GetAsInt(NumericType.Armor);
            
            // 随机 0 - 100%  根据敏捷值进行闪避
            int rate = random.Range(0, 1000000);
            Log.Debug("Rate:  " + rate.ToString());
            if ( rate < dodge )
            {
                //躲避成功
                Log.Debug("闪避成功");
                damage = 0;
            }
            
            if (damage > 0)
            {
                //扣掉护甲值
                damage = damage - aromr;
                //造成最小的1点伤害值
                damage = damage <= 0? 1 : damage;
            }
            
            //Log.Debug($"造成伤害值：{damage}");
            return damage;
        }
    }
}