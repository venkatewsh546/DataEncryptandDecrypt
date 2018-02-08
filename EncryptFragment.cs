using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using static DataEncryptAndDecrypt.CommonMethods;
using Newtonsoft.Json;
using System.IO;

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
            try
            {
                Java.IO.File myFile;

                if (System.String.IsNullOrEmpty(elFileSelectTextBox.Text))
                {
                    // myFile = new Java.IO.File(_context.GetExternalFilesDir(null), "EncryptDecrypt.txt");
                    myFile = new Java.IO.File(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "EncryptDecrypt.txt"));
                    elFileSelectTextBox.Text = myFile.AbsolutePath;

                    if (myFile.Exists())
                    {
                        GetDataFromJson(myFile.AbsolutePath);
                    }
                    else
                    {
                        myFile.CreateNewFile();

                        MFileData = new FileData
                        {
                            Mydata = new Mydata()
                        };
                        MFileData.Mydata.Unamepass = new List<Unamepass>();
                        MFileData.Mydata.Cardinfo = new List<Cardinfo>();
                    }
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
                    System.String account = string.Empty;
                    account = elTypeOfAccountTextBox.Text;
                    System.String userName = string.Empty;
                    userName = EncryptPassword(elUserNameTextBox.Text, elEncryptionKeyTextBox.Text);
                    System.String password = string.Empty;
                    password = EncryptPassword(elPasswordTextBox.Text, elEncryptionKeyTextBox.Text);
                    System.String encryptKey = string.Empty;
                    encryptKey = elEncryptionKeyTextBox.Text;

                    var index = MFileData.Mydata.Unamepass.FindIndex(x => (x.Source + ',' + x.UserName).Equals(account.ToString().ToUpper() + "," + userName));

                    if (index != -1)
                    {
                        MessageDialog("Updaing", "Old Password Will Replace With New One", _context);

                        var searchdata = account.ToString().ToUpper() + "," + userName;

                        MFileData.Mydata.Unamepass[index].Password = password;

                        File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(MFileData));

                        Toast.MakeText(_context, "data updated successfully", ToastLength.Short).Show();
                    }
                    else
                    {
                        MFileData.Mydata.Unamepass.Add(new Unamepass()
                        {
                            Source = account.ToUpper(),
                            UserName = userName,
                            Password = password
                        });

                        File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(MFileData));
                        Toast.MakeText(_context, "data encrypted successfully", ToastLength.Short).Show();

                    }
                }
                else
                {
                    MessageDialog("info", "please enter data in all fileds", _context);
                }
            }
            catch (IOException e)
            {
                MessageDialog("Error....", "writeToFile :- " + e.Message, _context);
            }
        }

    }
}