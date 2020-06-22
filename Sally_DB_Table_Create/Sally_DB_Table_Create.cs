using System;

namespace 의료IT공학과.데이터베이스
{
    class Sally_DB_Table_Create
    {
        static void Main(string[] args)
        {
            //xOleDB db = new xOleDB("Provider=Microsoft.ACE.OLEDB.12.0; " +
            //                      "Data Source=../../../DBFiles/SALLY_ENTERPRISE.accdb;  " +
            //                      "Persist Security Info=False");

            xLocalDB db = new xLocalDB("Provider=Microsoft.ACE.OLEDB.12.0; " +
                                  "Data Source=../../../DBFiles/SALLY_ENTERPRISE.accdb;  " +
                                  "Persist Security Info=False");

            db.Open();

            dropTables(db);
            createTables(db);
            insertRows(db);
            orders(db);

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

            if (DB_Query(db, queryStr) == false) return 0;

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

            if (DB_Query(db, queryStr) == false) return 0;

            Console.WriteLine("Table '{0}' Created.", tableName);

            return 1;
        }

        //------------------------------------------------------
        static int insertRow(iLocalDB db, string tableName, string dataStr)
        {

            string queryStr = string.Format("INSERT INTO {0} VALUES({1})", tableName, dataStr);

            if (DB_Query(db, queryStr) == false) return 0;

            Console.WriteLine("row inserted into '{0}'.", tableName);

            return 1;  //OK
        }

        //------------------------------------------------------
        static void dropTables(iLocalDB db)
        {
            int cnt = 0;

            cnt += dropTable(db, "PITCHER");
            cnt += dropTable(db, "RECIPE");
            //cnt += dropTable(db, "PITCHER");


            cnt += dropTable(db, "xORDER");
            cnt += dropTable(db, "SALESPERSON");
            //cnt += dropTable(db, "xORDER");

            cnt += dropTable(db, "NICKNAME");
            cnt += dropTable(db, "CUST_SPORT");
            cnt += dropTable(db, "CLASS_ATTRIBUTE");

            cnt += dropTable(db, "CUSTOMER");


            Console.WriteLine("{0} tables dropped.\n", cnt);
        }

        //-------------------------------------------------------
        static void createTables(iLocalDB db)
        {
            int cnt = 0;

            cnt += createTable(db, "RECIPE",
                                "Name   varchar(10)   not null, " +
                                "Sugar  real          not null, " +
                                "Lemon  int           not null, " +
                                "Water  real          not null, " +
                                "PRIMARY KEY(Name)");

            cnt += createTable(db, "PITCHER",
                                "xNumber      int          not null, " +   //less than 500
                                "xDate        varchar(10)  not null, " +   //yymmdd
                                "Recipe_used  varchar(10)  not null, " +   //subset of RECIPE[Name]
                                "xAmount       real         not null, " +   //현재 남아있는 음료수의 양
                                "PRIMARY KEY(xNumber), " +
                                "FOREIGN KEY(Recipe_used) REFERENCES RECIPE(Name)");

            cnt += createTable(db, "SALESPERSON",
                                "Name   varchar(20)  not null, " +   //whatever you call him or her
                                "PRIMARY KEY(Name)");

            cnt += createTable(db, "CUSTOMER",
                                "F_name   varchar(10)  not null, " +
                                "L_name   varchar(20)  not null, " +
                                "PRIMARY KEY(F_name, L_name)");

            cnt += createTable(db, "xORDER",
                                "Cust_f_name   varchar(10)  not null, " +
                                "Cust_l_name   varchar(20)  not null, " +
                                "Pitcher       int          not null, " +
                                "Salesperson   varchar(20)  not null, " +
                                "xDate         varchar(6)   not null, " +   //yymmdd
                                "xTime         varchar(4)   not null, " +   //hhmm
                                "xAmount       real         not null, " +   //9.99
                                "PRIMARY KEY(Cust_f_name, Cust_l_name, Pitcher, xTime), " +  //);
                                "FOREIGN KEY(Cust_f_name, Cust_l_name) REFERENCES CUSTOMER(F_name, L_name), " +
                                "FOREIGN KEY(Salesperson) REFERENCES SALESPERSON(Name)");

            cnt += createTable(db, "NICKNAME",
                                "Cust_f_name   varchar(10)  not null, " +
                                "Cust_l_name   varchar(20)  not null, " +
                                "Nickname      varchar(20)  not null, " +
                                "PRIMARY KEY(Cust_f_name, Cust_l_name), " +  //);
                                "FOREIGN KEY(Cust_f_name, Cust_l_name) REFERENCES CUSTOMER(F_name, L_name)");

            cnt += createTable(db, "CUST_SPORT",
                                "Cust_f_name     varchar(10)  not null, " +
                                "Cust_l_name     varchar(20)  not null, " +
                                "Favorite_sport  varchar(20)  not null, " +  //Soccer, Football, Basketball, Tennis, ski
                                "PRIMARY KEY(Cust_f_name, Cust_l_name, Favorite_sport), " +  //);
                                "FOREIGN KEY(Cust_f_name, Cust_l_name) REFERENCES CUSTOMER(F_name, L_name)");

            cnt += createTable(db, "CLASS_ATTRIBUTE",
                                "Relation_name    varchar(20)  not null, " +
                                "Attribute_name   varchar(20)  not null, " +
                                "xValue           varchar(20)  not null, " +
                                "PRIMARY KEY(Relation_name, Attribute_name)");


            Console.WriteLine("{0} tabels Created.\n", cnt);
        }

