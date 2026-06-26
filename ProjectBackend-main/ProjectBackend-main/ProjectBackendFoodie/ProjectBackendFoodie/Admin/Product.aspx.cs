using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;


namespace ProjectBackendFoodie.Admin
{
    public partial class Product : System.Web.UI.Page
    {
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Product";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else
                {
                    getProducts();
                }
            }
            lblMsg.Visible = false;
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty;
            string imagePath = string.Empty;
            string fileExtension = string.Empty;

            bool isValidToExecute = false;

            int productId = Convert.ToInt32(hdnId.Value);

            con = new MySqlConnection(Connection.GetConnectionString());

            cmd = new MySqlCommand("Product_Crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action",
                productId == 0 ? "INSERT" : "UPDATE");

            cmd.Parameters.AddWithValue("@p_ProductId", productId);

            cmd.Parameters.AddWithValue("@p_Name",
                txtName.Text.Trim());

            cmd.Parameters.AddWithValue("@p_Description",
                txtDescription.Text.Trim());

            cmd.Parameters.AddWithValue("@p_Price",
                Convert.ToDecimal(txtPrice.Text.Trim()));

            cmd.Parameters.AddWithValue("@p_Quantity",
                Convert.ToInt32(txtQunatity.Text.Trim()));

            cmd.Parameters.AddWithValue("@p_CategoryId",
                Convert.ToInt32(ddlCategories.SelectedValue));

