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
    public class ProductGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["GameCon"].ConnectionString;
        public SqlConnection Connection { get; set; }
        public ProductGateway()
        {
            Connection = new SqlConnection(connectionString);
        }

        public List<Product> GetAllProduct()
        { 
             
            List<Product> ProductList = new List<Product>();
            SqlCommand com = new SqlCommand("GetAllProduct", Connection);
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

            ProductList = (from DataRow dr in dt.Rows

                         select new Product()
                         {
                             ProductId = Convert.ToInt32(dr["ProductId"]),
                             ProductName= Convert.ToString(dr["ProductName"]),
                             ProductPrice = Convert.ToDouble(dr["ProductPrice"]),
                             ProductType = Convert.ToString(dr["ProductType"]),
                             Currency = Convert.ToString(dr["Currency"]),
                             UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]),
                             IsActive = Convert.ToBoolean(dr["IsActive"])

                         }).ToList();

             
            return ProductList; 
        }
        public bool SaveProduct(Product p)
        {
            SqlCommand com = new SqlCommand("SaveProduct", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProductId", p.ProductId);
            com.Parameters.AddWithValue("@ProductName", p.ProductName);
            com.Parameters.AddWithValue("@ProductPrice", p.ProductPrice);
            com.Parameters.AddWithValue("@ProductType", p.ProductType);
            com.Parameters.AddWithValue("@Currency", p.Currency);
            com.Parameters.AddWithValue("@IsActive", p.IsActive);

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
        public bool DeleteProduct(int ProductId)
        {
            SqlCommand com = new SqlCommand("DeleteProduct", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProductId", ProductId);

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