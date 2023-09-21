namespace CDN.API.ObjectMember
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Skillsets { get; set; }
        public string Hobby { get; set; }
    }

    public class UserData 
    { 
        public List<User> UserDataList { get; set; }
        public int iTotalRecord { get;set; }
        
        public int iPageSize { get; set; }

        public int iPageNo { get; set; }
    }
}
