//DataResult.cs
//Author - Prateek Jain

using System;
using System.Linq;
using System.Web.Mvc;


namespace MvcDojoGrid
{
    /// <summary>
    /// Wraps the JsonResult class to provide operations particularly for the Dojo Grid
    /// </summary>
    public class DataResult : JsonResult
    {
        private IQueryable<Object> _data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Object that needs to be serialized into JSON</param>
        /// <param name="behavior">The Content-Type (MIME)</param>
        public DataResult(IQueryable<Object> data, JsonRequestBehavior behavior) { base.Data = data; base.JsonRequestBehavior = behavior; _data = data; }

        /// <summary>
        /// Overriden ExecuteResult which sets additional headers to the context
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);
            //base.Data = _data;
            //base.JsonRequestBehavior = _behavior;
            IQueryable<Object> test = (IQueryable<Object>)base.Data;

            context.HttpContext.Response.AddHeader("Content-Range", context.HttpContext.Request.Headers["Range"] + "/" + _data.Count());
        }
    } 
}