using System;

namespace Dao.Entities
{
    public class User : Entity
    {
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        
        public User(string name, int phoneNum, string email, string add):base(name, 0)
        {
            PhoneNumber = phoneNum;
            EmailAddress = email;
            Address = add;
        }

        public override string[] GetProperties()
        {
            return new string[] { "Name", "PhoneNumber", "Email", "Address" };
        }
        public override string[] GetPropertyValues()
        {
            return new string[] { Name, PhoneNumber.ToString(), EmailAddress, Address };
        }
    }
}
