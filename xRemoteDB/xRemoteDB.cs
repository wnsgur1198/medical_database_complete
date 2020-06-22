using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;

namespace 의료IT공학과.데이터베이스
{
   public class xRemoteDB
   {
      //HttpWebRequest wReq = null;
      //HttpWebResponse wRes = null;
      String url;
      XmlDocument xdoc = null;
      XmlNodeList rows = null;
      

      public xRemoteDB(String url)
      {
         this.url = url.Trim();
         xdoc = new XmlDocument();

      }//생성자
      //--------------------------------------------------------
      private String makeXMLString(String sqlStr)
      {
         String trimedSqlStr = sqlStr.Trim();

         int index = trimedSqlStr.IndexOf(' ');

         String action = trimedSqlStr.Substring(0, index);

         return "<sql action='" + action + "'><![CDATA[" + trimedSqlStr.Substring(index) + "]]></sql>";
      }

      //--------------------------------------------------------
      public string Query(String sqlStr)
      {
         try
         {
            HttpWebResponse wRes = null;
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            wReq.Method = "POST"; // 전송 방법 "GET" or "POST"

            byte[] byteArray = Encoding.UTF8.GetBytes(makeXMLString(sqlStr));

            Stream dataStream = wReq.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            using (wRes = (HttpWebResponse)wReq.GetResponse())
            {
               Stream respPostStream = wRes.GetResponseStream();
               StreamReader readerPost = new StreamReader(respPostStream, Encoding.GetEncoding("utf-8"), true);

               try
               {
                  
                  xdoc.LoadXml(readerPost.ReadToEnd());
                  if (xdoc.DocumentElement.GetAttribute("code").Equals("ERROR"))
                  {
                     throw new Exception(xdoc.DocumentElement.InnerText);
                  }

                  //Console.WriteLine(xdoc.InnerXml);

                  rows = xdoc.DocumentElement.GetElementsByTagName("ROW");
 
               }
               catch (Exception ex)
               {
                  return (ex.Message);
               }
               
            }//using
         }
         catch (WebException ex)
         {
            if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
            {
               var resp = (HttpWebResponse)ex.Response;
               if (resp.StatusCode == HttpStatusCode.NotFound)
               {
                  return ("query: WebException Protocol Error: NotFound Error\n" + ex.Message);
               }
               else
               {
                  return ("query: WebException Protocol Error: 기타 Error\n" + ex.Message);
               }
               
            }
            else
            {
               return ("query: WebException: Non Protocol Error\n" + ex.Message);
               
            }
         }//catch
         catch (Exception ex)
         {
            return ("query: 알 수 없는 Error\n" + ex.Message);
            
         }

         return null;

      }//query;

      //-------------------------------
      public bool HasRows
      {
           get
           {
               if (rows == null) return false;
               if (rows.Count > 0) return true;
               return false;
           }

      }

      //-------------------------------
      public int FieldCount
      {
          get {
             if (!HasRows) return 0;
             XmlNodeList cols = rows[0].ChildNodes;
             if (cols == null) return 0;
             return cols.Count;
          }
      }

      //-------------------------------
      public int RowCount
      {
         get {
            if (!HasRows) return 0;
            return rows.Count;
         }
      }

      //---------------------------------
      public string GetName(int index)
      {
            if (!HasRows) return "";
            
            if (index < 0 || index > FieldCount) return "";
            XmlNodeList cols = rows[0].ChildNodes;
            return cols[index].LocalName;

      }

      //---------------------------------------------
      public string GetData(string elemName, int indx=0)
      {

         try
         {
            return rows[indx].SelectSingleNode(elemName).FirstChild.Value;
         }
         catch (Exception ex)
         {
            //Console.WriteLine("getValue exception: {0}, index: {1}\n{2}", elemName, indx, rows[indx].InnerXml);
            return "null";
         }
      }


   }//class
}//ns 
