using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web.UI;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProjectBackendFoodie.User
{
    public partial class Invoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] != null)
                {
                    if (Request.QueryString["Id"] != null)
                    {
                        repeaterOrderItems.DataSource = GetOrderDetails();
                        repeaterOrderItems.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("../Login.aspx"); // Điều chỉnh đường dẫn nếu cần
                }
            }
        }

        private DataTable GetOrderDetails()
        {
            double grandTotal = 0;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            DataTable dtTbl = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_GetInvoiceById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_PaymentId", Convert.ToInt32(Request.QueryString["Id"]));
                    cmd.Parameters.AddWithValue("p_UserId", Convert.ToInt32(Session["userId"]));

                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dtTbl);
                    }
                }
            }

            if (dtTbl.Rows.Count > 0)
            {
                foreach (DataRow row in dtTbl.Rows)
                {
                    grandTotal += Convert.ToDouble(row["TotalPrice"]);
                }

                DataRow dr = dtTbl.NewRow();
                dr["TotalPrice"] = grandTotal;
                dtTbl.Rows.Add(dr);
            }

            return dtTbl;
        }

        protected void lbDownloadInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                string downloadPath = Server.MapPath("~/Temp/order_invoice.pdf"); // Tạo folder Temp trong project

                // Đảm bảo thư mục tồn tại
                if (!Directory.Exists(Server.MapPath("~/Temp")))
                    Directory.CreateDirectory(Server.MapPath("~/Temp"));

                DataTable dtTbl = GetOrderDetails();
                ExportToPdf(dtTbl, downloadPath, "HOA DON DAT HANG");

                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Invoice.pdf");
                Response.TransmitFile(downloadPath);
                Response.End();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Lỗi: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

        void ExportToPdf(DataTable dtblTable, String strPdfPath, string strHeader)
        {
            using (FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(document, fs);
                document.Open();

                Paragraph prgHeading = new Paragraph(strHeader.ToUpper(), FontFactory.GetFont("Arial", 16, Font.BOLD, Color.GRAY));
                prgHeading.Alignment = Element.ALIGN_CENTER;
                document.Add(prgHeading);

                document.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(dtblTable.Columns.Count - 1); // Điều chỉnh cột cho khớp
                table.WidthPercentage = 100;

                foreach (DataColumn column in dtblTable.Columns)
                {
                    if (column.ColumnName != "Status") // Loại bỏ cột không cần thiết
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName.ToUpper(), FontFactory.GetFont("Arial", 9, Font.BOLD, Color.WHITE)));
                        cell.BackgroundColor = Color.GRAY;
                        table.AddCell(cell);
                    }
                }

                foreach (DataRow row in dtblTable.Rows)
                {
                    foreach (DataColumn column in dtblTable.Columns)
                    {
                        if (column.ColumnName != "Status")
                        {
                            table.AddCell(new Phrase(row[column.ColumnName].ToString(), FontFactory.GetFont("Arial", 8)));
                        }
                    }
                }

                document.Add(table);
                document.Close();
            }
        }
    }
}