using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Security;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;
using System.Web.Script.Serialization;

namespace tinhphicongchung.com.Areas.Admin.Services.Attributes
{
    public class MyAuthTicketAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!filterContext.Principal.Identity.IsAuthenticated)
                return;

            HttpCookie authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null)
                return;

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            
            if (authTicket != null && !authTicket.Expired)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                UserSerializer serializeModel =
                    serializer.Deserialize<UserSerializer>(authTicket.UserData);
                if (serializeModel != null)
                {
                    MyPrincipal myPrincipal = new MyPrincipal(authTicket.Name)
                    {
                        UserId = serializeModel.UserId,
                        UserName = serializeModel.UserName,
                        Email = serializeModel.Email,
                        PhoneNumber = serializeModel.PhoneNumber,
                        Avatar = serializeModel.Avatar
                    };

                    HttpContext.Current.User = myPrincipal;
                }    
            }    
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            
        }
    }
}