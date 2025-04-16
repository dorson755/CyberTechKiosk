using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace CyberTechKiosk
{
    public partial class MainForm : Form
    {
        // Control declarations
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private Label label1;
        private ComboBox cmbPrimaryMajor;
        private ComboBox cmbSecondaryMajor;
        private Button btnSubmit;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadMajors();
        }

        private void LoadMajors()
        {
            try
            {
                cmbPrimaryMajor.Items.Clear();
                cmbSecondaryMajor.Items.Clear();

                using (var conn = new SQLiteConnection(DatabaseManager.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("SELECT Id, Name FROM Majors WHERE IsActive = 1", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new MajorItem(
                                Convert.ToInt32(reader["Id"]),
                                reader["Name"].ToString()
                            );
                            cmbPrimaryMajor.Items.Add(item);
                            cmbSecondaryMajor.Items.Add(item);
                        }
                    }
                }

                cmbPrimaryMajor.DisplayMember = "Name";
                cmbSecondaryMajor.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading majors: {ex.Message}");
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || cmbPrimaryMajor.SelectedItem == null)
            {
                MessageBox.Show("Please enter your name and select a primary major!");
                return;
            }

            try
            {
                using (var conn = new SQLiteConnection(DatabaseManager.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        @"INSERT INTO StudentLeads 
                (FullName, Phone, Email, PrimaryMajorId, SecondaryMajorId)
                VALUES (@name, @phone, @email, @primary, @secondary)", conn))
                    {
                        var primary = (MajorItem)cmbPrimaryMajor.SelectedItem;
                        var secondary = cmbSecondaryMajor.SelectedItem as MajorItem;

                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@primary", primary.Id);

                        // Proper null handling for secondary major
                        if (secondary != null)
                        {
                            cmd.Parameters.AddWithValue("@secondary", secondary.Id);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@secondary", DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thank you for your interest!");

                        // Clear form
                        txtName.Text = txtEmail.Text = txtPhone.Text = "";
                        cmbPrimaryMajor.SelectedIndex = cmbSecondaryMajor.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Submission error: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPrimaryMajor = new System.Windows.Forms.ComboBox();
            this.cmbSecondaryMajor = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(364, 199);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(191, 20);
            this.txtName.TabIndex = 0;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(364, 238);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(191, 20);
            this.txtEmail.TabIndex = 1;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(364, 275);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(191, 20);
            this.txtPhone.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(380, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cyber Tech College";
            // 
            // cmbPrimaryMajor
            // 
            this.cmbPrimaryMajor.FormattingEnabled = true;
            this.cmbPrimaryMajor.Location = new System.Drawing.Point(364, 316);
            this.cmbPrimaryMajor.Name = "cmbPrimaryMajor";
            this.cmbPrimaryMajor.Size = new System.Drawing.Size(191, 21);
            this.cmbPrimaryMajor.TabIndex = 4;
            // 
            // cmbSecondaryMajor
            // 
            this.cmbSecondaryMajor.FormattingEnabled = true;
            this.cmbSecondaryMajor.Location = new System.Drawing.Point(364, 357);
            this.cmbSecondaryMajor.Name = "cmbSecondaryMajor";
            this.cmbSecondaryMajor.Size = new System.Drawing.Size(191, 21);
            this.cmbSecondaryMajor.TabIndex = 5;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(407, 400);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(106, 23);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(925, 555);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cmbSecondaryMajor);
            this.Controls.Add(this.cmbPrimaryMajor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtName);
            this.Name = "MainForm";
            this.Text = "Cyber Tech Career College Info Request";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }

    public class MajorItem
    {
        public int Id { get; }
        public string Name { get; }

        public MajorItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}