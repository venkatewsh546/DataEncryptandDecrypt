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
using Newtonsoft.Json;
using static DataEncryptAndDecrypt.CommonMethods;

namespace DataEncryptAndDecrypt
{
    public sealed class DeleteFragment : Android.Support.V4.App.Fragment, IChangeViewvalues
    {
        View deleteView;
        //MainDeleteLayout
        EditText delFileSelectTextBox;
        EditText delEncryptionKeyTextBox;
        Spinner delTypeofAccountSpinner;
        Spinner delUserNameSpinner;
        Button deleteDataButton;

        public void Changevalues()
        {
            delFileSelectTextBox.Text = Filepath;
            Spinner(CommonMethods.MFileData.Mydata.Unamepass, delTypeofAccountSpinner);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            deleteView=(View)inflater.Inflate(Resource.Layout.MainDeleteLayout, container, false);
            //MainDeleteLayout
            delFileSelectTextBox = deleteView.FindViewById<EditText>(Resource.Id.DelFileSelectTextBox);
            delFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            delFileSelectTextBox.SetCursorVisible(true);
            delFileSelectTextBox.Click += DisplayFilesAndFolders;
            delFileSelectTextBox.NextFocusDownId = Resource.Id.DelEncryptionKeyTextBox;


            delEncryptionKeyTextBox = deleteView.FindViewById<EditText>(Resource.Id.DelEncryptionKeyTextBox);

            delEncryptionKeyTextBox.TextChanged += DelEncryptionKeyTextBoxtextChnagelistener;

            delTypeofAccountSpinner = deleteView.FindViewById<Spinner>(Resource.Id.DelTypeofAccountSpinner);
            delTypeofAccountSpinner.ItemSelected += DelTypeOfAcoountSpinnerItemClick;
            delTypeofAccountSpinner.Enabled = false;
            delUserNameSpinner = deleteView.FindViewById<Spinner>(Resource.Id.DelUserNameSpinner);
            delUserNameSpinner.Enabled = false;

            deleteDataButton = deleteView.FindViewById<Button>(Resource.Id.DeleteDataButton);
            deleteDataButton.Click += DeleteButtonClick;

            return deleteView;
        }

        private void DelEncryptionKeyTextBoxtextChnagelistener(object sender, TextChangedEventArgs e)
        {
           if(delEncryptionKeyTextBox.Text.Length>0)
            {
                delTypeofAccountSpinner.Enabled = true;
            }
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            try
            {
                if ((!System.String.IsNullOrEmpty(delTypeofAccountSpinner.SelectedItem.ToString()) || !System.String.IsNullOrWhiteSpace(delTypeofAccountSpinner.SelectedItem.ToString())) &&
                   (!System.String.IsNullOrEmpty(delUserNameSpinner.SelectedItem.ToString()) || !System.String.IsNullOrWhiteSpace(delUserNameSpinner.SelectedItem.ToString())) &&
                   (!System.String.IsNullOrEmpty(delFileSelectTextBox.Text) || !System.String.IsNullOrWhiteSpace(delFileSelectTextBox.Text)) &&
                   (!System.String.IsNullOrEmpty(delEncryptionKeyTextBox.Text) || !System.String.IsNullOrWhiteSpace(delEncryptionKeyTextBox.Text))
                   )
                {

                    var index = CommonMethods.MFileData.Mydata.Unamepass.FindIndex(x => (x.Source+','+ x.UserName).Equals(delTypeofAccountSpinner.SelectedItem.ToString().ToUpper() + "," + EncryptPassword(delUserNameSpinner.SelectedItem.ToString(), delEncryptionKeyTextBox.Text)));

                    CommonMethods.MFileData.Mydata.Unamepass.RemoveAt(index);

                    System.IO.File.WriteAllText(delFileSelectTextBox.Text, JsonConvert.SerializeObject(CommonMethods.MFileData));
                    Spinner(CommonMethods.MFileData.Mydata.Unamepass, delTypeofAccountSpinner);
                    Toast.MakeText(_context, "data Deleted successfully", ToastLength.Short).Show();
                }
                else
                {
                    MessageDialog("Error", "enter data in all Required Fields \n TypeOfAccount \n  UserName \n FileSelect \n EncryptionKey",_context);
                }
            }
            catch (Exception Ex)
            {
                MessageDialog("Error", "ChangeEncryptionKey \n" + Ex.Message, _context);
            }
        }

        private void DelTypeOfAcoountSpinnerItemClick(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //if (delEncryptionKeyTextBox.Text.Length == 0 || delFileSelectTextBox.Text.Length == 0
            //     && !delTypeofAccountSpinner.SelectedItem.ToString().Equals("<<<< Select Item >>>>"))
            //{
            //    MessageDialog("Info", "select File/enter Encryption key Before Selecting Item",_context);
            //    delTypeofAccountSpinner.SetSelection(0);

            //}
            //else
            if (delEncryptionKeyTextBox.Text.Length > 0 || delFileSelectTextBox.Text.Length > 0
                 && !delTypeofAccountSpinner.SelectedItem.ToString().Equals("<<<< Select Item >>>>"))
            {
                delUserNameSpinner.Enabled = true;
                UserNameSpinner(CommonMethods.MFileData.Mydata.Unamepass, ((Spinner)sender).GetItemAtPosition(e.Position).ToString());
            }

        }

        private void UserNameSpinner(List<Unamepass> dataFromFile, String typeofaccount)
        {
            var userNameList = new List<String>();
            try
            {
                foreach (Unamepass str in dataFromFile)
                {
                    if (str.Source.ToUpper().Equals(typeofaccount))
                    {
                        if (!userNameList.Contains(DecryptPassword(str.UserName, delEncryptionKeyTextBox.Text)))
                        {
                            userNameList.Add(DecryptPassword(str.UserName, delEncryptionKeyTextBox.Text));
                        }
                    }
                }

                var userNameAdapter = new ArrayAdapter<System.String>(_context, Android.Resource.Layout.SimpleSpinnerItem, userNameList);
                userNameAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);                
                delUserNameSpinner.Adapter = userNameAdapter;              
                userNameList.Clear();
            }
            catch (Exception ex)
            {
                MessageDialog("UserNamespinner", ex.Message, _context);
            }
        }
    }
}