namespace ET.Server
{
    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType
    {
        General = 0,
        
        Black = 1
    }

    [ChildOf(typeof(Session))]
    public class AccountInfo:Entity,IAwake
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Account;

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord;

        /// <summary>
        /// 账号类型
        /// </summary>
        public int AccountType;

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime;
    }
}

