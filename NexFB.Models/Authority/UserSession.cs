namespace NexFB.Models.Authority
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class UserSession
    {
        public int UserId { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }

        public string NickName { get; set; }

        public string FullName { get; set; }

        public int GroupId { get; set; }

        public bool IsDisabled { get; set; }

        public short CompanyId { get; set; }

        public string Roles { get; set; }

        public IList<UserFunction> Functions { get; set; }
    }
}
