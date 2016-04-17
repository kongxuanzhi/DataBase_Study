namespace 登陆防注击
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Uid = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.TextBox();
            this.pwd = new System.Windows.Forms.TextBox();
            this.UPwd = new System.Windows.Forms.Label();
            this.Login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Uid
            // 
            this.Uid.AutoSize = true;
            this.Uid.Location = new System.Drawing.Point(108, 62);
            this.Uid.Name = "Uid";
            this.Uid.Size = new System.Drawing.Size(41, 12);
            this.Uid.TabIndex = 0;
            this.Uid.Text = "用户名";
            // 
            // ID
            // 
            this.ID.Location = new System.Drawing.Point(204, 59);
            this.ID.Name = "ID";
            this.ID.Size = new System.Drawing.Size(100, 21);
            this.ID.TabIndex = 1;
            // 
            // pwd
            // 
            this.pwd.Location = new System.Drawing.Point(204, 119);
            this.pwd.Name = "pwd";
            this.pwd.PasswordChar = '*';
            this.pwd.Size = new System.Drawing.Size(100, 21);
            this.pwd.TabIndex = 3;
            // 
            // UPwd
            // 
            this.UPwd.AutoSize = true;
            this.UPwd.Location = new System.Drawing.Point(108, 122);
            this.UPwd.Name = "UPwd";
            this.UPwd.Size = new System.Drawing.Size(29, 12);
            this.UPwd.TabIndex = 2;
            this.UPwd.Text = "密码";
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(303, 191);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(75, 23);
            this.Login.TabIndex = 4;
            this.Login.Text = "登陆";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 432);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.UPwd);
            this.Controls.Add(this.ID);
            this.Controls.Add(this.Uid);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Uid;
        private System.Windows.Forms.TextBox ID;
        private System.Windows.Forms.TextBox pwd;
        private System.Windows.Forms.Label UPwd;
        private System.Windows.Forms.Button Login;
    }
}

