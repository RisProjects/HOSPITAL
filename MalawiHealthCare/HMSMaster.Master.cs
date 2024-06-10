using MalawiHealthCare .Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HMSMalawi
{
    public partial class HMSMaster : System.Web.UI.MasterPage
    {
        DataTable dt = new DataTable();
        Dal dal = new Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //int UserId = Convert.ToInt32(Session["UserId"]);
                //if (UserId!=0)
                //{
                //    GetMenuData(0, UserId);
                //    //  lblUserName.Text = Session["UserName"].ToString();
                //}
                //else
                //{
                //    Response.Redirect("~/Login.aspx");
                //}

                //GetMenuData(0, 1);

            }

        }
        private void GetMenuData(int ParentMenuId, int UserId)
        {

            // for restriction of url from typing url in browser for user

            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);


            dt = dal.Fun_MenuItems(UserId);
            var urlAccess = from myRow in dt.AsEnumerable()
                            where myRow.Field<string>("Url").Contains(currentPage)
                            select myRow;
            int Count = urlAccess.Count();


            //bool AccessForAll = Convert.ToBoolean(ViewState["AccessForAll"]);
            bool AccessForAll = Convert.ToBoolean(Session["AccessForAll"]);
            if (Count == 0 && AccessForAll == false)
            {
                Response.Redirect("~/ErrorPage.aspx");
            }
            else
            {

                DataView view = new DataView(dt);
                view.RowFilter = "ParentMenuId =" + ParentMenuId;
                foreach (DataRowView row in view)
                {
                    MenuItem menuItem = new MenuItem(row["Description"].ToString(),
                    row["MenuId"].ToString());
                    menuItem.NavigateUrl = row["Url"].ToString();
                    menu_Items.Items.Add(menuItem);
                    AddChildItems(dt, menuItem);
                }
                Session["AccessForAll"] = null;
            }
        }
        private void AddChildItems(DataTable table, MenuItem menuItem)
        {
            DataView viewItem = new DataView(table);
            viewItem.RowFilter = "ParentMenuId=" + menuItem.Value;
            foreach (DataRowView childView in viewItem)
            {
                MenuItem childItem = new MenuItem(childView["Description"].ToString(),
                childView["MenuId"].ToString());
                childItem.NavigateUrl = childView["Url"].ToString();
                menuItem.ChildItems.Add(childItem);
                AddChildItems(table, childItem);
            }
        }


        protected void OnMenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (SiteMap.CurrentNode != null)
            {
                if (e.Item.Text == SiteMap.CurrentNode.Title)
                {
                    if (e.Item.Parent != null)
                    {
                        e.Item.Parent.Selected = true;
                    }
                    else
                    {
                        e.Item.Selected = true;
                    }
                }
            }
        }



    }
}