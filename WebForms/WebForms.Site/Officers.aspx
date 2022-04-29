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
        <table>
            <thead>
                <tr>
                    <th>Officer</th>
                    <th>Assignment</th>
                </tr>
            </thead>
            <tbody id="officers"></tbody>
        </table>
    </section>
</asp:Content>
