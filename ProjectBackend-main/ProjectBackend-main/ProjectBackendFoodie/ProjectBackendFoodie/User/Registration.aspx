<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="ProjectBackendFoodie.User.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="book_section layout_padding">
    <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
    <div class="container">

        <div class="heading_container">
            <div class="align-self-end">
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </div>

            <h2>User Registration</h2>
        </div>

        <div class="row">

            <!-- LEFT -->
            <div class="col-md-6">

                <!-- NAME -->
                <div class="form-group">
                    <asp:TextBox ID="txtName"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Enter Full Name">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvName"
                        runat="server"
                        ControlToValidate="txtName"
                        ErrorMessage="Full Name is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- USERNAME -->
                <div class="form-group">
                    <asp:TextBox ID="txtUsername"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Enter Username">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvUsername"
                        runat="server"
                        ControlToValidate="txtUsername"
                        ErrorMessage="Username is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- EMAIL -->
                <div class="form-group">
                    <asp:TextBox ID="txtEmail"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Email"
                        placeholder="Enter Email">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Email is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- MOBILE -->
                <div class="form-group">
                    <asp:TextBox ID="txtMobile"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Enter Mobile Number">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvMobile"
                        runat="server"
                        ControlToValidate="txtMobile"
                        ErrorMessage="Mobile Number is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

            </div>

            <!-- RIGHT -->
            <div class="col-md-6">

                <!-- ADDRESS -->
                <div class="form-group">
                    <asp:TextBox ID="txtAddress"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="2"
                        placeholder="Enter Address">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvAddress"
                        runat="server"
                        ControlToValidate="txtAddress"
                        ErrorMessage="Address is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- POSTCODE -->
                <div class="form-group">
                    <asp:TextBox ID="txtPostCode"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Enter Post/Zip Code">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvPostCode"
                        runat="server"
                        ControlToValidate="txtPostCode"
                        ErrorMessage="Post Code is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- IMAGE -->
                <div class="form-group">
                    <asp:FileUpload ID="fuUserImage"
                        runat="server"
                        CssClass="form-control" />

                    <asp:RequiredFieldValidator
                        ID="rfvImage"
                        runat="server"
                        ControlToValidate="fuUserImage"
                        ErrorMessage="User Image is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- PASSWORD -->
                <div class="form-group">
                    <asp:TextBox ID="txtPassword"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Password"
                        placeholder="Enter Password">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvPassword"
                        runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Password is required"
                        ForeColor="Red"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </div>

                <!-- REGISTER BUTTON -->
                <asp:Button ID="btnRegister"
                        runat="server"
                        Text="Register"
                        CssClass="btn btn-success"
                        OnClick="btnRegister_Click"
                        UseSubmitBehavior="false" />

                    <span class="pl-3">
                        Already registered?
                    </span>

                    <asp:HyperLink ID="hlLogin"
                        runat="server"
                        NavigateUrl="~/User/Login.aspx"
                        CssClass="badge badge-info">
                        Login here..
                    </asp:HyperLink>

                </div>

            </div>

        </div>

    </div>
</section>

</asp:Content>