        //-------------------------------------------------
        static void insertRows(iLocalDB db)
        {
            int cnt = 0;
            //(db, "테이블명", "데이터 문자열")

            //RECIPE -------------------------------------------
            cnt += insertRow(db, "RECIPE", "'LA_Premium', 3.5, 4, 2.0");
            cnt += insertRow(db, "RECIPE", "'LA_Regular', 3.0, 3, 2.0");
            cnt += insertRow(db, "RECIPE", "'LA_Sweet',   4.0, 2, 2.0");

            //SALESPERSON -------------------------------------------
            cnt += insertRow(db, "SALESPERSON", "'Hulk'");
            cnt += insertRow(db, "SALESPERSON", "'Spiderman'");
            cnt += insertRow(db, "SALESPERSON", "'Black Widow'");

            //CUSTOMER ----------------------------------------------
            cnt += insertRow(db, "CUSTOMER", "'Anne',    'Smith'");
            cnt += insertRow(db, "CUSTOMER", "'Peter',   'Smith'");
            cnt += insertRow(db, "CUSTOMER", "'George',  'Brown'");
            cnt += insertRow(db, "CUSTOMER", "'Paul',    'Brown'");
            cnt += insertRow(db, "CUSTOMER", "'William', 'Woods'");
            cnt += insertRow(db, "CUSTOMER", "'Rebecca', 'Cuthbert'");
            cnt += insertRow(db, "CUSTOMER", "'Merry',   'Cuthbert'");

            //CUST_SPORT ----------------------------------------------
            cnt += insertRow(db, "CUST_SPORT", "'Anne',    'Smith',    'Tennis'");
            cnt += insertRow(db, "CUST_SPORT", "'Anne',    'Smith',    'Ski'");
            cnt += insertRow(db, "CUST_SPORT", "'Peter',   'Smith',    'Football'");
            cnt += insertRow(db, "CUST_SPORT", "'Peter',   'Smith',    'Soccer'");
            cnt += insertRow(db, "CUST_SPORT", "'George',  'Brown',    'Basketball'");
            cnt += insertRow(db, "CUST_SPORT", "'Paul',    'Brown',    'Soccer'");
            cnt += insertRow(db, "CUST_SPORT", "'Rebecca', 'Cuthbert', 'Tennis'");


            Console.WriteLine("{0} rows inserted.", cnt);
        }

