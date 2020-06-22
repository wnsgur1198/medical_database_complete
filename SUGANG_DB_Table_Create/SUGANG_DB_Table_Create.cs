using System;

namespace 의료IT공학과.데이터베이스
{
    class SUGANG_DB_Table_Create
    {
        static void Main(string[] args)
        {


            xLocalDB db = new xLocalDB("Provider=Microsoft.ACE.OLEDB.12.0; " +
                                       "Data Source=../../../DBFiles/SUGANG_DB.accdb;  " +
                                       "Persist Security Info=False");

            //xLocalDB db = new xLocalDB(@"Data Source=(LocalDB)\MSSQLLocalDB;
            //                          AttachDbFilename=D:\1_pjkim\Backup-0\1강의\Database\DatabaseProgs\DBFiles\SUGANG_DB.mdf;
            //                          Integrated Security=True");


            db.Open();

            dropTables(db);

            createTables(db);
            insertRows(db);

			db.Close();


        }//Main

        //--------------------------------------------------
        static bool? tableExists(iLocalDB db, string tableName)
        {

            string result = db.Query(string.Format("select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME='{0}'", tableName));

            if (result != null) return null;

            return db.HasRows;

        }

        //----------------------------------------------------
        static int dropTable(iLocalDB db, string tableName)
        {
            //bool? res = tableExists(db, tableName);

            //if (res == null)
            //{
            //	Console.WriteLine("### Query Error in func dropTable.tableExists()...");
            //	return 0;
            //}
            //else if (res == false)
            //{
            //	Console.WriteLine("### Table '{0}'은 존재하지 않습니다.", tableName);
            //	return 0;
            //}

            string queryStr = string.Format("DROP TABLE {0}", tableName);

            if (!DB_Query(db, queryStr)) return 0;

            Console.WriteLine("Table '{0}' dropped.", tableName);

            return 1;
        }

        //----------------------------------------------------

        static int createTable(iLocalDB db, string tableName, string tableDesign)
        {
            //bool? res = tableExists(db, tableName);

            //if (res == null)
            //{
            //	Console.WriteLine("### Query Error in func createTable.tableExists()...");
            //	return 0;
            //}
            //else if (res == true)
            //{
            //	Console.WriteLine("### Table '{0}'은 이미 존재합니다.", tableName);
            //	return 0;
            //}


            string queryStr = string.Format("CREATE TABLE {0} ({1})", tableName, tableDesign);

            if (!DB_Query(db, queryStr)) return 0;

            Console.WriteLine("Table '{0}' Created.", tableName);

            return 1;
        }

        //------------------------------------------------------
        static int insertRow(iLocalDB db, string tableName, string dataStr)
        {

            string queryStr = string.Format("INSERT INTO {0} VALUES({1})", tableName, dataStr);

            if (!DB_Query(db, queryStr)) return 0;

            Console.WriteLine("row inserted into '{0}'.", tableName);

            return 1;  //OK
        }

        //------------------------------------------------------
        static void dropTables(iLocalDB db)
        {
            int cnt = 0;

            cnt += dropTable(db, "xSTUDENTS");
            cnt += dropTable(db, "xDEPARTMENT");
            cnt += dropTable(db, "xSTUDENT_STATUS");
            cnt += dropTable(db, "xTEACHERS");

            cnt += dropTable(db, "xENROLL_CLASS");
            cnt += dropTable(db, "xOPEN_CLASS");

            cnt += dropTable(db, "xCURRICULUM");
            cnt += dropTable(db, "xCLASSES");


            Console.WriteLine("{0} tables dropped.\n", cnt);
        }

