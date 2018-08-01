using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BraincorpExcercise.Models
{
    public class groups
    {
        public int gid { get; set; }
        public string name { get; set; }
        public List<string> members { get; set; }
        public groups(){
            members = new List<string>();
            }
    }
#if test_grp
    static void main(string[] args)
    {

    }
#endif
}