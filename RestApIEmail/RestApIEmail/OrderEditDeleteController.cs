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
    public class OrderEditDeleteController : ApiController
    {

        [HttpPost]
        public OrderDeleteUpdateResponse OrderDeleteUpdateAll([FromBody]OrderDeleteUpdateAllRequest request)
        {
            clsDB objDB = null;
            DataSet dstResult;
            string strErr = "";

            OrderDeleteUpdateResponse objResponse = new OrderDeleteUpdateResponse();

            try
            {
                int intUserID, Isdelete, IsCancelled, InDelivery, IsCompleted, IsApproved , OrederID;
                string ProductIDs, ProductQts, ShippingAddress;

                if (request != null)
                {


                    intUserID = request.intUserID;
                    Isdelete = request.Isdelete;
                    IsCancelled = request.IsCancelled;
                    InDelivery = request.InDelivery;
                    IsCompleted = request.IsCompleted;
                    ProductQts = request.ProductQts;
                    ProductIDs = request.ProductIDs;
                    IsApproved= request.IsApproved;
                    OrederID = request.OrederID;
                    ShippingAddress = request.ShippingAddress;
                    
                    objDB = new clsDB();
                    objDB.AddParameter("p_intUserID", intUserID);
                    objDB.AddParameter("p_Isdelete", Isdelete);
                    objDB.AddParameter("p_ProductQts", ProductQts, ProductQts.Length);
                    objDB.AddParameter("p_ProductIDs", ProductIDs, ProductIDs.Length);
                    objDB.AddParameter("p_OrderID", OrederID);
                   
                    objDB.AddParameter("p_IsCancelled", IsCancelled);
                    objDB.AddParameter("p_InDelivery", InDelivery);
                    objDB.AddParameter("p_IsCompleted", IsCompleted);
                    objDB.AddParameter("p_IsApproved", IsApproved);
                    objDB.AddParameter("p_ShippingAddress", ShippingAddress, ShippingAddress.Length);
                    objDB.AddParameter("p_ErrMsg", strErr, strErr.Length, ParameterDirection.Output);
                    dstResult = objDB.ExecuteSelect("test_spOrderDeleteUpdate", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMsg");
            
                    if (strErr != "")
                    {

                        strErr = "Error:" + strErr;
                        objResponse.ErrMessage = strErr;
                        objResponse.status = "Fail";

                    }
                    else
                    {
                        objResponse.ErrMessage = "";
                        objResponse.status = "Success";
                    }
                        
                }
                else
                {
                    objResponse.ErrMessage = "No data found.";
                    objResponse.status = "Fail";

                }

            }
            catch (Exception ex)
            {
                objResponse.ErrMessage = ex.Message;
                objResponse.status = "Fail";

            }
            return objResponse;
        }

        public class OrderDeleteUpdateResponse
        {
            public string ErrMessage { get; set; }
            public string status { get; set; }

        }
        public class OrderDeleteUpdateAllRequest
        {
            public int OrederID { get; set; }
            public string ProductIDs { get; set; }
            public string ProductQts { get; set; }
            public int intUserID { get; set; }
            public string ShippingAddress { get; set; }
            public int Isdelete { get; set; }
            public int IsCancelled { get; set; }
            public int InDelivery { get; set; }
            public int IsCompleted { get; set; }
            public int IsApproved { get; set; }
            


        }


        //[HttpGet]
        //public OrderViewAllRes OrderViewAll(int intUserID)
        //{
        //    string strErr = "";
        //    string strResult = "";
        //    clsDB objDB = null;
        //    DataSet dstOutPut = null;


        //    OrderViewAllRes objResponse = new OrderViewAllRes();

        //    try
        //    {


        //        dstOutPut = new DataSet();

        //        objDB = new clsDB();
        //        objDB.AddParameter("p_UserID", intUserID);
        //        dstOutPut = objDB.ExecuteSelect("test_spGetOrderDetails", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMsg");

        //       if (dstOutPut != null && dstOutPut.Tables.Count > 0 && dstOutPut.Tables[0].Rows.Count > 0)
        //        {
        //            if (dstOutPut != null && dstOutPut.Tables.Count > 0 && dstOutPut.Tables[0].Rows.Count > 0)
        //            {

        //                objResponse.Data = JsonConvert.DeserializeObject<OrderViewAllData>(JsonConvert.SerializeObject(dstOutPut));
        //                objResponse.status = "Success";
        //                objResponse.ErrMessage = "";
        //            }
        //            else
        //            {
        //                objResponse.Data = null;
        //                objResponse.status = "Fail";
        //                objResponse.ErrMessage = "No Data Found";
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objResponse.status = "Fail";
        //        objResponse.ErrMessage = ex.Message;
        //    }
        //    return objResponse;
        //}

        //public class OrderViewAllRes
        //{
        //    public string ErrMessage { get; set; }
        //    public string status { get; set; }
        //    public OrderViewAllData Data { get; set; }
        //}
        //public class OrderViewAllData
        //{
        //    public List<OrderViewAllDataDetails> Table { get; set; }
        //}

        //public class OrderViewAllDataDetails
        //{
        //    public string Name { get; set; }
        //    public int OrderId { get; set; }
        //    public string ShippingAddress { get; set; }
        //    public string status { get; set; }

        //}



    }
}