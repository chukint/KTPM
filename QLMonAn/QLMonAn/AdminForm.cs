
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace QLMonAn
{
    public partial class AdminForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-R6GO7FIS\\CHUKINT;Initial Catalog=QuanLyMonAn;Integrated Security=True";
        private SqlConnection connection;
        public AdminForm()
        {
            InitializeComponent();
            dgvDsMonAn.AutoGenerateColumns = false;
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            /*string connectionString = "Data Source=LAPTOP-R6GO7FIS\\CHUKINT;Initial Catalog=QuanLyMonAn;Integrated Security=True";*/
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                MessageBox.Show("Kết nối thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại: " + ex.Message);
            }
        }
        private void AdminForm_Load(object sender, System.EventArgs e)
        {
            HienThiDSMon();
            HienThiNhomMonAn();
        }

        private void HienThiNhomMonAn()
        {
            var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM NhomMon";
            var adapter = new SqlDataAdapter(command);
            var table = new DataTable("TenNhom");

            cbbNhom.DisplayMember = "TenNhom";
            cbbNhom.ValueMember = "NhomMonID";
            cbbNhom.DataSource = table;

            conn.Open();
            adapter.Fill(table);
            conn.Close();

        }

        private void HienThiDSMon()
        {
            var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();
            command.CommandText = "SELECT tt.MonAnID, tt.TenMon, tt.DonViTinh, tt.DonGia, nm.TenNhom " +
                               "FROM ThongTinMonAn tt " +
                                "INNER JOIN NhomMon nm ON tt.NhomMonID = nm.NhomMonID";
            var adapter = new SqlDataAdapter(command);
            var table = new DataTable("DSMon");
            
            conn.Open();
            adapter.Fill(table);
            conn.Close();

            
            dgvDsMonAn.DataSource = table;
        }

        private void HienThiDsMonAnTheoLoai(int maNhom)
        {
            var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();
            command.CommandText = "SELECT m.*, TenNhom " +
                                  "FROM ThongTinMonAn m, NhomMon nma " +
                                  "WHERE m.Nhom = nma.MaNhom and " +
                                  "m.Nhom = " + maNhom;

            var adapter = new SqlDataAdapter(command);
            var table = new DataTable("DanhSachMonAn");

            conn.Open();
            adapter.Fill(table);
            conn.Close();

            dgvDsMonAn.DataSource = table;
        }
        private void cbbNhom_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var ma = Convert.ToInt32(cbbNhom.SelectedValue);

            HienThiDsMonAnTheoLoai(ma);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var vanBanTim = txtSearch.Text;

            var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();
            command.CommandText = "SELECT m.*, TenNhom " +
                                  "FROM MonAn m, NhomMonAn nma " +
                                  "WHERE m.Nhom = nma.MaNhom and " +
                                  "m.TenMonAn like '%" + vanBanTim + "%'";

            var adapter = new SqlDataAdapter(command);
            var table = new DataTable("MonAn");
            conn.Open();
            adapter.Fill(table);
            conn.Close();

            dgvDsMonAn.DataSource = table;
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            var form = new FoodForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var nhom = form.NhomMA;
                HienThiDsMonAnTheoLoai(nhom);
            }
        }

        private void dgvDsMonAn_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDsMonAn.SelectedRows.Count > 0)
            {
                var maMonAn = Convert.ToInt32(dgvDsMonAn.SelectedRows[0].Cells[0].Value);

                var form = new FoodForm(maMonAn);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var nhom = form.NhomMA;
                    HienThiDsMonAnTheoLoai(nhom);
                }
            }
        }
        public bool AddFood(string foodName, string unit, decimal price, string group)
        {
            string query = "INSERT INTO ThongTinMonAn (TenMonAn, DonViTinh, DonGia, NhomMon) VALUES (@TenMonAn, @DonViTinh, @DonGia, @NhomMon)";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@TenMonAn", foodName);
                cmd.Parameters.AddWithValue("@DonViTinh", unit);
                cmd.Parameters.AddWithValue("@DonGia", price);
                cmd.Parameters.AddWithValue("@NhomMon", group);

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thêm món ăn thất bại: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
