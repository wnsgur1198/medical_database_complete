using System;
using System.Text;
using System.Windows;


namespace 의료IT공학과.데이터베이스
{

	class SQL_Test
    {


        //static xLocalDB db = new xLocalDB("Provider=Microsoft.ACE.OLEDB.12.0; " +
        //                          "Data Source=../../../DBFiles/SUGANG_DB.accdb;  " +
        //                          "Persist Security Info=False");

        //static xLocalDB db = new xLocalDB(@"Data Source=(LocalDB)\MSSQLLocalDB;
        //                              AttachDbFilename=D:\1_pjkim\Backup-0\1강의\Database\DatabaseProgs\DBFiles\SUGANG_DB.mdf;
        //                              Integrated Security=True");

        static xRemoteDB db = new xRemoteDB("http://localhost:8080");

        static void Main(string[] args)
		{

			//db.Open();

			string query = "select * from xDEPARTMENT";
           Do_Query(query);

         //db.Close();
			
		}

		static bool Do_Query(string query)
		{
			string error_msg = db.Query(query);
            
         if (error_msg != null)
			{
				MessageBox.Show(query + "\n\n" + error_msg, "SQL Error");
				return false;
			}

			Console.WriteLine("\n" + query);

			if (db.HasRows)
			{
				Console.WriteLine("==========================================================");
				for (int i = 0; i < db.FieldCount; i++)
				{
					Console.Write(db.GetName(i) + "\t");
				}
				Console.WriteLine();
				Console.WriteLine("==========================================================");

				for(int n=0; n<db.RowCount; n++)
				{
					for (int i = 0; i < db.FieldCount; i++)
					{
                        string str = db.GetData(db.GetName(i), n).ToString();

                        Console.Write(str + "\t");
					}
					Console.WriteLine();
				}
				Console.WriteLine("==========================================================\n");
			}

			return true;
		}
	}

}