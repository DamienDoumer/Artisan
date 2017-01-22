using System;

namespace Dao.Entities
{
    public class Entity
    {
        protected int iD;
        protected string name;

        public int ID { get { return iD; }
            set { iD = value; }
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (value.Length > 50)
                {
                    throw new Exception("The name of your event should not be of more than 100 characters.");
                }
                else
                {
                    name = value;
                }
            }
        }

        public Entity(string name, int id)
        {
            //if (name.Length > 50)
            //{
            //    ///----------------------------------///
            //    /// translation needed               ///
            //    /// ---------------------------------///
            //    throw new Exception("The name of your event should not be of more than 100 characters.");
            //}
            //else
            //{
            //    this.name = name;
            //}

            this.name = name;

            iD = id;
        }
        public virtual string[] GetProperties()
        {
            return new string[] { "ID", "Name" };
        }
        public virtual string[] GetPropertyValues()
        {
            return new string[] { ID.ToString(), Name };
        }
        public override string ToString()
        {
            return Name + " " + ID;
        }
    }
}