        //---------------------------------------------------
        static void orders(iLocalDB db)
        {
            order(db, "Anne", "Smith", "LA_Sweet", "180810", "1200", 0.3);
            order(db, "William", "Woods", "LA_Regular", "180810", "1220", 0.6);
            //order(db, "Rebecca", "Cuthbert", "LA_Sweet",   "180810", "1230", 0.6);
            order(db, "Merry", "Cuthbert", "LA_Sweet", "180810", "1231", 0.6);
            order(db, "George", "Brown", "LA_Sweet", "180810", "1310", 0.3);
            order(db, "Paul", "Brown", "LA_Regular", "180810", "1312", 0.3);
            //order(db, "Peter",   "Smith",    "LA_Regular", "180810", "1320", 0.6);
            order(db, "George", "Brown", "LA_Sweet", "180810", "1340", 0.3);

            order(db, "Anne", "Smith", "LA_Sweet", "180811", "1130", 0.3);
            order(db, "William", "Woods", "LA_Regular", "180811", "1210", 0.6);
            //order(db, "Rebecca", "Cuthbert", "LA_Sweet",   "180811", "1225", 0.6);
            //order(db, "Merry",   "Cuthbert", "LA_Sweet",   "180811", "1226", 0.6);
            order(db, "George", "Brown", "LA_Sweet", "180811", "1300", 0.3);
            //order(db, "Paul",    "Brown",    "LA_Regular", "180811", "1305", 0.3);
            order(db, "Peter", "Smith", "LA_Regular", "180811", "1320", 0.6);
            order(db, "George", "Brown", "LA_Sweet", "180811", "1325", 0.3);

            order(db, "길동", "홍", "LA_Premium", "180811", "1350", 0.6);

        }

        //---------------------------------------------------
        static void order(iLocalDB db, string fName, string lName,
                          string choice, string yymmdd, string hhmm, double orderAmount)
        {

            Add_if_New_Customer(db, fName, lName);

            Int32 current_Pitcher = getPitcher(db, choice);

            Console.WriteLine("ordered PitcherNumber: {0}", current_Pitcher);

            //해당하는 Pitcher가 없으면 새로 제조
            if (current_Pitcher == 0) current_Pitcher = makeNewPitcher(db, yymmdd, choice);


            //Pitcher가 있으면 주문양이 충분한지 체크하고 충분치 않으면 새로 제조
            double currentAmount = getPitcherAmount(db, current_Pitcher);


            if (currentAmount < orderAmount)
            {
                Int32 new_Pitcher;
                double newAmount;

                new_Pitcher = makeNewPitcher(db, yymmdd, choice);
                newAmount = getPitcherAmount(db, new_Pitcher);

                updatePitcherAmount(db, current_Pitcher, 0.0);
                updatePitcherAmount(db, new_Pitcher, newAmount - (orderAmount - currentAmount));

                insertOrder(db, fName, lName, choice, yymmdd, hhmm, current_Pitcher, currentAmount);
                insertOrder(db, fName, lName, choice, yymmdd, hhmm, new_Pitcher, (orderAmount - currentAmount));
            }
            else
            {
                updatePitcherAmount(db, current_Pitcher, currentAmount - orderAmount);
                insertOrder(db, fName, lName, choice, yymmdd, hhmm, current_Pitcher, orderAmount);
            }
        }

        //------------------------------------------------
        static void Add_if_New_Customer(iLocalDB db, string fName, string lName)
        {
            string query = string.Format("SELECT * FROM CUSTOMER WHERE F_name='{0}' AND L_name='{1}'",
                                            fName, lName);

            DB_Query(db, query);

            if (db.HasRows == false)
            {
                query = string.Format("INSERT INTO CUSTOMER VALUES('{0}','{1}')", fName, lName);

                DB_Query(db, query);

                Console.WriteLine("Customer '{0}', '{1}' added.", fName, lName);
            }

        }

