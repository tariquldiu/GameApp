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
    public class PaymentGatewayGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["GameCon"].ConnectionString;
        public SqlConnection Connection { get; set; }
        public PaymentGatewayGateway()
        {
            Connection = new SqlConnection(connectionString);
        }
        public List<PaymentGateway> GetAllPaymentGateway()
        { 
             
            List<PaymentGateway> paymentGatewayList = new List<PaymentGateway>();
            SqlCommand com = new SqlCommand("GetAllPaymentGateway", Connection);
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

            paymentGatewayList = (from DataRow dr in dt.Rows

                         select new PaymentGateway()
                         { 
                             GatewayId = Convert.ToInt32(dr["GatewayId"]),
                             GatewayName = Convert.ToString(dr["GatewayName"]),
                             AccountType = Convert.ToString(dr["AccountType"]),
                             AccountNumber = Convert.ToString(dr["AccountNumber"]),
                             DiscountAmount = Convert.ToInt32(dr["DiscountAmount"]),
                             ChargeAmount = Convert.ToInt32(dr["ChargeAmount"])

                         }).ToList();

             
            return paymentGatewayList; 
        }
        public bool SavePaymentGateway(PaymentGateway p)
        {
            SqlCommand com = new SqlCommand("SavePaymentGateway", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@GatewayId", p.GatewayId);
            com.Parameters.AddWithValue("@GatewayName", p.GatewayName);
            com.Parameters.AddWithValue("@AccountType", p.AccountType);
            com.Parameters.AddWithValue("@AccountNumber", p.AccountNumber);
            com.Parameters.AddWithValue("@DiscountAmount", p.DiscountAmount);
            com.Parameters.AddWithValue("@ChargeAmount", p.ChargeAmount);

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
        public bool DeletePaymentGateway(int gatewayId)
        {
            SqlCommand com = new SqlCommand("DeletePaymentGateway", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@GatewayId", gatewayId);

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