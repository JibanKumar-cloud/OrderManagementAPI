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
    //Order Booking Code
    [RoutePrefix("api")]
    [RestAPI.Auth]
    public class OrderController : ApiController
    {
        [HttpPost]
        public OrderInsertUpdateResponse OrderInsertUpdateAll([FromBody]OrderInsertUpdateAllRequest request)
        {
            clsDB objDB = null;
            DataSet dstResult;
            string strErr = "";

            OrderInsertUpdateResponse objResponse = new OrderInsertUpdateResponse();
          
            try
            {
                int intUserID;
                string ProductIDs, ProductQts, ShippingAddress;

                if (request != null)
                {
                    

                    intUserID = request.intUserID;
                    ProductQts = request.ProductQts;
                    ProductIDs = request.ProductIDs;
                    ShippingAddress = request.ShippingAddress;
                   
                    objDB = new clsDB();
                    objDB.AddParameter("p_intUserID", intUserID);
                    objDB.AddParameter("p_ProductQts", ProductQts, ProductQts.Length);
                    objDB.AddParameter("p_ProductIDs", ProductIDs, ProductIDs.Length);
                    objDB.AddParameter("p_ShippingAddress", ShippingAddress, ShippingAddress.Length);
                    objDB.AddParameter("p_ErrMsg", strErr, strErr.Length, ParameterDirection.Output);
                    dstResult =objDB.ExecuteSelect("test_spOrderInsertUpdate", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMsg");

                    if (dstResult != null && dstResult.Tables.Count > 0 && dstResult.Tables[0].Rows.Count > 0)
                    {
                       //Email Code 

                        try
                        {

                            string strFrom = "";
                            string strFromPwd = "";

                            MailMessage message = null;
                            try
                            {
                                string strBody = "<h1 style='text-align:center;'>Thank You For Shopping," + dstResult.Tables[0].Rows[0]["UserName"] + "</h2>";
                                string strSubject = "THANK YOU!!";
                                string strTo = dstResult.Tables[0].Rows[0]["EmailID"].ToString();


                                string smtpServer = "smtp.gmail.com";
                                string smtpPort = "587";
                                string tyEmailPassword = strFromPwd;

                                SmtpClient smtp = new SmtpClient(smtpServer, Convert.ToInt32(smtpPort));
                                message = new MailMessage();

                                smtp.UseDefaultCredentials = false;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.Timeout = 60000;

                                smtp.EnableSsl = true;
                                smtp.Credentials = new System.Net.NetworkCredential(strFrom, strFromPwd);
                                message.From = new MailAddress(strFrom);

                                string[] strArrEmails = strTo.Split(',');

                                foreach (string strToEmail in strArrEmails)
                                {
                                    if (strToEmail != "")
                                        message.To.Add(new MailAddress(strToEmail));
                                }


                                message.BodyEncoding = System.Text.Encoding.UTF8;
                                message.Body = strBody;
                                message.IsBodyHtml = true;
                                message.Subject = strSubject;

                                smtp.Send(message);

                               
                                objResponse.ErrMessage = "";
                                objResponse.status = "Success";

                            }
                            catch (Exception ex)
                            {
                                objResponse.status = "Fail";
                                objResponse.ErrMessage = ex.Message;
                            }
                            finally
                            {
                                if (message != null)
                                    message.Dispose();
                            }


                        }
                        catch (Exception Ex)
                        {

                        }
                    }
                    else
                    {

                        objResponse.ErrMessage = strErr;
                        objResponse.status = "Fail";
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

        public class OrderInsertUpdateResponse
        {
            public string ErrMessage { get; set; }
            public string status { get; set; }
            
        }
        public class OrderInsertUpdateAllRequest
        {
            public string ProductIDs { get; set; }
            public string ProductQts { get; set; }
            public int intUserID { get; set; }
            public string ShippingAddress { get; set; }
            
        }
        
    }
}