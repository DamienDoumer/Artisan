using System;

namespace Dao.Entities
{
    public class User : Entity
    {
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        
        public User(string name, string phoneNum, string email, string add):base(name, 0)
        {
            PhoneNumber = phoneNum;
            EmailAddress = email;
            Password = add;
        }

        public override string[] GetProperties()
        {
            return new string[] { "Name", "PhoneNumber", "Email", "Password" };
        }
        public override string[] GetPropertyValues()
        {
            return new string[] { Name, PhoneNumber, EmailAddress, Password };
        }
    }
}
