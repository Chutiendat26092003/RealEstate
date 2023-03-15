
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Models;

namespace RealEstate.Extension
{
    public class SessionFilter : Attribute, IActionFilter
    {
        private readonly RealEstateContext _context;

        public SessionFilter(RealEstateContext context)
        {
            _context = context;

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var result = context.HttpContext.Session.GetString("AccountId");
            var account = _context.Account.Where(a => a.Id == Convert.ToInt32(result)).Include(a => a.Users).AsNoTracking().FirstOrDefault();
            var user = account.Users.FirstOrDefault();


            if (result == null)
            {
                context.Result = new RedirectResult("~/Accounts/Login");
            }
            else if (result != null && account.Status == '0')
            {
                context.Result = new RedirectResult("~/");
            }
        }
    }
}