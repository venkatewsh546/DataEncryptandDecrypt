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
using Android.Support.Design.Widget;

namespace DataEncryptAndDecrypt
{
    public sealed class CardInfoFragment : Android.Support.V4.App.Fragment, IChangeViewvalues
    {
        View cardInfoView;
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
            cardInfoView= inflater.Inflate(Resource.Layout.CradInfoLayout, container, false); 
            ciFileSelectTextBox = cardInfoView.FindViewById<EditText>(Resource.Id.CiFileSelectTextBox);
            ciFileSelectTextBox.SetRawInputType(Android.Text.InputTypes.Null);
            ciFileSelectTextBox.SetCursorVisible(true);
            ciFileSelectTextBox.Click += DisplayFilesAndFolders;

            ciTypeOfAccountTextBox = cardInfoView.FindViewById<EditText>(Resource.Id.CiTypeOfAccounTextBox);
            ciCardNo = cardInfoView.FindViewById<EditText>(Resource.Id.CiCardNo);
            ciIfscCode = cardInfoView.FindViewById<EditText>(Resource.Id.CiIfsccode);
            ciEncryptionKeyTextBox = cardInfoView.FindViewById<EditText>(Resource.Id.CiEncryptionKeyTextBox);
            

            ciValidthrough = cardInfoView.FindViewById<EditText>(Resource.Id.CiValidthrough);
            ciValidFrom = cardInfoView.FindViewById<EditText>(Resource.Id.CiValidFrom);
            ciNameOnCard = cardInfoView.FindViewById<EditText>(Resource.Id.CiNameOnCard);
            ciThreeDSecureCode = cardInfoView.FindViewById<EditText>(Resource.Id.CiThreeDSecureCode);
            ciCVV = cardInfoView.FindViewById<EditText>(Resource.Id.CiCVV);
            ciNotes = cardInfoView.FindViewById<EditText>(Resource.Id.CiNotes);

            ciEncryptButton = cardInfoView.FindViewById<Button>(Resource.Id.CiEncryptButton);
            ciEncryptButton.Click += EncryptButtonClick;

            ciResetButton= cardInfoView.FindViewById<Button>(Resource.Id.CiResetButton);

            return cardInfoView;
        }

        private void EncryptButtonClick(object sender, EventArgs e)
        {
            EncryptbuttonWriteToFile();
            ciTypeOfAccountTextBox.Text =String.Empty;
            ciCardNo.Text = String.Empty;
            ciIfscCode.Text = String.Empty; 
            ciValidthrough.Text = String.Empty;
            ciValidFrom.Text = String.Empty;
            ciNameOnCard.Text = String.Empty;
            ciThreeDSecureCode.Text = String.Empty;
            ciCVV.Text = String.Empty;
            ciNotes.Text = String.Empty;
            ciEncryptionKeyTextBox.Text = String.Empty;

        }  

        private void EncryptbuttonWriteToFile()
        {
            System.String account = string.Empty;
            System.String cardNo = string.Empty;
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
                    myFile = new Java.IO.File(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "EncryptDecrypt.txt"));
                    ciFileSelectTextBox.Text = myFile.AbsolutePath;

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
                    myFile = new Java.IO.File(ciFileSelectTextBox.Text);
                }

                if ((!System.String.IsNullOrEmpty(ciTypeOfAccountTextBox.Text) || !System.String.IsNullOrWhiteSpace(ciTypeOfAccountTextBox.Text)) &&
                    (!System.String.IsNullOrEmpty(ciEncryptionKeyTextBox.Text) || !System.String.IsNullOrWhiteSpace(ciEncryptionKeyTextBox.Text)))
                {
                    encryptKey = ciEncryptionKeyTextBox.Text;
                    account = ciTypeOfAccountTextBox.Text;
                    cardNo = EncryptPassword(ciCardNo.Text, encryptKey);
                    ifscCode = EncryptPassword(ciIfscCode.Text, encryptKey);                                      
                    validthrough = EncryptPassword(ciValidthrough.Text, encryptKey);
                    validFrom = EncryptPassword(ciValidFrom.Text, encryptKey);
                    nameOnCard = EncryptPassword(ciNameOnCard.Text, encryptKey);
                    threeDSecureCode = EncryptPassword(ciThreeDSecureCode.Text, encryptKey);
                    cVV = EncryptPassword(ciCVV.Text, encryptKey);
                    notes = EncryptPassword(ciNotes.Text, encryptKey);

                    var searchdata = account.ToString().ToUpper() + "," + cardNo;
                    var index = MFileData.Mydata.Cardinfo.FindIndex(x => (x.Source+','+ x.CardNo).Equals(searchdata));

                if (index != -1)
                {
                    MessageDialog("Updaing", "Old Data Will Replace With New One", _context);                    

                    CommonMethods.MFileData.Mydata.Cardinfo[index].IFSCCODE = ifscCode;
                    CommonMethods.MFileData.Mydata.Cardinfo[index].Validthrough = validthrough;
                    CommonMethods.MFileData.Mydata.Cardinfo[index].ValidFrom = validFrom;
                    CommonMethods.MFileData.Mydata.Cardinfo[index].NameOnCard = nameOnCard;
                    CommonMethods.MFileData.Mydata.Cardinfo[index].ThreeDSecureCode = threeDSecureCode;
                    CommonMethods.MFileData.Mydata.Cardinfo[index].CVV = cVV;
                    CommonMethods.MFileData.Mydata.Cardinfo[index].Notes = notes;

                    System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(MFileData));
                            
                    Toast.MakeText(_context, "data updated successfully", ToastLength.Short).Show();
                }
                else
                {
                    CommonMethods.MFileData.Mydata.Cardinfo.Add(new Cardinfo()
                    {
                        Source = account.ToUpper(),
                        CardNo = cardNo,
                        IFSCCODE = ifscCode,
                        Validthrough = validthrough,
                        ValidFrom = validFrom,
                        NameOnCard = nameOnCard,
                        ThreeDSecureCode = threeDSecureCode,
                        CVV = cVV,
                        Notes = notes
                    });

                    System.IO.File.WriteAllText(myFile.AbsolutePath, JsonConvert.SerializeObject(MFileData));
                    Toast.MakeText(_context, "data encrypted successfully", ToastLength.Short).Show();

                }
                }
                else
                {
                    if (System.String.IsNullOrEmpty(ciEncryptionKeyTextBox.Text))
                    {
                        ciEncryptionKeyTextBox.SetError("Encryption key Should not Blank", null);
                    }
                    if (System.String.IsNullOrEmpty(ciTypeOfAccountTextBox.Text))
                    {
                        ciTypeOfAccountTextBox.SetError("Type of Account Should not Blank", null);
                    }

                }
            }
            catch (System.IO.IOException e)
            {
                MessageDialog("Error....", "writeToFile :- " + e.Message, _context);
            }
        }

    }
}