using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
namespace ProjectBackendFoodie.Admin
{
    public partial class Category : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Category";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else
                {
                    getCategories();
                }
            }
            lblMsg.Visible = false;
        }
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int categoryId = Convert.ToInt32(hdnId.Value);

            con = new MySqlConnection(Connection.GetConnectionString());
            cmd = new MySqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@p_CategoryId", categoryId);
            cmd.Parameters.AddWithValue("@p_Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@p_IsActive", cbIsActive.Checked);

            if (fuCategoryImage.HasFile)
            {
                if (Utils.IsValidExtension(fuCategoryImage.FileName))
                {
                    Guid obj = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuCategoryImage.FileName);
                    imagePath = "Images/Category/" + obj.ToString() + fileExtension;
                    fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension);
                    cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please select .jpg, .jpeg or .png image";
                    lblMsg.CssClass = "alert alert-danger";
                    isValidToExecute = false;
                }
            }
            else
            {
                isValidToExecute = true;
                cmd.Parameters.AddWithValue("@ImageUrl", "");
            }

            if (isValidToExecute)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    actionName = categoryId == 0 ? "inserted" : "updated";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Category " + actionName + " successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
                    clear();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error- " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void getCategories()
        {
            con = new MySqlConnection(Connection.GetConnectionString());
            cmd = new MySqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@p_CategoryId", 0);
            cmd.Parameters.AddWithValue("@p_Name", "");
            cmd.Parameters.AddWithValue("@p_IsActive", false);
            cmd.Parameters.AddWithValue("@p_ImageUrl", "");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }

        private void clear()
        {
            txtName.Text = string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            imgCategory.ImageUrl = String.Empty;
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void rCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")
            {
                con = new MySqlConnection(Connection.GetConnectionString());
                cmd = new MySqlCommand("Category_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);

                // Bắt buộc thêm 3 tham số ảo này cho MySQL khỏi báo lỗi thiếu tham số
                cmd.Parameters.AddWithValue("@Name", "");
                cmd.Parameters.AddWithValue("@IsActive", false);
                cmd.Parameters.AddWithValue("@ImageUrl", "");

                cmd.CommandType = CommandType.StoredProcedure;
                sda = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);

                // Đổ dữ liệu từ Database vào các ô nhập liệu
                txtName.Text = dt.Rows[0]["Name"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);

                // Hiển thị ảnh cũ lên khung xem trước
                imgCategory.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString())
                    ? "../Images/No_image.png" : "../" + dt.Rows[0]["ImageUrl"].ToString();
                imgCategory.Height = 200;
                imgCategory.Width = 200;

                hdnId.Value = dt.Rows[0]["CategoryId"].ToString();
                btnAddOrUpdate.Text = "Update"; // Đổi chữ nút thành Update

                // Đổi màu nút Edit vừa bấm để đánh dấu
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";
            }
            else if (e.CommandName == "delete")
            {
                con = new MySqlConnection(Connection.GetConnectionString());
                cmd = new MySqlCommand("Category_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@p_CategoryId", e.CommandArgument);

                // Bắt buộc thêm các tham số này cho đủ bộ 5 tham số của MySQL Procedure
                cmd.Parameters.AddWithValue("@p_Name", "");
                cmd.Parameters.AddWithValue("@p_IsActive", false);
                cmd.Parameters.AddWithValue("@p_ImageUrl", "");

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Category deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";

                    // Gọi lại hàm để cập nhật lại danh sách mới sau khi xóa
                    getCategories();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error- " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }
        protected void rCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Kiểm tra nếu dòng hiện tại là dòng dữ liệu (Item hoặc AlternatingItem)
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Tìm cái Label lblIsActive mà mình đã đặt ở file .aspx
                Label lbl = e.Item.FindControl("lblIsActive") as Label;

                if (lbl.Text == "True" || lbl.Text == "1")
                {
                    lbl.Text = "Active";
                    lbl.CssClass = "badge badge-success"; // Màu xanh mượt mà
                }
                else
                {
                    lbl.Text = "In-Active";
                    lbl.CssClass = "badge badge-danger"; // Màu đỏ cảnh báo
                }
            }
        }
    }
}