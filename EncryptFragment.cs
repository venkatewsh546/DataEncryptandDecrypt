using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.IO;
using Android;
using static DataEncryptAndDecrypt.CommonMethods;
using System.Text;
using static Android.App.ActionBar;
using Newtonsoft.Json;

namespace DataEncryptAndDecrypt
{
    public sealed class EncryptFragment : Android.Support.V4.App.Fragment, IChangeViewvalues
    {
        View encryptView;
        EditText elFileSelectTextBox;
        EditText elTypeOfAccountTextBox;
        EditText elUserNameTextBox;
        EditText elPasswordTextBox;
        EditText elEncryptionKeyTextBox;
        Button encryptDataButton;

        public void Changevalues()
        {
            elFileSelectTextBox.Text = Filepath;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            encryptView= inflater.Inflate(Resource.Layout.MainEncryptLayout, container, false); 
            elFileSelectTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElFileSelectTextBox);
            elFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            elFileSelectTextBox.SetCursorVisible(true);
            elFileSelectTextBox.Click += DisplayFilesAndFolders;
            elFileSelectTextBox.Text = Filepath;

            elTypeOfAccountTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElTypeOfAccounTextBox);
            elUserNameTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElUserNameTextBox);
            elPasswordTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElPasswordTextBox);
            elEncryptionKeyTextBox = encryptView.FindViewById<EditText>(Resource.Id.ElEncryptionKeyTextBox);

            encryptDataButton = encryptView.FindViewById<Button>(Resource.Id.EncryptButton);
            encryptDataButton.Click += EncryptButtonClick;

            return encryptView;
        }

        private void EncryptButtonClick(object sender, EventArgs e)
        {
            EncryptbuttonWriteToFile();
            elTypeOfAccountTextBox.Text = String.Empty;
            elUserNameTextBox.Text = String.Empty;
            elPasswordTextBox.Text = String.Empty;
        }  

        private void EncryptbuttonWriteToFile()
        {
            System.String account = string.Empty;
            System.String userName = string.Empty;
            System.String password = string.Empty;
            System.String encryptKey = string.Empty;
            try
            {
                Java.IO.File myFile;

                if (System.String.IsNullOrEmpty(elFileSelectTextBox.Text))
                {
                    myFile = new Java.IO.File(_context.GetExternalFilesDir(null), "EncryptDecrypt.txt");
                    elFileSelectTextBox.Text = myFile.AbsolutePath;
                }
                else
                {
                    myFile = new Java.IO.File(elFileSelectTextBox.Text);
                }


                if ((!System.String.IsNullOrEmpty(elTypeOfAccountTextBox.Text) || !System.String.IsNullOrWhiteSpace(elTypeOfAccountTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(elUserNameTextBox.Text) || !System.String.IsNullOrWhiteSpace(elUserNameTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(elPasswordTextBox.Text) || !System.String.IsNullOrWhiteSpace(elPasswordTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(elEncryptionKeyTextBox.Text) || !System.String.IsNullOrWhiteSpace(elEncryptionKeyTextBox.Text)))
                {
                    account = elTypeOfAccountTextBox.Text;
                    userName = EncryptPassword(elUserNameTextBox.Text, elEncryptionKeyTextBox.Text);
                    password = EncryptPassword(elPasswordTextBox.Text, elEncryptionKeyTextBox.Text);
                    encryptKey = elEncryptionKeyTextBox.Text;
                   

                    if (!myFile.Exists())
                    {
                        myFile.CreateNewFile();

                        CommonMethods.FileData.mydata.unamepass.Add(new Unamepass() {
                            UserName= userName,
                            Password= password,
                            Source= account
                        });

                        System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(CommonMethods.FileData));

                        Toast.MakeText(_context, "data encrypted successfully", ToastLength.Short).Show();
                    }
                    else
                    {
                        var index = CommonMethods.FileData.mydata.unamepass.FindIndex(x => (x.Source+','+ x.UserName).Equals(account.ToString().ToUpper() + "," + userName));

                        if (index != -1)
                        {
                            MessageDialog("Updaing", "Old Password Will Replace With New One", _context);

                            var searchdata = account.ToString().ToUpper() + "," + userName;

                            CommonMethods.FileData.mydata.unamepass[index].Password = password;

                            System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(CommonMethods.FileData));
                            
                            Toast.MakeText(_context, "data updated successfully", ToastLength.Short).Show();
                        }
                        else
                        {
                            CommonMethods.FileData.mydata.unamepass.Add(new Unamepass()
                            {
                                Source = account.ToUpper(),
                                UserName = userName,
                                Password = password
                            });

                            System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(CommonMethods.FileData));
                            Toast.MakeText(_context, "data encrypted successfully", ToastLength.Short).Show();

                        }
                    }
                }
                else
                {
                    MessageDialog("info", "please enter data in all fileds", _context);
                }
            }
            catch (System.IO.IOException e)
            {
                MessageDialog("Error....", "writeToFile :- " + e.Message, _context);
            }
        }

    }
}