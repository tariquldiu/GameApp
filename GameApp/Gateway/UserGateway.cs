using GameApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace GameApp.Gateway
{
    public class UserGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["GameCon"].ConnectionString;
        public SqlConnection Connection { get; set; }
        public UserGateway()
        {
            Connection = new SqlConnection(connectionString);
        }
        public List<User> GetAllUser()
        { 
             
            List<User> userList = new List<User>();
            SqlCommand com = new SqlCommand("GetAllUser", Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            try
            {
                Connection.Open();
                da.Fill(dt);
            }
            finally
            {
                Connection.Close();
            }

            userList = (from DataRow dr in dt.Rows

                         select new User()
                         {
                             UserId = Convert.ToInt32(dr["UserId"]),
                             FullName = Convert.ToString(dr["FullName"]),
                             Phone = Convert.ToString(dr["Phone"]),
                             Email = Convert.ToString(dr["Email"]),
                             UserName = Convert.ToString(dr["UserName"]),
                             Password = Convert.ToString(dr["Password"]),
                             RoleName = Convert.ToString(dr["RoleName"]),
                             IsActive = Convert.ToBoolean(dr["IsActive"])

                         }).ToList();

             
            return userList; 
        }
        public bool SaveUser(User u)
        {
            SqlCommand com = new SqlCommand("SaveUser", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserId", u.UserId);
            com.Parameters.AddWithValue("@FullName", u.FullName);
            com.Parameters.AddWithValue("@Phone", u.Phone);
            com.Parameters.AddWithValue("@Email", u.Email);
            com.Parameters.AddWithValue("@UserName", u.UserName);
            com.Parameters.AddWithValue("@Password", u.Password);
            com.Parameters.AddWithValue("@RoleName", u.RoleName);
            com.Parameters.AddWithValue("@IsActive", u.IsActive);

            int i;
            try
            {
                Connection.Open();
                i = com.ExecuteNonQuery();
            }
            finally
            {
                Connection.Close();
            }
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        public bool DeleteUser(int userId)
        {
            SqlCommand com = new SqlCommand("DeleteUser", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserId", userId);

            int i; 
            try
            {
                Connection.Open();
                i = com.ExecuteNonQuery();
            }
            finally
            {
                Connection.Close();
            }
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
    }
}