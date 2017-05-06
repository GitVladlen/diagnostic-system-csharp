namespace DiagnosticSystem
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.довідкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.cboSheetWork = new System.Windows.Forms.ComboBox();
            this.btnOpenWork = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClassifyWork = new System.Windows.Forms.Button();
            this.dgvWorkMode = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblTotalSuccessTest = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.cboSheet = new System.Windows.Forms.ComboBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClassify = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkMode)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.довідкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(668, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // довідкаToolStripMenuItem
            // 
            this.довідкаToolStripMenuItem.Name = "довідкаToolStripMenuItem";
            this.довідкаToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.довідкаToolStripMenuItem.Text = "Довідка";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(669, 499);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cboSheetWork);
            this.tabPage1.Controls.Add(this.btnOpenWork);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.btnClassifyWork);
            this.tabPage1.Controls.Add(this.dgvWorkMode);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(661, 473);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Робочий режим";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(421, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Вибрати лист:";
            // 
            // cboSheetWork
            // 
            this.cboSheetWork.FormattingEnabled = true;
            this.cboSheetWork.Location = new System.Drawing.Point(505, 6);
            this.cboSheetWork.Name = "cboSheetWork";
            this.cboSheetWork.Size = new System.Drawing.Size(150, 21);
            this.cboSheetWork.TabIndex = 10;
            this.cboSheetWork.SelectedIndexChanged += new System.EventHandler(this.cboSheetWork_SelectedIndexChanged);
            // 
            // btnOpenWork
            // 
            this.btnOpenWork.Location = new System.Drawing.Point(268, 6);
            this.btnOpenWork.Name = "btnOpenWork";
            this.btnOpenWork.Size = new System.Drawing.Size(147, 23);
            this.btnOpenWork.TabIndex = 11;
            this.btnOpenWork.Text = "Відкрити файл даних *.xls";
            this.btnOpenWork.UseVisualStyleBackColor = true;
            this.btnOpenWork.Click += new System.EventHandler(this.btnOpenWork_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(243, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Введіть дані пацієнтів або завантажте з файлу";
            // 
            // btnClassifyWork
            // 
            this.btnClassifyWork.Location = new System.Drawing.Point(480, 443);
            this.btnClassifyWork.Name = "btnClassifyWork";
            this.btnClassifyWork.Size = new System.Drawing.Size(171, 23);
            this.btnClassifyWork.TabIndex = 8;
            this.btnClassifyWork.Text = "Почати класифікацію";
            this.btnClassifyWork.UseVisualStyleBackColor = true;
            this.btnClassifyWork.Click += new System.EventHandler(this.btnClassifyWork_Click);
            // 
            // dgvWorkMode
            // 
            this.dgvWorkMode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkMode.Location = new System.Drawing.Point(6, 35);
            this.dgvWorkMode.Name = "dgvWorkMode";
            this.dgvWorkMode.Size = new System.Drawing.Size(645, 402);
            this.dgvWorkMode.TabIndex = 6;
            this.dgvWorkMode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvWorkMode_KeyUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblTotalSuccessTest);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.dataGridView);
            this.tabPage2.Controls.Add(this.cboSheet);
            this.tabPage2.Controls.Add(this.btnOpen);
            this.tabPage2.Controls.Add(this.btnClassify);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(661, 473);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Контрольний режим";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblTotalSuccessTest
            // 
            this.lblTotalSuccessTest.AutoSize = true;
            this.lblTotalSuccessTest.Location = new System.Drawing.Point(246, 447);
            this.lblTotalSuccessTest.Name = "lblTotalSuccessTest";
            this.lblTotalSuccessTest.Size = new System.Drawing.Size(77, 13);
            this.lblTotalSuccessTest.TabIndex = 10;
            this.lblTotalSuccessTest.Text = "Не визначено";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 447);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Відсоток правильно клсифікованих об\'єктів:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(398, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Вибрати лист:";
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(8, 35);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(645, 401);
            this.dataGridView.TabIndex = 5;
            // 
            // cboSheet
            // 
            this.cboSheet.FormattingEnabled = true;
            this.cboSheet.Location = new System.Drawing.Point(482, 11);
            this.cboSheet.Name = "cboSheet";
            this.cboSheet.Size = new System.Drawing.Size(171, 21);
            this.cboSheet.TabIndex = 4;
            this.cboSheet.SelectedIndexChanged += new System.EventHandler(this.cboSheet_SelectedIndexChanged);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(8, 6);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(147, 23);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.Text = "Відкрити файл даних *.xls";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnClassify
            // 
            this.btnClassify.Location = new System.Drawing.Point(484, 442);
            this.btnClassify.Name = "btnClassify";
            this.btnClassify.Size = new System.Drawing.Size(171, 23);
            this.btnClassify.TabIndex = 7;
            this.btnClassify.Text = "Почати класифікацію";
            this.btnClassify.UseVisualStyleBackColor = true;
            this.btnClassify.Click += new System.EventHandler(this.btnClassify_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 527);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Діагностика післяопераційних ускладень (мітральний клапан)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkMode)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem довідкаToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnClassify;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ComboBox cboSheet;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClassifyWork;
        private System.Windows.Forms.DataGridView dgvWorkMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotalSuccessTest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboSheetWork;
        private System.Windows.Forms.Button btnOpenWork;
    }
}

