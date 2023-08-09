using System;

namespace OPC_Kepserver
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ServerNode = new System.Windows.Forms.ComboBox();
            this.ServerName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.ConnBtn = new System.Windows.Forms.Button();
            this.WriteBtn = new System.Windows.Forms.Button();
            this.OpcList = new System.Windows.Forms.ComboBox();
            this.OpcItemViewer = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.OpcItemViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // ServerNode
            // 
            this.ServerNode.FormattingEnabled = true;
            this.ServerNode.Location = new System.Drawing.Point(241, 80);
            this.ServerNode.Name = "ServerNode";
            this.ServerNode.Size = new System.Drawing.Size(169, 20);
            this.ServerNode.TabIndex = 0;
            this.ServerNode.SelectedIndexChanged += new System.EventHandler(this.ServerNode_SelectedIndexChanged);
            // 
            // ServerName
            // 
            this.ServerName.FormattingEnabled = true;
            this.ServerName.Location = new System.Drawing.Point(241, 147);
            this.ServerName.Name = "ServerName";
            this.ServerName.Size = new System.Drawing.Size(169, 20);
            this.ServerName.TabIndex = 1;
            this.ServerName.SelectedIndexChanged += new System.EventHandler(this.ServerName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(134, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "ServerNode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(134, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "ServerName";
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Location = new System.Drawing.Point(475, 77);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.RefreshBtn.TabIndex = 4;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // ConnBtn
            // 
            this.ConnBtn.Location = new System.Drawing.Point(475, 147);
            this.ConnBtn.Name = "ConnBtn";
            this.ConnBtn.Size = new System.Drawing.Size(75, 23);
            this.ConnBtn.TabIndex = 5;
            this.ConnBtn.Text = "Connect";
            this.ConnBtn.UseVisualStyleBackColor = true;
            this.ConnBtn.Click += new System.EventHandler(this.ConnBtn_Click);

            // WriteBtn
            // 
            this.WriteBtn.Location = new System.Drawing.Point(0, 0);
            this.WriteBtn.Name = "WriteBtn";
            this.WriteBtn.Size = new System.Drawing.Size(75, 23);
            this.WriteBtn.TabIndex = 8;
            this.WriteBtn.Text = "WriteBtn";
            this.WriteBtn.UseVisualStyleBackColor = true;
            this.WriteBtn.Click += new System.EventHandler(this.WriteBtn_Click);
            // 
            // OpcList
            // 
            this.OpcList.FormattingEnabled = true;
            this.OpcList.Location = new System.Drawing.Point(232, 198);
            this.OpcList.Name = "OpcList";
            this.OpcList.Size = new System.Drawing.Size(189, 20);
            this.OpcList.TabIndex = 6;
            this.OpcList.SelectedIndexChanged += new System.EventHandler(this.OpcList_SelectedIndexChanged);
            // 
            // OpcItemViewer
            // 
            this.OpcItemViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OpcItemViewer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Value,
            this.TimeStamp,
            this.Quality});
            this.OpcItemViewer.Location = new System.Drawing.Point(152, 268);
            this.OpcItemViewer.Name = "OpcItemViewer";
            this.OpcItemViewer.RowTemplate.Height = 23;
            this.OpcItemViewer.Size = new System.Drawing.Size(446, 150);
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
            this.timer2.Interval = 250;
            // 
            // timer4
            // 
            this.timer4.Interval = 250;
            this.timer4.Enabled = true;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OpcItemViewer);
            this.Controls.Add(this.OpcList);
            this.Controls.Add(this.ConnBtn);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ServerName);
            this.Controls.Add(this.ServerNode);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OpcItemViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Button WriteBtn;
        private System.Windows.Forms.ComboBox ServerNode;
        private System.Windows.Forms.ComboBox ServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Button ConnBtn;

        private System.Windows.Forms.ComboBox OpcList;
        private System.Windows.Forms.DataGridView OpcItemViewer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quality;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer4;
    }
}

