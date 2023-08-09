using System;

namespace OPC_Kepserver
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.ConnBtn = new System.Windows.Forms.Button();
            this.WriteBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ServerNode = new System.Windows.Forms.ComboBox();
            this.ServerName = new System.Windows.Forms.ComboBox();
            this.OpcItemViewer = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.OpcList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.OpcItemViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Location = new System.Drawing.Point(564, 77);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.RefreshBtn.TabIndex = 0;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // ConnBtn
            // 
            this.ConnBtn.Location = new System.Drawing.Point(564, 129);
            this.ConnBtn.Name = "ConnBtn";
            this.ConnBtn.Size = new System.Drawing.Size(75, 23);
            this.ConnBtn.TabIndex = 1;
            this.ConnBtn.Text = "Connect";
            this.ConnBtn.UseVisualStyleBackColor = true;
            this.ConnBtn.Click += new System.EventHandler(this.ConnBtn_Click);
            // 
            // WriteBtn
            // 
            this.WriteBtn.Location = new System.Drawing.Point(564, 176);
            this.WriteBtn.Name = "WriteBtn";
            this.WriteBtn.Size = new System.Drawing.Size(75, 23);
            this.WriteBtn.TabIndex = 2;
            this.WriteBtn.Text = "Write";
            this.WriteBtn.UseVisualStyleBackColor = true;
            this.WriteBtn.Click += new System.EventHandler(this.WriteBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(201, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "ServerNode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "ServerName";
            // 
            // ServerNode
            // 
            this.ServerNode.FormattingEnabled = true;
            this.ServerNode.Location = new System.Drawing.Point(312, 80);
            this.ServerNode.Name = "ServerNode";
            this.ServerNode.Size = new System.Drawing.Size(169, 20);
            this.ServerNode.TabIndex = 5;
            this.ServerNode.SelectedIndexChanged += new System.EventHandler(this.ServerNode_SelectedIndexChanged);
            // 
            // ServerName
            // 
            this.ServerName.FormattingEnabled = true;
            this.ServerName.Location = new System.Drawing.Point(312, 131);
            this.ServerName.Name = "ServerName";
            this.ServerName.Size = new System.Drawing.Size(169, 20);
            this.ServerName.TabIndex = 6;
            this.ServerName.SelectedIndexChanged += new System.EventHandler(this.ServerName_SelectedIndexChanged);
            // 
            // OpcItemViewer
            // 
            this.OpcItemViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OpcItemViewer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Value,
            this.TimeStamp,
            this.Quality});
            this.OpcItemViewer.Location = new System.Drawing.Point(168, 259);
            this.OpcItemViewer.Name = "OpcItemViewer";
            this.OpcItemViewer.RowTemplate.Height = 23;
            this.OpcItemViewer.Size = new System.Drawing.Size(442, 150);
            this.OpcItemViewer.TabIndex = 7;
            this.OpcItemViewer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // TimeStamp
            // 
            this.TimeStamp.HeaderText = "TimeStamp";
            this.TimeStamp.Name = "TimeStamp";
            // 
            // Quality
            // 
            this.Quality.HeaderText = "Quality";
            this.Quality.Name = "Quality";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // OpcList
            // 
            this.OpcList.FormattingEnabled = true;
            this.OpcList.Location = new System.Drawing.Point(312, 179);
            this.OpcList.Name = "OpcList";
            this.OpcList.Size = new System.Drawing.Size(169, 20);
            this.OpcList.TabIndex = 8;
            this.OpcList.SelectedIndexChanged += new System.EventHandler(this.OpcList_SelectedIndexChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OpcList);
            this.Controls.Add(this.OpcItemViewer);
            this.Controls.Add(this.ServerName);
            this.Controls.Add(this.ServerNode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.WriteBtn);
            this.Controls.Add(this.ConnBtn);
            this.Controls.Add(this.RefreshBtn);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.OpcItemViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Button ConnBtn;
        private System.Windows.Forms.Button WriteBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ServerNode;
        private System.Windows.Forms.ComboBox ServerName;
        private System.Windows.Forms.DataGridView OpcItemViewer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quality;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ComboBox OpcList;
    }
}