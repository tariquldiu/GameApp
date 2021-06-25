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
    public class NoticeGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["GameCon"].ConnectionString;
        public SqlConnection Connection { get; set; }
        public NoticeGateway()
        {
            Connection = new SqlConnection(connectionString);
        }
        public List<Notice> GetAllNotice()
        { 
             
            List<Notice> NoticeList = new List<Notice>();
            SqlCommand com = new SqlCommand("GetAllNotice", Connection);
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

            NoticeList = (from DataRow dr in dt.Rows

                         select new Notice()
                         {
                             NoticeId = Convert.ToInt32(dr["NoticeId"]),
                             Description = Convert.ToString(dr["Description"]),
                             PositiveVote = Convert.ToInt32(dr["PositiveVote"]),
                             NegativeVote = Convert.ToInt32(dr["NegativeVote"]),
                             HasVoting = Convert.ToBoolean(dr["HasVoting"]),
                             UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]),
                             IsActive = Convert.ToBoolean(dr["IsActive"])

                         }).ToList();

             
            return NoticeList; 
        }
        public bool SaveNotice(Notice n)
        {
            SqlCommand com = new SqlCommand("SaveNotice", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@NoticeId", n.NoticeId);
            com.Parameters.AddWithValue("@Description", n.Description);
            com.Parameters.AddWithValue("@PositiveVote", n.PositiveVote);
            com.Parameters.AddWithValue("@NegativeVote", n.NegativeVote);
            com.Parameters.AddWithValue("@HasVoting", n.HasVoting);
            com.Parameters.AddWithValue("@IsActive", n.IsActive);

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
        public bool DeleteNotice(int NoticeId)
        {
            SqlCommand com = new SqlCommand("DeleteNotice", Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@NoticeId", NoticeId);

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