using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;     // JObject 쓰기 위함

namespace 의료IT공학과.데이터베이스
{

    class JSON_Test
    {
        static void Main(string[] args)
        {
            //JSON string 만들기
            //Person p = new Person { Id = 1, Name = "Alex" };  // Object Initializer

            Person p = new Person();
            p.Id = 2;
            p.Name = "홍길동";

            //===========================================================
            //C#의 클래스 객체를 JSON형태의 문자열로 변환(네트워크를 통한 전송 등을 위하여)
            string jsonString = JsonConvert.SerializeObject(p);
            Console.WriteLine("C#클래스 객체를 JSON문자열로 변환한 결과:");
            Console.WriteLine("{0}\n",jsonString);

            //===========================================================
            //JSON형태의 문자열을 C#의 클래스 객체로 변환(일치하는 클래스가 있어야 함)
            Person pObj = JsonConvert.DeserializeObject<Person>(jsonString);
            Console.WriteLine("JSON문자열을 다시 C#객체로 변환한 결과:");
            Console.WriteLine("id: {0},  이름: {1}\n", pObj.Id, pObj.Name);

            //===========================================================
            //당연히 문자열 데이터를 직접 코딩하여 C#클래스 객체로 바꿀 수도 있음.

            string str1 = "{Id: 99,  Name: '길길동'}";
            

            Person pObj2 = JsonConvert.DeserializeObject<Person>(str1);
            Console.WriteLine("데이터로 읽은 JSON문자열을 C#객체로 변환한 결과:");
            Console.WriteLine("id: {0},  이름: {1}\n", pObj2.Id, pObj2.Name);

            //===========================================================
            //클래스에 배열이 선언되어 있는 경우 JSON문자열로 변환하는 예
            Person2 p2 = new Person2();
            p2.hakbun = "12345006";
            p2.phone[0] = "010-123-4567";
            p2.phone[1] = "042-600-1234";
            p2.phone[2] = "041-730-9999";

            string jsonString2 = JsonConvert.SerializeObject(p2);

            Console.WriteLine("배열이 있는 경우 JSON 문자열:\n{0}\n", jsonString2);

            // Person2 - json->C#
            Person2 pObj3 = JsonConvert.DeserializeObject<Person2>(jsonString2);
            Console.WriteLine("Person2 - 데이터로 읽은 JSON문자열을 C#객체로 변환한 결과:");
            Console.WriteLine("id: {0},  이름: {1}\n", pObj3.hakbun, pObj3.phone[0]);

            //===========================================================

            Console.WriteLine("특정 클래스와 일치하지 않는(매치되는 클래스가 없는) JSON문자열을 객체로 바꾸기");
            string str2 = @"{ 
                addr: '대전시 서구 관저동 건양대학교',                
                building: '죽헌정보관',
                floor: 7,
                phone: ['042-123-4567', '042-123-9876']
            }";

            JObject jobj = JObject.Parse(str2);  //JOBJECT: using Newtonsoft.Json.Linq; 추가 필요

            Console.WriteLine("JObject 객체로부터 출력:");

            JToken addrToken = jobj.GetValue("addr"); //혹은 jobj["addr"]; = '대전시 서구 관저동 건양대학교'의 값을 가지는 토큰

            Console.WriteLine("주소: {0},  건물: {1}, 층: {2},  전화1: {3}\n",
                            addrToken.ToString(),
                            jobj.GetValue("building").ToString(), //출력문에서 ToString()은 default(생략가능)
                            jobj["floor"],  // + 1,  그러나 이 값은 정수값은 아니므로 수 연산에 사용할 수 없음
                            jobj["phone"][0]);

            Console.WriteLine("JObject 객체로부터 데이터 타입을 지정하여 데이터 획득:");
            string addrStr = jobj.Value<string>("addr");
            Console.WriteLine("주소: {0},  건물: {1}, 층: {2},  전화1: {3}\n",
                            addrStr,
                            jobj.Value<string>("building"),  //이 경우 앞의 방법이 더 간단.
                            jobj.Value<int>("floor") + 1,  // 이 경우 값이 정수 이므로 연산을 할 수 있음.
                            jobj.GetValue("phone")[0]);  //배열의 값을 직접 타입변환하는 방법은 모르겠다.

            //jobj.GetValue("phone").Values(0);

        }
    }//class

    class Person
    {
        //public int Id { get; set; }
        //public string Name { get; set; }

        public int Id;
        public string Name;
    }

    class Person2
    {
        public string hakbun;
        public string[] phone;

        public Person2()
        {
            phone = new string[3];
        }
    }
}//ns