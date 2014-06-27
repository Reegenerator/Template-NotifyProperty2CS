namespace Test {
    partial class PersonForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.TextBox4 = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.TextBox5 = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.TextBox6 = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.PersonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PersonBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TextBox1
            // 
            this.TextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PersonBindingSource, "Name", true));
            this.TextBox1.Location = new System.Drawing.Point(176, 69);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.ReadOnly = true;
            this.TextBox1.Size = new System.Drawing.Size(214, 20);
            this.TextBox1.TabIndex = 0;
            this.TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // TextBox2
            // 
            this.TextBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PersonBindingSource, "Address", true));
            this.TextBox2.Location = new System.Drawing.Point(176, 104);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(214, 20);
            this.TextBox2.TabIndex = 0;
            // 
            // TextBox3
            // 
            this.TextBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PersonBindingSource, "Age", true));
            this.TextBox3.Location = new System.Drawing.Point(176, 146);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(214, 20);
            this.TextBox3.TabIndex = 0;
            // 
            // TextBox4
            // 
            this.TextBox4.Location = new System.Drawing.Point(176, 186);
            this.TextBox4.Name = "TextBox4";
            this.TextBox4.Size = new System.Drawing.Size(214, 20);
            this.TextBox4.TabIndex = 0;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(36, 72);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 13);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Name";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(36, 107);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(45, 13);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "Address";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(36, 149);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(26, 13);
            this.Label3.TabIndex = 1;
            this.Label3.Text = "Age";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(36, 189);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(53, 13);
            this.Label4.TabIndex = 1;
            this.Label4.Text = "AgeString";
            // 
            // TextBox5
            // 
            this.TextBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PersonBindingSource, "LastName", true));
            this.TextBox5.Location = new System.Drawing.Point(176, 43);
            this.TextBox5.Name = "TextBox5";
            this.TextBox5.Size = new System.Drawing.Size(214, 20);
            this.TextBox5.TabIndex = 0;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(36, 46);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(121, 13);
            this.Label5.TabIndex = 1;
            this.Label5.Text = "Last Name (Not notified)";
            // 
            // TextBox6
            // 
            this.TextBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PersonBindingSource, "FirstName", true));
            this.TextBox6.Location = new System.Drawing.Point(176, 17);
            this.TextBox6.Name = "TextBox6";
            this.TextBox6.Size = new System.Drawing.Size(214, 20);
            this.TextBox6.TabIndex = 0;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(36, 20);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(57, 13);
            this.Label6.TabIndex = 1;
            this.Label6.Text = "First Name";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(406, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PersonBindingSource
            // 
            this.PersonBindingSource.DataSource = typeof(Test.Person);
            // 
            // PersonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 339);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TextBox4);
            this.Controls.Add(this.TextBox3);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.TextBox6);
            this.Controls.Add(this.TextBox5);
            this.Controls.Add(this.TextBox1);
            this.Name = "PersonForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.PersonBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.BindingSource PersonBindingSource;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.TextBox TextBox3;
        internal System.Windows.Forms.TextBox TextBox4;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox TextBox5;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox TextBox6;
        internal System.Windows.Forms.Label Label6;

        #endregion
        private System.Windows.Forms.Button button1;

    }
}