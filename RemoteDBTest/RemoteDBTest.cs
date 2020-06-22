using System;

namespace 의료IT공학과.데이터베이스
{
   class SUGANG_DB_Table_Create
   {
      static xRemoteDB db = new xRemoteDB("http://localhost:8080");

      static void Main(string[] args)
      {
         string errMsg = db.Query("Select * from xstudents");
         if(errMsg != null)
         {
            Console.WriteLine("Error: " + errMsg);
         }
      }
   }

}