            cmd.Parameters.AddWithValue("@p_IsActive",
                cbIsActive.Checked);
            if (fuProductImage.HasFile)
            {
                if (Utils.IsValidExtension(fuProductImage.FileName))
                {
                    Guid obj = Guid.NewGuid();

                    fileExtension = Path.GetExtension(fuProductImage.FileName);

                    imagePath = "Images/Product/" + obj.ToString() + fileExtension;

                    string fullPath = Server.MapPath("~/") + imagePath;

                    // Save image
                    fuProductImage.SaveAs(fullPath);

                    cmd.Parameters.AddWithValue("@p_ImageUrl", imagePath);

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

                cmd.Parameters.AddWithValue("@p_ImageUrl", "");
            }

            if (isValidToExecute)
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    con.Open();

                    cmd.ExecuteNonQuery();

                    actionName = productId == 0 ? "inserted" : "updated";

                    lblMsg.Visible = true;

                    lblMsg.Text = "Product " + actionName + " successful!";

                    lblMsg.CssClass = "alert alert-success";

                    getProducts();

                    clear();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private void getProducts()
        {
            con = new MySqlConnection(Connection.GetConnectionString());

            cmd = new MySqlCommand("Product_Crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            // PARAMETERS
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@p_ProductId", 0);
            cmd.Parameters.AddWithValue("@p_Name", "");
            cmd.Parameters.AddWithValue("@p_Description", "");
            cmd.Parameters.AddWithValue("@p_Price", 0);
            cmd.Parameters.AddWithValue("@p_Quantity", 0);
            cmd.Parameters.AddWithValue("@p_CategoryId", 0);
            cmd.Parameters.AddWithValue("@p_IsActive", false);
            cmd.Parameters.AddWithValue("@p_ImageUrl", "");

            sda = new MySqlDataAdapter(cmd);

            dt = new DataTable();

            sda.Fill(dt);

            rProduct.DataSource = dt;

            rProduct.DataBind();
        }
        private void getCategories()
        {
            con = new MySqlConnection(Connection.GetConnectionString());

            cmd = new MySqlCommand("Category_Crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@p_CategoryId", 0);
            cmd.Parameters.AddWithValue("@p_Name", "");
            cmd.Parameters.AddWithValue("@p_IsActive", true);
            cmd.Parameters.AddWithValue("@p_ImageUrl", "");

            sda = new MySqlDataAdapter(cmd);

            dt = new DataTable();

            sda.Fill(dt);

            ddlCategories.DataSource = dt;

            ddlCategories.DataTextField = "Name";

            ddlCategories.DataValueField = "CategoryId";

            ddlCategories.DataBind();

            ddlCategories.Items.Insert(0, new ListItem("Select Category", "0"));
        }

        private void clear()
        {
            txtName.Text = string.Empty;

            txtDescription.Text = string.Empty;

            txtQunatity.Text = string.Empty;

            txtPrice.Text = string.Empty;

            ddlCategories.SelectedIndex = 0;

            cbIsActive.Checked = false;

            hdnId.Value = "0";

            btnAddOrUpdate.Text = "Add";

            imgProduct.ImageUrl = string.Empty;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void rProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // =========================================
            // DELETE PRODUCT
            // =========================================
            if (e.CommandName == "Delete")
            {
                con = new MySqlConnection(Connection.GetConnectionString());

                cmd = new MySqlCommand("Product_Crud", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "DELETE");

                cmd.Parameters.AddWithValue("@p_ProductId",
                    Convert.ToInt32(e.CommandArgument));

                cmd.Parameters.AddWithValue("@p_Name", "");

                cmd.Parameters.AddWithValue("@p_Description", "");

                cmd.Parameters.AddWithValue("@p_Price", 0);

                cmd.Parameters.AddWithValue("@p_Quantity", 0);

                cmd.Parameters.AddWithValue("@p_CategoryId", 0);

                cmd.Parameters.AddWithValue("@p_IsActive", false);

                cmd.Parameters.AddWithValue("@p_ImageUrl", "");
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    con.Open();

                    cmd.ExecuteNonQuery();

                    lblMsg.Visible = true;

                    lblMsg.Text = "Product deleted successfully!";

                    lblMsg.CssClass = "alert alert-success";

                    getProducts();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;

                    lblMsg.Text = "Error - " + ex.Message;

                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }

            // =========================================
            // EDIT PRODUCT
            // =========================================
            else if (e.CommandName == "edit")
            {
                con = new MySqlConnection(Connection.GetConnectionString());

                cmd = new MySqlCommand("Product_Crud", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "GETBYID");

                cmd.Parameters.AddWithValue("@p_ProductId",
                    Convert.ToInt32(e.CommandArgument));

                cmd.Parameters.AddWithValue("@p_Name", "");

                cmd.Parameters.AddWithValue("@p_Description", "");

                cmd.Parameters.AddWithValue("@p_Price", 0);

                cmd.Parameters.AddWithValue("@p_Quantity", 0);

                cmd.Parameters.AddWithValue("@p_CategoryId", 0);

                cmd.Parameters.AddWithValue("@p_IsActive", false);

                cmd.Parameters.AddWithValue("@p_ImageUrl", "");

                sda = new MySqlDataAdapter(cmd);

                dt = new DataTable();

                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["Name"].ToString();

                    txtDescription.Text =
                        dt.Rows[0]["Description"].ToString();

                    txtPrice.Text =
                        dt.Rows[0]["Price"].ToString();

                    txtQunatity.Text =
                        dt.Rows[0]["Quantity"].ToString();

                    ddlCategories.SelectedValue =
                        dt.Rows[0]["CategoryId"].ToString();

                    cbIsActive.Checked =
                        Convert.ToBoolean(dt.Rows[0]["IsActive"]);

                    imgProduct.ImageUrl =
                        string.Format("../{0}",
                        dt.Rows[0]["ImageUrl"].ToString());

                    hdnId.Value =
                        dt.Rows[0]["ProductId"].ToString();

                    btnAddOrUpdate.Text = "Update";
                }
            }
        }

        protected void rProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Tìm cái Label lblIsActive mà mình đã đặt ở file .aspx
                Label lblIsActive = e.Item.FindControl("lblIsActive") as Label;
                Label lblQuantity = e.Item.FindControl("lblQuantity") as Label;


                if (lblIsActive.Text == "True" || lblIsActive.Text == "1")
                {
                    lblIsActive.Text = "Active";
                    lblIsActive.CssClass = "badge badge-success"; // Màu xanh mượt mà
                }
                else
                {
                    lblIsActive.Text = "In-Active";
                    lblIsActive.CssClass = "badge badge-danger"; // Màu đỏ cảnh báo
                }
                if (Convert.ToInt32(lblQuantity.Text) <= 5)
                {
                    lblQuantity.CssClass = "badge badge-danger";
                    lblQuantity.ToolTip = "Item about to be 'Out of stock'!";
                }
            }
        }
    }
}