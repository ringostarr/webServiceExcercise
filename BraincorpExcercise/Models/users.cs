using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BraincorpExcercise.Models
{
    public class user
    {
        public int id { get; set; }
        public string name { get; set; }
        public int gid { get; set; }

        public string home { get; set; }

        public string comment { get; set; }
        public string shell { get; set; }

        //unit Test Stub

        /*
#if Test_controller
        public static void Main(string[] args)
        {
            user usr = new user();
            usr.id = 1;
            usr.name = "Akshay";
            usr.gid = 1;
            usr.home = "Fremont";
            usr.comment = "random comment";
            usr.shell = "/bin/shell";
            Console.Write("\n ---- User Info -------\n");
            Console.Write("id:"+usr.id+"\n");
            Console.Write("name:"+usr.name+"\n");
            Console.Write("gid:"+usr.gid+"\n");
            Console.Write("comment:"+usr.comment+"\n");
            Console.Write("home:"+usr.home+"\n");
            Console.Write("shell:"+usr.shell+"\n");
            Console.Write("------End of User Info-------\n");

        }
#endif*/
    }



}