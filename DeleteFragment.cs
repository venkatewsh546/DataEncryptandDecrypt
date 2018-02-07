using System;
using System.Collections.Generic;
using Android.OS;
using Android.Text;
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
        RadioGroup deldataTypeRadioGroup;
        RadioButton deldataTypeRadiobutton;
        int selectedId;

        public void Changevalues()
        {
            delFileSelectTextBox.Text = Filepath;
            Spinner(MFileData.Mydata, delTypeofAccountSpinner, deldataTypeRadiobutton.Text);
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

            deldataTypeRadioGroup = deleteView.FindViewById<RadioGroup>(Resource.Id.DelDataTypeRadioGroup);
            deldataTypeRadioGroup.CheckedChange += ChnageinDatatype;
            selectedId = deldataTypeRadioGroup.CheckedRadioButtonId;
            deldataTypeRadiobutton = (RadioButton)deleteView.FindViewById(selectedId);

            return deleteView;
        }

        private void ChnageinDatatype(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            selectedId = deldataTypeRadioGroup.CheckedRadioButtonId;
            deldataTypeRadiobutton = (RadioButton)deleteView.FindViewById(selectedId);
            if (delFileSelectTextBox.Text.Length > 0)
            {
                Spinner(MFileData.Mydata, delTypeofAccountSpinner, deldataTypeRadiobutton.Text);
            }
            else
            {
                MessageDialog(title: "Error", data: "Please Select File First", context: _context);
            }
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
                    if (deldataTypeRadiobutton.Text == "CardInfo")
                    {
                        var cindex = MFileData.Mydata.Cardinfo.FindIndex(x => (x.Source + ',' + x.CardNo).Equals(delTypeofAccountSpinner.SelectedItem.ToString().ToUpper() + "," + EncryptPassword(delUserNameSpinner.SelectedItem.ToString().Substring(delUserNameSpinner.SelectedItem.ToString().IndexOf(':')+1).Trim(), delEncryptionKeyTextBox.Text)));

                        if (cindex != -1)
                        {
                            MFileData.Mydata.Cardinfo.RemoveAt(cindex);
                            System.IO.File.WriteAllText(delFileSelectTextBox.Text, JsonConvert.SerializeObject(MFileData));
                            Spinner(MFileData.Mydata, delTypeofAccountSpinner, deldataTypeRadiobutton.Text);
                            Toast.MakeText(_context, "data Deleted successfully", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        var uindex = MFileData.Mydata.Unamepass.FindIndex(x => (x.Source + ',' + x.UserName).Equals(delTypeofAccountSpinner.SelectedItem.ToString().ToUpper() + "," + EncryptPassword(delUserNameSpinner.SelectedItem.ToString(), delEncryptionKeyTextBox.Text)));
                        if (uindex != -1)
                        {
                            MFileData.Mydata.Unamepass.RemoveAt(uindex);
                            System.IO.File.WriteAllText(delFileSelectTextBox.Text, JsonConvert.SerializeObject(MFileData));
                            Spinner(MFileData.Mydata, delTypeofAccountSpinner, deldataTypeRadiobutton.Text);
                            Toast.MakeText(_context, "data Deleted successfully", ToastLength.Short).Show();
                        }
                    }
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
            if (delEncryptionKeyTextBox.Text.Length > 0 || delFileSelectTextBox.Text.Length > 0
                 && !delTypeofAccountSpinner.SelectedItem.ToString().Equals("<<<< Select Item >>>>"))
            {
                delUserNameSpinner.Enabled = true;
                UserNameSpinner(MFileData.Mydata, ((Spinner)sender).GetItemAtPosition(e.Position).ToString());
            }

        }

        private void UserNameSpinner(Mydata selectdataset, String typeofaccount)
        {
            var userNameList = new List<String>();
            try
            {
                if (deldataTypeRadiobutton.Text == "CardInfo")
                {
                    var selectedCardInfo = MFileData.Mydata.Cardinfo.FindAll(x => x.Source == delTypeofAccountSpinner.SelectedItem.ToString());

                    foreach (Cardinfo str in selectedCardInfo)
                    {
                        userNameList.Add("CardNo: " + DecryptPassword(str.CardNo, delEncryptionKeyTextBox.Text));
                    }
                }
                else
                {
                    var selectedUnamepass = MFileData.Mydata.Unamepass.FindAll(x => x.Source == delTypeofAccountSpinner.SelectedItem.ToString());

                    foreach (Unamepass str in selectedUnamepass)
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