using BraincorpExcercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace BraincorpExcercise.Services
{
    public class repository
    {
         private string pwdPath= "/etc/passwd.txt";
         private string grpPath= "/etc/groups.txt";
        private List<user> Userlist = new List<user>();
        private List<groups> GroupList = new List<groups>();
        //constructor , placeholder for future initializations (for now)
        public repository()
        {
           // Maybe some initial setup , maybe populate repository here
            
        }

        /*---
        //---            /ETC/PASSWD logic begins here
        //--
        //--*/
        //Sets custom path for /etc/passwd provided in QueryString , Cannot figure out how to do it using command line
        //because there is no .exe
        public bool SetUserPath(string path)
        {
            if (File.Exists(path))
            {

                grpPath = path; return true;
            }


            else
                return false;
        }

        
        //Parse every line read from etc/passwd , store them into a data structure and return the data structure
        public user ParseUserString(string line)
        {
            user parseduser = new user();
            string[] attr = line.Split(':');
            if (attr.Length != 7)//malformed line
                throw new HttpException("Malformed passwd File");
            
            parseduser.name = attr[0];//always name
            int id;
            Int32.TryParse(attr[2], out id); // always id
            parseduser.id = id;
            Int32.TryParse(attr[3], out id);// always gid,reusing id variable
            parseduser.gid = id;
            parseduser.comment = attr[4];//always comment
            parseduser.home = attr[5];//always home
            parseduser.shell = attr[6];//always shell


            return parseduser;

        }

        ///populate Userlist from passwd file
        public void PopulateUsers()
        {
            Userlist.RemoveRange(0, Userlist.Count);
            string line;
            //string p = System.AppDomain.CurrentDomain.BaseDirectory;
            StreamReader file = new System.IO.StreamReader(pwdPath);
            while((line = file.ReadLine())!=null)
            {
                // supply each line to Function which parses the line
                user newuser = ParseUserString(line);
                Userlist.Add(newuser);
            }
        }

        // Parse each line recieved when populating groups and return a group data structure
        public groups ParseGroupString(string line)
        {
            string[] attr = line.Split(':');
            groups newgrp = new groups();
            if (attr.Length != 4)
                throw new HttpException("Malformed groups file");
            newgrp.name = attr[0];
            newgrp.gid = Convert.ToInt32( attr[2]);
            if(attr.Length>3)
            {
                string[] usrnams = attr[3].Split(',');
                foreach(string u in usrnams)
                {
                    if(u!="")
                    newgrp.members.Add(u);
                }
            }
            return newgrp;
        }

        //Populate GroupList from groups file
        public void PopulateGroups()
        {
            GroupList.RemoveRange(0, GroupList.Count);
            string line;
            //string p = System.AppDomain.CurrentDomain.BaseDirectory;
            StreamReader file = new System.IO.StreamReader(grpPath);
            while ((line = file.ReadLine()) != null)
            {
                groups newgroup = ParseGroupString(line);
                // add each group to list
                GroupList.Add(newgroup);
            }
        }

        //Function to get all users , just returns Userlist
        public user[] GetAllUsers()
        {
            PopulateUsers();
            user[] userarray = Userlist.ToArray();
            return userarray;
        }
        //Function to get users by query , one or more parameters can be provided.
        public user[] GetUserByQuery(int id, string name, int gid, string comment, string home, string shell)
        {
           PopulateUsers();

            bool chck=true; // used to chck if match is found , false if match isnt found - else true . 
            List<user> requsr = new List<user>();

            foreach (user u in Userlist)
            {
                chck = true;
                if (id != -1 && u.id != id)
                    chck = false;
                if (name != null && name != u.name)
                    chck = false;

                if (gid != -1 && u.gid != gid)
                    chck = false;
                if (comment != null && comment != u.comment)
                    chck = false;

                if (home != null && u.home != home)
                    chck = false;
                if (shell != null && shell != u.shell)
                    chck = false;

                if (chck == true)//if all checks are passed , add user to list
                    requsr.Add(u);
            }
            //return list as array
            return requsr.ToArray();
        }

        //Get single user by Id , else return null
        public user GetUserById(int id)
        {
            PopulateUsers();
            user specificuser = new user();
            foreach(user u in Userlist)
            {
                if (id == u.id)
                {
                    specificuser = u;
                    return specificuser;
                }
            }
            return null;
           
        }

        //Get groups for a single user
        public groups[] GetUserByGroup(int id)
        {
            PopulateUsers();
            PopulateGroups();
            string userName="";
            List<groups> reqgrps = new List<groups>();
            bool chck = false ;
            foreach(user u in Userlist)
            {
                // loop to get username from /etc/passwd - stored in userName
                if (u.id == id)
                {
                    userName = u.name;
                    chck = true;
                    break;
                }
            }
            //if userName was found , check all groups which contain that name
            if(chck==true)
            {
                foreach(groups g in GroupList)
                {
                    if (g.members.Contains(userName))// if group contains userName , add to result group list
                        reqgrps.Add(g);
                }
            }
            return reqgrps.ToArray();
        }
        /*---
        //---            /ETC/GROUPS logic begins here
        //--
        //--*/
        //Function returns all groups present
        public groups[] GetAllGroups()
        {
            PopulateGroups();
            groups[] grouparray = GroupList.ToArray();
            return grouparray;
        }
        //Sets custom path for /etc/groups provided in QueryString , Cannot figure out how to do it using command line
        //because there is no .exe or main function :(
        public bool SetGrpPath(string path)
        {
            if (File.Exists(path))
            {

                grpPath = path; return true;
            }


            else
                return false;
        }

        //Function returns all groups matching the query , one or more parameters may be supplied
        public groups[] GetGroupsByQuery(string name,int id,string[] members)
        {
            PopulateGroups();
            bool chck=true;
            List<groups> reqgrps = new List<groups>();
            foreach(groups g in GroupList)
            {
                chck = true;
                if (name != null && g.name != name)
                    chck = false;
                if (id != -1 && g.gid != id)
                    chck = false;
                if (members != null && g.members!=null)
                {
                    foreach (string m in members)
                    {
                        if (g.members.Contains(m) == false)
                            chck = false;
                    }
                }
                if (chck == true)
                    reqgrps.Add(g);
            }

            return reqgrps.ToArray();
        }
        //Function returns single group matched by gid - groupid
        public groups GetSingleGroupById(int id)
        {
            PopulateGroups();
            foreach(groups g in GroupList)
            {
                if (g.gid == id)
                    return g;
            }
            return null;
        }

        /*
         * 
         *  TEST STUB .. Please uncomment to test as seperate c# project
         * 
         */ 
         /* 
        public void DisplayUserlist(user[] Userlist)
        {
            if (Userlist == null)
            {
                Console.Write("\n Error: Userlist not found or corrupted");
                return;
            }
                Console.Write("\n----------Displaying Users\n");
            foreach (user u in Userlist)
            {
                Console.Write("\nUsername:" + u.name);
                Console.Write("\nid:" + u.id);
                Console.Write("\ngid:" + u.gid);
                Console.Write("\nComment:" + u.comment);
                Console.Write("\nShell:" + u.shell);
                Console.Write("\n:" + u.home);
            }


        }
        public void Displaygrplist(groups[] GroupList)
        {
            if (GroupList == null)
            {
                Console.Write("\n Error: GroupList not found or corrupted");
                return;
            }
            Console.Write("\n----------Displaying Users\n");
            foreach (groups g in GroupList)
            {
                Console.Write("\nUsername:" + g.name);
                Console.Write("\nid:" + g.gid);
                Console.Write("Members:");
                foreach(string s in g.members)
                {
                    Console.Write(s);
                }
                Console.Write("\n");
            }


        }
        
        public static void Main(string[] args)
        {
            //put etc in execution folder
            repository TestRepo = new repository();
            
            Console.Write("\n---------Testing Repository-------\n");
            Console.Write("----Populating Groups-----\n");
            TestRepo.PopulateGroups();
            Console.Write("Done!\n");
            Console.Write("----Populating Users-----\n");
            TestRepo.PopulateUsers();
            Console.Write("Done!\n");
            Console.Write("\nDisplaying all users");
            TestRepo.DisplayUserlist(TestRepo.GetAllUsers());
            Console.Write("\nDisplaying all groups\n");
            TestRepo.Displaygrplist(TestRepo.GetAllGroups());
            Console.Write("\nDisplaying user with query id =1000\n");
            TestRepo.DisplayUserlist(TestRepo.GetUserByQuery(1000,null,-1,null,null,null));

            Console.Write("\nDisplaying user with query id =1000\n");
            



            Console.Write("\nDisplaying all users");
            TestRepo.DisplayUserlist(TestRepo.GetAllUsers());
            Console.Write("\nDisplaying all users");
            TestRepo.DisplayUserlist(TestRepo.GetAllUsers());
            Console.Write("\n Displaying groups by query id = 2 \n");
            TestRepo.Displaygrplist(TestRepo.GetGroupsByQuery(null, 2, null));
            Console.Write("\n Displaying single group by id =2 \n");
            //TestRepo.Displaygrplist(TestRepo.GetSingleGroupById(2));

        }
        */
    }
   
    
   
}