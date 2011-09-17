//GridActionFilters.cs
//Author - Prateek Jain

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace MvcDojoGrid.ActionFilters
{
    public class GridActionFilter : ActionFilterAttribute, IResultFilter, IActionFilter
    {
        private bool sortFlag = false;
        private bool rangeFlag = false;
        private char sortOrder;
        private string sortField = "";
        private int startItem = 0;
        private int endItem = 0;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Check if the sort parameter is there
            string temp = filterContext.HttpContext.Request.QueryString.ToString();
            Regex re = new Regex("[sort(]");
            if (re.IsMatch(temp))
            {
                try
                {
                    sortFlag = true;
                    sortOrder = temp.Split('(')[1][0];
                    sortField = temp.Split(sortOrder)[1].TrimEnd(')');
                }
                catch (Exception)
                {
                    throw new HttpException(400, "The request contains invalid parameters");
                }
            }
            else
            { 
                sortFlag = false;
            }

            //Check if the range header is present and set appropriate variables
            string range = filterContext.HttpContext.Request.Headers["Range"];
            if (!string.IsNullOrEmpty(range))
            {
                try
                {
                    rangeFlag = true;
                    startItem = Int32.Parse(range.Split('=')[1].Split('-')[0]);
                    endItem = Int32.Parse(range.Split('-')[1]);
                }
                catch (Exception)
                {
                    throw new HttpException(400, "The request contains invalid headers");
                }
            }
            else
            {
                rangeFlag = false;
            }
        }

        /// <summary>
        /// Performs various actions on the data depending upon the parameters sent
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            JsonResult result = (JsonResult)filterContext.Result;
            IQueryable<Object> data = (IQueryable<Object>)result.Data;

            if (!rangeFlag)
                endItem = data.Count();

            if (sortFlag)
            {
                if (sortOrder == '+')
                    result.Data = data.OrderBy(sortField + " asc").Skip(startItem).Take(endItem - startItem + 1);
                else
                    result.Data = data.OrderBy(sortField + " desc").Skip(startItem).Take(endItem - startItem + 1);
            }
            else
            {
                result.Data = data.Take(endItem - startItem); 
            } 
        }
    }
}