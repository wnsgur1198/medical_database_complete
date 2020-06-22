using System;
using System.Windows;


namespace 의료IT공학과.데이터베이스
{

	class SQL_Test
	{
		//static LocalDB db = new LocalDB("Provider=Microsoft.ACE.OLEDB.12.0; " +
		//					  "Data Source=../../../DBFiles/STUDENT_ENROLL.accdb;  " +
		//					  "Persist Security Info=False");
		static LocalDB db = new LocalDB("Provider=Microsoft.ACE.OLEDB.12.0; " +
							  "Data Source=../../../DBFiles/SALLY_ENTERPRISE.accdb;  " +
							  "Persist Security Info=False");

		static void Main(string[] args)
		{
			
			db.Open();

			string query = "SELECT * FROM RECIPE";

			//query = "SELECT A.Student_name FROM STUDENT A WHERE A.Grade_level='GR' AND NOT EXISTS(SELECT * FROM ENROLLMENT B WHERE A.SID=B.Student_number AND B.Class_name IN(SELECT C.Class_name FROM ENROLLMENT C WHERE B.Class_name = C.Class_name AND C.Student_number IN (SELECT D.SID FROM STUDENT D WHERE C.Student_number=D.SID AND D.Grade_level<>'GR')))";

			//RECIPE가 모두 몇 종류인지 개수를 출력
			query = "SELECT COUNT(*) AS Recipe_count FROM RECIPE";
			Do_Query(query);

			//고객들이 좋아하는 스포츠의 이름을 중복되지 않게 출력
			query = "SELECT DISTINCT Favorite_sport FROM CUST_SPORT";
			Do_Query(query);

			//고객이 좋아하는 스포츠의 종료와 각 스포츠를 좋아하는 고객의 숫자를 출력
			query = "SELECT Favorite_sport, COUNT(*)AS xCount FROM CUST_SPORT GROUP BY Favorite_sport";
			Do_Query(query);

			//판매된 음료수의 총량
			query = "SELECT ROUND(SUM(xAmount),2) AS TotalSale FROM xORDER";
			Do_Query(query);

			//레시피가 LA_SWEET인 Pitcher에서 주문된 총 음료수(Lemonade)의 양
			query = "SELECT ROUND(SUM(xAmount),2) AS La_Sweet FROM xORDER WHERE Pitcher IN(select xNumber FROM PITCHER WHERE Recipe_used='LA_Sweet')";
			Do_Query(query);

			//고객 중에서 한번도 주문을 하지 않은 사람의 이름(F_name, L_name)을 출력
			query = "SELECT F_name, L_Name FROM CUSTOMER WHERE NOT EXISTS (SELECT * FROM xORDER WHERE F_name=Cust_f_name AND L_name=Cust_l_name)";
			Do_Query(query);

			//주문을 한번 이상 한 고객의 이름(Cust_f_name, Cust_l_name)과 주문한 음료수의 총량을 출력
			query = "SELECT Cust_f_name, Cust_l_name, ROUND(SUM(xAmount),2) AS Total FROM xORDER GROUP BY Cust_f_name, Cust_l_name";
			Do_Query(query);

			//주문한 음료수의 양이 1.0이상인 고객의  이름(Cust_f_name, Cust_l_name)과 주문한 음료수의 총량을 출력
			query = "SELECT * FROM (SELECT Cust_f_name, Cust_l_name, ROUND(SUM(xAmount),2) AS Total FROM xORDER GROUP BY Cust_f_name, Cust_l_name) WHERE Total >= 1.0";
			Do_Query(query);

			//사용된 레몬의 총 개수를 출력
			query = "SELECT ROUND(SUM(Lemon),2) AS Lemons_used FROM PITCHER P, RECIPE R WHERE P.Recipe_used=R.Name";
			Do_Query(query);

			//레시피별 판매량을 출력
			query = "SELECT P.Recipe_used, ROUND(SUM(O.xAmount), 2) AS x_Amount FROM xORDER O, PITCHER P WHERE O.Pitcher=P.xNumber GROUP BY P.Recipe_used";
			Do_Query(query);



         db.Close();
			
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

				while (db.Read())
				{
					for (int i = 0; i < db.FieldCount; i++)
					{
						Console.Write(db.GetData(i) + "\t");
					}
					Console.WriteLine();
				}
				Console.WriteLine("==========================================================\n");
			}

			return true;
		}
	}

}