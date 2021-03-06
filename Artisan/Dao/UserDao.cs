﻿using System;
using System.Text;
using System.Data.SQLite;
using Dao.Entities;

namespace Dao
{
    public class UserDao : BaseDao
    {
        public static string CreateStatement { get; } = "CREATE TABLE `User` ( " +
                                                        " `Name`	TEXT NOT NULL, "+
	                                                    " `Email`	Varchar(50) NOT NULL, "+
                                                        " `PhoneNumber`	Varchar(20) NOT NULL, " +
                                                        " `Password`	Varchar(50) NOT NULL " +
                                                        " ) ";

        public UserDao(string table):base(table)
        {}

        public bool Save(User data)
        {
            return ChangeUserName(data.Name) && ChangePhoneNumber(data.PhoneNumber)
                && ChangeEmail(data.EmailAddress) && ChangePassword(data.Password);
        }
        public bool SaveNew(User data)
        {
            string attribs = CreateStringAttribute(data.GetProperties());
            string attribVals = CreateStringAttributeValues(data.GetPropertyValues());
            StringBuilder build = new StringBuilder();
            int success = 0;

            build.Append("Insert into ").Append(Table)
                        .Append(" " + attribs).Append(" Values " + attribVals);

            success = base.ExecuteQuery(build.ToString());

            if (success == 1)
                return true;
            return false;
        }//Works
        public bool ChangeUserName(string name)
        {
            return Update("Name", name);
        }//Works
        public bool ChangeEmail(string email)
        {
            return Update("Email", email);
        }//Works
        public bool ChangePhoneNumber(string phoneNum)
        {
            return Update("PhoneNumber", phoneNum.ToString());
        }//Works
        public bool ChangePassword(string psw)
        {
            return Update("Password", psw);
        }//Works
        public User retrieveUser()
        {
            User user = null;
            string query = "Select * from User;";
            Query = query;

            using (connection = new Connection())
            using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        user = new User(reader[0].ToString(),
                            reader[1].ToString(),
                            reader[2].ToString(), reader[3].ToString());
                    }
                }
                catch
                {
                    return null;
                }
               
                reader.Close();
            }

            return user;
        }//Works
    }
}