        //------------------------------------------------
        static void insertOrder(iLocalDB db, string fName, string lName, string choice,
                                string yymmdd, string hhmm, int pitcher, double serviceAmount)
        {
            //--Insert order into ORDER table ----------------
            string salesPerson = getSalesPerson(db, hhmm);

            string query = string.Format("INSERT INTO xORDER VALUES('{0}','{1}',{2},'{3}','{4}','{5}',{6})",
                                         fName, lName, pitcher, salesPerson, yymmdd, hhmm, serviceAmount);

            DB_Query(db, query);

            Console.WriteLine("Order Inserted: ('{0}','{1}',{2},'{3}','{4}','{5}',{6})",
                                         fName, lName, pitcher, salesPerson, yymmdd, hhmm, serviceAmount);
        }

        //-----------------------------------------------
        static string getSalesPerson(iLocalDB db, string hhmm)
        {
            Int32 hh = Convert.ToInt32(hhmm.Substring(0, 2));

            if (hh < 11) return "Sally";
            if (hh < 12) return "Hulk";
            if (hh < 13) return "Spiderman";
            if (hh < 14) return "Black Widow";
            return "Sally";
        }

        //-------------------------------------------------
        static void updatePitcherAmount(iLocalDB db, Int32 pitcherNumber, double xAmount)
        {
            string query = string.Format("UPDATE PITCHER SET xAmount={0} WHERE xNumber={1}", xAmount, pitcherNumber);

            DB_Query(db, query);
        }

        //---------------------------------------------------
        static double getPitcherAmount(iLocalDB db, Int32 pitcherNumber)
        {
            string query = string.Format("SELECT xAmount From PITCHER WHERE xNumber={0}", pitcherNumber);

            DB_Query(db, query);

            db.Read();
            double xAmount = Convert.ToDouble(db.GetData("xAmount").ToString());

            return xAmount;
        }

        //-----------------------------------------------------
        static double getRecipeAmount(iLocalDB db, string recipeName)
        {
            string query = string.Format("SELECT Water From RECIPE WHERE Name='{0}'", recipeName);

            DB_Query(db, query);

            if (db.HasRows == false)
            {
                Console.WriteLine("Receipe name error");
                Environment.Exit(1);
            }

            db.Read();
            double waterAmount = Convert.ToDouble(db.GetData("Water").ToString());

            return waterAmount;
        }

        //---------------------------------------------------
        static Int32 getPitcher(iLocalDB db, string choice)
        {
            string query = string.Format("SELECT xNumber FROM PITCHER WHERE Recipe_used='{0}' AND xAmount > 0", choice);

            DB_Query(db, query);

            if (db.HasRows == false) return 0;

            db.Read();
            Int32 xNumber = Convert.ToInt32(db.GetData("xNumber").ToString());

            return xNumber;
        }

        //---------------------------------------------------
        static Int32 makeNewPitcher(iLocalDB db, string yymmdd, string choice)
        {

            string query = "SELECT Max(xNumber) AS MaxNum FROM PITCHER";

            DB_Query(db, query);

            if (db.HasRows == false) return 1;

            db.Read();

            string maxNumStr = db.GetData("MaxNum").ToString();

            Int32 maxNumber;

            if (string.IsNullOrEmpty(maxNumStr))
            {
                maxNumber = 1;
            }

            else
            {
                maxNumber = Convert.ToInt32(db.GetData("MaxNum").ToString()) + 1;
            }

            double recipeAmount = getRecipeAmount(db, choice);

            query = string.Format("INSERT INTO PITCHER VALUES({0},'{1}','{2}',{3})",
                                  maxNumber, yymmdd, choice, recipeAmount);
            DB_Query(db, query);

            Console.WriteLine("new Pitcher {0}: '{1}' made", maxNumber, choice);

            return maxNumber;
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