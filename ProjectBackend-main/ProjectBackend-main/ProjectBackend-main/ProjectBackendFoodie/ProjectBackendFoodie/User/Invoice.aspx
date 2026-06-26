<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="ProjectBackendFoodie.User.Invoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                var messageLabel = document.getElementById("<%= lblMsg.ClientID %>");
                if (messageLabel != null) {
                    messageLabel.style.display = "none";
                }
            }, seconds * 1000);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>

        <div class="container">
            <asp:Repeater ID="repeaterOrderItems" runat="server">
                <HeaderTemplate>
                    <table id="tableInvoice" class="table table-responsive-sm table-bordered table-hover">
                        <thead class="bg-dark text-white">
                            <tr>
                                <th>STT</th>
                                <th>Mã đơn hàng</th>
                                <th>Tên sản phẩm</th>
                                <th>Giá bán</th>
                                <th>Số lượng</th>
                                <th>Tổng tiền</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# string.IsNullOrEmpty(Eval("SrNo").ToString()) ? "" : Eval("SrNo") %>
                        </td>
                        <td><%# Eval("OrderNo") %></td>
                        <td><%# Eval("ProductName") %></td>
                        <td>
                            <%# string.IsNullOrEmpty(Eval("Price").ToString()) ? "" : "đ " + Eval("Price") %>
                        </td>
                        <td><%# Eval("Quantity") %></td>
                        <td>
                            <strong>
                                <%# string.IsNullOrEmpty(Eval("ProductName").ToString()) ? "Tổng cộng: đ " + Eval("TotalPrice") : "đ " + Eval("TotalPrice") %>
                            </strong>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <div class="d-flex justify-content-center mt-4">
                <asp:LinkButton ID="lbDownloadInvoice" runat="server" OnClick="lbDownloadInvoice_Click" CssClass="btn btn-info">
                    <i class="fa fa-download mr-2"></i> Tải hóa đơn (PDF)
                </asp:LinkButton>
            </div>
        </div>
    </section>

</asp:Content>