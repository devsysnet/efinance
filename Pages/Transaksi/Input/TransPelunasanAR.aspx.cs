﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace eFinance.Pages.Transaksi.Input
{
    public partial class TransPelunasanAR : System.Web.UI.Page
    {
        Database ObjDb = new Database();
        Systems ObjSys = new Systems();
        private GlobalLibrary ObjGlobal = new GlobalLibrary();
        public Dictionary<string, string> Param = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtBayar.Text = Convert.ToDateTime(ObjSys.GetNow).ToString("dd-MMM-yyyy");
                loadDataComboFirst();
                loadDataCombo();
                ShowHideGridAndForm(true, true);
                button.Visible = false;
            }
        }

        private void loadDataComboFirst()
        {
            if (ObjSys.GetstsPusat == "3")
            {
                cboPerwakilanUnit.DataSource = ObjDb.GetRows("select a.* from (select '0' id,'-Pilih Unit-' name, 0 noUrut union all SELECT distinct nocabang id, namaCabang name, noUrut FROM vCabang WHERE stsCabang = 2 and parent = '" + ObjSys.GetParentCabang + "') a order by a.noUrut");
                cboPerwakilanUnit.DataValueField = "id";
                cboPerwakilanUnit.DataTextField = "name";
                cboPerwakilanUnit.DataBind();
            }
            else if (ObjSys.GetstsPusat == "2")
            {
                cboPerwakilanUnit.DataSource = ObjDb.GetRows("SELECT distinct nocabang id, namaCabang name FROM vCabang WHERE stsCabang = 2 and noCabang = '" + ObjSys.GetCabangId + "'");
                cboPerwakilanUnit.DataValueField = "id";
                cboPerwakilanUnit.DataTextField = "name";
                cboPerwakilanUnit.DataBind();
            }
            else
            {
                cboPerwakilanUnit.DataSource = ObjDb.GetRows("select a.* from (select '0' id,'-Pilih Unit-' name, 0 noUrut union all SELECT distinct nocabang id, namaCabang name, noUrut FROM vCabang WHERE stsCabang = 2) a order by a.noUrut");
                cboPerwakilanUnit.DataValueField = "id";
                cboPerwakilanUnit.DataTextField = "name";
                cboPerwakilanUnit.DataBind();
            }
            loadDataDiskon(cboPerwakilanUnit.Text);
            loadDataKelas(cboPerwakilanUnit.Text);
            loadDataJnsTransaksi(cboPerwakilanUnit.Text, cboJnsDiskon.Text);
        }

        protected void cboPerwakilanUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseMessage();
            loadDataDiskon(cboPerwakilanUnit.Text);
            loadDataKelas(cboPerwakilanUnit.Text);
            loadDataJnsTransaksi(cboPerwakilanUnit.Text, cboJnsDiskon.Text);
        }

        protected void loadDataDiskon(string PerwakilanUnit = "")
        {
            if (PerwakilanUnit == "0")
            {
                cboJnsDiskon.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Diskon-' as name)x");
                cboJnsDiskon.DataValueField = "id";
                cboJnsDiskon.DataTextField = "name";
                cboJnsDiskon.DataBind();
            }
            else
            {
                cboJnsDiskon.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Diskon-' as name union select distinct noParamdisc as id, namaDiskon as name from parameterDisc where noCabang = " + PerwakilanUnit + ")x");
                cboJnsDiskon.DataValueField = "id";
                cboJnsDiskon.DataTextField = "name";
                cboJnsDiskon.DataBind();
            }
        }
        protected void loadDataKelas(string PerwakilanUnit = "")
        {
            if (PerwakilanUnit == "0")
            {
                cboKelas.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Kelas-' as name)x");
                cboKelas.DataValueField = "id";
                cboKelas.DataTextField = "name";
                cboKelas.DataBind();

                cboTahun.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Tahun-' as name)x");
                cboTahun.DataValueField = "id";
                cboTahun.DataTextField = "name";
                cboTahun.DataBind();
            }
            else
            {
                cboKelas.DataSource = ObjDb.GetRows("select '-Pilih Kelas-' as id, '-Pilih Kelas-' as name union all select distinct kelas as id, kelas as name from TransKelas where nocabang = '" + PerwakilanUnit + "' and sts=1 order by name");
                cboKelas.DataValueField = "id";
                cboKelas.DataTextField = "name";
                cboKelas.DataBind();

                cboTahun.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Tahun-' as name union select distinct year(tgl) as id, CONVERT(varchar,year(tgl)) as name from TransPiutang where nocabang = '" + PerwakilanUnit + "')x");
                cboTahun.DataValueField = "id";
                cboTahun.DataTextField = "name";
                cboTahun.DataBind();
            }

        }

        protected void loadDataJnsTransaksi(string PerwakilanUnit = "", string Diskon = "")
        {
            if (PerwakilanUnit == "0")
            {
                cboJnsTrans.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Transaksi-' as name)x");
                cboJnsTrans.DataValueField = "id";
                cboJnsTrans.DataTextField = "name";
                cboJnsTrans.DataBind();
            }
            else
            {
                if (Diskon == "0")
                {
                    //tutup 04-03-2021 karena ada masalah jika ada unit sebagian dibayar cash org datang dan transfer ke rekening perwakilan
                    //cboJnsTrans.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Transaksi-' as name union select distinct noTransaksi as id, jenisTransaksi as name from mJenisTransaksi where nocabang = '" + PerwakilanUnit + "' and pelunasan = '" + ObjSys.GetstsPusat + "')x");
                    cboJnsTrans.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Transaksi-' as name union select distinct noTransaksi as id, jenisTransaksi as name from mJenisTransaksi where nocabang = '" + PerwakilanUnit + "')x");
                    cboJnsTrans.DataValueField = "id";
                    cboJnsTrans.DataTextField = "name";
                    cboJnsTrans.DataBind();
                }
                else
                {
                    //tutup 04-03-2021 karena ada masalah jika ada unit sebagian dibayar cash org datang dan transfer ke rekening perwakilan
                    //cboJnsTrans.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Transaksi-' as name union select distinct a.noTransaksi as id, a.jenisTransaksi as name from mJenisTransaksi a inner join ParameterDisc b on a.noTransaksi = b.noTransaksi where a.nocabang = '" + PerwakilanUnit + "' and a.pelunasan = '" + ObjSys.GetstsPusat + "' and b.noParamDisc = "+ Diskon +")x");
                    cboJnsTrans.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Transaksi-' as name union select distinct a.noTransaksi as id, a.jenisTransaksi as name from mJenisTransaksi a inner join ParameterDisc b on a.noTransaksi = b.noTransaksi where a.nocabang = '" + PerwakilanUnit + "' and b.noParamDisc = " + Diskon + ")x");
                    cboJnsTrans.DataValueField = "id";
                    cboJnsTrans.DataTextField = "name";
                    cboJnsTrans.DataBind();
                }
            }

        }

        private void loadDataCombo()
        {
            cboBulan.DataSource = ObjDb.GetRows("select * from (select '0' as id, '-Pilih Bulan-' as name union select distinct month(tgl) as id, DATENAME(mm, tgl) as name from TransPiutang)x");
            cboBulan.DataValueField = "id";
            cboBulan.DataTextField = "name";
            cboBulan.DataBind();

            cboBank.DataSource = ObjDb.GetRows("select * from ( "+
                "select '0' as id, '-Pilih Bank-' as name "+
                "union "+
                "select norek as id, kdrek+' - '+ket as name from mRekening where jenis in (1) and sts=2 "+
                "union "+
                "select norek as id, kdrek+' - '+ket as name from mRekening where jenis in (2) and sts=2 and noCabang = " + ObjSys.GetCabangId + " " +
                "union " +
                "select norek as id, kdrek+' - '+ket as name from mRekening where jenis = 22 " +
                ")x");
            cboBank.DataValueField = "id";
            cboBank.DataTextField = "name";
            cboBank.DataBind();

        }

        protected void loadDataFirst()
        {
            ObjGlobal.Param.Clear();
            ObjGlobal.Param.Add("tglBayar", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"));
            ObjGlobal.Param.Add("Bulan", cboBulan.SelectedValue);
            ObjGlobal.Param.Add("Tahun", cboTahun.SelectedValue);
            ObjGlobal.Param.Add("JnsTrans", cboJnsTrans.SelectedValue);
            ObjGlobal.Param.Add("Kelas", cboKelas.SelectedValue);
            ObjGlobal.Param.Add("Cabang", cboPerwakilanUnit.Text);
            ObjGlobal.Param.Add("Nama", txtNamaSiswa.Text);
            ObjGlobal.Param.Add("tipeDiskon", cboJnsDiskon.Text);
            grdARSiswa.DataSource = ObjGlobal.GetDataProcedure("SPLoadDataPelunasanAR1", ObjGlobal.Param);
            grdARSiswa.DataBind();
            if (grdARSiswa.Rows.Count > 0)
                button.Visible = true;
            else
            {
                button.Visible = false;
                txtTotal.Text = "";
            }
        }
        protected void grdARSiswa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CloseMessage();
            grdARSiswa.PageIndex = e.NewPageIndex;
            loadDataFirst();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CloseMessage();
            loadDataFirst();
            ShowHideGridAndForm(true, true);
            ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "Calculate()", "Calculate();", true);

        }

        protected void ShowMessage(string _class = "", string _message = "")
        {
            ((Label)Master.FindControl("lblMessage")).Text = ObjSys.GetMessage(_class, _message);
            ((Label)Master.FindControl("lblMessage")).Visible = true;
        }
        protected void CloseMessage()
        {
            ((Label)Master.FindControl("lblMessage")).Text = "";
            ((Label)Master.FindControl("lblMessage")).Visible = false;
        }

        protected void ShowHideGridAndForm(bool DivGrid, bool DivForm)
        {
            tabGrid.Visible = DivGrid;
            tabForm.Visible = DivForm;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string message = "";
            bool valid = true;
            int count = 0, selisih = 0;
            for (int i = 0; i < grdARSiswa.Rows.Count; i++)
            {
                CheckBox chkCheck = (CheckBox)grdARSiswa.Rows[i].FindControl("chkCheck");
                HiddenField hdnSaldo = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnSaldo");
                TextBox txtbayar = (TextBox)grdARSiswa.Rows[i].FindControl("txtbayar");
                TextBox txtDiskonD = (TextBox)grdARSiswa.Rows[i].FindControl("txtDiskonD");
                if (chkCheck.Checked == true && txtbayar.Text != "")
                    count++;

                if ((Convert.ToDecimal(txtbayar.Text) + Convert.ToDecimal(txtDiskonD.Text)) > Convert.ToDecimal(hdnSaldo.Value))
                    selisih++;
            }

            DataSet dataSaldoKas = ObjDb.GetRows("select isnull(MAX(Tgl),'1/1/1900') as tglMaxPost from tSaldokas where noCabang = '" + ObjSys.GetCabangId + "' and norek = '" + cboBank.Text + "' ");
            if (dataSaldoKas.Tables[0].Rows.Count > 0)
            {
                DataRow myRowSK = dataSaldoKas.Tables[0].Rows[0];
                string tglMaxPost = myRowSK["tglMaxPost"].ToString();
                // cek Post Saldo Kas Terakhir
                // sts = 0 udah posting, 1 = belum posting
                // cek Post Saldo Bulanan (Belum)
                if (Convert.ToDateTime(dtBayar.Text) < Convert.ToDateTime(tglMaxPost))
                {
                    message += ObjSys.CreateMessage("Tanggal Kas harus lebih besar tanggal terakhir posting.");
                    valid = false;
                }
            }

            DataSet dataSaldobln1 = ObjDb.GetRows("select distinct month(tgl) as bln from tsaldobln where noCabang = '" + ObjSys.GetCabangId + "' and sts=0 and year(tgl)='" + Convert.ToDateTime(dtBayar.Text).Year + "'");
            if (dataSaldobln1.Tables[0].Rows.Count > 0)
            {
                DataRow myRowSK = dataSaldobln1.Tables[0].Rows[0];
                int blnDb = int.Parse(myRowSK["bln"].ToString());

                //if (Convert.ToDateTime(dtBayar.Text).Month != blnDb)
                //{
                    message += ObjSys.CreateMessage("Sudah Posting Bulanan GL");
                    valid = false;
                //}

            }

            DataSet dataSaldobln = ObjDb.GetRows("select distinct year(tgl) as thn from btsaldobln where noCabang = '" + ObjSys.GetCabangId + "' and sts=0 and year(tgl)='" + Convert.ToDateTime(dtBayar.Text).Year + "'");
            if (dataSaldobln.Tables[0].Rows.Count > 0)
            {
                DataRow myRowSK = dataSaldobln.Tables[0].Rows[0];
                int thnDb = int.Parse(myRowSK["thn"].ToString());

                //if (Convert.ToDateTime(dtBayar.Text).Year != thnDb)
                //{
                    message += ObjSys.CreateMessage("Sudah Posting Tahunan GL");
                    valid = false;
                //}

            }


            if (dtBayar.Text == "")
            {
                message += ObjSys.CreateMessage("Tgl Bayar harus dipilih.");
                valid = false;
            }
            if (cboBank.Text == "0")
            {
                message += ObjSys.CreateMessage("Akun harus dipilih.");
                valid = false;
            }
            if (count == 0)
            {
                message += ObjSys.CreateMessage("Data harus dipilih.");
                valid = false;
            }
            if (selisih > 0)
            {
                message += ObjSys.CreateMessage("Nilai bayar + diskon harus <= nilai saldo.");
                valid = false;
            }
            

            if (valid == true && count != 0 && selisih == 0)
            {
                try
                {
                    decimal totalbayar = 0, totaldisc = 0;
                    string cek = "";
                    for (int i = 0; i < grdARSiswa.Rows.Count; i++)
                    {
                        HiddenField hdnNoPiut = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnNoPiut");
                        HiddenField hdnNoSiswa = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnNoSiswa");
                        HiddenField hdnNIK = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnNIK");
                        CheckBox chkCheck = (CheckBox)grdARSiswa.Rows[i].FindControl("chkCheck");
                        TextBox txtbayar = (TextBox)grdARSiswa.Rows[i].FindControl("txtbayar");
                        HiddenField hdnTglJt = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnTglJt");
                        HiddenField hdnThAj = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnThAj");
                        HiddenField hdnnoTrans = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnnoTrans");
                        // Diskon otomatis tidak di pake
                        //HiddenField hdnnilaiDisc = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnnilaiDisc");
                        TextBox txtDiskonD = (TextBox)grdARSiswa.Rows[i].FindControl("txtDiskonD");

                        if (chkCheck.Checked == true)
                        {
                            cek += hdnNoPiut.Value + ",";
                            decimal saldo = 0, sisasaldo = 0, diskon = 0, bayar = 0, nilaibayar = 0;
                            DataSet dataSNx = ObjDb.GetRows("SELECT nilaibayar, saldo, isnull(diskon,0) as diskon FROM TransPiutang WHERE noPiutang = '" + hdnNoPiut.Value + "'");
                            if (dataSNx.Tables[0].Rows.Count > 0)
                            {
                                DataRow myRowSnx = dataSNx.Tables[0].Rows[0];
                                bayar = Convert.ToDecimal(myRowSnx["nilaibayar"]);
                                saldo = Convert.ToDecimal(myRowSnx["saldo"]);
                                diskon = Convert.ToDecimal(myRowSnx["diskon"]);
                            }

                            nilaibayar = (Convert.ToDecimal(txtbayar.Text));

                            sisasaldo = (Convert.ToDecimal(saldo) - Convert.ToDecimal(txtbayar.Text) - Convert.ToDecimal(txtDiskonD.Text));

                            diskon = (Convert.ToDecimal(diskon) + Convert.ToDecimal(txtDiskonD.Text));

                            ObjGlobal.Param.Clear();
                            ObjGlobal.Param.Add("noPiutang", hdnNoPiut.Value);
                            ObjGlobal.Param.Add("noSiswa", hdnNoSiswa.Value);
                            ObjGlobal.Param.Add("saldo", Convert.ToDecimal(sisasaldo).ToString());
                            ObjGlobal.Param.Add("nobank", cboBank.Text);
                            ObjGlobal.Param.Add("nilaibayar", Convert.ToDecimal(nilaibayar).ToString());
                            ObjGlobal.Param.Add("tglbayar", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"));
                            ObjGlobal.Param.Add("diskon", Convert.ToDecimal(diskon).ToString());
                            ObjGlobal.Param.Add("norekdis", hdnNoRekDisc.Value);
                            ObjGlobal.GetDataProcedure("SPUpdateTransPiutang", ObjGlobal.Param);
                           
                            string denda = "";
                            DataSet dataDenda = ObjDb.GetRows("SELECT isnull(denda,0) as denda FROM mJenisTransaksi WHERE noTransaksi = '" + hdnnoTrans.Value + "'");
                            if (dataDenda.Tables[0].Rows.Count > 0)
                            {
                                DataRow myRowDnd = dataDenda.Tables[0].Rows[0];
                                denda = myRowDnd["denda"].ToString();
                            }

                            if (Convert.ToDateTime(dtBayar.Text) > Convert.ToDateTime(hdnTglJt.Value))
                            {
                                DataSet totnildenda = ObjDb.GetRows("select noTransaksi from mJenisTransaksi where jenisTransaksi like '%Denda%' and denda > 0 and noCabang = "+ cboPerwakilanUnit.Text +"");
                                if (totnildenda.Tables[0].Rows.Count > 0)
                                {
                                    DateTime tglbayar = DateTime.ParseExact(dtBayar.Text, "dd-MMM-yyyy", null).AddMonths(1);
                                    var datedenda = new DateTime(tglbayar.Year, tglbayar.Month, 10);

                                    DataRow myRowDenda = totnildenda.Tables[0].Rows[0];
                                    string noTranDenda = myRowDenda["noTransaksi"].ToString();

                                    ObjGlobal.Param.Clear();
                                    ObjGlobal.Param.Add("tahunajaran", hdnThAj.Value);
                                    ObjGlobal.Param.Add("noSiswa", hdnNoSiswa.Value);
                                    ObjGlobal.Param.Add("noTransaksi", noTranDenda);
                                    ObjGlobal.Param.Add("tgl", datedenda.ToString());
                                    ObjGlobal.Param.Add("tgljttempo", datedenda.ToString());
                                    ObjGlobal.Param.Add("nilai", Convert.ToDecimal(denda).ToString());
                                    ObjGlobal.Param.Add("saldo", Convert.ToDecimal(denda).ToString());
                                    ObjGlobal.Param.Add("noCabang", ObjSys.GetCabangId);
                                    ObjGlobal.GetDataProcedure("SPInputTransPiutang", ObjGlobal.Param);

                                }
                            }

                            totalbayar += Convert.ToDecimal(txtbayar.Text);
                            totaldisc += Convert.ToDecimal(txtDiskonD.Text);
                            DataSet cabangYangBisaCetak = ObjDb.GetRows("select * from mCabang where cetakvoucher = 1 and noCabang = " + cboPerwakilanUnit.Text + "");
                            if (cabangYangBisaCetak.Tables[0].Rows.Count > 0)
                            {
                                //Print Kuwitansi
                                HttpContext.Current.Session["ParamReport"] = null;
                                Session["REPORTNAME"] = null;
                                Session["REPORTTITLE"] = null;
                                Param.Clear();
                                Param.Add("noPiutang", hdnNoPiut.Value);
                                HttpContext.Current.Session.Add("ParamReport", Param);
                                Session["REPORTNAME"] = "PrintKuwitansi.rpt";
                                Session["REPORTTILE"] = "Kwitansi";
                            }
                                
                        }
                    }
                    
                    string Kode = ObjSys.GetCodeAutoNumberNewCustom("1", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"), Convert.ToInt32(cboBank.Text));

                    string kdrekkas = "";
                    DataSet dataSN = ObjDb.GetRows("SELECT kdrek FROM mRekening WHERE norek = '" + cboBank.Text + "'");
                    if (dataSN.Tables[0].Rows.Count > 0)
                    {
                        DataRow myRowSn = dataSN.Tables[0].Rows[0];
                        kdrekkas = myRowSn["kdrek"].ToString();
                    }

                    ObjGlobal.Param.Clear();
                    ObjGlobal.Param.Add("nomorKode", Kode);
                    ObjGlobal.Param.Add("type", "Kas/Bank Masuk");
                    ObjGlobal.Param.Add("Tgl", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"));
                    ObjGlobal.Param.Add("noRek", cboBank.Text);
                    ObjGlobal.Param.Add("kdrek", kdrekkas);
                    ObjGlobal.Param.Add("noCus", cboJnsTrans.Text);
                    ObjGlobal.Param.Add("Uraian", "Pembayaran Siswa "+ hdnjnsTrans.Value + "");
                    ObjGlobal.Param.Add("noMataUang", "0");
                    ObjGlobal.Param.Add("kursKas", "1");
                    ObjGlobal.Param.Add("Nilai", Convert.ToDecimal(totalbayar).ToString());
                    ObjGlobal.Param.Add("nilaiRp", Convert.ToDecimal(totalbayar).ToString());
                    ObjGlobal.Param.Add("noCabang", ObjSys.GetCabangId);
                    ObjGlobal.Param.Add("sts", "1");
                    ObjGlobal.Param.Add("StsApv", "0");
                    ObjGlobal.Param.Add("createdBy", ObjSys.GetUserId);
                    ObjGlobal.Param.Add("createdDate", ObjSys.GetNow);
                    ObjGlobal.GetDataProcedure("SPInputKasPelunasan", ObjGlobal.Param);

                    

                    for (int i = 0; i < grdARSiswa.Rows.Count; i++)
                    {
                        HiddenField hdnNoPiut = (HiddenField)grdARSiswa.Rows[i].FindControl("hdnNoPiut");
                        CheckBox chkCheck = (CheckBox)grdARSiswa.Rows[i].FindControl("chkCheck");

                        if (chkCheck.Checked == true)
                        {
                            ObjGlobal.Param.Clear();
                            ObjGlobal.Param.Add("noPiutang", hdnNoPiut.Value);
                            ObjGlobal.Param.Add("nomorKode", Kode);
                            ObjGlobal.GetDataProcedure("SPUpdateNomorKodeTransPiutang", ObjGlobal.Param);
                        }
                    }

                    ObjGlobal.Param.Clear();
                    ObjGlobal.Param.Add("nomorKode", Kode);
                    ObjGlobal.Param.Add("type", "Kas/Bank Masuk");
                    ObjGlobal.Param.Add("Tgl", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"));
                    ObjGlobal.Param.Add("noCabang", ObjSys.GetCabangId);
                    ObjGlobal.GetDataProcedure("SPInputKasHistoryBayarPelunasan", ObjGlobal.Param);

                    DataSet mySet = ObjDb.GetRows("select * from tkas where nomorKode = '" + Kode + "'");
                    DataRow myRow = mySet.Tables[0].Rows[0];
                    string Id = myRow["noKas"].ToString();

                    ObjGlobal.Param.Clear();
                    ObjGlobal.Param.Add("noKas", Id);
                    ObjGlobal.Param.Add("kdTran", Kode);
                    ObjGlobal.Param.Add("jenisTran", "Pembayaran Siswa");
                    ObjGlobal.Param.Add("noTran", Id);
                    ObjGlobal.Param.Add("noRek", cboBank.Text);
                    ObjGlobal.Param.Add("kdRek", kdrekkas);
                    ObjGlobal.Param.Add("Uraian", "Pembayaran Siswa " + hdnjnsTrans.Value + "");
                    ObjGlobal.Param.Add("Debet", Convert.ToDecimal(totalbayar).ToString());
                    ObjGlobal.Param.Add("Kredit", "0");
                    ObjGlobal.Param.Add("sts", "0");
                    ObjGlobal.Param.Add("Tgl", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"));
                    ObjGlobal.Param.Add("noCabang", ObjSys.GetCabangId);
                    ObjGlobal.Param.Add("createdBy", ObjSys.GetUserId);
                    ObjGlobal.Param.Add("createdDate", ObjSys.GetNow);
                    ObjGlobal.GetDataProcedure("SPInputKasDetilPelunasan", ObjGlobal.Param);

                    ObjGlobal.Param.Clear();
                    ObjGlobal.Param.Add("noPiut", cek);
                    DataSet mySetH = ObjGlobal.GetDataProcedure("SPLoadTampnoPiut", ObjGlobal.Param);
                    foreach (DataRow myRowH in mySetH.Tables[0].Rows)
                    {
                        string kdreDd = ""; string norekDb = "";
                        DataSet dataSNx = ObjDb.GetRows("select a.norekDb, b.kdRek from mJenisTransaksi a inner join mRekening b on a.norekDb = b.noRek where a.noTransaksi = '" + myRowH["noTransaksi"].ToString() + "'");
                        if (dataSNx.Tables[0].Rows.Count > 0)
                        {
                            DataRow myRowSnx = dataSNx.Tables[0].Rows[0];
                            norekDb = myRowSnx["norekDb"].ToString();
                            kdreDd = myRowSnx["kdrek"].ToString();
                        }

                        ObjGlobal.Param.Clear();
                        ObjGlobal.Param.Add("noKas", Id);
                        ObjGlobal.Param.Add("kdTran", Kode);
                        ObjGlobal.Param.Add("jenisTran", "Pembayaran Siswa");
                        ObjGlobal.Param.Add("noTran", Id);
                        ObjGlobal.Param.Add("noRek", norekDb);
                        ObjGlobal.Param.Add("kdRek", kdreDd);
                        ObjGlobal.Param.Add("Uraian", "Pembayaran Siswa " + hdnjnsTrans.Value + "");
                        ObjGlobal.Param.Add("Debet", "0");
                        ObjGlobal.Param.Add("Kredit", Convert.ToDecimal(Convert.ToDecimal(totalbayar) + Convert.ToDecimal(totaldisc)).ToString());
                        ObjGlobal.Param.Add("sts", "0");
                        ObjGlobal.Param.Add("Tgl", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"));
                        ObjGlobal.Param.Add("noCabang", ObjSys.GetCabangId);
                        ObjGlobal.Param.Add("createdBy", ObjSys.GetUserId);
                        ObjGlobal.Param.Add("createdDate", ObjSys.GetNow);
                        ObjGlobal.GetDataProcedure("SPInputKasDetilPelunasan2", ObjGlobal.Param);
                        
                    }

                    //jika ada diskon
                    if (Convert.ToDecimal(totaldisc) > 0)
                    {
                        string kdreDdDis = ""; string norekDbDis = "";
                        DataSet dataDis = ObjDb.GetRows("select norek, kdRek from mRekening where noRek = '" + hdnNoRekDisc.Value + "'");
                        if (dataDis.Tables[0].Rows.Count > 0)
                        {
                            DataRow myRowDis = dataDis.Tables[0].Rows[0];
                            norekDbDis = myRowDis["norek"].ToString();
                            kdreDdDis = myRowDis["kdrek"].ToString();
                        }

                        ObjGlobal.Param.Clear();
                        ObjGlobal.Param.Add("noKas", Id);
                        ObjGlobal.Param.Add("kdTran", Kode);
                        ObjGlobal.Param.Add("jenisTran", "Pembayaran Siswa");
                        ObjGlobal.Param.Add("noTran", Id);
                        ObjGlobal.Param.Add("noRek", norekDbDis);
                        ObjGlobal.Param.Add("kdRek", kdreDdDis);
                        ObjGlobal.Param.Add("Uraian", "Diskon Pembayaran Siswa " + hdnjnsTrans.Value + "");
                        ObjGlobal.Param.Add("Debet", Convert.ToDecimal(totaldisc).ToString());
                        ObjGlobal.Param.Add("Kredit", "0");
                        ObjGlobal.Param.Add("sts", "0");
                        ObjGlobal.Param.Add("Tgl", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"));
                        ObjGlobal.Param.Add("noCabang", ObjSys.GetCabangId);
                        ObjGlobal.Param.Add("createdBy", ObjSys.GetUserId);
                        ObjGlobal.Param.Add("createdDate", ObjSys.GetNow);
                        ObjGlobal.GetDataProcedure("SPInputKasDetilDanaPelunasan", ObjGlobal.Param);
                        
                    }
                    ObjSys.UpdateAutoNumberCodeNewCustom("1", Convert.ToDateTime(dtBayar.Text).ToString("yyyy-MM-dd"), Convert.ToInt32(cboBank.Text));
                    DataSet cabangBisaCetak = ObjDb.GetRows("select * from mCabang where cetakvoucher = 1 and noCabang = " + cboPerwakilanUnit.Text + "");
                    if (cabangBisaCetak.Tables[0].Rows.Count > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OpenReport", "OpenReport();", true);
                        // end code
                    }else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                    }
                    loadDataFirst();
                    ShowMessage("success", "Data berhasil disimpan.");
                    
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                    ShowMessage("error", ex.Message);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                ShowMessage("error", message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (cboKelas.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
            //    ShowMessage("error", "Kelas harus dipilih.");
            //}
            //else 
            if (cboJnsTrans.Text == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                ShowMessage("error", "Jenis Transaksi harus dipilih.");
            }
            else if (cboBulan.Text == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                ShowMessage("error", "Bulan harus dipilih.");
            }
            else if (cboTahun.Text == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "ToTheTop", "ToTopOfPage();", true);
                ShowMessage("error", "Tahun harus dipilih.");
            }
            else
            {
                string noRek = "0", nilaiDiskon = "0";
                if (cboJnsDiskon.Text != "0")
                {
                    DataSet mySet = ObjDb.GetRows("select isnull(noRek,0) as noRek, case when jns = 'Persen' then nilai/100 else nilai end as nilaiDiskon from parameterDisc where noParamdisc = '" + cboJnsDiskon.Text + "' ");
                    DataRow myRow = mySet.Tables[0].Rows[0];
                    nilaiDiskon = Convert.ToDecimal(myRow["nilaiDiskon"]).ToString();
                    ////noRek = myRow["noRek"].ToString();
                }
                hdnNoRekDisc.Value = noRek;
                hdnnilaiDiskon.Value = nilaiDiskon;

                DataSet mySetJnsT = ObjDb.GetRows("select jenisTransaksi from mJenisTransaksi where noTransaksi = '" + cboJnsTrans.Text + "' ");
                DataRow myRowJnsT = mySetJnsT.Tables[0].Rows[0];
                string jenisTransaksi = myRowJnsT["jenisTransaksi"].ToString();
                hdnjnsTrans.Value = jenisTransaksi;

                CloseMessage();
                loadDataFirst();
                ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "Calculate", "Calculate();", true);
            }
        }

        protected void cboJnsDiskon_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseMessage();
            loadDataJnsTransaksi(cboPerwakilanUnit.Text, cboJnsDiskon.Text);
        }
    }
}