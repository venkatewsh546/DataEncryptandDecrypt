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
using System.IO;

namespace DataEncryptAndDecrypt
{
    public sealed class CardInfoFragment : Android.Support.V4.App.Fragment, IChangeViewvalues
    {
        View encryptView;
        EditText ciFileSelectTextBox;
        EditText ciTypeOfAccountTextBox;
        EditText ciCardNo;
        EditText ciIfscCode;
        EditText ciValidthrough;
        EditText ciValidFrom;
        EditText ciNameOnCard;
        EditText ciThreeDSecureCode;
        EditText ciCVV;
        EditText ciNotes;
        EditText ciEncryptionKeyTextBox;
        Button ciEncryptButton;
        Button ciResetButton;

        public void Changevalues()
        {
            ciFileSelectTextBox.Text = Filepath;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            encryptView= inflater.Inflate(Resource.Layout.CradInfoLayout, container, false); 
            ciFileSelectTextBox = encryptView.FindViewById<EditText>(Resource.Id.CiFileSelectTextBox);
            ciFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            ciFileSelectTextBox.SetCursorVisible(true);
            ciFileSelectTextBox.Click += DisplayFilesAndFolders;
            ciFileSelectTextBox.Text = Filepath;

            ciTypeOfAccountTextBox = encryptView.FindViewById<EditText>(Resource.Id.CiTypeOfAccounTextBox);
            ciCardNo = encryptView.FindViewById<EditText>(Resource.Id.CiCardNo);
            ciIfscCode = encryptView.FindViewById<EditText>(Resource.Id.CiIfsccode);
            ciEncryptionKeyTextBox = encryptView.FindViewById<EditText>(Resource.Id.CiEncryptionKeyTextBox);

            ciValidthrough = encryptView.FindViewById<EditText>(Resource.Id.CiValidthrough);
            ciValidFrom = encryptView.FindViewById<EditText>(Resource.Id.CiValidFrom);
            ciNameOnCard = encryptView.FindViewById<EditText>(Resource.Id.CiNameOnCard);
            ciThreeDSecureCode = encryptView.FindViewById<EditText>(Resource.Id.CiThreeDSecureCode);
            ciCVV = encryptView.FindViewById<EditText>(Resource.Id.CiCVV);
            ciNotes = encryptView.FindViewById<EditText>(Resource.Id.CiNotes);

            ciEncryptButton = encryptView.FindViewById<Button>(Resource.Id.CiEncryptButton);
            ciEncryptButton.Click += EncryptButtonClick;

            ciResetButton= encryptView.FindViewById<Button>(Resource.Id.CiResetButton);

            return encryptView;
        }

        private void EncryptButtonClick(object sender, EventArgs e)
        {
            EncryptbuttonWriteToFile();
            ciTypeOfAccountTextBox.Text = String.Empty;
            ciCardNo.Text = String.Empty;
            ciIfscCode.Text = String.Empty;
        }  

        private void EncryptbuttonWriteToFile()
        {
            System.String account = string.Empty;
            System.String cardNo = string.Empty;
            System.String password = string.Empty;
            System.String encryptKey = string.Empty;            
            System.String ifscCode = string.Empty;
            System.String validthrough = string.Empty;
            System.String validFrom = string.Empty;
            System.String nameOnCard = string.Empty;
            System.String threeDSecureCode = string.Empty;
            System.String cVV = string.Empty;
            System.String notes = string.Empty;

            try
            {
                Java.IO.File myFile;

                if (System.String.IsNullOrEmpty(ciFileSelectTextBox.Text))
                {
                    // myFile = new Java.IO.File(_context.GetExternalFilesDir(null), "EncryptDecrypt.txt");
                    myFile = new Java.IO.File(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "EncryptDecrypt.txt"));
                    ciFileSelectTextBox.Text = myFile.AbsolutePath;
                    myFile.CreateNewFile();

                    MFileData = new FileData
                    {
                        Mydata = new Mydata()
                    };
                    MFileData.Mydata.Unamepass = new List<Unamepass>();
                    MFileData.Mydata.Cardinfo = new List<Cardinfo>();

                }
                else
                {
                    myFile = new Java.IO.File(ciFileSelectTextBox.Text);
                }


                if ((!System.String.IsNullOrEmpty(ciTypeOfAccountTextBox.Text) || !System.String.IsNullOrWhiteSpace(ciTypeOfAccountTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(ciEncryptionKeyTextBox.Text) || !System.String.IsNullOrWhiteSpace(ciEncryptionKeyTextBox.Text)))
                {
                    account = ciTypeOfAccountTextBox.Text;
                    cardNo = EncryptPassword(ciCardNo.Text, ciEncryptionKeyTextBox.Text);
                    password = EncryptPassword(ciIfscCode.Text, ciEncryptionKeyTextBox.Text);
                    encryptKey = ciEncryptionKeyTextBox.Text;                   

                    //if (!myFile.Exists())
                    //{ 
                    //    CommonMethods.MFileData.mydata.unamepass.Add(new Unamepass() {
                    //        UserName= userName,
                    //        Password= password,
                    //        Source= account
                    //    });

                    //    System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(MFileData));

                    //    Toast.MakeText(_context, "data encrypted successfully", ToastLength.Short).Show();
                    //}
                    //if
                    //{
                 var index = MFileData.Mydata.Unamepass.FindIndex(x => (x.Source+','+ x.UserName).Equals(account.ToString().ToUpper() + "," + cardNo));

                if (index != -1)
                {
                    MessageDialog("Updaing", "Old Password Will Replace With New One", _context);

                    var searchdata = account.ToString().ToUpper() + "," + cardNo;

                    CommonMethods.MFileData.Mydata.Unamepass[index].Password = password;

                    System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(CommonMethods.MFileData));
                            
                    Toast.MakeText(_context, "data updated successfully", ToastLength.Short).Show();
                }
                else
                {
                    CommonMethods.MFileData.Mydata.Unamepass.Add(new Unamepass()
                    {
                        Source = account.ToUpper(),
                        UserName = cardNo,
                        Password = password
                    });

                    System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(CommonMethods.MFileData));
                    Toast.MakeText(_context, "data encrypted successfully", ToastLength.Short).Show();

                }
                   // }
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