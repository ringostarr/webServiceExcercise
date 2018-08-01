using BraincorpExcercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using BraincorpExcercise.Services;

namespace BraincorpExcercise.Controllers
{
   
    [RoutePrefix("api")]
    public class usersController : ApiController
    {
        ///repository should be initialized only once - hence the static keyword
        public static repository Userrepo = new repository();
        public usersController()
        {
            //this.Userrepo = new repository();
        }
        ///route for /api/users - to display all users
        ///example - /api/users
        [Route("users")]
        public user[] Get()
        {
            return Userrepo.GetAllUsers();            
        }

        /// route for setting path , can't figure out how to use command line arguments
        ///TODO : use command line arguments or POST/PUT to change path
        [Route("users/setpath")]
        public void GetSetPath()
        {
            string path = this.Request.RequestUri.Query;
            path = path.Substring(1);
            bool pathcheck=Userrepo.SetUserPath(path);
            if (pathcheck == false)
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        ///route for /user/query?[Query]
        //
        /// General querystring can be api/users/query?id=1&name=[placeholder]&gid=100&comment=[placeholder] . . .. . ..
        ///available options - id,name,gid,comment,home,shell
        [Route("users/query")]
        public user[] GetbyQuery(int id=-1,string name =null,int gid=-1,string comment =null , string home=null,string shell=null )
        {
            string query=this.Request.RequestUri.Query;
            return Userrepo.GetUserByQuery(id,name,gid,comment,home,shell);
        }

        ///Route for users/id(int)
        ///example - api/users/1
        [Route("users/{id:int}")]
        public user GetSpecificuid()
        {
            string fullstr = this.Url.Request.RequestUri.OriginalString;
            string tempuid;
            int uid;
            int uidpos = fullstr.LastIndexOf('/');
            tempuid = fullstr.Substring(uidpos+1);
            Int32.TryParse(tempuid,out uid);
            user reqUsr = Userrepo.GetUserById(uid);
            if (reqUsr != null)
                return reqUsr;
            else
            {
                //throw 404 exception if id is not found
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            
        }

        ///route for api/users/id(int)/groups
        ///example - api/users/1/groups
        [Route("users/{id:int}/groups")]
        public groups[] GetGroupbyUser()
        {
           string id_string = this.RequestContext.RouteData.Values["id"].ToString();
            int id_value = Convert.ToInt32(id_string);
            return Userrepo.GetUserByGroup(id_value);
            
        }
        //route for api/groups
        // Displays all groups from the file
        //example - /api/groups
        [Route("groups")]
        public groups[] GetAllGroups()
        {
            return Userrepo.GetAllGroups();
        }
        //route for api/groups/query
        // possible options are gid, name , array of members
        //example /api/groups/query?gid=1&name=[placeholder]&member=[placeholder]&member=[placeholder]...
        [Route("groups/query")]
        public groups[] GetGroupsByQuery(string name=null,int gid=-1,[FromUri] string[] members=null)
        {
            return Userrepo.GetGroupsByQuery(name, gid, members);
        }

        ///route for setting path to /etc/groups
        ///TODO : figure out better method than querystring
       ///example use /api/groups/setpath?[Path relative to IIS folder(Server folder in general)]
        [Route("groups/setpath")]
        public void GetSetGrpPath()
        {
            string path = this.Request.RequestUri.Query;
            path = path.Substring(1);
            bool pathcheck=Userrepo.SetGrpPath(path);
            if (pathcheck == false)
                throw new HttpResponseException(HttpStatusCode.NotFound);

        }
        ///route for getting specific group by ID
        ///example - /api/groups/1
        [Route("groups/{gid:int}")]
        public groups GetSpecificGroup()
        {
            string fullstr = this.Url.Request.RequestUri.OriginalString;
            string tempuid;
            int id;
            int idpos = fullstr.LastIndexOf('/');
            tempuid = fullstr.Substring(idpos + 1);
            Int32.TryParse(tempuid, out id);
            groups reqGrp = Userrepo.GetSingleGroupById(id);
            if (reqGrp != null)
                return reqGrp;
            else
            {
                //throw 404 exception if gid not found
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /*unit  TEST STUB   ----- need to uncomment display function from repository also.
        static void Main(string[] args)
        {
            usersController uc = new usersController();
            Userrepo.DisplayUserList(uc.Get());
            Userrepo.DisplayUserList(uc.GetbyQuery(1001));

            //groups g = uc.Getspecificgroup();
            Userrepo.DisplaygrpList(uc.Getallgroups());
            Userrepo.DisplayUserList(uc.Getgroupsbyquery("daemon"));
            //Userrepo.DisplayUserList(uc.Get());

        }
        */
    }

   
}
