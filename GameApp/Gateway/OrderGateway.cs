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
    public class OrderGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["GameCon"].ConnectionString;
        public SqlConnection Connection { get; set; }
        public OrderGateway()
        {
            Connection = new SqlConnection(connectionString);
        }


        public List<Order> GetAllOrder() 
        { 
             
            List<Order> OrderList = new List<Order>();
            SqlCommand com = new SqlCommand("GetAllOrder", Connection);
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

            OrderList = (from DataRow dr in dt.Rows

                         select new Order()
                         {
                             OrderId = Convert.ToInt32(dr["OrderId"]),
                             OrderNo = Convert.ToString(dr["OrderNo"]),
                             PlayerId = Convert.ToString(dr["PlayerId"]),
                             AccountType = Convert.ToString(dr["AccountType"]),
                             AccountName = Convert.ToString(dr["AccountName"]),
                             GameName = Convert.ToString(dr["GameName"]),
                             OfferName = Convert.ToString(dr["OfferName"]),
                             AccountPassword = Convert.ToString(dr["AccountPassword"]),
                             AccountSecurityCode = Convert.ToString(dr["AccountSecurityCode"]),
                             UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]),
                             ProductIds = Convert.ToString(dr["ProductIds"]),
                             GameTopupId = Convert.ToInt32(dr["GameTopupId"]),
                             //UserId = Convert.ToInt32(dr["UserId"]),
                             UserName = Convert.ToString(dr["UserName"]),
                             FirstName = Convert.ToString(dr["FirstName"]),
                             LastName = Convert.ToString(dr["LastName"]),
                             Country = Convert.ToString(dr["Country"]),
                             Phone = Convert.ToString(dr["Phone"]),
                             Email = Convert.ToString(dr["Email"]),
                             OrderNote = Convert.ToString(dr["OrderNote"]),
                             AccountNumber = Convert.ToString(dr["AccountNumber"]),
                             TransactionId = Convert.ToString(dr["TransactionId"]),
                             Amount = Convert.ToDouble(dr["Amount"])

                         }).ToList();

             
            return OrderList; 
        } 
       
        public int SaveOrder(Order o) 
        {
            SqlCommand com = new SqlCommand("SaveOrder", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@OrderId", o.OrderId);
            com.Parameters.AddWithValue("@OrderNo", o.OrderNo);
            com.Parameters.AddWithValue("@PlayerId", o.PlayerId);
            com.Parameters.AddWithValue("@AccountType", o.AccountType);
            com.Parameters.AddWithValue("@AccountName", o.AccountName);
            com.Parameters.AddWithValue("@AccountPassword", o.AccountPassword);
            com.Parameters.AddWithValue("@AccountSecurityCode", o.AccountSecurityCode);
            com.Parameters.AddWithValue("@ProductIds", o.ProductIds);
            com.Parameters.AddWithValue("@GameTopupId", o.GameTopupId);
            com.Parameters.AddWithValue("@UserId", o.UserId);
            com.Parameters.AddWithValue("@FirstName", o.FirstName);
            com.Parameters.AddWithValue("@LastName", o.LastName);
            com.Parameters.AddWithValue("@Country", o.Country);
            com.Parameters.AddWithValue("@Phone", o.Phone);
            com.Parameters.AddWithValue("@Email", o.Email);
            com.Parameters.AddWithValue("@OrderNote", o.OrderNote);
            com.Parameters.AddWithValue("@AccountNumber", o.AccountNumber);
            com.Parameters.AddWithValue("@TransactionId", o.TransactionId);
            com.Parameters.AddWithValue("@Amount", o.Amount);
            int i;
            try
            {
                Connection.Open();
                i = (int)com.ExecuteScalar();
            }
            finally
            {
                Connection.Close();
            }
            if (i != null)
            {

                return i;

            }
            else
            {
                return 0;
            }


        }
        public bool DeleteOrder(int OrderId)
        {
            SqlCommand com = new SqlCommand("DeleteOrder", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@OrderId", OrderId);

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