using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using static DataEncryptAndDecrypt.CommonMethods;

namespace DataEncryptAndDecrypt
{
    public class DecryptFragment : Android.Support.V4.App.Fragment, Classvalues
    {

        View decryptView;
        //MainDecyptLayout
        EditText dlFileSelectTextBox;
        Spinner dlTypeofAccountSpinner;
        EditText dlEncryptionKeyTextBox;
        ListView decrypteddataListView;
        Button decryptDataButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            decryptView=(View)inflater.Inflate(Resource.Layout.MainDecryptLayout, container, false);
            
            //MainDecyptLayout
            dlFileSelectTextBox = decryptView.FindViewById<EditText>(Resource.Id.DlFileSelectTextBox);
            dlFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            dlFileSelectTextBox.SetCursorVisible(true);
            dlFileSelectTextBox.Click += DisplayFilesAndFolders;

            dlTypeofAccountSpinner = decryptView.FindViewById<Spinner>(Resource.Id.DlTypeofAccountSpinner);

            dlEncryptionKeyTextBox = decryptView.FindViewById<EditText>(Resource.Id.DlEncryptionKeyTextBox);

            decrypteddataListView = decryptView.FindViewById<ListView>(Resource.Id.DecrypteddataListView);
            decrypteddataListView.ItemLongClick += CommonMethods.ListViewLongClickListener;

            decryptDataButton = decryptView.FindViewById<Button>(Resource.Id.DecryptButton);
            decryptDataButton.Click += DecryptButtonClick;

            return decryptView;
        }

        public void Changevalues()
        {
            dlFileSelectTextBox.Text = Filepath;
            Spinner(_fileData.mydata.unamepass, dlTypeofAccountSpinner);

        }

        private void DecryptButtonClick(object sender, EventArgs e)
        {

            List<System.String> datacol = new List<System.String>();

            if (dlFileSelectTextBox.Text.Length != 0 && dlEncryptionKeyTextBox.Text.Length != 0)
            {
                foreach (Unamepass str in _fileData.mydata.unamepass)
                {
                    if (str.Source.Equals(dlTypeofAccountSpinner.SelectedItem.ToString()))
                    {
                        datacol.Add(str.Source);
                        datacol.Add(DecryptPassword(str.UserName, dlEncryptionKeyTextBox.Text));
                        datacol.Add(DecryptPassword(str.Password, dlEncryptionKeyTextBox.Text));
                    }
                }
                var adapter = new ArrayAdapter<System.String>(Activity.ApplicationContext, Android.Resource.Layout.SimpleSpinnerItem, datacol);
                decrypteddataListView.Adapter = adapter;
            }
            else
            {
                MessageDialog("Info", "Please Enter data in FileSelect/EncryptionKey fields", Activity.ApplicationContext);
            }
        }

       
    }
}