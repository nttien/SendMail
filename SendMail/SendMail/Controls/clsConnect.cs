using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
//using System.Web.Script.Serialization;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace SendMail
{
    public class clsConnect
    {
        static string hostname = @"http://localhost:3000";

        static string getjsonString(string url)
        {
            string strUrl = "";

            using (var jsonString = new WebClient())
            {
                strUrl = jsonString.DownloadString(hostname + url);
            }
            return strUrl;
        }

        public static bool Insert(objLicense objL)
        {
            bool flag = false;
            try
            {
                string strUrl = getjsonString("/api/license/new/&email=" + objL.email + "&username=" + objL.userName
                                  + "&password=" + objL.password + "&key=" + objL.key);

                flag = (strUrl.Equals("successful")) ? true : false;
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }
            return flag;
        }

        public static bool Update(objLicense objL)
        {
            bool flag = false;
            try
            {
                string strUrl = getjsonString("/api/license/update/&email=" + objL.email + "&key=" + objL.key
                              + "&countday=30" + "&hardware_id=" + clsSendMail.getUniqueID("C"));

                flag = (strUrl.Equals("successful")) ? true : false;
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }
            return flag;
        }

        //private static string DeCodeJson(string token)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        //    string json = jsonToken.Claims.First(claim => claim.Type == "license").Value;

        //    return json;
        //}

        //public static objLicense getLicense(string email)
        //{
        //    objLicense _objL = new objLicense();
        //    try
        //    {
        //        dynamic jsonObject = JObject.Parse(getjsonString("/api/license/check/&email=" + email));
        //        string token = jsonObject.token;
        //        //parse json string
        //        JavaScriptSerializer js = new JavaScriptSerializer();
        //        dynamic d = js.Deserialize<dynamic>(DeCodeJson(token));
        //        Dictionary<string, object> dic = d;

        //        _objL.email = d["email"];
        //        _objL.userName = d["userName"];
        //        _objL.password = d["password"];
        //        _objL.status = d["status"];
        //        _objL.key = d["key"];
        //        _objL.hardwareId = d["hardwareId"];
        //        _objL.expDate = DateTime.Parse(d["expDate"]);
        //        _objL.countDate = int.Parse(d["countDate"]);
        //    }
        //    catch (Exception _err)
        //    {
        //        clsUtilities.ShowMessageError("Error: " + _err);
        //    }
        //    return _objL;
        //}

        public static bool checkEmail(string email)
        {
            bool flag = false;

            try
            {
                JArray array = JArray.Parse(getjsonString("/api/license/check/&email=" + email));

                foreach (JObject o in array.Children<JObject>())
                {
                    string chkemail = o["email"].ToString();

                    flag = (chkemail == email) ? true : false;
                }
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err);
            }
            return flag;
        }

        public static bool checkLicense(string key)
        {
            bool flag = false;
            try
            {
                var jsontring = new WebClient().DownloadString("http://localhost:3000/api/license/&keychk=" + key);
                JArray objects = JArray.Parse(jsontring);
                //JArray array = JArray.Parse(getjsonString("/api/license/check/&email=" + email));

                foreach (JObject o in objects.Children<JObject>())
                {
                    string chkemail = o["key"].ToString();

                    flag = (chkemail == key) ? true : false;
                }
                //var jquer = JQuerry.Parse
                //jsona
                //JArray array = JArray.Parse(getjsonString("/api/license/&keychk=" + key));
                //JObject jObject = JObject.Parse(objects[0].ToString());
                //JToken memberName = jObject["license"].First["key"];
                //Console.WriteLine(memberName);
                //JObject o = JObject.Parse(objects[0].ToString());

                //flag = (o[0].ToString() == key) ? true : false;
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err);
            }
            return flag;
        }

        public static bool checkHardwareID(string hardwareid)
        {
            bool flag = false;
            try
            {
                JArray array = JArray.Parse(getjsonString("/api/license/hardwareidchk=" + hardwareid));
                JObject obj = JObject.Parse(array[0].ToString());

                string chkhardwareid = obj["hardwareId"].ToString();
                int countday = Convert.ToInt32(obj["countDate"].ToString());

                if (chkhardwareid == "" || chkhardwareid == null && countday == 0)
                    flag = false;
                else
                    if (chkhardwareid != "" || chkhardwareid != null && countday < 30)
                    flag = false;
                else
                    if (chkhardwareid != "" || chkhardwareid != null && countday == 30)
                    flag = true;
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }
            return flag;
        }
    }
}