        //-------------------------------------------------------
        static void createTables(iLocalDB db)
        {
            int cnt = 0;

            cnt += createTable(db, "xSTUDENT_STATUS",
                                @"xStatus_code        int         not null, 
                                  xStatus_title       nvarchar(5)  not null,
                                  PRIMARY KEY(xStatus_code)");

            cnt += createTable(db, "xDEPARTMENT",
                                @"xDept_code        varchar(10)   not null, 
                                  xDept_name        varchar(30)   not null,
                                  xDept_use         int           not null,
                                  xDept_year        varchar(4)    not null,
                                  PRIMARY KEY(xDept_code)" );

            cnt += createTable(db, "xSTUDENTS",
                                @"xHakbun        varchar(8)   not null, 
                                  xName          varchar(20)  not null, 
                                  xPassword      varchar(20)  not null, 
                                  xDept          varchar(10)  not null, 
                                  xStatus        int          not null, 
                                  xAddress       varchar(50)  not null, 
                                  xEmail         varchar(30), 
                                  xPhone         varchar(20), 
                                  PRIMARY KEY(xHakbun), 
                                  FOREIGN KEY(xDept) REFERENCES xDEPARTMENT(xDept_code), 
                                  FOREIGN KEY(xStatus) REFERENCES xSTUDENT_STATUS(xStatus_code)");

            cnt += createTable(db, "xTEACHERS",
                                @"xID            varchar(8)   not null, 
                                  xName          varchar(20)  not null, 
                                  xPassword      varchar(20)  not null, 
                                  xDept          varchar(10)  not null, 
                                  xEMail         varchar(30), 
                                  xPhone         varchar(20), 
                                  PRIMARY KEY(xID)" );

            cnt += createTable(db, "xCLASSES",
                                @"xCode          varchar(9)   not null, 
                                  xName          varchar(20)  not null, 
                                  xCredit        int  not null, 
                                  xHours         int  not null, 
                                  xYear          varchar(4)     not null, 
                                  PRIMARY KEY(xCode)");

            cnt += createTable(db, "xCURRICULUM",
                               @"xYear          varchar(4)  not null, 
                                 xCode          varchar(9)  not null, 
                                 xPre_class     varchar(9), 
                                 PRIMARY KEY(xYear, xCode), 
                                 FOREIGN KEY(xPre_class) REFERENCES xCLASSES(xCode), 
                                 FOREIGN KEY(xCode) REFERENCES xCLASSES(xCode)");

            cnt += createTable(db, "xOPEN_CLASS",
                                @"xCurri_year    varchar(4)  not null, 
                                  xCode          varchar(9)  not null, 
                                  xOpen_year     varchar(4)  not null, 
                                  xOpen_semester int         not null, 
                                  xDivision      int,  
                                  xClass_time    varchar(30) not null, 
                                  xTeacher       varchar(8)  not null,
                                  PRIMARY KEY(xCurri_year, xCode),
                                  FOREIGN KEY(xTeacher) REFERENCES xTEACHERS(xID), 
                                  FOREIGN KEY(xCurri_year, xCode) REFERENCES xCURRICULUM(xYear, xCode)");

            cnt += createTable(db, "xENROLL_CLASS",
                                @"xCurri_year    varchar(4)   not null, 
                                  xCode          varchar(9)  not null, 
                                  xHakbun        varchar(8)  not null, 
                                  xGrade         varchar(1)  not null,  
                                  PRIMARY KEY(xCurri_year, xCode, xHakbun),  
                                  FOREIGN KEY(xCurri_year, xCode) REFERENCES xOPEN_CLASS(xCurri_year, xCode)");




            Console.WriteLine("{0} tabels Created.\n", cnt);
        }

        //-------------------------------------------------
        static void insertRows(iLocalDB db)
        {
            int cnt = 0;
			//(db, "테이블명", "데이터 문자열")

			//xSTUDENT_STATUS -------------------------------------------
			cnt += insertRow(db, "xSTUDENT_STATUS", "0, '재학'");
			cnt += insertRow(db, "xSTUDENT_STATUS", "1, '휴학'");
			cnt += insertRow(db, "xSTUDENT_STATUS", "2, '자퇴'");
			cnt += insertRow(db, "xSTUDENT_STATUS", "3, '제적'");
			cnt += insertRow(db, "xSTUDENT_STATUS", "4, '수료'");
			cnt += insertRow(db, "xSTUDENT_STATUS", "9, '졸업'");

			//xDEPARTMENT -------------------------------------------
			cnt += insertRow(db, "xDEPARTMENT", "'615', '의료IT공학과', 1, '2012'");
			cnt += insertRow(db, "xDEPARTMENT", "'620', '의공학부', 0, '2012'");
			cnt += insertRow(db, "xDEPARTMENT", "'621', '제약생명공학과', 0, '2012'");
			cnt += insertRow(db, "xDEPARTMENT", "'622', '의료공간디자인', 0, '2012'");

			Console.WriteLine("{0} rows inserted.------------", cnt);

			cnt = 0;

			//xStudents ---------------------------------------------
			cnt += insertStudent(db, "19615001", "강만기", "kanxxx1234", "615", 0, 
				                     "서울시 종로구 효자동로 123번지", "kang@naver.com", "010-123-4567" );

            cnt += insertStudent(db, "19615002", "김한길", "Kim_1234!", "615", 0,
                                     "대전시 서구 관저동로 333번지", "khk@naver.com", "010-543-2525");

            cnt += insertStudent(db, "19615003", "민상호", "Min@1234#", "615", 0,
                                     "대구시 달성구 달성공원로 543번지", "msh@naver.com", "010-222-3355");

            cnt += insertStudent(db, "19615004", "박길동", "Park999!", "615", 0,
                                     "충남 세종시 신세종로 001번지", "pkdong@naver.com", "010-544-2216");

            cnt += insertStudent(db, "18615001", "고민봉", "MinBong1234!", "615", 0,
                                     "충북 청주시 상당1로 555번지", "minbong@naver.com", "010-876-4576");

            cnt += insertStudent(db, "18615002", "나경우", "Na_1234!", "615", 0,
                                     "대전시 중구 태평로 333번지", "Nawoo@naver.com", "010-377-7589");

            cnt += insertStudent(db, "18615003", "도미노", "Domino7658@", "615", 0,
                                     "서울시 용산구 이촌동로 122번지", "mino@naver.com", "010-555-6756");

            cnt += insertStudent(db, "17615001", "구현미", "KuHyun9876!", "615", 0,
                                     "부산시 동래구 온천로 428번지", "hyun0302@naver.com", "010-231-2256");

            cnt += insertStudent(db, "17615002", "송상수", "XangSu2234!", "615", 0,
                                     "경기도 성남시 수정구 태평1로 234번지", "sss@naver.com", "010-786-1985");

            cnt += insertStudent(db, "17615003", "안민수", "An$1234!", "615", 0,
                                     "강원도 춘천시 효자동로 599번지", "AnMin@naver.com", "010-466-4656");

            cnt += insertStudent(db, "16615001", "김혁재", "hjKim8234!", "615", 0,
                                     "경남 합천군 쌍백로 468번지", "hjhj@naver.com", "010-436-2545");

            cnt += insertStudent(db, "16615002", "노수영", "No!1234!", "615", 0,
                                     "강원도 평창군 용평로 976번지", "NoNoNo@naver.com", "010-424-2323");


            Console.WriteLine("{0} students inserted.--------------", cnt);
        }

		//-----------------------------------------------
		static int insertStudent(iLocalDB db, string hakbun, string name, string pass, string dept, int status, string addr, string email, string phone)
		{
            hakbun = hakbun.Trim();
            if (IsValid_IDENTIFIER(db, hakbun) == false) return 0;
            if (IsNewHakbun(db, hakbun) == false) return 0;  //새 학생 추가인 경우에만 실행

            if (IsValid_PEOPLE_NAME(name) == false) return 0;
            if (IsValid_PASSWORD(pass) == false) return 0;

            string str = string.Format("'{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}'", 
				                        hakbun, name, pass, dept, status, addr, email, phone);
		
			return insertRow(db, "xSTUDENTS", str);
		}

        //--------------------------------------------------
        static bool IsValid_PEOPLE_NAME(string name)
        {
            if (string.IsNullOrEmpty(name)) return Error("사람 이름문자열이  null이거나 비어있습니다.");
            if (name.Length > 20) return Error("사람 이름은 20문자를 초과할 수 없습니다.");
            if (stringContains_Oneof(name, " \t\r\n")) return Error("사람 이름은 공백이 허용되지 않습니다.");
            return true;
        }

        //--------------------------------------------------
        static bool IsValid_PASSWORD(string pass)
        {
            int errorCount = 0;

            if (string.IsNullOrEmpty(pass)) errorCount++;
            else if (pass.Length > 20 || pass.Length < 8) errorCount++;
            else if (stringContains_Oneof(pass, " \t\r\n")) errorCount++;
            else if (!stringContains_Oneof(pass, "!@#$%&*")) errorCount++;
            else if (!stringContains_Oneof(pass, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) errorCount++;
            else if (!stringContains_Oneof(pass, "abcdefghijklmnopqrstuvwxyz")) errorCount++;
            else if (!stringContains_Oneof(pass, "0123456789")) errorCount++;

            if (errorCount > 0) return Error("비밀번호는 대소문자 숫자 특수문자를 포함해서 10문자 ~ 20문자 사이의 길이여야 합니다.");
            return true;
        }

        //---------------------------------------------------
        static bool IsNewHakbun(iLocalDB db, string hakbun)
        {
            string query = string.Format("SELECT xHakbun FROM xSTUDENTS WHERE xHakbun='{0}'", hakbun);
            string res = db.Query(query);
            if (res != null)
            {
                Console.WriteLine(res);
                return false;
            }

            if (db.HasRows) return Error("같은 학번이 이미 존재합니다." + hakbun);

            return true;
        }

        //-----------------------------------------------
        static bool IsValid_IDENTIFIER(iLocalDB db, string id)
        {
            if (string.IsNullOrEmpty(id)) return Error("학번이 null이거나 빈문자열입니다.");
            if (id.Length != 8) return Error("학번의 길이는 8문자이어야 합니다");
            if (IsNumericString(id.Substring(0, 2)) == false) return Error("학번의 처음 두 문자는 년도를 나타내야 합니다.(예: 19)");
            if (id.Substring(5, 3).Equals("000")) return Error("'000'은 올바른 학번의 일련번호가 아닙니다");
            if (IsNumericString(id.Substring(5, 3)) == false) return Error("학번의 마지막 세 문자는 일련번호여야 합니다.(예: 001)");
            if (IsValidDeptCode(db, id.Substring(2, 3)) == false) return Error("학번의 3,4,5번째 문자에 일치하는 학과코드가 없습니다.");

            return true;
        }

        //-----------------------------------------------
        static bool Error(string msg)
		{
			Console.WriteLine("*** Error: " + msg + " ***");
			return false;
		}

		//-----------------------------------------------
		static bool IsValidDeptCode(iLocalDB db, string code)
		{
			string query = string.Format("SELECT * FROM xDEPARTMENT WHERE xDept_code='{0}'", code);
			string res = db.Query(query);
			if(res != null)
			{
				Console.WriteLine(res);
				return false;
			}
			
			if (db.HasRows) return true;

			return false;
		}

		//-----------------------------------------------
		static bool IsNumericString(string str)
		{
			for(int i=0; i<str.Length; i++)
			{
		
                if("0123456789".Contains(str.Substring(i, 1)) == false) return false;
			}
			return true;
		}
		//------------------------------------------------
		static bool stringContains_Oneof(string str, string oneof)
		{
			for(int i=0; i<oneof.Length; i++)
			{
				if (str.Contains(oneof.Substring(i, 1))) return true;
			}

			return false;
		}

		//------------------------------------------------
		static bool DB_Query(iLocalDB db, string query)
        {
            string err_msg = db.Query(query);

            if (err_msg != null)
            {
                Console.WriteLine("Error\n" + err_msg + "\n" + query);
                return false;
            }
            return true;
        }

    }//class
}//ns