namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        
        // 110000以下的错误请看ErrorCore.cs
        
        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
        
        /// <summary>
        /// 重复登录
        /// </summary>
        public const int ERR_LoginRepeatedly      = 200001;
        
        
        /// <summary>
        /// 用户名或者密码为空
        /// </summary>
        public const int ERR_LoginInfoNullOrEmpty  = 200002;
        
        /// <summary>
        /// 用户名格式错误
        /// </summary>
        public const int ERR_AccountNameFormError  = 200003;

        /// <summary>
        /// 密码格式错误
        /// </summary>
        public const int ERR_PassWordFormError     = 200004;
        
        /// <summary>
        /// 黑账户
        /// </summary>
        public const int ERR_BlackAccount           = 200005;
        
        /// <summary>
        /// 黑账户
        /// </summary>
        public const int ERR_LoginPassWordError     = 200006;
        
        /// <summary>
        /// 有其他账户在登录 
        /// </summary>
        public const int ERR_OtherAccountLogin      = 200007;

        /// <summary>
        /// 网络错误
        /// </summary>
        public const int ERR_NetWorkError           = 200008;
        
        /// <summary>
        /// token错误
        /// </summary>
        public const int ERR_TokenError           = 200009;
        
        /// <summary>
        /// 角色名称为空
        /// </summary>
        public const int ERR_RoleNameEmpty         = 200010;
        /// <summary>
        /// 查询不到角色
        /// </summary>
        public const int ERR_RoleNotExist          = 200011;
        
        /// <summary>
        /// 重复请求
        /// </summary>
        public const int ERR_RequestRepeatedly     = 200012;
        
        /// <summary>
        /// 
        /// </summary>
        public const int ERR_SessionPlayerError    = 200013;

        /// <summary>
        /// 
        /// </summary>
        public const int ERR_NonePlayerError       = 200014;

        /// <summary>
        /// 
        /// </summary>
        public const int ERR_PlayerSessionError    = 200015;
        
        /// <summary>
        /// session状态错误
        /// </summary>
        public const int ERR_SessionStateError     = 200016;
        /// <summary>
        /// 二次登录失败
        /// </summary>
        public const int ERR_ReEnterGameError      = 200017;
        public const int ERR_ReEnterGameError2     = 200018;

        /// <summary>
        /// 进入游戏失败
        /// </summary>
        public const int ERR_EnterGameError        = 200019;

        /// <summary>
        /// 同服务器角色名称重复
        /// </summary>
        public const int ERR_RoleNameRepeat         = 200020;
        
        /// <summary>
        /// 空消息
        /// </summary>
        public const int ERR_ChatMessageEmpty       = 200021;   

    }
}