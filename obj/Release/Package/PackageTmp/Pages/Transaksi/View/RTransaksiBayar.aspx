﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/WebMaster.Master" AutoEventWireup="true" CodeBehind="RTransaksiBayar.aspx.cs" Inherits="eFinance.Pages.Transaksi.View.RTransaksiBayar" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
    <style>
        .table {
            margin-bottom: 0;
        }
        #BodyContent_grdAccountPanelHeaderContentFreeze {
            height: auto !important;
        }
    </style>
    <script type="text/javascript">
        function OpenReport() {
            //            OpenReportViewer();
            window.open('../../../Report/ReportViewer.aspx', 'name', 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=no,modal=yes,width=1000,height=600');
        }
        function money(money) {
            var num = money;
            if (num != "") {
                var string = String(num);
                var string = string.replace('.', ' ');

                var array2 = string.toString().split(' ');
                num = parseInt(num).toFixed(0);

                var array = num.toString().split('');

                var index = -3;
                while (array.length + index > 0) {
                    array.splice(index, 0, ',');
                    index -= 4;
                }
                if (array2.length == 1) {
                    money = array.join('') + '.00';
                } else {
                    money = array.join('') + '.' + array2[1];
                    if (array2.length == 1) {
                        money = array.join('') + '.00';
                    } else {
                        if (array2[1].length == 1) {
                            money = array.join('') + '.' + array2[1].substring(0, 2) + '0';
                        } else {
                            money = array.join('') + '.' + array2[1].substring(0, 2);
                        }
                    }
                }
            } else {
                money = '0.00';
            }
            return money;
        }
        function getAmount() {

            var julpend = document.querySelectorAll(".julpend");;
            for (let i = 0; i < julpend.length; i++) {

                julpend[i].innerHTML = money(julpend[i].textContent);

            }
            console.log(julpend)
        };
    </script>
    <asp:HiddenField ID="hdnYayasan" runat="server" Value="0" />
    <div id="tabGrid" runat="server">
        <div class="row">
            <div class="col-sm-12">
                <div class="form-inline">
                    <div class="col-sm-12">
                        <label>Filter :   </label>
                                            <asp:DropDownList ID="cboPerwakilan" runat="server" Width="290" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cboPerwakilan_SelectedIndexChanged"></asp:DropDownList>

                         <asp:DropDownList ID="cboCabang" runat="server" CssClass="form-control"  Width="250" AutoPostBack="true" OnSelectedIndexChanged="cboCabang_SelectedIndexChanged"></asp:DropDownList>
                         <asp:DropDownList ID="cboKelas" runat="server" CssClass="form-control" Width="140"></asp:DropDownList>
                         <asp:DropDownList ID="cboJnsTrans" runat="server" CssClass="form-control" Width="180"></asp:DropDownList>
                        <asp:DropDownList ID="cbothnajaran" runat="server" CssClass="form-control" Width="180"></asp:DropDownList>
                        <asp:Button runat="server" ID="Button1" CssClass="btn btn-primary" Text="Search" OnClick="btnPosting_Click"></asp:Button>
                        <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Download" OnClick="btnExport_Click" ></asp:Button>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-sm-12 overflow-x-table">
                <asp:GridView ID="grdAccount" DataKeyNames="namaSiswa" SkinID="GridView" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="Nama Siswa" SortExpression="Ket" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="left">
                            <ItemTemplate>
                               <asp:Label ID="Ket" runat="server" Text='<%# Eval("namaSiswa").ToString().Replace(" ", "&nbsp;") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kelas" SortExpression="Ket" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="left">
                            <ItemTemplate>
                                <asp:Label ID="Ket" runat="server" Text='<%# Eval("kelas").ToString().Replace(" ", "&nbsp;") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:TemplateField HeaderText="Jul" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan1" runat="server" Text='<%# Eval("Jul") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ags" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan2" runat="server" Text='<%# Eval("Ags") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sept" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan3" runat="server" Text='<%# Eval("Sept") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Okt" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan4" runat="server" Text='<%# Eval("Okt") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Nov" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan5" runat="server" Text='<%# Eval("Nov") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Des" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan6" runat="server" Text='<%# Eval("Des") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jan" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan7" runat="server" Text='<%# Eval("Jan") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feb" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan8" runat="server" Text='<%# Eval("Feb") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Mar" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan9" runat="server" Text='<%# Eval("Mar") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Apr" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan10" runat="server" Text='<%# Eval("Apr") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mei" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan11" runat="server" Text='<%# Eval("Mei") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jun" SortExpression="ptd" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:Label ID="penerimaan12" runat="server" Text='<%# Eval("Jun") %>' CssClass="julpend"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                      
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    </asp:UpdatePanel>
     
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
            $('#BodyContent_grdAccount').gridviewScroll({
                width: '99%',
                height: 400,
                freezesize: 2, // Freeze Number of Columns. 
            });
        });

        function getAmount() {

            var julpend = document.querySelectorAll(".julpend");;
            for (let i = 0; i < julpend.length; i++) {

                julpend[i].innerHTML = money(julpend[i].textContent);

            }
            console.log(julpend)
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="server">
</asp:Content>