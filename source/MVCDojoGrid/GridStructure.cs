//GridStructure.cs
//Author - Prateek Jain

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Reflection;
using System.Web;

namespace MvcDojoGrid
{
    /// <summary>
    /// Constructs the structure for the Dojo Grid
    /// </summary>
    public class GridStructure
    {
        private List<Dictionary<String, String>> _structure;
        private PropertyInfo[] _props;
        private string _idColumn;
        private string _linkedColumn;

        public GridStructure() { }

        /// <summary>
        /// Creates the grid structure with all the columns in the entity
        /// </summary>
        /// <param name="entity">The enity for which the structure has to be generated</param>
        public GridStructure(Type entity)
        {
            
            _structure = new List<Dictionary<string, string>>();
            
            PropertyInfo[] props = entity.GetProperties();
            int i = 0;
            _props = props;
            foreach(PropertyInfo property in props)
            {
                i++;
                Dictionary<String, String> cell = new Dictionary<string, string>();
                ColumnParser parser = new ColumnParser();
                string name = parser.ColumnName(property.Name);
                cell.Add("name", name);
                cell.Add("field", property.Name);
                cell.Add("width", "auto");
                cell.Add("headerStyles", "font-weight:bold;");
                if (name.Contains("id") || name.Contains("Id") || name.Contains("ID"))
                    cell.Add("hidden", "true");
                _structure.Add(cell);
                if (i >= 15)
                    break;
            }
        }

        public GridStructure(Type entity, string linkedColumn = "", string idColumn = "", GridEditStyle editStyle = GridEditStyle.None) : this(entity)
        {
            if (editStyle == GridEditStyle.Linked)
            {
                ColumnParser parser = new ColumnParser();
                _idColumn = parser.ColumnName(idColumn);
                _linkedColumn = parser.ColumnName(linkedColumn);
                //int columnNum = -1;
                //foreach (PropertyInfo property in _props)
                //{
                //    columnNum++;
                //    if (property.Name == _linkedColumn)
                //        break;
                //}

                this[_linkedColumn].Add("get", "getLink");
                this[_linkedColumn].Add("formatter", "formatEditLink");
                //this[_idColumn].Add("hidden", "true");

                var cookie1 = new HttpCookie("grid_id", idColumn);
                var cookie2 = new HttpCookie("grid_link", linkedColumn);

                HttpContext.Current.Response.AppendCookie(cookie1);
                HttpContext.Current.Response.AppendCookie(cookie2);
            }
        }

        public Dictionary<String, String>[] structure
        {
            get
            {
                return _structure.ToArray();
            }
        }

        public Dictionary<string,string> this[string columnName]
        {
            get
            {
                foreach (Dictionary<string, string> dict in structure)
                {
                    if (dict["name"] == columnName)
                        return dict;
                }
                return null;
            }

            set
            {
                int columnNum = -1;
                foreach (Dictionary<string, string> dict in structure)
                {
                    columnNum++;
                    if (dict["name"] == columnName)
                        break;
                }
                _structure.Remove(_structure[columnNum]);
                _structure.Add(value);
            }
        }
    }

    public enum GridEditStyle
    {
        Inline,
        Linked,
        None
    }
}