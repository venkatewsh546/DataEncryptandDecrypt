using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using static DataEncryptAndDecrypt.CommonMethods;

namespace DataEncryptAndDecrypt
{    
    [Activity(Label = "ChangeEncKey", Theme = "@style/AppTheme")]
    public class RenamePassCodeScreen : Activity
    {
        EditText FileSelectTextBox;
        EditText OldENKey;
        EditText NewENKey;     
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainChangeKeyLayout);
            FileSelectTextBox = FindViewById<EditText>(Resource.Id.CkFileSelectEditBox);
            OldENKey=FindViewById<EditText>(Resource.Id.Ckoldkey);
            NewENKey = FindViewById<EditText>(Resource.Id.cknewkey);
            
            if (!String.IsNullOrEmpty(Intent.GetStringExtra("selectedFile")))
            {
                FileSelectTextBox.Text = Intent.GetStringExtra("selectedFile");
            }
            else
            {
                FileSelectTextBox.Text = "NoFileSelected";
            }

            Button changekeybutton = (Button)FindViewById(Resource.Id.ChangeKeyButton);
            changekeybutton.Click += ChangekeybuttonClick;           
           
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent();
            intent.PutExtra("selectedFile", FileSelectTextBox.Text);
            SetResult(Result.Ok, intent);
            this.Finish();
        }       

        private void ChangekeybuttonClick(object sender, EventArgs e)
        {
            try
            {
                if ( FileSelectTextBox.Text != "NoFileSelected" &&
                    (!System.String.IsNullOrEmpty(NewENKey.Text) || !System.String.IsNullOrWhiteSpace(NewENKey.Text)) &&
                    (!System.String.IsNullOrEmpty(OldENKey.Text) || !System.String.IsNullOrWhiteSpace(OldENKey.Text))
                    )
                {
                    ChangeEncryptionKey();                  
                }
                else
                {
                    CommonMethods.MessageDialog("Info", "success"+System.Environment.NewLine+
                        "going back to main screen to select File",this);                  
                }
            }
            catch (Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, this);
            }
        }

        private void ChangeEncryptionKey()
        {
            try
            {
                List<String> Filedata = new List<String>(GetDataFromFile(FileSelectTextBox.Text));
                List<String> NewdataEncrypt = new List<string>();

                foreach (String str in Filedata)
                {
                    string[] singlestr = str.Split(',');
                    
                    NewdataEncrypt.Add(singlestr[0] + "," + 
                        EncryptPassword(DecryptPassword(singlestr[1], OldENKey.Text),NewENKey.Text) + "," + 
                        EncryptPassword(DecryptPassword(singlestr[2], OldENKey.Text), NewENKey.Text));
                }               

                NewdataEncrypt.Insert(0, "Source,UserName,Password");

                System.IO.File.WriteAllLines(FileSelectTextBox.Text, NewdataEncrypt.ToArray());
            }
            catch(Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, this);
            }
        }      
    }
}