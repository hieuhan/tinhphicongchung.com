using System.Web.Mvc;

namespace tinhphicongchung.com.Areas.Admin.Services.Sercurity
{
    public abstract class MyUserViewPage : WebViewPage
    {
        public virtual MyPrincipal CurrentUser
        {
            get { return base.User as MyPrincipal; }
        }
    }
    public abstract class MyUserViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual MyPrincipal CurrentUser
        {
            get { return base.User as MyPrincipal; }
        }
    }
}