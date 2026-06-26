using System;
using System.Data;
using System.Web;
// Sử dụng thư viện MySQL thay vì SqlClient của SQL Server
using MySql.Data.MySqlClient;

namespace ProjectBackendFoodie.User
{
    public partial class Payment : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(Connection.GetConnectionString());
        MySqlCommand cmd;
        MySqlDataReader dr;
        MySqlTransaction transaction = null;

        string name, cardNo, expiryDate, address, paymentMode;
        int cvv;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Chặn không cho người dùng chưa đăng nhập truy cập trang này
                if (Session["userId"] == null)
                {
                    Response.Redirect("login.aspx");
                }
            }
        }

        // Sự kiện khi nhấn nút "Xác nhận thanh toán bằng Thẻ"
        protected void lbCardSubmit_Click(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                name = txtName.Text.Trim();
                address = txtAddress.Text.Trim();
                paymentMode = "Card"; // Chế độ thanh toán qua thẻ

                // Định dạng ẩn số thẻ (Chỉ giữ lại 4 số cuối)
                string rawCardNo = txtCardNo.Text.Trim();
                cardNo = string.Format("************{0}", rawCardNo.Substring(rawCardNo.Length - 4));

                // Gộp tháng/năm hết hạn
                expiryDate = txtExpMonth.Text.Trim() + "/" + txtExpYear.Text.Trim();
                cvv = Convert.ToInt32(txtCvv.Text.Trim());

                // Gọi hàm xử lý chính
                OrderPayment(name, cardNo, expiryDate, cvv, address, paymentMode);
            }
        }
        // Sự kiện khi nhấn nút "Xác nhận thanh toán khi nhận hàng (COD)"
        protected void lbCodSubmit_Click(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                name = "Cash On Delivery"; // Hoặc lấy tên của User từ Session/Database nếu có
                address = txtCODAddress.Text.Trim(); // Lấy ô địa chỉ của phần COD
                paymentMode = "COD";

                cardNo = null;
                expiryDate = null;
                cvv = 0;

                // Gọi chung hàm xử lý chính
                OrderPayment(name, cardNo, expiryDate, cvv, address, paymentMode);
            }
        }
        // Xử lý chính: Lưu thông tin, trừ kho, xóa giỏ hàng
        private void OrderPayment(string name, string cardNo, string expiryDate, int cvv, string address, string paymentMode)
        {
            int paymentId = 0;
            int userId = Convert.ToInt32(Session["userId"]);

            // Tạo bảng tạm trong C# để lưu trữ danh sách giỏ hàng trước khi xử lý vòng lặp
            DataTable dtCart = new DataTable();
            dtCart.Columns.Add("ProductId", typeof(int));
            dtCart.Columns.Add("Quantity", typeof(int));

            try
            {
                con.Open();
                transaction = con.BeginTransaction(); // Bắt đầu Giao dịch an toàn (Transaction)

                // --- BƯỚC A: Lưu thông tin thanh toán (Gọi Procedure save_payment) ---
                cmd = new MySqlCommand("save_payment", con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_Name", name);
                cmd.Parameters.AddWithValue("@p_CardNo", (object)cardNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_ExpiryDate", (object)expiryDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_CvvNo", cvv);
                cmd.Parameters.AddWithValue("@p_Address", address);
                cmd.Parameters.AddWithValue("@p_PaymentMode", paymentMode);

                // Cấu hình để nhận lại ID thanh toán tự động tăng (OUT Parameter) từ MySQL
                MySqlParameter outputParam = new MySqlParameter("@p_InsertedId", MySqlDbType.Int32);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();
                paymentId = Convert.ToInt32(outputParam.Value); // Lấy được ID vừa chèn

                // --- BƯỚC B: Lấy danh sách sản phẩm trong giỏ hàng của User ---
                cmd = new MySqlCommand("SELECT ProductId, Quantity FROM Carts WHERE UserId = @UserId", con, transaction);
                cmd.Parameters.AddWithValue("@UserId", userId);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dtCart.Rows.Add(dr["ProductId"], dr["Quantity"]);
                }
                dr.Close(); // Đóng Reader ngay lập tức để thực hiện các lệnh tiếp theo

                // Tạo mã đơn hàng ngẫu nhiên (Dùng chung cho toàn bộ lượt mua này)
                string orderNo = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                // --- BƯỚC C & D: Duyệt qua từng sản phẩm để tạo hóa đơn, trừ kho và xóa giỏ ---
                foreach (DataRow row in dtCart.Rows)
                {
                    int productId = Convert.ToInt32(row["ProductId"]);
                    int quantity = Convert.ToInt32(row["Quantity"]);

                    // 1. Chèn món ăn vào bảng Orders bằng Procedure save_order_item
                    MySqlCommand cmdOrder = new MySqlCommand("save_order_item", con, transaction);
                    cmdOrder.CommandType = CommandType.StoredProcedure;
                    cmdOrder.Parameters.AddWithValue("@p_OrderNo", orderNo);
                    cmdOrder.Parameters.AddWithValue("@p_ProductId", productId);
                    cmdOrder.Parameters.AddWithValue("@p_Quantity", quantity);
                    cmdOrder.Parameters.AddWithValue("@p_UserId", userId);
                    cmdOrder.Parameters.AddWithValue("@p_Status", "Pending");
                    cmdOrder.Parameters.AddWithValue("@p_PaymentId", paymentId);
                    cmdOrder.ExecuteNonQuery();

                    // 2. Cập nhật trừ số lượng tồn kho trong bảng Products
                    MySqlCommand cmdUpdate = new MySqlCommand("UPDATE Products SET Quantity = Quantity - @Qty WHERE ProductId = @PId", con, transaction);
                    cmdUpdate.Parameters.AddWithValue("@Qty", quantity);
                    cmdUpdate.Parameters.AddWithValue("@PId", productId);
                    cmdUpdate.ExecuteNonQuery();

                    // 3. Xóa món này ra khỏi giỏ hàng Carts
                    MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Carts WHERE ProductId = @PId AND UserId = @UId", con, transaction);
                    cmdDelete.Parameters.AddWithValue("@PId", productId);
                    cmdDelete.Parameters.AddWithValue("@UId", userId);
                    cmdDelete.ExecuteNonQuery();
                }

                transaction.Commit();

                Response.Redirect("invoice.aspx?id=" + paymentId);
            }
            catch (Exception ex)
            {
                // Nếu có bất kỳ lỗi nào xảy ra, hủy bỏ toàn bộ quá trình để tránh lỗi mất tiền mà không có đơn hàng
                if (transaction != null) transaction.Rollback();
                Response.Write("<script>alert('Lỗi hệ thống: " + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }
        }
    }
}