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
    public class GameTopupGateway
    {

        private string connectionString = WebConfigurationManager.ConnectionStrings["GameCon"].ConnectionString;
        public SqlConnection Connection { get; set; }
        public GameTopupGateway()
        {
            Connection = new SqlConnection(connectionString);
        }
        public List<GameTopup> GetAllGameTopup() 
        { 
             
            List<GameTopup> GameTopupList = new List<GameTopup>();
            SqlCommand com = new SqlCommand("GetAllGameTopup", Connection);
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

            GameTopupList = (from DataRow dr in dt.Rows

                         select new GameTopup()
                         {
                             GameTopupId = Convert.ToInt32(dr["GameTopupId"]),
                             GameName = Convert.ToString(dr["GameName"]),
                             ImageUrl = Convert.ToString(dr["ImageUrl"]),
                             OfferName = Convert.ToString(dr["OfferName"]),
                             IsEnterPlayerId = Convert.ToBoolean(dr["IsEnterPlayerId"]),
                             IsSocialAccount = Convert.ToBoolean(dr["IsSocialAccount"]),
                             IsAccountType = Convert.ToBoolean(dr["IsAccountType"]),
                             IsFacebookAccount = Convert.ToBoolean(dr["IsFacebookAccount"]),
                             IsGmailAccount = Convert.ToBoolean(dr["IsGmailAccount"]),
                             IsAccountPassword = Convert.ToBoolean(dr["IsAccountPassword"]),
                             IsAccountSecurityCode = Convert.ToBoolean(dr["IsAccountSecurityCode"]),
                             IsActive = Convert.ToBoolean(dr["IsActive"]),
                             PrePurchaseMessage = Convert.ToString(dr["PrePurchaseMessage"]),
                             Guideline = Convert.ToString(dr["Guideline"]),
                             ProductIds = Convert.ToString(dr["ProductIds"]),
                             UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"])
                            
                         }).ToList();

             
            return GameTopupList; 
        } 
        public int UpdateGameTopupImagePath(GameTopup path)
        {
            SqlCommand com = new SqlCommand("UpdateGameTopupImagePath", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@GameTopupId", path.GameTopupId);
            com.Parameters.AddWithValue("@ImageUrl", path.ImageUrl);
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
        public int SaveGameTopup(GameTopup g) 
        {
            SqlCommand com = new SqlCommand("SaveGameTopup", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@GameTopupId", g.GameTopupId);
            com.Parameters.AddWithValue("@GameName", g.GameName);
            com.Parameters.AddWithValue("@ImageUrl", g.ImageUrl);
            com.Parameters.AddWithValue("@OfferName", g.OfferName);
            com.Parameters.AddWithValue("@IsEnterPlayerId", g.IsEnterPlayerId);
            com.Parameters.AddWithValue("@IsSocialAccount", g.IsSocialAccount);
            com.Parameters.AddWithValue("@IsAccountType", g.IsAccountType);
            com.Parameters.AddWithValue("@IsFacebookAccount", g.IsFacebookAccount);
            com.Parameters.AddWithValue("@IsGmailAccount", g.IsGmailAccount);
            com.Parameters.AddWithValue("@IsAccountPassword", g.IsAccountPassword);
            com.Parameters.AddWithValue("@IsAccountSecurityCode", g.IsAccountSecurityCode);
            com.Parameters.AddWithValue("@IsActive", g.IsActive);
            com.Parameters.AddWithValue("@PrePurchaseMessage", g.PrePurchaseMessage);
            com.Parameters.AddWithValue("@Guideline", g.Guideline);
            com.Parameters.AddWithValue("@ProductIds", g.ProductIds);
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
        public bool DeleteGameTopup(int GameTopupId)
        {
            SqlCommand com = new SqlCommand("DeleteGameTopup", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@GameTopupId", GameTopupId);

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