using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using api_gamebai.Models;
using System.IO;

namespace api_gamebai.Controllers
{
    public class DemoController : ApiController
    {
        [HttpGet]
        public string Main()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", 20);
                Console.WriteLine("ket noi den server chat...");
                Stream stream = client.GetStream();
                //while (true)
                //{

                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;
                    //Console.Write("nhap: ");
                    string strName = "khoa";
                    writer.WriteLine(strName);
                    string recive = reader.ReadLine();
                    Console.WriteLine(recive);
                // }
                return "kết nối thành công";
            }
            catch (Exception)
            {

                return "kết nối lỗi";
            }
        }
    }
}