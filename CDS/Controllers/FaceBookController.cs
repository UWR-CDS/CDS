using CDS.Logic;
using CDS.Manager;
using CDS.Models;
using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CDS.Controllers
{
    public class FaceBookController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "121159455261287",
                client_secret = "2d123778cf073aaca85660a4109d3ae9",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            try
            {
                var fb = new FacebookClient();
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "121159455261287",
                    client_secret = "2d123778cf073aaca85660a4109d3ae9",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code
                });

                var accessToken = result.access_token;

                // Store the access token in the session for farther use
                Session["AccessToken"] = accessToken;

                // update the facebook client with the access token so
                // we can make requests on behalf of the user
                fb.AccessToken = accessToken;

                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                if (me.email == null || me.email == "")
                {
                    me.email = me.first_name + "@testemail.com";
                }
                string email = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                // Set the auth cookie
                FormsAuthentication.SetAuthCookie(email, false);
                LoginModel obj1 = new ManageUser().ManageSocialAccount(email, fb.AccessToken, "Facebook", firstname, lastname, EncyptionDcryption.GetEncryptedText("12345678"));
                return RedirectToAction("LoginSocailUser", "User", new
                {
                    Email = obj1.Email,
                    Password = EncyptionDcryption.GetEncryptedText(obj1.Password)
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("LoginUser", "User");
            }

        }
    }
}