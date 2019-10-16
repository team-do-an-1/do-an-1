﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Diagnostics;

namespace QuanLyDiemSinhVien
{
    public partial class MonHoc : Form
    {
        private Connect ketnoi = new Connect();
        SqlConnection conn = null;

        public MonHoc()
        {
            InitializeComponent();
        }

        private void MonHoc_Load(object sender, EventArgs e)
        {
            conn = ketnoi.Connected();
            if (conn.State == ConnectionState.Open) ;
            string sqlkhoa = "select * from  [QuanLyDiemSinhVien].[dbo].[Mon]";
            SqlCommand comand = new SqlCommand(sqlkhoa, conn);
            SqlDataAdapter sqlcom = new SqlDataAdapter(comand);
            DataTable table = new DataTable();
            sqlcom.Fill(table);
            dgrMON.DataSource = table;

            string select = "Select MaKhoa from Khoa ";
            SqlCommand cmd = new SqlCommand(select, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                cboKhoa.Items.Add(reader.GetString(0));
            }
            reader.Dispose();
            cmd.Dispose();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string select1 = "Select MaMon from Mon where MaMon='" + txtMaMon.Text + "' ";
            SqlCommand cmd1 = new SqlCommand(select1, conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            errorProvider1.Clear();
            if (txtMaMon.Text == "")
            {
                errorProvider1.SetError(txtMaMon, "Mã môn không để trống!");
            }
            else if (reader1.Read())
            {
                {
                    MessageBox.Show("Bạn đã nhập thông tin cho môn: " + txtTenMon.Text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaMon.Focus();

                }


                //Tra tai nguyen 
                reader1.Dispose();
                cmd1.Dispose();
            }
            else
            {
                reader1.Dispose();
                cmd1.Dispose();
                // Thực hiện truy vấn
                string insert = "Insert Into Mon(MaMon,TenMon,SoTC,MaGV,HocKi,MaKhoa)" +
                "Values('" + txtMaMon.Text + "',N'" + txtTenMon.Text + "','" + txtSDVHT.Text + "','" +
                txtMaGV.Text + "','" + txtHocKy.Text + "','" + cboKhoa.Text + "')";
                SqlCommand cmd = new SqlCommand(insert, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Nhập thông tin thành công", "Thông báo!");

                // Trả tài nguyên


                cmd.Dispose();
                //Fill du lieu vao Database
                FillDataGridView_MON();
            }
            reader1.Dispose();
            cmd1.Dispose();
        }
        public void FillDataGridView_MON()
        {
            // Thực hiện truy vấn
            string select = "Select * From Mon  ";
            SqlCommand cmd = new SqlCommand(select, conn);

            // Tạo đối tượng DataSet
            DataSet ds = new DataSet();

            // Tạo đối tượng điều hợp
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;

            // Fill dữ liệu từ adapter vào DataSet
            adapter.Fill(ds, "SINHVIEN");

            // Đưa ra DataGridView
            dgrMON.DataSource = ds;
            dgrMON.DataMember = "SINHVIEN";
            cmd.Dispose();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Thực hiện truy vấn
            string update = "Update Mon Set TenMon=N'" + txtTenMon.Text + "',SoTC='" +
                            txtSDVHT.Text + "',MaGV='" + txtMaGV.Text + "',HocKi='" +
                            txtHocKy.Text + "',MaKhoa='" + cboKhoa.Text + "' where MaMon='" + txtMaMon.Text + "' ";
            SqlCommand cmd = new SqlCommand(update, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Load lai du lieu
            FillDataGridView_MON();
            // Trả tài nguyên
            cmd.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Thuc hien xoa du lieu
            string delete = "delete from Mon where MaMon='" + txtMaMon.Text + "' ";
            SqlCommand cmd = new SqlCommand(delete, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Xóa dữ liệu thành công", "Thông báo!");

            // Trả tài nguyên
            cmd.Dispose();
            //Load lai du lieu
            FillDataGridView_MON();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgrMON_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaMon.Text = dgrMON.CurrentRow.Cells[0].Value.ToString();
            txtTenMon.Text = dgrMON.CurrentRow.Cells[1].Value.ToString();
            txtSDVHT.Text = dgrMON.CurrentRow.Cells[2].Value.ToString();
            txtMaGV.Text = dgrMON.CurrentRow.Cells[3].Value.ToString();
            txtHocKy.Text = dgrMON.CurrentRow.Cells[4].Value.ToString();
            cboKhoa.Text = dgrMON.CurrentRow.Cells[5].Value.ToString();
        }
    }
}
