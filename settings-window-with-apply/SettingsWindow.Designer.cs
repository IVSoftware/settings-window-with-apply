namespace settings_window_with_apply
{
    partial class SettingsWindow
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
            propertyGrid = new PropertyGrid();
            panel = new Panel();
            buttonApply = new Button();
            buttonCancel = new Button();
            panel.SuspendLayout();
            SuspendLayout();
            // 
            // propertyGrid
            // 
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.Font = new Font("Segoe UI", 10F);
            propertyGrid.Location = new Point(0, 0);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(578, 544);
            propertyGrid.TabIndex = 0;
            // 
            // panel
            // 
            panel.Controls.Add(buttonApply);
            panel.Controls.Add(buttonCancel);
            panel.Dock = DockStyle.Bottom;
            panel.Location = new Point(0, 497);
            panel.Name = "panel";
            panel.Size = new Size(578, 47);
            panel.TabIndex = 1;
            // 
            // buttonApply
            // 
            buttonApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonApply.Location = new Point(336, 5);
            buttonApply.Name = "buttonApply";
            buttonApply.Size = new Size(112, 34);
            buttonApply.TabIndex = 0;
            buttonApply.Text = "Apply";
            buttonApply.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Location = new Point(454, 5);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(112, 34);
            buttonCancel.TabIndex = 0;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsWindow
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 544);
            Controls.Add(panel);
            Controls.Add(propertyGrid);
            Name = "SettingsWindow";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SettingsWindow";
            panel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid propertyGrid;
        private Panel panel;
        private Button buttonApply;
        private Button buttonCancel;
    }
}