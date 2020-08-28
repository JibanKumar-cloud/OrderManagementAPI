using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;


using System.Net.Mail;
using Newtonsoft.Json;

namespace RestApI
{
    
    [RoutePrefix("api")]
    [RestAPI.Auth]
    public class GetOrderDetailsController : ApiController
    {
        [HttpGet]
        public OrderViewAllRes OrderViewAll(int intUserID)
        {
            string strErr = "";
            string strResult = "";
            clsDB objDB = null;
            DataSet dstOutPut = null;


            OrderViewAllRes objResponse = new OrderViewAllRes();

            try
            {


                dstOutPut = new DataSet();

                objDB = new clsDB();
                objDB.AddParameter("p_intUserID", intUserID);
                objDB.AddParameter("p_ErrMsg", strErr, strErr.Length, ParameterDirection.Output);
                dstOutPut = objDB.ExecuteSelect("test_spGetOrderDetails", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMsg");

                if (dstOutPut != null && dstOutPut.Tables.Count > 0 && dstOutPut.Tables[0].Rows.Count > 0)
                {
                    if (dstOutPut != null && dstOutPut.Tables.Count > 0 && dstOutPut.Tables[0].Rows.Count > 0)
                    {

                        objResponse.Data = JsonConvert.DeserializeObject<OrderViewAllData>(JsonConvert.SerializeObject(dstOutPut));
                        objResponse.status = "Success";
                        objResponse.ErrMessage = "";
                    }
                    else
                    {
                        objResponse.Data = null;
                        objResponse.status = "Fail";
                        objResponse.ErrMessage = "No Data Found";
                    }

                }
            }
            catch (Exception ex)
            {
                objResponse.status = "Fail";
                objResponse.ErrMessage = ex.Message;
            }
            return objResponse;
        }

        public class OrderViewAllRes
        {
            public string ErrMessage { get; set; }
            public string status { get; set; }
            public OrderViewAllData Data { get; set; }
        }
        public class OrderViewAllData
        {
            public List<OrderViewAllDataDetails> Table { get; set; }
        }

        public class OrderViewAllDataDetails
        {
           
            public int OrderID { get; set; }
            public int ProductID { get; set; }
            public int Items { get; set; }
            public string OrderDateTime { get; set; }
            public string ShippingAddress { get; set; }
            public string status { get; set; }

        }



    }
}