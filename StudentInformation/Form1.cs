/*
  This File is Created by Mr. Mr. ikramul islam Siddique.
  This is A Windows Form Application.This Task Was Given by Asif Vai.
  After Completing i just Uploaded it to Github.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentInformation
{
    public partial class Form1 : Form
    {
        tblStudentsDetail model = new tblStudentsDetail();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtName.Text = txtClass.Text = txtRoll.Text = txtAge.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.Id = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            model.FullName = txtName.Text.Trim();
            model.Class = txtClass.Text.Trim();
            model.Roll = txtRoll.Text.Trim();
            model.Age = txtAge.Text.Trim();
            using (StudentInformationEntities db = new StudentInformationEntities())
            {
                if (model.Id == 0)//Insert
                    db.tblStudentsDetails.Add(model);
                else //Update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted Successfully");
        }

        void PopulateDataGridView()
        {
            dgvStudentInfo.AutoGenerateColumns = false;
            using (StudentInformationEntities db = new StudentInformationEntities())
            {
                dgvStudentInfo.DataSource = db.tblStudentsDetails.ToList();

            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvStudentInfo.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvStudentInfo.CurrentRow.Cells["Id"].Value);
                using (StudentInformationEntities db = new StudentInformationEntities())
                {
                    model = db.tblStudentsDetails.Where(x => x.Id == model.Id).FirstOrDefault();
                    txtName.Text = model.FullName;
                    txtClass.Text = model.Class;
                    txtRoll.Text = model.Roll;
                    txtAge.Text = model.Age;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure to Delete this Record ?", "StudenInformation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StudentInformationEntities db = new StudentInformationEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.tblStudentsDetails.Attach(model);
                    db.tblStudentsDetails.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }
    }
}
