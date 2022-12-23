<%@ Page Title="Officers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Officers.aspx.cs" Inherits="WebMeDown.Site.Officers" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getOfficers() {
            $.getJSON("api/officers"),
                function (data) {
                    $('#officers').empty();

                    $.each(data, function (key, item) {
                    });
                }
        }

        $(document).ready(getOfficers);
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section>
        <div>
            <hgroup>
                <h2>Officers</h2>
            </hgroup>
        </div>
        <asp:GridView runat="server" ID="officersGrid" ItemType="WebMeDown.Site.Models.Officer" DataKeyNames="Id" SelectMethod="officersGrid_GetData" AutoGenerateColumns="false">
            <Columns>
                <asp:DynamicField DataField="SerialNo" />
                <asp:DynamicField DataField="Rank" />
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="<%# Item.FirstName%>" />
                        <asp:Label runat="server" Text="<%# Item.MiddleName %>" />
                        <asp:Label runat="server" Text="<%# Item.LastName%>" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </section>
</asp:Content>
