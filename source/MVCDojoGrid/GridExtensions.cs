//GridExtensions.cs
//Author - Prateek Jain

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using System.IO;


namespace MVCDojoGrid.Helpers
{
    public static class GridExtensions
    {
        /// <summary>
        /// Creates a div element which places a dojo grid.
        /// Usage: @using LSS.Mvc.Helpers
        ///        @Html.Grid("TestGrid", "structure")
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">id of the div tag holding the grid</param>
        /// <param name="url">url from where the structure of grid will be retreived in json format</param>
        /// <returns>div element for the grid</returns>
        public static HtmlString Grid(this HtmlHelper helper, string id, string structureURL, string dataURL, string editURL)
        {
            //Using tagbuilder to create the div tag which will hold the grid
            TagBuilder tagGrid = new TagBuilder("div");
            tagGrid.Attributes.Add("dojoType", "dijit.layout.ContentPane");
            tagGrid.Attributes.Add("style", "min-width:700px;");
            tagGrid.GenerateId(id);

            //Generates the script tag, did not use the tagbuilder as it would html encode the javascript itself!!
            
            MVCDojoGrid.Grid page = new MVCDojoGrid.Grid(id, structureURL, dataURL, editURL);
            string pageContent = page.TransformText();              //Generates the javascript from the T4 template
            StringBuilder temp = new StringBuilder("<script src=\"http://ajax.googleapis.com/ajax/libs/dojo/1.6.1/dojo/dojo.xd.js\" type=\"text/javascript\" djConfig=\"parseOnLoad: true, debugAtAllCosts: true\"></script>");
            temp.Append("<link media=\"screen\" href=\"http://ajax.googleapis.com/ajax/libs/dojo/1.6.0/dojox/grid/enhanced/resources/claro/EnhancedGrid.css\" rel=\"stylesheet\">");
            //temp.Append("<link media=\"screen\" href=\"http://ajax.googleapis.com/ajax/libs/dojo/1.6.0/dojo/resources/dojo.css\" rel=\"stylesheet\">");
            temp.Append("<link media=\"screen\" href=\"http://ajax.googleapis.com/ajax/libs/dojo/1.6.0/dijit/themes/claro/claro.css\"  rel=\"stylesheet\">");
            temp.Append("<script type=\"text/javascript\">");
            temp.Append(pageContent);
            temp.Append("</script>");

            return new HtmlString(temp.ToString() + "\n" + tagGrid.ToString());
        }
    }
}

namespace MVCDojoGrid
{
    /// <summary>
    /// Partial class for the T4 template to generate the javascript.
    /// This class creates a constructor to pass parameters to the template.
    /// </summary>
    public partial class Grid
    {
        private string _gridName;
        private string _structureURL;
        private string _dataURL;
        private string _editURL;
        public Grid(string gridName, string structureURL, string dataURL, string editURL) { this._gridName = gridName; this._structureURL = structureURL; this._dataURL = dataURL; this._editURL = editURL; }
    }
}