using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using MrCy.BaseClass;

namespace MrCy {
    public partial class Login : Form {
        /// <summary>
        /// 创建登录实例时的操作
        /// </summary>
        public Login() {
            InitializeComponent();
        }


        /// <summary>
        /// 初始化loading的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e) {
            txtName.Focus();
        }


        /// <summary>
        /// 回车操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtPwdKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) {
                BtnSubmitClick(sender, e);
            }
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmitClick(object sender, EventArgs e) {
            if (txtName.Text == "") {
                MessageBox.Show(Constants.PleaseInputUserName, Constants.Warn, MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else {
                if (txtPwd.Text == "") {
                    MessageBox.Show(Constants.PleaseInputPassword, Constants.Warn, MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else {
                    var conn = DbConn.CyCon();
                    conn.Open();
                    var cmd = new SqlCommand(
                        "select count(*) from tb_User where UserName='" + txtName.Text + "' and UserPwd='" +
                        txtPwd.Text + "'", conn);
                    var i = Convert.ToInt32(cmd.ExecuteScalar());
                    if (i > 0) {
                        cmd = new SqlCommand("select * from tb_User where UserName='" + txtName.Text + "'", conn);
                        var sdr = cmd.ExecuteReader();
                        sdr.Read();
                        var userPower = sdr["power"].ToString().Trim();
                        conn.Close();
                        var main = new Main {
                            power = userPower,
                            Names = txtName.Text,
                            Times = DateTime.Now.ToShortDateString()
                        };
                        main.Show();
                        Hide();
                    }
                    else {
                        MessageBox.Show(Constants.UserNameOrPasswordError);
                    }
                }
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelClick(object sender, EventArgs e) {
            if (MessageBox.Show(Constants.ExitSystem, Constants.Warn, MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Asterisk) ==
                DialogResult.OK) {
                Application.Exit();
            }
        }
    }
}