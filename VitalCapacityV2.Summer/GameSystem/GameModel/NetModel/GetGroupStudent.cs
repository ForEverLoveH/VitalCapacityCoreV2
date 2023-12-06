using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace VitalCapacityV2.Summer.GameSystem.GameModel
{
    public class GetGroupStudent
    {
        /// <summary>
        ///
        /// </summary>
        public Results Results { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Error { get; set; }

        public static string[] CheckJson(string json)
        {
            string[] strs = new string[2];
            string ResultISNull = "0";
            string ResultError = "";
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var jsObject = JObject.Parse(json);
                    foreach (JToken child in jsObject.Children())
                    {
                        var property1 = child as JProperty;
                        if (property1.Name == "Error")
                        {
                            if (string.IsNullOrEmpty(property1.Value.ToString()))
                            {
                                ResultISNull = "1";
                            }
                            else
                            {
                                ResultISNull = "0";
                                ResultError = property1.Value.ToString();
                            }
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    ResultISNull = "0";
                    ResultError = "";
                }
            }
            strs[0] = ResultISNull;
            strs[1] = ResultError;
            return strs;
        }
    }
}