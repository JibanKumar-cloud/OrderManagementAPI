using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml;
using System.IO;
using System.Xml.Serialization;


namespace RestApI
{
    public class clsDB
    {
        MySqlCommand cmd;
        string strConnString;
        string strConnStringMasterBkp;
        string strConnStringSlave1;
        string strConnStringSlave2;
        string strConnStringSlave3;
        string strConnStringLogs;
        string strSlaveMode1;
        string strSlaveMode2;
        string strSlaveMode3;
        bool blnIsAllBlank = false;
        public static int intRanSlaves = 0;
        public static int intRanSlavesRepate = 1;
        string strMasterPrivateIP;
        string strMasterPublicIP;

        public clsDB()
        {

            // <add key="ConnectionString" value="server=127.0.0.1;User Id=root;database=PassionWorld2018;password=root;pooling=false;" />



            cmd = new MySqlCommand();


        }
        public void AddParameter(string strParamName, string strParamValue, int intParamSize, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.VarChar, intParamSize).Value = strParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public void AddParameter(string strParamName, int intParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.Int32).Value = intParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public void AddParameter(string strParamName, bool blnParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.Bit).Value = blnParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public void AddParameter(string strParamName, DateTime dtParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.DateTime).Value = dtParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public void AddParameter(string strParamName, decimal dclParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.Decimal).Value = dclParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public void AddParameter(string strParamName, double dblParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.Decimal).Value = dblParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public int ExecuteDML(string strSQL, CommandType cmdType, int intTimeout, ref string strErr)
        {
            int status = 0;
            MySqlConnection conn = new MySqlConnection(strConnString);
            strErr = "";
            try
            {
                cmd.CommandText = strSQL;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = intTimeout;
                conn.Open();
                cmd.Connection = conn;
                status = cmd.ExecuteNonQuery();

                try
                {
                    if (cmd.Parameters["p_ErrMsg"] != null && cmd.Parameters["p_ErrMsg"].Value != null)
                        strErr = cmd.Parameters["p_ErrMsg"].Value.ToString();
                }
                catch { }

                cmd.Parameters.Clear();
            }
            catch (System.Exception ex)
            {
                status = -1;
                strErr = ex.Message;

            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                conn = null;
            }
            return status;
        }

        public DataSet ExecuteSelect(string strSQL, CommandType cmdType, int intTimeout, ref string strErr, string strOpt = "p_ErrMsg")
        {
            DataSet ds = new DataSet();
            MySqlDataAdapter adp = new MySqlDataAdapter();

            string server = "localhost";
            string database = "passionworld2019";
            string uid = "root";
            string password = "password";

            strConnString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            strConnString = "server=18.138.168.149;User Id=SlaveDev;database=crs2011;password=$7@vu$e5;pooling=true;";

           MySqlConnection conn = new MySqlConnection(strConnString);
            strErr = "";
            try
            {
                cmd.CommandText = strSQL;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = intTimeout;
                conn.Open();
                cmd.Connection = conn;
                adp.SelectCommand = cmd;
                adp.Fill(ds);

                try
                {
                    if (cmd.Parameters[strOpt] != null && cmd.Parameters[strOpt].Value != null)
                        strErr = cmd.Parameters[strOpt].Value.ToString();
                }
                catch { }

                cmd.Parameters.Clear();
                //conn.Close();
            }
            catch (System.Exception ex)
            {
                ds = null;
                strErr = ex.Message;
            }
            finally
            {
                adp.Dispose();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                conn = null;
            }
            return ds;
        }




    }
}
